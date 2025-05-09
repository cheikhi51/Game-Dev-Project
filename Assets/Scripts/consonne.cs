using UnityEngine;

public class Consonne : MonoBehaviour
{
    public SpriteMask mySpriteMask; // Reference to the Sprite Mask
    public float dropSpeed = 2f; // Speed at which the consonne drops
    private bool isVisible = false; // Initially hidden
    private float yStartPosition; // Initial Y position

    void Start()
    {
        // Ensure the Sprite Mask is initially hidden
        if (mySpriteMask != null)
        {
            mySpriteMask.enabled = isVisible;
            // Record the starting Y position
            yStartPosition = mySpriteMask.transform.position.y;
        }
        else
        {
            Debug.LogError("Sprite Mask is not assigned! Assign it in the Inspector.");
        }

        // Start the drop after a delay (optional)
        //Invoke("StartDropping", 1f); // Drop after 1 second
        StartDropping(); // Drop immediately
    }

    void Update()
    {
        if (isVisible)
        {
            // Make the Sprite Mask drop
            mySpriteMask.transform.Translate(Vector3.down * dropSpeed * Time.deltaTime);

            // Optional: Destroy or recycle when it goes off-screen
            if (mySpriteMask.transform.position.y < -5f)
            {
                ResetConsonne(); // Reset its position
                //Destroy(gameObject); // Or destroy it
            }
        }
    }

    void StartDropping()
    {
        isVisible = true;
        mySpriteMask.enabled = isVisible; // Show the Sprite Mask
    }

    void ResetConsonne()
    {
        isVisible = false;
        mySpriteMask.enabled = isVisible;
        mySpriteMask.transform.position = new Vector3(
            mySpriteMask.transform.position.x,
            yStartPosition,
            mySpriteMask.transform.position.z
        );
    }
}