using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

public class RandomCameraSwitcher : MonoBehaviour
{
    public List<CinemachineVirtualCamera> cameras;
    public float switchInterval = 5f;

    private float timer;
    private int previousIndex;

    private void Start()
    {
        foreach (var cam in cameras)
        {
            cam.Priority = 0;
        }

        if (cameras.Count > 0)
        {
            previousIndex = Random.Range(0, cameras.Count);
            cameras[previousIndex].Priority = 1;
        }

        timer = switchInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SwitchCamera();
            timer = switchInterval;
        }
    }

    private void SwitchCamera()
    {
        cameras[previousIndex].Priority = 0;

        int newIndex;
        do
        {
            newIndex = Random.Range(0, cameras.Count);
        } while (newIndex == previousIndex);

        cameras[newIndex].Priority = 1;

        previousIndex = newIndex;
    }
}
