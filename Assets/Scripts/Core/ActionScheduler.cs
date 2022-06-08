using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;

        public void StartAction(IAction action)
        {
            if (currentAction == action) return;
            if (currentAction != null)
            {
                // All IActions must have a Cancel() method 

                print("Canceling action");
                currentAction.Cancel();

            }

            currentAction = action;
        }
    }
}
