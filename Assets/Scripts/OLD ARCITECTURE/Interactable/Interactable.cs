using UnityEngine;

public class Interactable : InteractableBase
{       
    
    GameObject playerObj;    
    [HideInInspector]
    public Animator animator;    
    public bool pickedUp = false;

    public override void onFocused(GameObject playerObj)
    {
        base.onFocused(playerObj);        
        animator = playerObj.GetComponent<Animator>();
    }
    
    public override void Interact()
    {}
}
