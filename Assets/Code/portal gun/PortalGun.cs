using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PortalGun : MonoBehaviour
{
    [SerializeField]
    public GameObject OrangePortal;
    [SerializeField]
    public GameObject BluePortal;

    [SerializeField]
    public GameObject previewPortal;


    [SerializeField]
    public GameObject BluePortalPreview;

    [SerializeField]
    public GameObject OrangePortalPreview;
    Camera playerCamera;

    [SerializeField] private float maxShootDistance = 100f;   
    [SerializeField] private float maxNormalAngle = 20f;
    [SerializeField] private float maxPointDistance = 0.1f;

    [SerializeField] private float offset = 0.0001f;
    private string tagOrange = "OrangePortal";
    private string tagBlue = "BluePortal";


    //preview state control
    private bool showingPreview = false;
    private GameObject currentPreview;
    private GameObject currentPortalPrefab;
    private string currentTag;

    //resize

    private float currentScale = 1f;
    private float minScale=0.5f;
    private float maxScale = 2f;

    private float initalScale = 1f;

    private float scrollSens = 0.01f;



    void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera")?.GetComponent<Camera>();
        if (playerCamera == null)
        {
            Debug.LogWarning("PortalGun: camera not set");
        }
        if (OrangePortal == null || BluePortal == null)
        {
            Debug.LogWarning("PortalGun: portal not set, check inspector");
        }


    }


    void Update()
    {
        if (showingPreview && currentPreview != null)
        {
            UpdatePreviewPosition(currentPreview);
        }
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        HandleInput(context, BluePortal, BluePortalPreview, tagBlue);
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        HandleInput(context, OrangePortal, OrangePortalPreview, tagOrange);
    }


    public void OnScroll(InputAction.CallbackContext context)
    {

        if (!showingPreview || currentPreview == null|currentTag!=tagBlue) return;

        Vector2 scrollValue = context.ReadValue<Vector2>();

        if (Mathf.Abs(scrollValue.y) > scrollSens)
        {

            currentScale += scrollValue.y * 0.1f;
            currentScale = Mathf.Clamp(currentScale, minScale, maxScale);
            
            currentPreview.transform.localScale = Vector3.one * currentScale;
        }
    }

    private void HandleInput(InputAction.CallbackContext context, GameObject portal, GameObject previewPrefab, string tag)
    {
        if (context.started)
        {
            currentScale = initalScale;
            showingPreview = true;
            currentPortalPrefab = portal;
            currentTag = tag;
            currentPreview = Instantiate(previewPrefab);
        }
        else if (context.performed)
        {
            if (showingPreview && currentPreview != null)
                UpdatePreviewPosition(currentPreview);
        }
        else if (context.canceled)
        {
            showingPreview = false;
            DestroyCurrentPreview();
            HandleShoot(portal, tag,currentScale);
        }
    }
    


    private void UpdatePreviewPosition(GameObject preview)
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxShootDistance))
        {
            if (!hit.collider.CompareTag("validWall"))
            {
                preview.SetActive(false);
                return;
            }
            preview.transform.position = hit.point + hit.normal * offset;
            preview.transform.rotation = Quaternion.LookRotation(hit.normal);

            if (IsValidSpawn(preview))
            {
                if (!preview.activeSelf)
                    preview.SetActive(true);
            }
            else
            {
                preview.SetActive(false);
            }
        }
        else
        {
            preview.SetActive(false);
        }
    }
        private void DestroyCurrentPreview()
    {
        if (currentPreview != null)
        {
            Destroy(currentPreview);
            currentPreview = null;
        }
    }
    private void HandleShoot(GameObject portal, string tag,float scale)
    {

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxShootDistance))
        {
            if (hit.collider.CompareTag("validWall"))
            {
                GameObject preview = Instantiate(previewPortal, hit.point, Quaternion.LookRotation(-hit.normal));
                bool isValid = IsValidSpawn(preview);
                Destroy(preview);
                if (isValid)
                {
                    GameObject actualPortal = GameObject.FindGameObjectWithTag(tag);
                    if (actualPortal != null)
                    {

                        Debug.Log("Destroyed");
                        actualPortal.SetActive(false); //protect from spams
                        Destroy(actualPortal);


                    }
                    Vector3 spawnPos = hit.point + hit.normal * offset;//new portal pos
                    GameObject newPortal = Instantiate(portal, spawnPos, Quaternion.LookRotation(hit.normal));

                    newPortal.GetComponent<Portal>().setWall(hit.collider.gameObject);


                    if (tag.Equals(tagOrange))
                    {
                        GameObject otherPortal = GameObject.FindGameObjectWithTag(tagBlue);
                        PortalEvents.RaiseOrangePortalActivated();
                        if (otherPortal == null) { return; }//check if the other portal is active
                        newPortal.GetComponent<Portal>().setOtherPortal(otherPortal);
                        otherPortal.GetComponent<Portal>().setOtherPortal(newPortal);
                    }
                    else if (tag.Equals(tagBlue))
                    {
                        newPortal.transform.localScale = Vector3.one * scale;
                        GameObject otherPortal = GameObject.FindGameObjectWithTag(tagOrange);
                        PortalEvents.RaiseBluePortalActivated();
                        if (otherPortal == null) { return; }//check if the other portal is active
                        newPortal.GetComponent<Portal>().setOtherPortal(otherPortal);
                        otherPortal.GetComponent<Portal>().setOtherPortal(newPortal);

                    }

                }
            }

        }
    }
    
    private bool IsValidSpawn(GameObject previewInstance)
    {
        Transform[] validPoints = previewInstance.GetComponentsInChildren<Transform>();

        foreach (Transform point in validPoints)
        {
            if (!point.CompareTag("validPoint"))
                continue;

            Vector3 pointPos = point.position;
            Vector3 direction = (pointPos - playerCamera.transform.position).normalized;

            if (Physics.Raycast(playerCamera.transform.position, direction, out RaycastHit hit, maxShootDistance))
            {
                float distancia = Vector3.Distance(hit.point, pointPos);
                if (distancia > maxPointDistance)
                {
                    Debug.DrawLine(hit.point, pointPos, Color.red, 1f);
                    return false;
                }

                float angle = Vector3.Angle(hit.normal, -previewInstance.transform.forward);
                if (angle > maxNormalAngle)
                {
                    Debug.DrawRay(hit.point, hit.normal * 0.3f, Color.yellow, 1f);
                    return false;
                }

                if (!hit.collider.CompareTag("validWall"))
                {
                    Debug.DrawRay(hit.point, hit.normal * 0.3f, Color.magenta, 1f);
                    return false;
                }

                Debug.DrawLine(playerCamera.transform.position, pointPos, Color.green, 0.3f);
            }
            else
            {
                Debug.DrawLine(playerCamera.transform.position, pointPos, Color.red, 1f);
                return false;
            }
        }
        return true;
    }

}
