using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cameraTransform;
    [Space]
    [Header("Checkers")]
    public Transform groundCheckerTransform;
    public Transform ceilingCheckerTransform;
    public Transform interactiveCheckerTransform;
    public LayerMask notPlayerMask;
    public LayerMask interactiveMask;
    [Space]
    [Header("Movement settings")]
    public float speed = 2.0f;
    public float rotationSpeed = 20.0f;
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
    private bool isTryingCrouchUp = false;
    private bool isCrouching = false;
    private bool isCanMove = true;
    private bool isMoving = false;

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
        isMoving = rigidbody.velocity.magnitude>0.1f || rigidbody.velocity.magnitude < -0.1f;

        if (Input.GetKeyDown(KeyCode.L))
        {
            CurrentSettings.UpdateControlMod();
        }

        Locomotion();

        if (isCanMove && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.E) && isGrounded && !isCrouching && !isMoving)
        {
            isCanMove = false;
            animator.SetTrigger("Click");
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            SetCapsuleCollider("Crouch");
            crouchSpeedKef = 0.4f;
            isCrouching = true;
            animator.SetBool("Crouch", true);
        }

        if (isCrouching && (isTryingCrouchUp || Input.GetKeyUp(KeyCode.LeftShift)))
        {
            if (!FindCeilingCheckCapsule(radiusStayCollider))
            {
                isTryingCrouchUp = false;
                SetCapsuleCollider("Stay");
                isCrouching = false;
                crouchSpeedKef = 1.0f;
                animator.SetBool("Crouch", false);
            }
            else
            {
                isTryingCrouchUp = true;
            }
        }

        if (FindGroundCheckSphere(capsuleCollider.radius))
        {
            animator.SetBool("IsInAir", false);
            isGrounded = true;
        }
        else
        {
            animator.SetBool("IsInAir", true);
            isGrounded = false;
        }
    }

    private void SetCapsuleCollider(string type)
    {
        try
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
        finally
        {
            UpdateCeilingCheckerPosition(new Vector3(0, capsuleCollider.height - capsuleCollider.radius, 0));
        }
    }

    private void Locomotion()
    {
        CalculateRotation(CurrentSettings.controlMod);

        direction = Vector3.ClampMagnitude(transform.rotation * (new Vector3(0, 0, Input.GetAxis("Vertical"))), 1);

        float resultSpeed = speed * crouchSpeedKef;
        if (isGrounded)
        {
            rigidbody.velocity = isCanMove ? new Vector3(direction.x * resultSpeed, rigidbody.velocity.y, direction.z * resultSpeed) : Vector3.zero;
            animator.SetFloat("Speed", (isCanMove ? 1 : 0) * (Input.GetAxis("Vertical") < 0 ? -1 : 1) * direction.magnitude);
        }
        rigidbody.angularVelocity = new Vector3();
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

    private bool FindGroundRaycast(float dist) => Physics.Raycast(groundCheckerTransform.position, Vector3.down, dist, notPlayerMask);
    private bool FindGroundCheckSphere(float radius) => Physics.CheckSphere(groundCheckerTransform.position - new Vector3(0, 0.1f, 0), radius - 0.01f, notPlayerMask);

    private bool FindCeilingRaycast(float dist) => Physics.Raycast(ceilingCheckerTransform.position, Vector3.up, dist, notPlayerMask);
    private bool FindCeilingCheckSphere(float radius) => Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y + heightStayCollider - radiusStayCollider , transform.position.z), 
                                                                            radius, notPlayerMask);
    private bool FindCeilingCheckCapsule(float radius) => Physics.CheckCapsule(new Vector3(transform.position.x, transform.position.y + radiusStayCollider + 0.1f, transform.position.z),
                                                                        new Vector3(transform.position.x, transform.position.y + heightStayCollider - radiusStayCollider, transform.position.z),
                                                                        radius, notPlayerMask);
    private void UpdateCeilingCheckerPosition(Vector3 newPos) => ceilingCheckerTransform.localPosition = newPos;



    public void TryInteraction()
    {
        Collider[] colliders = Physics.OverlapSphere(interactiveCheckerTransform.position, 0.12f, interactiveMask);

        if (colliders.Length > 0)
        {
            colliders[0].GetComponent<Item>().Interaction();
        }
    }

    public void UpdateCanMove() => isCanMove = !isCanMove;
}
