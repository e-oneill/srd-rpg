using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.TurnManager;
using RPG.Core;
using RPG.Characters;
using RPG.Saves;

namespace RPG.Explore.Movement
{
public class Mover : MonoBehaviour, IAction, ISavable
{
    // [SerializeField] Transform Target;
    NavMeshAgent navMeshAgent;
    PlayerController playerController;
    GridCharacter gridCharacter;

    SessionManager sm;
    
    private void Start() 
    {

        navMeshAgent = GetComponent<NavMeshAgent>();
        playerController = GetComponent<PlayerController>();
        gridCharacter = GetComponent<GridCharacter>();
    }

    public void Cancel()
    {
        Stop();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        if (!playerController.isTurnCombat)
        {
            UpdateAnimator();
        }
        // GetComponent<NavMeshAgent>().destination = Target.position;
    }

    public void MoveTo(Vector3 destination)
    {
        
        // Debug.Log(gridCharacter + "moving to " + destination.x + "," + destination.y + "," + destination.z);
        navMeshAgent.isStopped = false;
        // GetComponent<ActionScheduler>().StartAction(this);
        navMeshAgent.destination = destination;
        NavMeshPath nextpath = navMeshAgent.path;
        navMeshAgent.CalculatePath(destination, nextpath);
        float pathdistance = navMeshAgent.remainingDistance;
        // Debug.Log("Distance to destination: " + pathdistance);
    }

    public void Stop()
    {
        navMeshAgent.isStopped = true;
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
    }

    public object CaptureState()
    {
        return new SerializableVector3(transform.position);
    }

    public void RestoreState(object state)
    {
        SerializableVector3 restorePos = (SerializableVector3)state;
        GetComponent<NavMeshAgent>().enabled = false;
        transform.position = restorePos.ToVector();
        GetComponent<NavMeshAgent>().enabled = true;
        // Debug.Log("Restoring State for " + GetUniqueID());
    }
}
}
