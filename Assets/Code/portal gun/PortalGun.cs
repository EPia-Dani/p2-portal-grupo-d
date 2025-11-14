using UnityEngine;
using UnityEngine.InputSystem;

public class PortalGun : MonoBehaviour
{
    [SerializeField] private GameObject orangePortal;
    [SerializeField] private GameObject bluePortal;
    [SerializeField] private GameObject previewPortal;
    [SerializeField] private GameObject bluePortalPreview;
    [SerializeField] private GameObject orangePortalPreview;

    [SerializeField] private float maxShootDistance = 100f;
    [SerializeField] private float offset = 0.0001f;

    private Camera playerCamera;

    private PortalPreview previewHandler;
    private PortalShooter shooterHandler;

    private string tagOrange = "OrangePortal";
    private string tagBlue = "BluePortal";

    private void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera")?.GetComponent<Camera>();
        if (playerCamera == null)
            Debug.LogWarning("PortalGun: camera not set");

        previewHandler = new PortalPreview(playerCamera, maxShootDistance, offset);
        shooterHandler = new PortalShooter(maxShootDistance, offset, previewPortal,bluePortal,orangePortal);

        previewHandler.SetPreviewPrefabs(bluePortalPreview, orangePortalPreview);
    }

    private void Update()
    {
        previewHandler?.UpdatePreviewPosition();
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        HandleInput(context,PortalType.Blue);
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        HandleInput(context, PortalType.Orange);
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        previewHandler?.HandleScroll(context, tagBlue);
    }

    private void HandleInput(InputAction.CallbackContext context, PortalType type)
    {
        if (context.started)
        {
            previewHandler.StartPreview(type);
        }
        else if (context.performed)
        {
            previewHandler.UpdatePreviewPosition();
        }
        else if (context.canceled)
        {
            float finalScale = previewHandler.CurrentScale;
            previewHandler.EndPreview();
            shooterHandler.HandleShoot(playerCamera, type, finalScale);
        }
    }
}
