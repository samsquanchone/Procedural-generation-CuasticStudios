using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStats : CharacterStats
{
    public override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }
}
