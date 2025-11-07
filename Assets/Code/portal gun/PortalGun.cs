using UnityEngine;

public class PortalGun : MonoBehaviour
{

    [SerializeField]
    GameObject OrangePortal;

    [SerializeField]
    GameObject BluePortal;

    Camera playerCamera;

    float shootDistance = 20f;

    public void OnRightClick()
    {
        ShootPortal(OrangePortal);
    }
    public void OnLeftClick()
    {
        ShootPortal(BluePortal);
    }


    void ShootPortal(GameObject portal)
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, shootDistance))
        {
            Vector3 spawnPos = hit.point;
            Quaternion spawnRot = Quaternion.LookRotation(hit.normal); 

            PortalValidator portalValidator = portal.GetComponent<PortalValidator>();
            if (portalValidator.IsValidPosition(spawnPos, spawnRot))
            {
                portal.transform.position = spawnPos;
                portal.transform.rotation = spawnRot;
                portal.gameObject.SetActive(true);
            }
        }
    }
}
