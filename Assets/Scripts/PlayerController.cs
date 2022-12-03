using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Control Settings")]
    [SerializeField] private float walkSpeed = 8f;
    [SerializeField] private float runSpeed = 12f;
    [SerializeField] private float gravityModifer = 0.95f;
    [SerializeField] private float jumpPower = 0.25f;
    [SerializeField] private InputAction newMovementInput;
    [Header("Mouse Control Options")]
    [SerializeField] float mouseSensivity = 1f;
    [SerializeField] float maxViewAngle = 60f;
    [SerializeField] bool invertX;
    [SerializeField] bool invertY;
    [Header("SoundSettings")]
    [SerializeField] List<AudioClip> footStepSounds = new List<AudioClip>();
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip landSound;

    private CharacterController characterController;
    private float currentSpeed = 8f;
    private float horizontalInput;
    private float verticalInput;

    private Vector3 heightMovement;
    private bool jump;

    private Transform mainCamera;

    private Animator anim;

    private AudioSource audioSource;

    private int lastIndex = -1;
    private bool landSoundPlayed = true;



    private void Awake()
    {
        characterController = GetComponent<CharacterController>(); 
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if(Camera.main.GetComponent<CameraController>() == null)
        {
            Camera.main.gameObject.AddComponent<CameraController>();
        }
        mainCamera = GameObject.FindWithTag("CameraPoint").transform;
    }



    private void OnEnable()
    {
        newMovementInput.Enable();
    }





    private void OnDisable()
    {
        newMovementInput.Disable();
    }



    void Update()
    {
        KeyboardInput();
        AnimationChanger();
    }

   

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }



    private void Move()
    {
        if (jump)
        {
            heightMovement.y = jumpPower;
            jump = false;
        }

        heightMovement.y -= gravityModifer * Time.deltaTime;


        Vector3 localVerticalVector = transform.forward * verticalInput;
        Vector3 localHorizontalVector = transform.right * horizontalInput;

        Vector3 movementVector = new Vector3(horizontalInput, 0f, verticalInput);
        movementVector.Normalize();
        movementVector *= currentSpeed * Time.deltaTime;

        characterController.Move(movementVector + heightMovement);

        if (characterController.isGrounded)
        {
            heightMovement.y = 0f;
            if (!landSoundPlayed)
            {
                audioSource.PlayOneShot(landSound);
                landSoundPlayed = true;
            }
        }
    }





    private void Rotate()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + MouseInput().x, transform.eulerAngles.z);
        if (mainCamera != null)
        {
            if (mainCamera.eulerAngles.x > maxViewAngle && mainCamera.eulerAngles.x < 180f)
            {
                mainCamera.rotation = Quaternion.Euler(maxViewAngle, mainCamera.eulerAngles.y, mainCamera.eulerAngles.z);
            }
            else if (mainCamera.eulerAngles.x > 180f && mainCamera.eulerAngles.x < 360f - maxViewAngle)
            {
                mainCamera.rotation = Quaternion.Euler(360f - maxViewAngle, mainCamera.eulerAngles.y, mainCamera.eulerAngles.z);
            }
            else
            {
                mainCamera.rotation = Quaternion.Euler(mainCamera.rotation.eulerAngles +
                    new Vector3(-MouseInput().y, 0f, 0f));
            }
        }
    }





    private void AnimationChanger()
    {
        if(newMovementInput.ReadValue<Vector2>().magnitude > 0f)
        {
            if (currentSpeed == walkSpeed)
            {
                anim.SetBool("Walk", true);
                anim.SetBool("Run", false);
            }
            else if(currentSpeed == runSpeed)
            {
                anim.SetBool("Run", true);
                anim.SetBool("Walk", false);
            }
        }
        else
        {
            anim.SetBool("Run", false);
            anim.SetBool("Walk", false);
        }
    }



    private void PlayFootStepSound()
    {

        if(footStepSounds.Count > 0 && audioSource != null)
        {
            int index;
            do
            {
                index = UnityEngine.Random.Range(0, footStepSounds.Count);
                if(lastIndex != index)
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(footStepSounds[index]);
                        lastIndex = index;
                        break;
                    }
                   
                }
            } while (index == lastIndex);
        }
    }





    private void KeyboardInput()
    {
        horizontalInput = newMovementInput.ReadValue<Vector2>().x;
        verticalInput = newMovementInput.ReadValue<Vector2>().y;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && characterController.isGrounded)
        {
            jump = true;
            landSoundPlayed = false;
            audioSource.PlayOneShot(jumpSound);
        }
        if (Keyboard.current.leftShiftKey.isPressed)
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }



    private Vector2 MouseInput()
    {
        return new Vector2(invertX ? -Mouse.current.delta.x.ReadValue() : Mouse.current.delta.x.ReadValue(),
            invertY ? -Mouse.current.delta.y.ReadValue() : Mouse.current.delta.y.ReadValue()) * mouseSensivity;
    }

    
   
}
