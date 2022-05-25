using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New equipment", menuName = "Item/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipSlot;
    public int defMultiplier;
    public int attkMultiplier;

    public SkinnedMeshRenderer mesh;

    public override void Use()
    {
        base.Use();

        EquipmentManager.instance.Equip(this);
        RemoveFromInventory();
    }

}

public enum EquipmentSlot {  Head, Chest, Legs, Weapons, Shield, Feet }
