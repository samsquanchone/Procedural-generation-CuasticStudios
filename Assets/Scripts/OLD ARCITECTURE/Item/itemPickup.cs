using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemPickup : Interactable
{
    public Item item;


    public override void Interact()
    {
        //base.interact();

        PickUp();      
        
    }

    void PickUp()
    {        
        Debug.Log("Picked up " + item.name);
        Inventory.instance.Add(item);
        Destroy(gameObject);
        pickedUp = true;
    }
}
