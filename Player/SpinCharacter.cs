using UnityEngine;

/*
 *  This class controls the UI player rotation
 */

public class SpinCharacter : MonoBehaviour
{
    public float spinSpeed = 80f;
    public float moveSpeed = 1f;
    public float moveDistance = 0.3f;
    public float secondaryRotation = 2f;

    private float startY;

    private void Start()
    {
        startY = transform.position.y;
    }

    // Make the UI character spin slowly and move up/down
    void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
        transform.Rotate(Vector3.right, secondaryRotation * Time.deltaTime);
        var newY = startY + Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

}
