using UnityEngine;

public class SnowFollowCamera : MonoBehaviour
{
    public Transform cameraTransform;

    void LateUpdate()
    {
        transform.position = new Vector3(
            cameraTransform.position.x,
            cameraTransform.position.y,
            transform.position.z
        );
    }
}

