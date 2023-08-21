using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AcornController : MonoBehaviour
{
    public float tiltAngle = 45;
    public float bounceIntensity;
    public float bounceFrequency;
    private float currentX;
    private float currentY;
    private float currentZ;

    // Start is called before the first frame update
    void Start()
    {
        this.currentX = this.transform.position.x;
        this.currentY = this.transform.position.y;
        this.currentZ = this.transform.position.z;
        bounceIntensity = 3f;
        bounceFrequency = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        float tilt = tiltAngle * Mathf.Sin(Time.time * Mathf.PI);
        transform.Rotate(new Vector3(0, 45, tilt) * Time.deltaTime);

        transform.position = new Vector3(
            this.currentX,
            this.currentY - Mathf.Cos(Time.time * bounceFrequency) * (bounceIntensity / 100),
            this.currentZ);
    }
}
