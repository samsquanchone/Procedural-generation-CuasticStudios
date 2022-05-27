using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Interactable
{
    public Item resourceItem;
    public ResourceType resourceType = new ResourceType();
    public float harvestTime = 1f;
    public bool isHarvesting = false;
    private bool canHarvest = true;
    public MeshRenderer visuals;

    // To be abstraced to the resource scriptable object
    public int levelToHarvest = 1;
    public int expWorth = 1;
    public int respawnTime = 2;

    private void Awake()
    {
    }

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
        //Destroy(gameObject);        
        StartCoroutine(Respawn(respawnTime));
    }

    IEnumerator Respawn(int respawnTime)
    {
        canHarvest = false;
        pickedUp = true;
        visuals.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        visuals.enabled = true; ;
        canHarvest = true;
        pickedUp = false;
    }

    public bool CheckIfCanHarvest()
    {
        return canHarvest;
    }
}

public enum ResourceType
{
    Wood,
    Ore
}

