using UnityEngine;

public class LawnMowerController : MonoBehaviour
{
    Animator animator;
    
    public AudioClip engineRunning;
    
    public AudioClip engineShutdown;
    
    public AudioClip engineStartup;
    
    private AudioSource audioSource;
    
    GameObject sid;
    
    private EngineState engineState = EngineState.Off;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        sid = GameObject.Find("Sid");
    }
    
    void StopMotion()
    {
        animator.ResetTrigger("forward");
        animator.ResetTrigger("left90");
        animator.ResetTrigger("reverse");
        animator.ResetTrigger("right90");
        
        animator.SetTrigger("stop");
    }

    void Update() { 
        if (!audioSource.isPlaying && engineState == EngineState.Starting)
        {
            transitionToEngineRunning();
            return;
        }
        
        if (!audioSource.isPlaying && engineState == EngineState.ShuttingDown)
        {
            StopMotion();
            transitionToEngineOff();
            return;
        }
        
        if (engineState == EngineState.Off)
        {
            return;
        }
        
        
        if (engineState == EngineState.Running)
        {
            var localTarget = transform.InverseTransformPoint(sid.transform.position);
            var targetRad = Mathf.Atan2(localTarget.x, localTarget.z);
            var targetDeg = targetRad * Mathf.Rad2Deg;
            
            if (targetDeg < -10.0f)
            {
                StopMotion();
                animator.SetTrigger("left90"); 
            }
            else if (targetDeg > 10.0f)
            {
                StopMotion();
                animator.SetTrigger("right90");
            }
            else
            {
                StopMotion();
                animator.SetTrigger("forward");
            }
        }
    }
    
    public void reset()
    {
        GameObject spawnPoint = GameObject.Find("LawnMowerSpawnPoint");
        transform.position = spawnPoint.transform.position;
        transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        audioSource.Stop();
        engineState = EngineState.Off;
    }
    
    public void shutdown()
    {
        if (engineState != EngineState.Running)
        {
            return;
        }
        engineState = EngineState.ShuttingDown;
        audioSource.clip = engineShutdown;
        audioSource.loop = false;
        audioSource.Play();
    }
    
    public void startup()
    {
        if (engineState != EngineState.Off)
        {
            return;
        }
        engineState = EngineState.Starting;
        audioSource.clip = engineStartup;
        audioSource.loop = false;
        audioSource.Play();
    }
    
    private void transitionToEngineRunning()
    {
        engineState = EngineState.Running;
        audioSource.clip = engineRunning;
        audioSource.loop = true;
        audioSource.Play();
    }
    
    private void transitionToEngineOff()
    {
        engineState = EngineState.Off;
        audioSource.Stop();
    }
}
