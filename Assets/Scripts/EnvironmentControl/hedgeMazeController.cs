using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hedgeMazeController : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject hedgeCamera;
    public GameObject player;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {         
            if (player.transform.position.x <= -200) // In case player enters from other side of the maze
            {
                hedgeCamera.transform.rotation = Quaternion.Euler(90, 90, 0);
            }
            else
            {
                hedgeCamera.transform.rotation = Quaternion.Euler(90, 90, 180);
            }
            mainCamera.transform.position = hedgeCamera.transform.position;
            mainCamera.transform.rotation = hedgeCamera.transform.rotation;   
            mainCamera.SetActive(false);
            hedgeCamera.SetActive(true);
        }
        else if (other.CompareTag("ground"))
        {

        }
        else {            
            hedgeCamera.SetActive(false);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            float playerPositionX = player.transform.position.x;
            Vector3 newCameraPosition = new Vector3(playerPositionX, hedgeCamera.transform.position.y, hedgeCamera.transform.position.z);
            hedgeCamera.transform.position = newCameraPosition;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {     
            hedgeCamera.SetActive(false);
            mainCamera.SetActive(true);
        }
    }
}
