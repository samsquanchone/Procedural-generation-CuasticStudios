using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInput : MonoBehaviour
{

public Button[] worldSizeButtons;


    // Start is called before the first frame update
    void Awake()
    {
      for(int i = 0; i < worldSizeButtons.Length; i++){ //Iterate over button array sending array                                                  index to WorldType method
        int value = i;
        worldSizeButtons[i].onClick.AddListener(delegate {WorldType(value); });
       
       
        }
    }

    
    void WorldType(int worldType)
    {
   
       if (worldType == 0)
       {
      Debug.Log(0);
       }
       
       else if (worldType == 1)
       {
      Debug.Log(1);
       }
       
       else
       {
      Debug.Log(2);
       }
      
    }
    
}
