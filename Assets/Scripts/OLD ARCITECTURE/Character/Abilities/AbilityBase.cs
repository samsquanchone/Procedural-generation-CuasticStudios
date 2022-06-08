using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBase : ScriptableObject
{
    public string abilityName;
    public int damage;
    public AbilityType abilityType;

    public virtual void Ability()
    {
        // Do ability sutff here, instaniate a projectile, indlifct stats positives/negatives etc
    }
}

public enum AbilityType
{
    melee,
    ranged,
    support,
    buff,
    nerf
}
