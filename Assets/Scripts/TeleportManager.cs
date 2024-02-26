using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public Transform initialPositionObject;

    public void TeleportObjectToInitialPosition(GameObject objToTeleport)
    {
        objToTeleport.SetActive(true);
        SpriteRenderer rend = objToTeleport.GetComponent<SpriteRenderer>();
        
        if (initialPositionObject != null)
        {
            objToTeleport.transform.SetPositionAndRotation(initialPositionObject.position, initialPositionObject.rotation);
        }
        else
        {
            Debug.LogError("Initial position object is not assigned!");
        }
    }
}
