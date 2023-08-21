using UnityEngine;

public class SidNarratorController : MonoBehaviour
{
    /// ******* Old Tree memory prompt *******
    public AudioClip oldTreeMemoryPrompt;
    bool oldTreeMemoryPromptRan = false;
    
    
        /// ******* Old Tree memory prompt *******
    public AudioClip kittySpottedPrompt;
    bool kittySpottedPromptRan = false;
    
    
    /// ******* Dog prompt *******
    public AudioClip dogBonePrompt;
    bool dogeBonePromptRan = false;
    
    
    /// ******* Lawn mower prompt *******
    public AudioClip lawnMowerPrompt;
    bool lawnMowerPromptRan = false;
    
    
    /// ******* Tree of plenty prompt *******
    public AudioClip treeOfPlentyPrompt;
    bool treeOfPlentyPromptRan = false;
    
    
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        PlayCommentary();
    }
    
    public void ResetNarrations()
    {
        oldTreeMemoryPromptRan = false;
        kittySpottedPromptRan = false;
        dogeBonePromptRan = false;
        lawnMowerPromptRan = false;
        treeOfPlentyPromptRan = false;
    }
    
    private void PlayCommentary()
    {
        if (!oldTreeMemoryPromptRan && transform.position.z > 14f)
        {
            audioSource.clip = oldTreeMemoryPrompt;
            audioSource.Play();
            oldTreeMemoryPromptRan = true;
        }
        
        if (!kittySpottedPromptRan && transform.position.x < 13f)
        {
            audioSource.clip = kittySpottedPrompt;
            audioSource.Play();
            kittySpottedPromptRan = true;
        }
        
        if (!dogeBonePromptRan && transform.position.x < -45f)
        {
            audioSource.clip = dogBonePrompt;
            audioSource.Play();
            dogeBonePromptRan = true;
        }
        
        if (!lawnMowerPromptRan && transform.position.x < -160f)
        {
            audioSource.clip = lawnMowerPrompt;
            audioSource.Play();
            lawnMowerPromptRan = true;
        }
        
        if (!treeOfPlentyPromptRan && transform.position.x < -233f)
        {
            audioSource.clip = treeOfPlentyPrompt;
            audioSource.Play();
            treeOfPlentyPromptRan = true;
        }
    }
}
