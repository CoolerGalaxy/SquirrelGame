using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;

public class SquirrelAudioController : MonoBehaviour
{
    public AudioClip[] grassWalkSound;
    public AudioClip[] grassRunSound;
    public AudioClip[] grassLanding;

    public AudioClip[] woodWalkSound;
    public AudioClip[] woodRunSound;
    public AudioClip[] woodLanding;

    public AudioClip[] metalWalkSound;
    public AudioClip[] metalRunSound;
    public AudioClip[] metalLanding;

    public GameObject[] woodObjects;
    public GameObject[] metalObjects;

    private AudioSource audioSource;
    private Rigidbody rb;
    private GameObject currentStanding;
    private GameObject lastLandingObject;

    private float audioCooldown = 0.1f;
    private float lastWalkAudio;
    private float speed;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        lastWalkAudio = Time.time;
    }

    private void Update()
    {
        Vector3 velocityVector = rb.velocity;
        speed = velocityVector.magnitude;
        float rayLength = 0.5f;
        //Debug.Log("speed: " + speed);

        RaycastHit hit; // what am I standing on?
        if (Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), -Vector3.up, out hit, rayLength))
        {
            if (hit.collider.gameObject != currentStanding)
            {
                currentStanding = hit.collider.gameObject;
                LandingAudio();
                //Debug.Log("on top of: " + currentStanding.name);
            }
        }
        else if (currentStanding != null)
        {
            currentStanding = null;
            //Debug.Log("not on object");
        }
    }

    private void PlayAudio()
    {
        bool shouldPlay = Time.time - lastWalkAudio >= audioCooldown; //this is a lockout to prevent multiple overlapping events from being played

        if (shouldPlay && speed >= 1)
        {
            lastWalkAudio = Time.time;
            audioSource.clip = FindAudio(running: true);
            audioSource.Play();
        }
        else if (shouldPlay && speed < 1)
        {
            lastWalkAudio = Time.time;
            audioSource.clip = FindAudio(running: false);
            audioSource.Play();
        }
    }

    private AudioClip FindAudio(bool running)
    {
        if (currentStanding == null)
        {
            return null;
        }
        else if (Array.Exists(woodObjects, e => e == currentStanding))
        {
            if (running)
            {
                return SelectRandomSound(woodRunSound);
            }
            else
            {
                return SelectRandomSound(woodWalkSound);
            }
        }
        else if (Array.Exists(metalObjects, e => e == currentStanding))
        {
            if (running)
            {
                return SelectRandomSound(metalRunSound);
            }
            else
            {
                return SelectRandomSound(metalWalkSound);
            }
        }
        else
        {
            if (running)
            {
                return SelectRandomSound(grassRunSound);
            }
            else
            {
                return SelectRandomSound(grassWalkSound);
            }
        }
    }

    private AudioClip SelectRandomSound(AudioClip[] arr) 
    {
        return arr[UnityEngine.Random.Range(0, arr.Length)];
    }

    private void LandingAudio()
    {
        if (Array.Exists(woodObjects, e => e == currentStanding))
        {
            audioSource.clip = SelectRandomSound(woodLanding);
            audioSource.Play();
        }
        else if (Array.Exists(metalObjects, e => e == currentStanding))
        {
            // this is a special check to handle complex geometry on top of the AC unit
            if (!((currentStanding.name == "Circle.032" && lastLandingObject.name == "Side Yard AC Unit") ||
                (currentStanding.name == "Side Yard AC Unit" && lastLandingObject.name == "Circle.032")))
            {
                audioSource.clip = SelectRandomSound(metalLanding);
                audioSource.Play();
            }
        }
        else
        {
            audioSource.clip = SelectRandomSound(grassLanding);
            audioSource.Play();
        }
        lastLandingObject = currentStanding;
    }

    public void stepEvent()
    {
        PlayAudio();
    }
}