using UnityEngine;

namespace RPG.TurnManager 
{
    [CreateAssetMenu(menuName="RPG Game/Phases/States Tick")]
    public class StateTickPhase : Phase {
        public override bool IsComplete(SessionManager sm, Turn turn)
        {
            turn.player.stateManager.Tick(sm, turn);
            return false;
        }

        public override void OnStartPhase(SessionManager sm, Turn turn)
        {

        }
    }
}