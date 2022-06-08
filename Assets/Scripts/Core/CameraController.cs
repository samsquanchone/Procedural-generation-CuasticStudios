using UnityEngine;

namespace RPG.Core
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float cameraDistance = 5;
        [SerializeField] float cameraHeight = 5;
        [SerializeField] float cameraAngle = 45; // To convert to degress;

        void LateUpdate()
        {
            // Camera postion 
            Vector3 cameraPostion = target.position;

            cameraPostion.z -= cameraDistance;
            cameraPostion.y += cameraHeight;

            // Camera roation

            gameObject.transform.localRotation = Quaternion.Euler(new Vector3(cameraAngle, 0, 0));

            // Applying the positon and roation
            gameObject.transform.position = cameraPostion;
            // Also apply camera roation
        }
    }
}
