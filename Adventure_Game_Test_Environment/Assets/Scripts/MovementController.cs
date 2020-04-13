using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class MovementController : MonoBehaviour
{
    private float inputX, inputZ, speed, gravity;

    private Camera cam;
    private CharacterController characterController;

    private Vector3 desiredMoveDirection;

    [SerializeField] private float rotationSpeed = 0.3f;
    [SerializeField] private float allowRotation = 0.1f;
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float gravityMultiplier = 1f;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        InputDecider();
        MovementManager();
    }

    void InputDecider()
    {
        speed = new Vector2(inputX, inputZ).sqrMagnitude;

        if(speed > allowRotation)
        {
            RotationManager();
        }
        else
        {
            desiredMoveDirection = Vector3.zero;
        }
    }

    void RotationManager()
    {
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * inputZ + right * inputX;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed);
    }

    void MovementManager()
    {
        gravity -= 9.8f * Time.deltaTime;
        gravity = gravity * gravityMultiplier;

        Vector3 moveDirection = desiredMoveDirection * (movementSpeed * Time.deltaTime);

        moveDirection = new Vector3(moveDirection.x, gravity, moveDirection.z);

        characterController.Move(moveDirection);

        if(characterController.isGrounded)
        {
            gravity = 0f;
        }
    }
}
