using UnityEngine;

namespace RPG.Core
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float cameraDistance = 5;
        [SerializeField] float cameraAngle = 45;
        [SerializeField] int cameraSpeed = 45;

        private float startTime;

        void LateUpdate()
        {
            // Camera postion 
            Vector3 newCameraPosition = target.position;

            float playerHieght = 1.8f;
            newCameraPosition.z -= cameraDistance;
            newCameraPosition.y += cameraDistance + playerHieght;

            // Camera roation
            transform.localRotation = Quaternion.Euler(new Vector3(cameraAngle, 0, 0));

            // Applying the positon and *roation  -- *No ratoation to apply as of yet


            if(transform.position != newCameraPosition)
            {
                transform.position = Vector3.Lerp(transform.position, newCameraPosition, cameraSpeed * Time.deltaTime);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                cameraDistance += 1;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                cameraDistance -= 1;
            }
        }
    }
}
