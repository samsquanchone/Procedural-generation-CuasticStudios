using UnityEngine;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        void Update()
        {
            if (InteractCombat()) return;       // Combat interaction method         
            if (InteractMovement()) return;     // Player movement method
            //if (InteractResource()) return;   // Resource interaction method --> This may not be needed unsure yet
                // Make method InteractResource()
                // needs to test if player clicks on resource (raycast same as before)
                // take into account player skill for that type of resource to calculate damage

            //print("Nothing to do");
        }

        private bool InteractCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach(RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if(Input.GetMouseButtonDown(0))
                {
                    transform.LookAt(target.transform);
                    GetComponent<Fighter>().Attack(target);
                }
                return true; // Outside of if statement for affordance, i.e still return true if hovering over
            }
            return false;
        }

        private bool InteractMovement()
        {            
            RaycastHit hit; // a place to store racycast hit info
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit); // sotre raycast hit info, and make bool to test if/what it anything
            if (hasHit) // testing if it hit
            {
                if(Input.GetMouseButton(0))
                {
                    GetComponent<Motor>().StartMoveAction(hit.point); // Move to raycast hit position in world space                  
                }
                return true; // Outside of if statement for affordance, i.e still return true if hovering over
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
