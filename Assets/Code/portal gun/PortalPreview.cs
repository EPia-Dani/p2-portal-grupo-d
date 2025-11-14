using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class PortalPreview
{
    private readonly Camera playerCamera;
    private readonly float maxShootDistance;
    private readonly float offset;

    private GameObject bluePreviw;
    private GameObject orangePreview;
    private GameObject currentPreviw;

    //private GameObject currentPreview;

    private bool showingPreview = false;
    private float currentScale = 1f;
    private readonly float minScale = 0.5f;
    private readonly float maxScale = 2f;
    private readonly float scrollSens = 0.01f;

    private static readonly Vector3 HiddenPos = new Vector3(-200, -2000, -2000);
    private readonly Vector3 baseScale = new Vector3(0.3f, 0.3f, 0.3f);


    public float CurrentScale => currentScale;

    public PortalPreview(Camera camera, float maxShootDistance, float offset, GameObject blue, GameObject orange)
    {
        this.playerCamera = camera;
        this.maxShootDistance = maxShootDistance;
        this.offset = offset;
        bluePreviw = blue;
        orangePreview = orange;

    }

 /*   public void SetPreviewPrefabs(GameObject blue, GameObject orange)
    {
        bluePreviewPrefab = blue;
        orangePreviewPrefab = orange;
    }*/

    public void StartPreview(PortalType type)
    {

        if (type == PortalType.Blue)
        {
            currentPreviw = bluePreviw;
        }
        else
        {
            currentPreviw = orangePreview;
        }
        currentPreviw.transform.localScale = baseScale;
        currentScale=1f;
        showingPreview = true;

    }

    public void EndPreview()
    {
        setDefaultPos();
        showingPreview = false;
    }

    private void  setDefaultPos()
    {
        if (currentPreviw == bluePreviw)
        {
            bluePreviw.transform.position = HiddenPos;
        }
        else
        {
            orangePreview.transform.position = HiddenPos;
        }
    }

    public void UpdatePreviewPosition()
    {
        if (!showingPreview||currentPreviw==null) return;
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxShootDistance))
        {
            if (!hit.collider.CompareTag("validWall"))
            {
                setDefaultPos();
                return;
            }

            currentPreviw.transform.position = hit.point + hit.normal * offset;
            currentPreviw.transform.rotation = Quaternion.LookRotation(hit.normal);

            if (!(IsValidSpawn(currentPreviw))){

                setDefaultPos();
            }
        }
        else
        {
            setDefaultPos();
        }
    }

    public void HandleScroll(InputAction.CallbackContext context, string activeTag)
    {
        if (!showingPreview || currentPreviw!=bluePreviw) return;

        Vector2 scrollValue = context.ReadValue<Vector2>();

        if (Mathf.Abs(scrollValue.y) > scrollSens)
        {
            currentScale += scrollValue.y * 0.1f;
            currentScale = Mathf.Clamp(currentScale, minScale, maxScale);
            currentPreviw.transform.localScale = baseScale * currentScale;
        }
    }

    private bool IsValidSpawn(GameObject previewInstance)
    {
        Camera cam = playerCamera;
        Transform[] validPoints = previewInstance.GetComponentsInChildren<Transform>();

        Collider firstHitCollider = null; 

        foreach (Transform point in validPoints)
        {
            if (!point.CompareTag("validPoint"))
                continue;

            Vector3 pointPos = point.position;
            Vector3 direction = (pointPos - cam.transform.position).normalized;

            if (Physics.Raycast(cam.transform.position, direction, out RaycastHit hit, maxShootDistance))
            { 
                if (!hit.collider.CompareTag("validWall")){
                    return false;
                }
                if (firstHitCollider == null)
                {
                    firstHitCollider = hit.collider; 
                }
                else if (hit.collider != firstHitCollider)
                {
                    return false;
                }
            }
            else{
                return false;
                    }
        }
        return true;
    }
}
