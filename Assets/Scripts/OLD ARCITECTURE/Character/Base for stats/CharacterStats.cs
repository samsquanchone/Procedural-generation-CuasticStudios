using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth { get; private set; }

    public Stat damage;
    public Stat armour;
    public float attackRange;
    public float attackSpeed;
    //public Stat attackRange;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // Takes damage, die, etc...
    }

    public void TakeDamage(int damage)
    {
        damage -= armour.GetValue();
        Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;
        Debug.Log(transform.name + " took " + damage + " damage");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // this is meant to be overidden
        // method used for character dying
        Debug.Log(transform.name + " died");
    }
}
