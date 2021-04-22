using UnityEngine;
using RPG.TurnManager;
using RPG.Characters;

namespace RPG.Grid 
{
    [CreateAssetMenu(menuName="RPG Game/Phases/Place Units")]

    public class PlaceUnits : Phase 
    {
        public override bool IsComplete(SessionManager sm, Turn turn) 
        {
            return true;
        }

        public override void OnStartPhase(SessionManager sm, Turn turn)
        {
            if (isInit) return;
            isInit = true;
            PlaceUnitsOnGrid(sm);
        }

        public override void OnEndPhase(SessionManager sm, Turn turn)
        {
            
        }

        void PlaceUnitsOnGrid (SessionManager sm)
        {
            GridCharacter[] units = sm.gridObject.GetComponentsInChildren<GridCharacter>();

            foreach (GridCharacter u in units)
            {
                Node n = sm.gridManager.GetNode(u.transform.position);
                if (n != null)
                {
                    u.transform.position = n.worldPosition;
                }
            }
        }
    }
}