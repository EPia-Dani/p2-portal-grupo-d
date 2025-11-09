using System;
using UnityEngine;

public class PortalGun : MonoBehaviour
{



    [SerializeField]
    public GameObject OrangePortal;
    [SerializeField]
    public GameObject BluePortal;

    [SerializeField]
    public GameObject previewPortal;
    Camera playerCamera;

    [SerializeField] private float maxShootDistance = 100f;   
    [SerializeField] private float maxNormalAngle = 20f;
    [SerializeField] private float maxPointDistance = 0.1f;

    [SerializeField] private float offset = 0.05f;
    private string tagOrange = "OrangePortal";
    private string tagBlue = "BluePortal";


    void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera")?.GetComponent<Camera>();
        if (playerCamera == null)
        {
            Debug.LogWarning("PortalGun: camera not set");
        }
        if (OrangePortal == null || BluePortal == null)
        {
            Debug.LogWarning("PortalGUn: portal not set, check inspector");
        }


    }

    public void OnRightClick()
    {

        handleShoot(OrangePortal,tagOrange);
    }
    public void OnLeftClick()
    {
        handleShoot(BluePortal,tagBlue);


    }


    private void handleShoot(GameObject portal,string tag)
    {

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxShootDistance))
        {
            if (hit.collider.CompareTag("validWall"))
            {
                GameObject preview = Instantiate(previewPortal, hit.point, Quaternion.LookRotation(-hit.normal));
                bool isValid = isValidSpawn(preview);
                Destroy(preview);
                if (isValid)
                {
                    GameObject actualPortal = GameObject.FindGameObjectWithTag(tag);
                    if (actualPortal != null) { Destroy(actualPortal); Debug.Log("PortalGun: object found"); }
                    Vector3 spawnPos= hit.point + hit.normal * offset;
                    GameObject newPortal =Instantiate(portal, spawnPos, Quaternion.LookRotation(hit.normal));

                    
                    newPortal.GetComponent<Portal>().setWall(hit.collider.gameObject);
                    if (tag.Equals(tagOrange)){
                        newPortal.GetComponent<Portal>().setOtherPortal(GameObject.FindGameObjectWithTag(tagBlue));
                        GameObject.FindGameObjectWithTag(tagBlue).GetComponent<Portal>().setOtherPortal(newPortal);

                    }
                    else if(tag.Equals(BluePortal)) 
                    {
                        newPortal.GetComponent<Portal>().setOtherPortal(GameObject.FindGameObjectWithTag(tagOrange));
                        GameObject.FindGameObjectWithTag(tagOrange).GetComponent<Portal>().setOtherPortal(newPortal);

                    }

                }
            }

        }
    }
    
    private bool isValidSpawn(GameObject previewInstance)
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
