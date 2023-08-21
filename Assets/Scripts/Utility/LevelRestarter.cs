using GameProperties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class LevelRestarter : MonoBehaviour
{
    public GameObject sid;
    public GameObject kitty;
    public GameObject soccerBall;
    public GameObject sprinklerCan;
    public GameObject bone;
    public GameStateTracker gameStateTracker;
    public KittyAI kittyAI;
    public GameObject yardOneAcornParent;
    public AcornCollector acornCollector;
    public AudioSource audioSource;
    public AudioClip gameMusic;
    public LawnMowerController lawnMowerController;
    public SidNarratorController sidNarratorController;

    public void LoadCheckpoint()
    {
        sidNarratorController.ResetNarrations();
        switch (gameStateTracker.currentLevel)
        {
            case Levels.sideYard:
                SceneManager.LoadScene("MAIN_SCENE");
                break;
            case Levels.yardOne:
                Animator kittyAnim = kitty.GetComponent<Animator>();
                NavMeshAgent kittyAgent = kitty.GetComponent<NavMeshAgent>();

                sid.transform.SetPositionAndRotation(new Vector3(18.5f, 0f, 10f), new Quaternion(0f, 0f, 0f, 0f));

                // reset kitty to default state
                kitty.transform.position = new Vector3(-8.4f, 4.25f, 9.53f);
                kitty.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                kittyAnim.SetFloat("velocity", 0);
                kittyAnim.SetBool("chasing", false);
                kittyAgent.ResetPath();
                kittyAI.StartSleeping();

                // reset missing acorns
                acornCollector.yardOneCollectedAcorns = 0;

                GameObject[] acorns = new GameObject[yardOneAcornParent.transform.childCount];

                for (int i = 0; i < yardOneAcornParent.transform.childCount; i++)
                {
                    acorns[i] = yardOneAcornParent.transform.GetChild(i).gameObject;
                }

                foreach (GameObject acorn in acorns)
                {
                    if (!acorn.activeSelf)
                    {
                        acorn.SetActive(true);
                    }
                }

                // reset soccer ball
                soccerBall.GetComponent<Rigidbody>().isKinematic = true; // this destroys any velocity and/or momentum
                soccerBall.transform.position = new Vector3(12.55f, .5f, 25.2f);
                soccerBall.GetComponent<Rigidbody>().isKinematic = false;

                // reset sprinkler can
                sprinklerCan.transform.position = new Vector3(14.37f, 0f, 40.47f);
                sprinklerCan.transform.rotation = Quaternion.Euler(0f, -218.837f, 0f);

                // TODO: any additional interactive yard 1 items should be added here

                break;
            case Levels.yardTwo:
                sid.transform.SetPositionAndRotation(new Vector3(-33.53f, 0f, 24.34f), new Quaternion(0f, 0.73351f, 0f, -0.67968f));

                bone.GetComponent<Rigidbody>().isKinematic = true;
                bone.transform.position = new Vector3(-54.7f,4.49f, 32.49f);
                break;
                
            case Levels.yardThree:
                sid.transform.SetPositionAndRotation(new Vector3(-152.8204f, 0f, 30.16009f), new Quaternion(0f, 0.73351f, 0f, -0.67968f));
                lawnMowerController.reset();
                break;
        }

        // reset audio
        audioSource.Stop();
        audioSource.clip = gameMusic;
        audioSource.Play();
    }
}
