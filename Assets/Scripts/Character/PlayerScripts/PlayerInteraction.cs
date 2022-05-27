using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCam;
    public InteractableBase focus;
    public bool isInteracting = false;
    float interactTime;
    Animator animator;    
    public SkillManager skillManager;

    // For focus bar ui
    public delegate void OnFocusChanged();
    public OnFocusChanged onFocusChanged;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        if (onFocusChanged != null)
               onFocusChanged.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        SetInteractTime();


        LeftMouseClick();
        InteractButtonPress(focus);

        if (Input.GetButtonDown("Fire2"))
        {
            RemoveFocus();
        }
    }

    void SetInteractTime()
    {
        if (focus != null && focus.GetComponent<Resource>() != null)
        {
            interactTime = focus.GetComponent<Resource>().harvestTime;
        }
        if (focus == null || focus.GetComponent<Resource>() == null)
        {
            interactTime = 0;
        }
    }

    public void LeftMouseClick()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                InteractableBase interactable = hit.collider.GetComponent<InteractableBase>();
                if (interactable != null)
                {
                    //Debug.Log("Clicked on " + interactable.nameOfObject);                    
                    SetFocus(hit.collider.GetComponent<InteractableBase>());
                }
            }
        }
    }

    void InteractButtonPress(InteractableBase interactable)
    {
        var interactButton = Input.GetKeyDown(KeyCode.Space);

        if (interactButton && focus == null)
        {
            Debug.Log("Nothings focused");
            return;
        }
        if (interactButton && focus != null)
        {
            if (focus.isFocus && !focus.hasInteracted) // Check theres nothing focused and hasnt interacted
            {
                float distance = Vector3.Distance(gameObject.transform.position, focus.interactionTransform.position); // Getting distance from player to interactable
                if (distance <= focus.radius) // compare distance to interactable range (radius)
                {
                    // Can interact so call the objects interact
                    focus.hasInteracted = true; 
                    
                    if (focus.GetComponent<Resource>() != null) // Interaction specific to resource
                    {
                        Resource resource = focus.GetComponent<Resource>(); // Gets resource component
                        if (resource.CheckIfCanHarvest()) // Checks if resource can be harvested
                        {
                            InteractWithResource(resource);                            
                        }
                        return;
                    }

                    if(focus.GetComponent<NpcInteractable>() != null) // Interaction specific to NPC's
                    {
                        focus.GetComponent<NpcInteractable>().TakeDamage(10);
                        focus.hasInteracted = false;
                        return;
                    }

                    if(focus.GetComponent<Interactable>() != null) // Interaction specific to generic interactable 
                    {
                        focus.Interact();
                        focus = null;
                    }

                    return;
                }
                else Debug.Log("Out of range");
            }
        }
    }

    private void InteractWithResource(Resource resource)
    {     

        switch (resource.resourceType)
        {
            case ResourceType.Wood:
                {
                    // CHECK WOOD CUTTING SKILL
                    if (resource.levelToHarvest > skillManager.woodCutting.level)
                    {
                        Debug.Log("Level " + resource.levelToHarvest + " is needed");
                        return;
                    }
                    if (resource.levelToHarvest <= skillManager.woodCutting.level)
                    {
                        resource.Interact();
                        StartCoroutine(StopMovement(interactTime, resource));                   
                        focus = null;
                    }
                    break;
                }
            case ResourceType.Ore:
                {
                    // CHECK MINING SKILL
                    break;
                }
        } 

    }

    void SetFocus(InteractableBase newFocus)
    {
        if (onFocusChanged != null)
            onFocusChanged.Invoke(); // Change focus bar ui here
        if (newFocus != focus)
        {
            if (focus != null)
            {
                focus.onDeFocused();
            }
            focus = newFocus;
        }
        newFocus.onFocused(gameObject);
    }

    void RemoveFocus()
    {
        if (onFocusChanged != null)
            onFocusChanged.Invoke(); // Change focus bar ui here
        if (focus != null)
        {
            focus.onDeFocused();
        }
        focus = null;
    }

    IEnumerator StopMovement(float time ,Resource resource)
    {
        //Debug.Log("Started to interact");
        isInteracting = true;
        yield return new WaitForSeconds(time);
        //Debug.Log("Finished to interact");
        skillManager.woodCutting.AddExp(resource.expWorth);
        Debug.Log("Wood cutting exp: " + skillManager.woodCutting.GetCurrentExp() + "/" + skillManager.woodCutting.GetExpToNextLevel());
        isInteracting = false;
    }
}
