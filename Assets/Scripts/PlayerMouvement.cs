using UnityEngine;

public class PlayerMouvement : MonoBehaviour
{
    public float vitesseDeplacement = 5f;

    private Rigidbody2D rb;
    private float deplacementHorizontal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        deplacementHorizontal = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(deplacementHorizontal * vitesseDeplacement, rb.linearVelocity.y);
    }
}
