using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cameraTransform;
    public float speed = 2.0f;
    public float rotationSpeed = 20.0f;
    public Transform groundCheckerTransform;
    public LayerMask notPlayerMask;
    public float jumpForce = 2.0f;
    [Space]
    [Header("Stay Capsule Collider")]
    public Vector3 centerStayCollider;
    public float radiusStayCollider;
    public float heightStayCollider;
    [Space]
    [Header("Crouch Capsule Collider")]
    public Vector3 centerCrouchCollider;
    public float radiusCrouchCollider;
    public float heightCrouchCollider;

    private Animator animator;
    new private Rigidbody rigidbody;
    private Vector3 direction;
    private float crouchSpeedKef = 1.0f;
    private CapsuleCollider capsuleCollider;
    private bool isGrounded = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        SetCapsuleCollider("Stay");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            CurrentSettings.UpdateControlMod();
        }

        Locomotion();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && FindGroundRaycast(0.1f))
        {
            SetCapsuleCollider("Crouch");
            crouchSpeedKef = 0.4f;
            animator.SetBool("Crouch", true);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            SetCapsuleCollider("Stay");
            crouchSpeedKef = 1.0f;
            animator.SetBool("Crouch", false);
        }

        if (Physics.CheckSphere(groundCheckerTransform.position, 0.17f, notPlayerMask))
        {
            animator.SetBool("IsInAir", false);
            isGrounded = true;
        }
        else
        {
            animator.SetBool("IsInAir", true);
            isGrounded = true;
        }
    }

    private void SetCapsuleCollider(string type)
    {
        switch (type)
        {
            case "Stay":
                capsuleCollider.center = centerStayCollider;
                capsuleCollider.radius = radiusStayCollider;
                capsuleCollider.height = heightStayCollider;
                break;

            case "Crouch":
                capsuleCollider.center = centerCrouchCollider;
                capsuleCollider.radius = radiusCrouchCollider;
                capsuleCollider.height = heightCrouchCollider;
                break;

            default:
                capsuleCollider.center = centerStayCollider;
                capsuleCollider.radius = radiusStayCollider;
                capsuleCollider.height = heightStayCollider;
                break;
        }
    }

    private void Locomotion()
    {
        CalculateRotation(CurrentSettings.controlMod);

        direction = Vector3.ClampMagnitude(transform.rotation * (new Vector3(0, 0, Input.GetAxis("Vertical"))), 1);

        //rigidbody.velocity = new Vector3(direction.x * speed, rigidbody.velocity.y, direction.z * speed);
        float resultSpeed = speed * crouchSpeedKef;
        rigidbody.velocity = new Vector3(direction.x * resultSpeed, rigidbody.velocity.y, direction.z * resultSpeed);
        rigidbody.angularVelocity = Vector3.zero;
        animator.SetFloat("Speed", (Input.GetAxis("Vertical")<0?-1:1) * direction.magnitude);
    }

    private void CalculateRotation(int controlMod)
    {
        switch (controlMod)
        {
            case 0:
                transform.rotation = transform.rotation * Quaternion.AngleAxis(Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed, Vector3.up);
                break;

            case 1:
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, cameraTransform.eulerAngles.y, transform.eulerAngles.z);
                break;

            default:
                transform.rotation = transform.rotation * Quaternion.AngleAxis(Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed, Vector3.up);
                break;
        }
    }

    private void Jump()
    {
        if (!animator.GetBool("Crouch") && isGrounded)
        {
            animator.SetTrigger("Jumping");
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool FindGroundRaycast(float dist) => (Physics.Raycast(groundCheckerTransform.position, Vector3.down, dist, notPlayerMask));
}
