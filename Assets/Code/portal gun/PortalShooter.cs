using UnityEngine;

public class PortalShooter
{
    private readonly float maxShootDistance;
    private readonly float offset;
    private readonly GameObject preview;

    private GameObject bluePortal;
    private GameObject orangePortal;


    private static readonly Vector3 HiddenPos = new Vector3(-200, -2000, -2000);

    public PortalShooter(float maxShootDistance, float offset, GameObject previewPortal, GameObject bluePortal, GameObject orangePortal)
    {
        this.maxShootDistance = maxShootDistance;
        this.offset = offset;
        this.preview = previewPortal;
        this.bluePortal = bluePortal;
        this.orangePortal = orangePortal;
    }

    public void HandleShoot(Camera cam, PortalType type, float scale)
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (!hit.collider.CompareTag("validWall")) return;

            preview.transform.position = hit.point;
            preview.transform.rotation = Quaternion.LookRotation(hit.normal);
            if (type == PortalType.Blue)
            {
                preview.transform.localScale = Vector3.one * scale;
            }
            bool isValid = IsValidSpawn(preview, cam);
            preview.transform.position = HiddenPos;

            if (!isValid) return;


            //move position
            if (type == PortalType.Blue)
            {
                Debug.Log("trying to teleport");
                Debug.Log(bluePortal.transform.position + "  " +  bluePortal.gameObject);
                bluePortal.transform.position = hit.point+hit.normal*offset;
                bluePortal.transform.rotation = Quaternion.LookRotation(hit.normal);
                bluePortal.GetComponent<Portal>().setWall(hit.collider.gameObject);
                bluePortal.GetComponent<Portal>().setScale(scale);
                PortalEvents.RaiseOrangePortalActivated();
                if (orangePortal.transform.position != HiddenPos)
                {
                    resetPortals();//update new cameras 
                }


            }
            else
            {
                orangePortal.transform.position = hit.point+hit.normal*offset;
                orangePortal.transform.rotation = Quaternion.LookRotation(hit.normal);
                orangePortal.GetComponent<Portal>().setWall(hit.collider.gameObject);
                PortalEvents.RaiseBluePortalActivated();
                if (bluePortal.transform.position != HiddenPos)
                {
                    resetPortals();//update new cameras 
                }
            }
        }
    }
    private void resetPortals()
    {
        orangePortal.GetComponent<Portal>().setOtherPortal(bluePortal);
        bluePortal.GetComponent<Portal>().setOtherPortal(orangePortal);

    }
    private bool IsValidSpawn(GameObject previewInstance,Camera cam)
    {
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
                    Debug.Log("location invalid");
                    return false;
                }
                if (firstHitCollider == null)
                {
                    Debug.Log("location invalid");
                    firstHitCollider = hit.collider; 
                }
                else if (hit.collider != firstHitCollider)
                {
                    Debug.Log("location invalid");

                    return false;
                }
            }
            else{
                Debug.Log("location invalid");

                return false;
                    }
        }
        Debug.Log("Valid location");
        return true;
    }
}
