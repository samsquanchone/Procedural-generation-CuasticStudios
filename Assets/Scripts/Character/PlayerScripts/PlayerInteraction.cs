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

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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
                    Debug.Log("Clicked on " + interactable.nameOfObject);
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
                    focus.Interact(); 
                    
                    if(focus.GetComponent<Resource>() != null) // Interation specific to Resources
                    {
                        StartCoroutine(StopMovement(interactTime));
                    }


                    if(focus.GetComponent<Interactable>() != null) // Interaction specific to generic interactable 
                    {
                        focus = null;
                    }

                    if(focus.GetComponent<NpcInteractable>() != null) // Interaction specific to NPC's
                    {
                        focus.GetComponent<NpcInteractable>().TakeDamage(10);
                        focus.hasInteracted = false;
                    }

                    return;
                }
                else Debug.Log("Out of range");
            }
        }
    }

    void SetFocus(InteractableBase newFocus)
    {
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
        if (focus != null)
        {
            focus.onDeFocused();
        }
        focus = null;
    }

    IEnumerator StopMovement(float time)
    {
        Debug.Log("Started to interact");
        isInteracting = true;
        yield return new WaitForSeconds(time);
        isInteracting = false;
        Debug.Log("Finished to interact");
    }
}
