using UnityEngine;

public class Diamond : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D m_rigibody2D;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        m_rigibody2D = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spriteRenderer.enabled = false;
            m_rigibody2D.simulated = false;
            gameManager.AddDiamond();
        }
    }
}
