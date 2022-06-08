using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float maxHealth = 100f;
        float Currenthealth;
        bool isDead = false;

        private void Start()
        {
            Currenthealth = maxHealth;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            Currenthealth = Mathf.Max(Currenthealth - damage, 0);
            if(Currenthealth == 0)
            {
                Die();
            }
        }

        void Die()
        {
            if(!isDead)
            {
                GetComponent<Animator>().SetTrigger("die");
                isDead = true;
            }
        }
    }
}


