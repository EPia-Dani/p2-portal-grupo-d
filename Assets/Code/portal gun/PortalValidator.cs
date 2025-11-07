using UnityEngine;

public class PortalValidator : MonoBehaviour
{
    public Transform[] validPoints;         
    public float maxDistance = 0.5f;        
    public float maxAngle = 10f;        
    public Camera playerCamera;   

    public bool IsValidPosition(Vector3 portalPosition, Quaternion portalRotation)
    {
        foreach (Transform validPoint in validPoints)
        {
            Vector3 worldPoint = portalPosition + portalRotation * validPoint.localPosition;
            Vector3 direction = worldPoint - playerCamera.transform.position;

            Ray ray = new Ray(playerCamera.transform.position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, direction.magnitude + maxDistance))
            {


                float distance = Vector3.Distance(hit.point, worldPoint);
                float angle = Vector3.Angle(hit.normal, validPoint.forward);
                if ((!hit.collider.CompareTag("pared pintable"))||distance > maxDistance||angle > maxAngle)
                {
                    return false;
                }

                
            }
            else
            {
                return false;
            }
        }

        return true;
    }
}
