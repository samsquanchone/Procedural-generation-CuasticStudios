using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : CharacterStats
{
    // Start is called before the first frame update
    void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
    }

    public override void Die()
    {
        base.Die();
        SceneManager.LoadScene("TestRealm");
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if(newItem != null)
        {
            armour.AddModifer(newItem.armourModifer);
            damage.AddModifer(newItem.damageModifer); 
        }

        if(oldItem != null)
        {
            armour.RemoveModifer(oldItem.armourModifer);
            damage.RemoveModifer(oldItem.damageModifer);
        }
    }
}
