using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;

public class FirstPersonController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float crouchingSpeed;
    [SerializeField] private float rotationSpeed;   // Mouse sensitivity. How fast player can look around.
    [SerializeField] private float crouchHeight;

    [SerializeField] private Vector3 offset;        // Set to height of character's eyes.

    private GameObject playerCamera;
    private CharacterController characterController;

    private readonly float gravity = -9.8f;
    private Vector3 velocity;
    private Vector3 input;
    private float mouseX;
    private float mouseY;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        characterController = GetComponent<CharacterController>();
        playerCamera = FindObjectOfType<Camera>().gameObject;
    }

    void Update()
    {
        // Rotate player & camera

        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -89, 89);

        playerCamera.transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        transform.rotation = Quaternion.Euler(0, mouseX, 0);

        // Gravity

        if(!characterController.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // Move player

        input.x = Input.GetAxis("Horizontal");
        input.z = Input.GetAxis("Vertical");

        Quaternion direction = Quaternion.AngleAxis(playerCamera.transform.eulerAngles.y, Vector3.up);

        float speed = walkSpeed;
        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
        }
        if(Input.GetKey(KeyCode.LeftControl))
        {
            speed = crouchingSpeed;
        }

        Vector3 playerMove = direction * input * speed + velocity;

        characterController.Move(Time.deltaTime * playerMove);

        // Crouch player

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(1.5f, crouchHeight, 1.5f);
        }
        
        if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }

    void LateUpdate()
    {
        playerCamera.transform.position = transform.position + offset;  // Move camera
    }
}
