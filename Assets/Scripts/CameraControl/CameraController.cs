using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float smoothSpeed = 0.125f;

    public float minZoom = 3f;
    public float maxZoom = 20f;
    public float zoomSpeed = 1f;
    public float cameraDistance = 6f;

    private float cameraX = 0f;
    private float cameraY = 0f;
    public float cameraSpeedX = 5f;
    public float cameraSpeedY = 10f;

    public bool canMove = true;

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        cameraX = angles.y;
        cameraY = angles.x;
    }

    private void LateUpdate()
    {   
        if (canMove) {
            Transform playerTarget = player.transform;
            float camMinHeight = 0.5f;
            float camStandoffDistance = 0.1f;

            if (playerTarget)
            {
                cameraDistance = Mathf.Clamp(cameraDistance - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, minZoom, maxZoom);

                cameraX += Input.GetAxis("Mouse X") * cameraSpeedX * cameraDistance * smoothSpeed;
                cameraY -= Input.GetAxis("Mouse Y") * cameraSpeedY * smoothSpeed;

                cameraX += Input.GetAxis("VerticalAim") * cameraSpeedX * cameraDistance * smoothSpeed;
                cameraY -= Input.GetAxis("HorizontalAim") * cameraSpeedY * smoothSpeed;

                cameraY = Mathf.Clamp(cameraY, 5, 90); // clamps rotation to not go below the character

                Quaternion rotation = Quaternion.Euler(cameraY, cameraX, 0);

                Vector3 offset = rotation * new Vector3(0.0f, 0.0f, -cameraDistance); 
                Vector3 position = playerTarget.position + offset; // decomposed original position code to use offset below

                RaycastHit hit;
                if (Physics.SphereCast(playerTarget.position, 0.2f, offset.normalized, out hit, cameraDistance)) // spherecast needed, raycast not course enough to detect intersect
                {
                    transform.position = hit.point + hit.normal * camStandoffDistance; // normal vector calculation for camera position override

                    if (transform.position.y < camMinHeight) // prevents camera height from going below minimum
                    {
                        transform.position = new Vector3(transform.position.x, camMinHeight, transform.position.z);
                    }
                }
                else
                {
                    transform.position = position; // no obstacles
                }

                transform.rotation = rotation;
            }
        }
    }
}