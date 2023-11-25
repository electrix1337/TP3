using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SprintComponent))]
public class playerMovement : MonoBehaviour
{
    [SerializeField] float initialSpeed = 5f;
    [SerializeField] float acceleration = 2f;
    [SerializeField] float vitesseCamera = 2f;
    [SerializeField] float gravity = 0.2f;
    [SerializeField] float MaxDown = 20f;
    [SerializeField] float jumpHeight = 100f;
    [SerializeField] InputActionAsset inputAsset;
    Animator animator;
    SprintComponent sprintComponent;
    Vector2 movement;
    float speed;
    bool jump = false;
    CharacterController cc;
    Camera cam;
    Vector3 downVelocity = Vector3.zero;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = GetComponentInChildren<Camera>();
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        speed = initialSpeed;
        sprintComponent = gameObject.GetComponent<SprintComponent>();

        //inputs
        InputActionMap inputMap = inputAsset.FindActionMap("player");
        InputAction movementAction = inputMap.FindAction("movement");
        InputAction jumpAction = inputMap.FindAction("jump");
        InputAction sprintAction = inputMap.FindAction("sprint");

        movementAction.performed += Movement;
        movementAction.canceled += Movement;
        sprintAction.performed += SprintOn;
        sprintAction.canceled += SprintOff;
        jumpAction.performed += JumpOn;
        jumpAction.canceled += JumpOff;
    }

    void Movement(InputAction.CallbackContext action)
    {
        movement = action.ReadValue<Vector2>();
        animator.SetBool("Walking", movement.magnitude > 0);
        animator.SetFloat("Speed", movement.magnitude);
    }
    void SprintOn(InputAction.CallbackContext action)
    {
        speed = initialSpeed + acceleration;
        animator.SetBool("Sprinting", true);
    }
    void SprintOff(InputAction.CallbackContext action)
    {
        speed = initialSpeed;
        animator.SetBool("Sprinting", false);
    }
    void JumpOn(InputAction.CallbackContext action)
    {
        jump = true;
    }
    void JumpOff(InputAction.CallbackContext action)
    {
        jump = false;
    }


    void Update()
    {

        if (cc.isGrounded) //si touche le sol, arreter la chute
        {
            downVelocity.y = 0f;
        }
        else
        {
            downVelocity.y -= gravity * 100 * Time.deltaTime;
        }
        if (jump && cc.isGrounded)
        {
            downVelocity.y = jumpHeight * gravity;
        }


        if (!Game.isGameOver)
        {
            Quaternion prevRot = cam.transform.rotation;
            cam.transform.Rotate(-Input.GetAxis("Mouse Y") * vitesseCamera, 0, 0); //pivoter la camera

            float x = cam.transform.rotation.eulerAngles.x;
            print(x);
            if (x >= 56 && x <= 315)
            {
                cam.transform.rotation = prevRot;
            }

            transform.Rotate(0, Input.GetAxis("Mouse X") * vitesseCamera, 0); //tourner la capsule et ses enfants

            Vector3 direction = transform.rotation * new Vector3(movement.x, 0, movement.y); //obtenir la position de la souris

            //deplacer le joueur
            if (sprintComponent.sprinting)
                cc.Move((initialSpeed + acceleration) * Time.deltaTime * direction.normalized + downVelocity * Time.deltaTime);
            else
                cc.Move(initialSpeed * Time.deltaTime * direction.normalized + downVelocity * Time.deltaTime);
        }
    }
}
