using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // to make sure theres only one instance of the inventory
    #region Singleton
    public static Inventory instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of inventory found");
            return;
        }
        instance = this;
    }
    #endregion  

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int inventorySize = 20;
    public bool isFull = false;
    public List<Item> items = new List<Item>();

    private void Update()
    {
        if (items.Count == inventorySize)
        {
            isFull = true;
            Debug.Log("Inventory Full");
        }
        else if (items.Count > inventorySize)
        {
            isFull = false;
        }
    }

    public bool Add(Item item)
    {

        if(!item.isDefaultItem)
        {
            if(items.Count >= inventorySize) // Check inventory size
            {
                Debug.Log("Not enough space");
                return false; ;
            }
            
            items.Add(item);
            if (onItemChangedCallback != null) // for UI stuff
                onItemChangedCallback.Invoke();

        }

        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null) // For UI stuff
        {
            onItemChangedCallback.Invoke();
        }
    }
}
