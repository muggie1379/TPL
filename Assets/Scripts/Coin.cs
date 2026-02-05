using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private Vector3 randomjump;

    [SerializeField]
    [Range(0f, 2f)]
    private float animationSpeed = 1f;

    private float minY = -7f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        animator.speed = animationSpeed;
        Jump();
        if (transform.position.y < minY) Destroy(gameObject);
    }

    void Jump()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

        Vector2 jumpVelocity = new Vector2(Random.Range(-randomjump.x, randomjump.x), Random.Range(randomjump.y, randomjump.z));

        rigidbody.AddForce(jumpVelocity, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
