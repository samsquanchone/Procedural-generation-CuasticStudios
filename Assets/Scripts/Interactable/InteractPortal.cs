using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class InteractPortal : Interactable
{
    public override void Interact()
    {
     base.Interact();
       Debug.Log("Interacted with" + nameOfObject);
       SceneManager.LoadScene("DungeonTest", LoadSceneMode.Additive);
    }
}
