using System;
using System.Diagnostics;
using UnityEngine;

/*
 * This class enables player to move around the scene
 */
public class PlayerMovement : MonoBehaviour
{
    public CharacterController Controller = null!; // set in editor

    [SerializeField]
    private Transform Camera = null!;// => GetComponent<Camera>().transform;

    // public Transform camera = null!; // set in editor

    [SerializeField]
    private float
        speed = 6f,
        sprintSpeed = 9f,
        jumpForce = 10f,
        turnSmoothness = 0.1f,
        gravityMultiplier = 3.0f;


    private int jumps;
    [SerializeField] private int maxJumps = 2;

    private const float gravity = -9.81f; // gravity could change in future development



    private float velocity, turnSmoothVelocity;

    // Check for player inputs
    private void Update()
    {
        Movement();
        Jumping();

        PlayerStats.Instance.BlockStaminaRegen = IsSprinting;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            DrainSprint();
        }
    }

    private void DrainSprint()
    {
        if (!IsSprinting) return;
        PlayerStats.Instance.DrainStamina(5);
        Invoke(nameof(DrainSprint), 1);
    }

    private bool CanSprint => PlayerStats.Instance.Stamina >= 5;
    private bool IsSprinting => Input.GetKey(KeyCode.LeftShift);

    // Move the player based on input
    private void Movement()
    {
        var horizontal = Input.GetAxisRaw("Horizontal"); // -1 = a | +1 = d
        var vertical = Input.GetAxisRaw("Vertical"); // -1 = s | +1 = w
        var direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (!(direction.magnitude >= 0.1f)) return;
        var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.eulerAngles.y;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
            turnSmoothness); //smoothens turning animation of player
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        var moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; // ensures player moves in camera direction
        var moveSpeed = IsSprinting && CanSprint ? sprintSpeed : speed; // if player sprints
        Controller.Move(moveDirection.normalized * (moveSpeed * Time.deltaTime));
    }

    // Enables player to jump
    private void Jumping()
    {
        if (Controller.isGrounded)
        {
            jumps = 0;
            velocity = 0f;
        }

        // Allows the player to double jump
        if (Input.GetButtonDown("Jump") && jumps < maxJumps)
        {
            velocity = jumpForce;
            jumps++;
        }

        velocity += gravity * Time.deltaTime * gravityMultiplier;
        Controller.Move(new Vector3(0, velocity * Time.deltaTime, 0));
    }

    // Enables player to interact with items
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            UnityEngine.Debug.Log($"Player collided with {other.name}");
            ItemPickUp itemPickUp = other.GetComponent<ItemPickUp>();
            if (itemPickUp != null)
            {
                itemPickUp.MoveObject(transform.position);
            }
        }
    }
}
