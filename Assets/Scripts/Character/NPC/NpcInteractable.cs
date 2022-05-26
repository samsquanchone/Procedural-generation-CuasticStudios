using UnityEngine;

[RequireComponent(typeof(NpcStats))]
public class NpcInteractable : InteractableBase
{
    NpcStats stats;

    private void Awake()
    {
        stats = GetComponent<NpcStats>();
        nameOfObject = "New Base NPC";
    }

    public void TakeDamage(int damage)
    {
        stats.TakeDamage(damage);
    }
}
