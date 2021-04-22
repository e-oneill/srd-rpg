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
            currentAction = action;
            if (currentAction != null)
            {
                currentAction.Cancel();
            }
        }

        public void StopAction()
        {
            if (currentAction != null)
            {
                currentAction.Cancel();
                currentAction = null;
            }
        }
    }
}
