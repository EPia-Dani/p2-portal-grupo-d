using UnityEngine;

public class PortalShooter
{
    private readonly float maxShootDistance;
    private readonly float offset;
    private readonly GameObject previewPortal;

    public PortalShooter(float maxShootDistance, float offset, GameObject previewPortal)
    {
        this.maxShootDistance = maxShootDistance;
        this.offset = offset;
        this.previewPortal = previewPortal;
    }

    public void HandleShoot(Camera cam, GameObject portalPrefab, string tag, float scale)
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (!hit.collider.CompareTag("validWall")) return;

            GameObject preview = Object.Instantiate(previewPortal, hit.point, Quaternion.LookRotation(-hit.normal));
            preview.transform.localScale = Vector3.one * scale;
            bool isValid = IsValidSpawn(preview, cam);
            Object.Destroy(preview);

            if (!isValid) return;

            GameObject existingPortal = GameObject.FindGameObjectWithTag(tag);
            if (existingPortal != null)
            {
                existingPortal.SetActive(false);
                Object.Destroy(existingPortal);
            }

            Vector3 spawnPos = hit.point + hit.normal * offset;
            GameObject newPortal = Object.Instantiate(portalPrefab, spawnPos, Quaternion.LookRotation(hit.normal));

            newPortal.GetComponent<Portal>().setWall(hit.collider.gameObject);

            if (tag == "OrangePortal")
            {
                GameObject other = GameObject.FindGameObjectWithTag("BluePortal");
                PortalEvents.RaiseOrangePortalActivated();

                if (other != null)
                {
                    Portal otherPortalComponent = other.GetComponent<Portal>();
                    Portal newPortalComponent = newPortal.GetComponent<Portal>();

                    if (otherPortalComponent != null && newPortalComponent != null)
                    {
                        newPortalComponent.setOtherPortal(other);
                        otherPortalComponent.setOtherPortal(newPortal);
                    }
                }
            }
            else if (tag == "BluePortal")
            {
                // Ajustar escala del portal azul
                newPortal.transform.localScale = Vector3.one * scale;
                GameObject other = GameObject.FindGameObjectWithTag("OrangePortal");
                PortalEvents.RaiseBluePortalActivated();

                if (other != null)
                {
                    Portal otherPortalComponent = other.GetComponent<Portal>();
                    Portal newPortalComponent = newPortal.GetComponent<Portal>();

                    if (otherPortalComponent != null && newPortalComponent != null)
                    {
                        newPortalComponent.setOtherPortal(other);
                        otherPortalComponent.setOtherPortal(newPortal);
                        newPortalComponent.setScale(scale);
                    }
                }
            }
        }
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
