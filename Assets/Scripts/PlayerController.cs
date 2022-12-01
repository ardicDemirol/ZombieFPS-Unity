using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Control Settings")]
    [SerializeField] private float walkSpeed = 8f;
    [SerializeField] private float runSpeed = 12f;

    private CharacterController characterController;
    private float currentSpeed = 8f;
    private float horizontalInput;
    private float verticalInput;



    private void Awake()
    {
        characterController = GetComponent<CharacterController>();  
    }

    void Start()
    {
        
    }

    void Update()
    {
        KeyboardInput();
    }

    private void FixedUpdate()
    {
        Vector3 localVerticalVector = transform.forward * verticalInput;
        Vector3 localHorizontalVector = transform.right * horizontalInput;

        Vector3 movementVector = new Vector3(horizontalInput,0f,verticalInput);
        movementVector.Normalize();
        movementVector *= currentSpeed * Time.deltaTime;

        characterController.Move(movementVector * currentSpeed *Time.deltaTime);
    }

    private void KeyboardInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }
}
