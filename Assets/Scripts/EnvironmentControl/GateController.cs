using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public AudioSource gateAudioSource;
    public Animator anim;

    public void OpenGate()
    {
        anim.SetBool("GateOpen", true);
        gateAudioSource.Play();
    }

}
