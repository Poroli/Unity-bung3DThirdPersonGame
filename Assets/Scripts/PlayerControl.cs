using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static CinemachineFreeLook playerTPCam;
    public float Speed = 6f;
    public float TurnSmoothTime = 0.1f;

    [SerializeField] private GameObject mainCam;
    [SerializeField] private Animator animator;
    private Rigidbody Rb;
    private Vector3 direction;
    private Vector3 moveDir;
    private Vector3 playerVelocity;
    private float turnSmoothVelocity;
    private float horizontalP1;
    private float verticalP1;
    private float angle;
    private float targetAngle;

    private void CalculateMoveDirection()
    {
        targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, TurnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }

    private void Walking()
    {
        moveDir = direction.magnitude * Speed * moveDir.normalized;
        moveDir.y = Rb.velocity.y;
        animator.SetFloat("MoveSpeed", new Vector2(moveDir.x, moveDir.z).magnitude);
        Rb.velocity = moveDir;
    }
    private void Start()
    {
        Rb = GetComponent<Rigidbody>();
        mainCam = GameObject.Find("Main Camera");
        playerTPCam = GetComponentInChildren<CinemachineFreeLook>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (GameLogic.GamePause)
        {
            return;
        }

        horizontalP1 = Input.GetAxisRaw("Horizontal");
        verticalP1 = Input.GetAxisRaw("Vertical");

        direction.x = horizontalP1;
        direction.z = verticalP1;

        if (direction.magnitude != 0)
        {
            CalculateMoveDirection();
            Walking();
        }
        else
        {
            playerVelocity.y = Rb.velocity.y;
            Rb.velocity = playerVelocity;
            animator.SetFloat("MoveSpeed", 0);
        }
    }
}

