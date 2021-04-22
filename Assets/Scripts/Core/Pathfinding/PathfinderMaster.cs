using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using RPG.Characters;

namespace RPG.Grid 
{
    public class PathfinderMaster : MonoBehaviour
    {
        public static PathfinderMaster singleton;
        List<Pathfinder> currentJobs = new List<Pathfinder>();
        List<Pathfinder> toDoJobs = new List<Pathfinder>();
        public int MaxJobs = 3;
        public float timerThreshold = 5;

        private void Awake()
        {
            singleton = this;
        }

        private void Update()
        {
            int i = 0;
            float delta = Time.deltaTime;
            while (i < currentJobs.Count)
            {
                if(currentJobs[i].jobDone)
                {
                    currentJobs[i].NotifyComplete();
                    currentJobs.RemoveAt(i);
                }
                else 
                {
                    currentJobs[i].timer += delta;
                    if (currentJobs[i].timer > timerThreshold)
                    {
                        currentJobs[i].jobDone = true;
                    }
                    i++; 
                }
            }

            if (toDoJobs.Count > 0 && currentJobs.Count < MaxJobs)
            {
                Pathfinder job = toDoJobs[0];
                toDoJobs.RemoveAt(0);
                currentJobs.Add(job);
                Thread jobThread = new Thread(job.FindPath);
                jobThread.Start();

            }
        }

        public void RequestPathfind(GridCharacter character, Node start, Node target, Pathfinder.PathfindingComplete callback, GridManager gridManager)
        {
            // Debug.Log("Request Pathfind function called");
            Pathfinder newJob = new Pathfinder(character, start, target, callback, gridManager);
            toDoJobs.Add(newJob);
        }
    }
}
