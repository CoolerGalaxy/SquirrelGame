using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;
    public float audioTransitionSpeed;
    private bool isTransitioning = false;

    public bool IsTransitioning
    {
        get { return isTransitioning; }
    }

    public void ChangeMusic()
    {
        StartCoroutine(FadeOutMusic());
    }

    private IEnumerator FadeOutMusic()
    {
        isTransitioning = true;
        while (audioSource.volume > 0f)
        {
            audioSource.volume -= Time.deltaTime / audioTransitionSpeed;
            yield return null;
        }
        audioSource.Stop();
        isTransitioning = false;
    }
}
