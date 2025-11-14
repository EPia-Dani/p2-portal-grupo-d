using UnityEngine;
using UnityEngine.InputSystem;


public class PortalPreview
{
    private readonly Camera playerCamera;
    private readonly float maxShootDistance;
    private readonly float offset;

    private GameObject bluePreviewPrefab;
    private GameObject orangePreviewPrefab;
    private GameObject currentPreview;

    private bool showingPreview = false;
    private string currentTag;
    private float currentScale = 1f;
    private readonly float minScale = 0.5f;
    private readonly float maxScale = 2f;
    private readonly float scrollSens = 0.01f;

    private const float maxNormalAngle = 20f;
    private const float maxPointDistance = 0.1f;

    public float CurrentScale => currentScale;

    public PortalPreview(Camera camera, float maxShootDistance, float offset)
    {
        this.playerCamera = camera;
        this.maxShootDistance = maxShootDistance;
        this.offset = offset;
    }

    public void SetPreviewPrefabs(GameObject blue, GameObject orange)
    {
        bluePreviewPrefab = blue;
        orangePreviewPrefab = orange;
    }

    public void StartPreview(PortalType type)
    {
        showingPreview = true;
        currentScale = 1f;
        currentTag = type == PortalType.Blue ? "BluePortal" : "OrangePortal";

        var prefab = type == PortalType.Blue ? bluePreviewPrefab : orangePreviewPrefab;
        currentPreview = Object.Instantiate(prefab);
    }

    public void EndPreview()
    {
        showingPreview = false;
        if (currentPreview != null)
            Object.Destroy(currentPreview);
        currentPreview = null;
    }

    public void UpdatePreviewPosition()
    {
        if (!showingPreview || currentPreview == null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxShootDistance))
        {
            if (!hit.collider.CompareTag("validWall"))
            {
                currentPreview.SetActive(false);
                return;
            }

            currentPreview.transform.position = hit.point + hit.normal * offset;
            currentPreview.transform.rotation = Quaternion.LookRotation(hit.normal);

            if (IsValidSpawn(currentPreview))
            {
                if (!currentPreview.activeSelf)
                    currentPreview.SetActive(true);
            }
            else
            {
                currentPreview.SetActive(false);
            }
        }
        else
        {
            currentPreview.SetActive(false);
        }
    }

    public void HandleScroll(InputAction.CallbackContext context, string activeTag)
    {
        if (!showingPreview || currentPreview == null || currentTag != activeTag) return;

        Vector2 scrollValue = context.ReadValue<Vector2>();

        if (Mathf.Abs(scrollValue.y) > scrollSens)
        {
            currentScale += scrollValue.y * 0.1f;
            currentScale = Mathf.Clamp(currentScale, minScale, maxScale);
            currentPreview.transform.localScale = Vector3.one * currentScale;
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
