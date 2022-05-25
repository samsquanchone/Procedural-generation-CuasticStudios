using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;
    private float moveSpeed;
    public float sprintSpeed = 3f;
    public float walkSpeed = 1f;
    public float sneakSpeed = 0.5f;
    public Camera playerCam;
    float gravity;
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public float distanceFromGround;

    

    public void Start()
    {
        controller = GetComponent<CharacterController>();
        distanceFromGround = GetComponent<Collider>().bounds.extents.y;
        moveSpeed = walkSpeed;
    }
    public void Update()
    {
        if (!GetComponent<PlayerInteraction>().isInteracting)
        { 
            PlayerMove();
        }
        PlayerIdle();      

    }


    void PlayerIdle()
    {
        if(Input.GetAxisRaw("Horizontal") < 0.1f && Input.GetAxisRaw("Vertical") < 0.1f)
        {
            if (Input.GetAxisRaw("Horizontal") > -0.1f && Input.GetAxisRaw("Vertical") > -0.1f)
            {
                animator.SetBool("isWalking", false);
            }
        }
    }

    void PlayerMove()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && animator.GetBool("isWalking"))
        {
            moveSpeed = sprintSpeed;
            animator.speed = sprintSpeed - 0.5f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = walkSpeed;
            animator.speed = walkSpeed;
        }
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(horizontal, 0, vertical);
        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        if (dir.magnitude != 0)
        {
            animator.SetBool("isWalking", true);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            controller.Move(dir * moveSpeed * Time.deltaTime);
        }
        
        gravity -= (float)9.81 * Time.deltaTime;
        dir.y = gravity;
        controller.Move(dir * Time.deltaTime);
        if (controller.isGrounded) gravity = 0;

    }
}
