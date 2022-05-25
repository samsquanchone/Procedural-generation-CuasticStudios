using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Interactable
{
    public Item resourceItem;
    public ResourceType resourceType = new ResourceType();
    public float harvestTime = 10f;
    public bool isHarvesting = false;

    public override void Interact()
    {
        if (!Inventory.instance.isFull)
            StartCoroutine(harvestEnumerator(harvestTime));
    }
    IEnumerator harvestEnumerator(float Time) // This is harvest methods, so player interacts plays animation and collects resource with delay
    {
        if (isHarvesting)
            yield break;

        
        isHarvesting = true;
        animator.SetBool("harvesting", true);
        animator.speed = 2f;
        yield return new WaitForSeconds(harvestTime);

        animator.SetBool("harvesting", false);
        animator.speed = 1f;
        pickUp();

        isHarvesting = false;
    }


    void pickUp() 
    {
        Debug.Log("Picked up " + resourceItem.name);
        Inventory.instance.Add(resourceItem);
        Destroy(gameObject);
        pickedUp = true;
    }
}

public enum ResourceType
{
    Wood,
    Ore
}

