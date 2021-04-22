using UnityEngine;

namespace RPG.TurnManager 
{
    [CreateAssetMenu(menuName="RPG Game/Phases/Idle Phase")]

    public class IdlePhase : Phase 
    {
        public override bool IsComplete(SessionManager sm, Turn turn) 
        {
            return false;
        }

        public override void OnStartPhase(SessionManager sm, Turn turn)
        {
            if (isInit) return;
            isInit = true;
            Debug.Log("Starting Idle Phase");
        }

        public override void OnEndPhase(SessionManager sm, Turn turn)
        {
            
        }
    }
}