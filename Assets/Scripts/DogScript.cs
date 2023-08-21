
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogScript : MonoBehaviour
{
    public GameObject bone;
    public Transform fetchTarget;
    public float fetchDistance = 3f;
    
    private UnityEngine.AI.NavMeshAgent agent;
    private Animator animator;
    private bool isAwake = false;
    private bool isFetching = false;
    
    private void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (!isAwake)
        {
            // Check if the player pushes the bone down to the ground
            if (bone != null && bone.transform.position.y < 0.1f)
            {
                isAwake = true;
                StartFetching();
            }
        }
        else if (isFetching)
        {
            // Move the dog to the fetch target if it's not there yet
            float distanceToTarget = Vector3.Distance(transform.position, fetchTarget.position);
            if (distanceToTarget > fetchDistance)
            {
                agent.SetDestination(fetchTarget.position);
                animator.SetBool("IsMoving", true);
            }
            else
            {
                // The dog reached the fetch target, stop moving and start fetching the bone
                isFetching = false;
                animator.SetBool("IsMoving", false);
                FetchBone();
            }
        }
    }
    
    private void StartFetching()
    {
        // Set the dog's destination to the target
        isFetching = true;
        agent.SetDestination(fetchTarget.position);
        animator.SetBool("IsMoving", true);
    }
    
    private void FetchBone()
    {
        animator.SetBool("FoundFood", true); // Perform the eating animation 
    }
}
