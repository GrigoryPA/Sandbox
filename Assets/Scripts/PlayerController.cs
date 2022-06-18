using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 2.0f;
    public float rotationSpeed = 20.0f;
    public Transform groundCheckerTransform;
    public LayerMask notPlayerMask;
    public float jumpForce = 2.0f;

    private Animator animator;
    new private Rigidbody rigidbody;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Locomotion();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Physics.CheckSphere(groundCheckerTransform.position, 0.17f, notPlayerMask))
        {
            animator.SetBool("IsInAir", false);
        }
        else
        {
            animator.SetBool("IsInAir", true);
        }
    }

    private void Locomotion()
    {
        //переделать управление движением так, чтобы W - двигаться вперед, A&D - вращение без движения, S - пятиться назад с другой анимацией.
        direction = Vector3.ClampMagnitude(transform.rotation * (new Vector3(0, 0, Input.GetAxis("Vertical"))), 1);

        transform.rotation = transform.rotation * Quaternion.AngleAxis(Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed, Vector3.up);

        //rigidbody.velocity = new Vector3(direction.x * speed, rigidbody.velocity.y, direction.z * speed);
        rigidbody.velocity = new Vector3(direction.x * speed, rigidbody.velocity.y, direction.z * speed);
        rigidbody.angularVelocity = Vector3.zero;
        animator.SetFloat("Speed", (Input.GetAxis("Vertical")<0?-1:1) * direction.magnitude);
    }

    private void Jump()
    {
        if (Physics.Raycast(groundCheckerTransform.position, Vector3.down, 0.1f, notPlayerMask))
        {
            animator.SetTrigger("Jumping");
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
