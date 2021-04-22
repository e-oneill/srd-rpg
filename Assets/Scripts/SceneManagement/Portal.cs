using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Saves;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            a,
            b
        }

        [SerializeField] int SceneToLoad = -1;
        [SerializeField] Transform spawnpoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeinTime = 1f;
        [SerializeField] float fadewaitTime = .5f;

        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player")
            {
            StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {    
            if (SceneToLoad < 0)
            {
                Debug.LogError("Scene to load not yet assigned.");
                yield break;
            } 
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            
            yield return fader.FadeOut(fadeOutTime);
            savingWrapper.SaveGame("Checkpoint");
            yield return SceneManager.LoadSceneAsync(SceneToLoad);
            savingWrapper.LoadGame("Checkpoint");
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            savingWrapper.SaveGame("Checkpoint");
            yield return new WaitForSeconds(fadewaitTime);
            yield return fader.FadeIn(fadeinTime);

            Destroy(gameObject);
        }

        public void UpdatePlayer(Portal otherPortal)
        {
            GameObject[] playerParty = GameObject.FindGameObjectsWithTag("Player");
            // Debug.Log(playerParty);
            int i = 0;
            foreach (GameObject character in playerParty)
                {
                    NavMeshAgent navMeshAgent = character.GetComponent<NavMeshAgent>();
                    navMeshAgent.enabled = false;
                    Vector3 target = otherPortal.spawnpoint.position;
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
                    // Debug.Log("Moving Character to " + gridCharacter + ". \n Target: " + target.x + "," + target.z + "\n i =" + i);
                    character.transform.position = target;
                    character.transform.rotation = otherPortal.spawnpoint.rotation;

                    navMeshAgent.enabled = true;

                }

        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal==this) continue;
                if (portal.destination == this.destination)
                {
                    return portal;
                }

                // Debug.Log(portal);
                return null;
            }

            return null;
        }
    }
}