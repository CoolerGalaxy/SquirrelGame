using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The different activities kitty can perform.
/// </summary>
enum KittyActivity
{
    Chasing,
    Investigating,
    Sleeping
}

public class KittyAI : MonoBehaviour
{
    /// <summary>
    /// Kitty's current activity.
    /// </summary>
    KittyActivity currentActivity;

    /// <summary>
    /// Kitty has a set of destinations he travels between when investigating. This keeps track of the index of his current destination.
    /// </summary>
    int currentDestinationIndex;
    
    /// <summary>
    /// Kitty's nav mesh agent.
    /// </summary>
    NavMeshAgent agent;
    
    /// <summary>
    /// Kitty's animator.
    /// </summary>
    Animator animator;
    
    /// <summary>
    /// The array of destinations Kitty will walk to while investigating.
    /// </summary>
    public GameObject[] destinations;
    
    /// <summary>
    /// Kitty's Rigidbody
    /// </summary>
    private Rigidbody rb;
    
    /// <summary>
    /// A simple reference to Sid.
    /// </summary>
    GameObject sid;
    
    /// <summary>
    /// The maximum amount of distance to consider that Kitty "arrived" at his destination.
    /// </summary>
    const float stoppingDistance = 0.5f;

    /// <summary>
    /// The timestamp that Sid last fell into Kitty's line of sight.
    /// </summary>
    float timeSidWasLastSeen;
    
    /// <summary>
    /// A forward Raycast positioned at Kitty's feet. Helps detect objects in front of Kitty such as a step in a staircase.
    /// </summary>
    public GameObject forwardRaycastLower;
    
    /// <summary>
    /// A forward Raycast positioned at Kitty's waist. Helps detect objects in front of Kitty such as a step in a staircase.
    /// </summary>
    public GameObject forwardRaycastMid;
    
    /// <summary>
    /// Picks a random destination and assign it as the nav mesh agent's goal.
    /// </summary>
    void AssignNextDestination()
    {
        var nextDestinationIndex = currentDestinationIndex + 1;
        if(currentDestinationIndex + 1 == destinations.Length) {
            currentDestinationIndex = 0;
        } else {
            currentDestinationIndex++;
        }
        agent.destination = destinations[currentDestinationIndex].transform.position; 
    }
    
    /// <summary>
    /// Performs a Raycast to see if Sid is within Kitty's line of sight.
    /// </summary>
    bool CanSeeSid()
    {
        RaycastHit hit;
        var rayDirection = transform.TransformDirection(Vector3.forward);
        
        if (Physics.Raycast(transform.position, rayDirection, out hit, Mathf.Infinity)) 
        {
            if (hit.transform == sid.transform) {
                timeSidWasLastSeen = Time.time;
                return true;
            } else {
                return false;
            }
        }
        return false;
    }
    
    /// <summary>
    /// Routine for when Kitty is chasing Sid.
    /// </summary>
    void Chase()
    {
        float currentTime = Time.time;
        float timeSinceSidWasSeen = currentTime - timeSidWasLastSeen;
        
        if ((timeSinceSidWasSeen >= 20.0) && !CanSeeSid()) {
            StopChasing();
        } else if (timeSidWasLastSeen >= 5 && DistanceToSid() <= 8.0f) {
            agent.SetDestination(sid.transform.position);
        }
    }
    
    /// <summary>
    /// Checks Kitty's lower and mid Raycast hits. If there's a lower Raycast hit but not an upper Raycast hit, Kitty is likely
    /// at an object he should step up like a step in a staircase.
    /// Credits to DawnsCrow Games for this algorithm.
    /// </summary>
    void CheckForAndClimbStep()
    {
        RaycastHit hitLower;
        if (Physics.Raycast(forwardRaycastLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.2f))
        {
            RaycastHit hitMid;
            if (!Physics.Raycast(forwardRaycastMid.transform.position, transform.TransformDirection(Vector3.forward), out hitMid, 0.3f))
            {
                rb.position -= new Vector3(0f, -0.2f, 0f);
            }
        }
    }
    
    /// <summary>
    /// Kitty's distance to Sid
    /// </summary>
    float DistanceToSid()
    {
        return Vector2.Distance(transform.position, sid.transform.position);
    }
    
    /// <summary>
    /// Routine for when Kitty is investigating the yard.
    /// </summary>
    void Investigate()
    {
        if (agent.remainingDistance <= stoppingDistance)
        {
            AssignNextDestination();
        }
        if (CanSeeSid() || DistanceToSid() <= 3.0f) {
            StartChasing();
        }
        
        animator.SetFloat("velocity", agent.velocity.magnitude / agent.speed);
    }
    
    /// <summary>
    /// Initialize Kitty's nav mesh agent and assign the first destination.
    /// </summary>
    void Start()
    {
        // Attach vital components
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        sid = GameObject.Find("Sid");
        rb = GetComponent<Rigidbody>();
        
        // Set initial state
        StartSleeping();
    }
    
    /// <summary>
    /// Starts a chase.
    /// </summary>
    void StartChasing()
    {
        animator.SetBool("chasing", true);
        currentActivity = KittyActivity.Chasing;
        agent.SetDestination(sid.transform.position);
    }
    
    /// <summary>
    /// Starts an investigation of the yard.
    /// </summary>
    void StartInvestigating()
    {
        currentActivity = KittyActivity.Investigating;
        currentDestinationIndex = -1;
        AssignNextDestination();
    }
    
    /// <summary>
    /// Puts kitty to sleep.
    /// </summary>
    public void StartSleeping()
    {
        animator.SetBool("sleeping", true);
        currentActivity = KittyActivity.Sleeping;
    }
    
    /// <summary>
    /// Kitty's work to perform while sleeping. He simply checks if Sid is near, if he is, it's time to wake up.
    /// </summary>
    void Sleep()
    {
        if (DistanceToSid() <= 12.0f)
        {
            StopSleeping();
        }
    }
    
    /// <summary>
    /// Stops a chase and returns kitty to go back to investigating.
    /// </summary>
    void StopChasing()
    {
        animator.SetBool("chasing", false);
        currentActivity = KittyActivity.Investigating;
    }
    
    /// <summary>
    /// Awakes kitty from his slumber.
    /// </summary>
    void StopSleeping()
    {
        animator.SetBool("sleeping", false);
        StartInvestigating();
    }
    
    /// <summary>
    /// Check if Kitty has arrived and assign his next destination if he has.
    /// </summary>
    void Update()
    {
        CheckForAndClimbStep();
        
        switch (currentActivity)
        {
            case KittyActivity.Chasing:
                Chase();
                break;
            case KittyActivity.Investigating:
                Investigate();
                break;
            case KittyActivity.Sleeping:
                Sleep();
                break;
            default:
                break;
        }
    }
}
