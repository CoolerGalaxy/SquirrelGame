using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class PlayerMovementController : MonoBehaviour
{
    PlayerInputActions playerInputActions;
    public Vector2 movementInput;
    public Transform cameraTransform;
    private Rigidbody playerRigidBody;
    public NPCDialogController npcDialogController;

    public float smoothedSpeed = 0f;
    public float jogSpeed = 2f;
    public float sprintSpeed = 4f;
    public float jumpForce = 1f;
    private Vector2 speedVector;
    private float smoothedAngle;

    public float playerAcceleration;

    public float playerRotationSmoothing = 0.1f;
    public float playerAccelerationSmoothing = 0.2f;

    private bool groundContact;
    // private bool fallingStatus;

    float turnSmoothSpeed;
    public float animatorSpeed;
    Animator playerAnimator;

    public float throwForce = 10f;  // Define the force with which the player throws the ball
    private GameObject pickedObject = null;
    public GameObject playerHand;
    public GameObject throwingTutorialText;
    private TextMeshProUGUI throwingButtonText;

    /// <summary>
    /// A downwards Raycast positioned at Sid's feet. Helps detect objects below Sid.
    /// </summary>
    public GameObject downwardRaycast;

    /// <summary>
    /// A forward Raycast positioned at Sid's feet. Helps detect objects in front of Sid such as a step in a staircase.
    /// </summary>
    public GameObject forwardRaycastLower;

    /// <summary>
    /// A forward Raycast positioned at Sid's waist. Helps detect objects in front of Sid such as a step in a staircase.
    /// </summary>
    public GameObject forwardRaycastMid;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        playerInputActions.Actions.Jump.performed += x => Jump();
        playerInputActions.Actions.Throw.performed += x => Throw();
        throwingButtonText = throwingTutorialText.GetComponent<TextMeshProUGUI>();

    }

    void Update()
    {
        movePlayer();
        CheckForAndClimbStep();
    }

    private void Jump()
    {
        if (groundContact && IsGroundBelow() && npcDialogController.jumpEnabled)
        {
            
            playerRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerAnimator.SetBool("Jumping", true);
        }
    }

    private void movePlayer()
    {
        movementInput = playerInputActions.Movement.Movement.ReadValue<Vector2>();
        float horizontal = movementInput.x;
        float vertical = movementInput.y;
        Vector3 playerDirection = new Vector3(horizontal, 0f, vertical).normalized;

        moveRelativeToCamera(playerDirection);
    }

    void moveRelativeToCamera(Vector3 playerDirection)
    {
        float targetAngle;
        float smoothedAngle;
        if (playerInputActions.Movement.Movement.IsPressed())
        {
            /* We only want to redirect the character relative to the camera IF the movement controls are pressed.
             If we update direction even when the movement buttons are not pressed, the character will spin around to face forwards
            as soon as the player lets go of the movement input. */
            targetAngle = Mathf.Atan2(playerDirection.x, playerDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothSpeed, playerRotationSmoothing);
            transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);
        }
        else
        {
            smoothedAngle = 0f;
        }

        smoothSpeed();

        if (IsGroundBelow())
        {
            playerAnimator.SetFloat("AnimatorSpeed", smoothedSpeed);
        }
        else
        {
            // Allow the player to move while mid-air by bypassing root motion
            Vector3 movementDirection = Quaternion.Euler(0f, smoothedAngle, 0f) * Vector3.forward;
            playerRigidBody.AddForce(movementDirection * smoothedSpeed * 25);
        }
    }

    private void smoothSpeed()
    {
        smoothedSpeed = Mathf.SmoothDamp(smoothedSpeed, targetMoveSpeed(), ref playerAcceleration, playerAccelerationSmoothing);
    }

    private float targetMoveSpeed()
    {
        if (playerInputActions.Actions.Sprint.IsPressed() && playerInputActions.Movement.Movement.IsPressed())
        {
            return sprintSpeed;
        }
        else if (playerInputActions.Movement.Movement.IsPressed())
        {
            speedVector = new Vector2(Mathf.Abs(movementInput.x), Mathf.Abs(movementInput.y));

            return speedVector.magnitude * jogSpeed;
        }
        else
        {
            return 0f;
        }
    }

    bool IsGroundBelow()
    {
        float rayLength = 1.4f;

        Ray ray = new Ray(transform.position + new Vector3(0, 1, 0), Vector3.down);
        int layerMask = LayerMask.GetMask("Default"); //needed to prevent ray collision with Sid
        RaycastHit[] hits = Physics.RaycastAll(ray, rayLength, layerMask);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.CompareTag("ground"))
            {
                return true;
            }
        }
        return false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            playerAnimator.SetBool("Grounded", true);
            playerAnimator.SetBool("Falling", false);
            groundContact = true;
        }
        if (collision.gameObject.CompareTag("Throwable"))
        {
            throwingTutorialText.SetActive(true);
            string buttonToThrow = GetGamepadType();
            throwingButtonText.text = "Press " + buttonToThrow + " to throw!";
            pickedObject = collision.gameObject;
            pickedObject.GetComponent<Rigidbody>().isKinematic = true;
            pickedObject.transform.SetParent(playerHand.transform);

            Collider pickedObjectCollider = pickedObject.GetComponent<Collider>();

            if (pickedObjectCollider != null)
            {
                pickedObjectCollider.enabled = false;
            }

            AcornController objectRotationScript = collision.gameObject.GetComponent<AcornController>();

            if (objectRotationScript != null)
            {
                // Do something with the script
                objectRotationScript.enabled = false;
            }

            pickedObject.transform.localPosition = Vector3.zero; 
            
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            RaycastHit hitGround;
            if (!Physics.Raycast(downwardRaycast.transform.position, transform.TransformDirection(Vector3.down), out hitGround, 0.15f))
            {
                if (playerRigidBody.velocity.y > 1.5f)
                {
                    playerAnimator.SetBool("Grounded", false);
                    playerAnimator.SetBool("Falling", true);
                }
                groundContact = false;
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            playerAnimator.SetBool("Jumping", false);
            playerAnimator.SetBool("Falling", false);
            playerAnimator.SetBool("Grounded", true);
            groundContact = true;
        }
    }

    /// <summary>
    /// Checks Sid's lower and mid Raycast hits. If there's a lower Raycast hit but not an upper Raycast hit, Sid is likely
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
                playerRigidBody.position -= new Vector3(0f, -0.06f, 0f);
            }
        }
    }

    private void Throw()
    {
        if (pickedObject != null)
        {
            playerAnimator.Play("Throwing");
            Collider pickedObjectCollider = pickedObject.GetComponent<Collider>();

            pickedObject.GetComponent<Rigidbody>().isKinematic = false;
            pickedObject.transform.parent = null;
            Vector3 throwDirection = (transform.forward * throwForce) + (transform.up * (throwForce/2));
            pickedObject.GetComponent<Rigidbody>().AddForce(throwDirection, ForceMode.VelocityChange);

            Vector3 colliderSize = GetComponent<Collider>().bounds.size;
            float maxColliderDim = Mathf.Max(colliderSize.x, colliderSize.y, colliderSize.z);

            // After throwing the object, start a coroutine to re-enable the collider after a distance from player
            StartCoroutine(EnableColliderAfterDistance(pickedObjectCollider, maxColliderDim)); 

            pickedObject = null;
            throwingTutorialText.SetActive(false);
        }
    }

    private IEnumerator EnableColliderAfterDistance(Collider collider, float distance)
    {
        Vector3 playerPosition = transform.position;
        
        while (Vector3.Distance(playerPosition, collider.transform.position) < distance)
        {
            yield return null; 
        }

        if (collider != null)
        {
            collider.enabled = true; 
        }
    }

    private string GetGamepadType()
    {
        string buttonText = "E";
        
        Gamepad gamepadObject = Gamepad.current;

        if (gamepadObject is not null)
        {
            if (Gamepad.current.name.Contains("X"))
            {
                buttonText = "Y";
            }
            else
            {
                buttonText = "Triangle";
            }
        }

        return buttonText;
    }
}
