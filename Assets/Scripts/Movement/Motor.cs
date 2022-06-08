using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Motor : MonoBehaviour, IAction
    {
        NavMeshAgent navMeshAgent;


        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity; // Getting the nav mesh agent velocity as direction
            Vector3 locaVelcoity = transform.InverseTransformDirection(velocity); // Take from global and turn to local (READ MORE ON UNITY DOCS)      
            float speed = locaVelcoity.z; // Getting the velocity of the forward axis
            GetComponent<Animator>().SetFloat("forwardSpeed", speed); // Applying the same velcoity on z axis to the animator blend tree parameter
        }
    }
}
