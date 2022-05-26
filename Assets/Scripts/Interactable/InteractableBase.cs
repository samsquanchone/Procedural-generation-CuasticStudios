using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBase : MonoBehaviour
{
    public float radius = 3f;
    public Transform interactionTransform;
    public string nameOfObject = "Something";
    public bool isFocus = false;
    Transform player;
    [HideInInspector]
    public bool hasInteracted = false;

    public virtual void Interact()
    {
        // This is meant ot be overiiden
        Debug.Log("Interacted with " + nameOfObject);
    }

    public virtual void onFocused(GameObject playerObj)
    {
        isFocus = true;
        player = playerObj.transform;
        hasInteracted = false;
    }

    public virtual void onDeFocused()
    {
        isFocus = false;
        player = null;
        hasInteracted = false;
    }

    private void OnDrawGizmosSelected()
    {
        // to display interactable range in editor
        if (interactionTransform == null)
        {
            interactionTransform = transform;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}
