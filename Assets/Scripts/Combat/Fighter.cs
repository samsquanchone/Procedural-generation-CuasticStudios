using UnityEngine;
using RPG.Core;
using RPG.Movement;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float attackRange = 2f; // To be moved to a stats script
        [SerializeField] float attackSpeed = 1f; // To be moved to a stats script
        [SerializeField] float damage = 10f;     // To be moved to a stats script

        Health target;                  // Reference to the target (only pointing the health component to sav eon resources)
        float timeSinceLastAttack = 0;  // Used as a counter for attack speed

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;     // If no target at all bounce out
            if (target.IsDead()) return;    // If target is already dead bounce out
            
            if (!GetIsInRange())            // If target is not in range move closer
            {
                GetComponent<Motor>().MoveTo(target.transform.position);
            }
            else // If target is in range then stop player motor and attack ***Eventually to be a else if for Combat target--> May not be needed if done in player controller
            {
                GetComponent<Motor>().Cancel();
                AttackBehaviour();
            }
            // add else if for resoruce target --> May not be needed if done in player controller
                // Make HarvestBehaviour method
                    // Handles the harvesting similar to attack behvaiour
                    // Triggers harvest animation
                    // uses the Hit() method to take damage
                    // The damage passed through will take into account the players skill (handled in player controller)
        }

        private void AttackBehaviour()
        {
            float currentTime = Time.deltaTime;

            if(timeSinceLastAttack > attackSpeed)
            {
                // This will trigger Hit() event
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0;                
            }
        }
        void Hit() // Animation Event wont find any references
        {
           target.TakeDamage(damage);         
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < attackRange;
        }

        public void Attack(CombatTarget target)
        {
            GetComponent<ActionScheduler>().StartAction(this);            
            this.target = target.GetComponent<Health>();            
        }

        public void Cancel()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            target = null;
        }
                
    }
}
