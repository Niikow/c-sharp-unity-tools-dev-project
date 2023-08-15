using UnityEngine;

/*
 * This class ensures the player always moves forward whilst looking ahead of the camera
 */
public class CameraMovement : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb = null!;
    public Transform orientation = null!;
    public Transform player = null!;
    public Transform playerObject = null!;
    // suppress null because they will be set in the editor

    public float rotationSpeed = 10f;

    private void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    private void Update()
    {
        // Find the forward direction of player object
        var viewDir = player.position - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // Rotate the player object to face the direction of the camera
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        var inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;


        if (inputDir != Vector3.zero)
            playerObject.forward = Vector3.Slerp(playerObject.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);

    }
}
