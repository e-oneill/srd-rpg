using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.TurnManager;
using RPG.Explore.Movement;
using RPG.Core;
using RPG.Characters;
using Cinemachine;

namespace RPG.Explore
{
public class PlayerController : MonoBehaviour
{
    public bool isTurnCombat;
    public SessionManager sessionManager;
    public ActionScheduler actionScheduler;
    public GridCharacter character;
    public IAction mover;
    // Start is called before the first frame update
    void Start()
    {
        sessionManager = FindObjectOfType<SessionManager>();
        // Debug.Log(sessionManager);
        isTurnCombat = sessionManager.turnCombat;
        actionScheduler = GetComponent<ActionScheduler>();
        character = GetComponent<GridCharacter>();
        // Debug.Log(isTurnCombat);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetMouseButton(0) && !isTurnCombat)
        {
            MoveToCursor();
            // int roll = randomnumbergenerator.DiceRoll(12);
            // int abimod = character.abilitiesArray[1,1];
            // int profbonus = 0;
            // int rollResult = roll+abimod+profbonus;
            // Debug.Log(character.name + " rolled a dexterity check. Result: " + rollResult + " ( " + roll + " + " + abimod + " )");
           
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isTurnCombat)
        {
            actionScheduler.StopAction();
        }  
    }

    public void MoveToCursor()
    {
            int i = 0;
            // CinemachineVirtualCamera followCam = FindObjectOfType<CinemachineVirtualCamera>();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // Debug.Log("Drawing Ray");
            Debug.DrawRay(ray.origin, ray.direction * 1000);
            bool hasHit = Physics.Raycast(ray, out hit);
            if (hasHit && !isTurnCombat)
            {
                Mover[] movers = FindObjectsOfType<Mover>();
                actionScheduler.StartAction(mover);
                
                foreach (Mover mover in movers)
                {
                    Vector3 target = hit.point;
                    if (i == 1)
                    {
                        target.x += 2;
                    }
                    if (i == 2)
                    {
                        target.z += 2;
                    }
                    if (i == 3)
                    {
                        target.x += 2;
                        target.z += 2;
                    }
                    i++;
                    GridCharacter gridCharacter = mover.GetComponentInParent<GridCharacter>();
                    // Debug.Log("Issuing Move to command to " + gridCharacter + ". \n Target: " + target.x + "," + target.z + "\n i =" + i);
                    mover.MoveTo(target);
                }
                // GetComponentInParent<RPG.Grid.GridCharacter>();
                // IAction mover = GetComponent<Mover>();
                // actionScheduler.StartAction(mover);
                // GetComponent<Mover>().MoveTo(hit.point);
            }
    }
}
}
