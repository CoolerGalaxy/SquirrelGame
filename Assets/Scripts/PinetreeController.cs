using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinetreeController : MonoBehaviour
{
    public Rigidbody[] pinecones;
    //public Collider treeCollider;

    private bool isShaking = false;

    private void Start()
    {
        isShaking = false;    }

    private void Update()
    {
        if (isShaking)
        {
            ShakePinecones();
            isShaking = false; // Set isShaking to false after shaking the pinecones
        }
    }

    private void ShakePinecones()
    {
        foreach (Rigidbody pinecone in pinecones)
        {
            pinecone.useGravity = true;
            pinecone.isKinematic = false; // Enable physics simulation for the pinecone
            pinecone.transform.parent = null; // Remove the pinecone from the tree hierarchy
            pinecone.AddForce(Vector3.down * Random.Range(2f, 5f), ForceMode.Impulse); // Apply a random downward force
			StartCoroutine(DisableCollider(pinecone)); // Disable collider after a delay
       

        }
    }


    private IEnumerator DisableCollider(Rigidbody pinecone)
    {
        yield return new WaitForSeconds(1f); // Adjust the delay time as needed

        Collider pineconeCollider = pinecone.GetComponent<Collider>();
        if (pineconeCollider != null)
        {
            pineconeCollider.enabled = false; // Disable the collider for the acorn
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isShaking = true; // Set isShaking to true when the player triggers the collider
        }
    }



}
