using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCam;
    public Interactable focus;
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
        if (focus == null)
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
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    Debug.Log("Clicked on " + interactable.name);
                    SetFocus(hit.collider.GetComponent<Interactable>());
                }
            }
        }
    }

    void InteractButtonPress(Interactable interactable)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (focus.isFocus && !focus.hasInteracted) // Check theres nothing focused and hasnt interacted
            {
                float distance = Vector3.Distance(gameObject.transform.position, focus.interactionTransform.position); // Getting distance from player to interactable
                if (distance <= focus.radius) // compare distance to interactable range (radius)
                {
                    // Can interact so call the objects interact
                    focus.hasInteracted = true;
                    focus.Interact(); 
                    
                    if(focus.GetComponent<Resource>() != null)
                    {
                        StartCoroutine(StopMovement(interactTime));
                    }

                    focus = null;
                }
                else Debug.Log("Out of range");
            }
        }
    }

    void SetFocus(Interactable newFocus)
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
