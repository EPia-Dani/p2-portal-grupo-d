using UnityEngine;
using UnityEngine.UI;

public class HUD_controller : MonoBehaviour
{
    private bool bluePortalOn;
    private bool orangePortalOn;
    [SerializeField] private Image crossair;

    [Header("Images for crossair")]
    [SerializeField] private Sprite crossairOB;
    [SerializeField] private Sprite crossairO;
    [SerializeField] private Sprite crossairB;
    private void OnEnable()
    {
        PortalEvents.OnBluePortalActivated += OnBluePortal;
        PortalEvents.OnOrangePortalActivated += OnOrangePortal;
    }

    private void OnBluePortal()
    {
        bluePortalOn = true;
        UpdateCrossair();
    }

    private void OnOrangePortal()
    {
        orangePortalOn = true;
        UpdateCrossair();
    }
    private void UpdateCrossair()
    {
        if(bluePortalOn)
        {
            if(orangePortalOn)
            {
                crossair.sprite=crossairOB;
                return;
            }
            crossair.sprite=crossairB;
            return;
        }
        if(orangePortalOn)
        {
            crossair.sprite=crossairO;
            return;
        }
    }
}
