using System;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform m_transform;
    private Rigidbody2D m_rigidbody2D;
    private GatherInput m_gatherInput;    // lo hago asi por que los listo con la m para poder usarlos rapido y siempre si son de un mismo game obj
    private Animator m_animator;

    //ANIMATOR IDS
    private int idSpeed;
    private int idIsGrounded;

    [Header("Move settings")]
    [SerializeField] private float speed;
    private int direction = 1;

    [Header("Jump settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private int extrajumps;
    [SerializeField] private int counterExtraJumps;
    [SerializeField] private bool canDoubleJump;

    [Header("Ground settings")]
    [SerializeField] private Transform lFoot, rFoot;
    RaycastHit2D lFootRay;
    RaycastHit2D rFootRay;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float rayLenght;
    [SerializeField] private LayerMask groundLayer;

    [Header("Wall settings")]
    [SerializeField] private float checkwallDistance;
    [SerializeField] private bool isWallDetected;

    private void Awake()
    {
        m_gatherInput = GetComponent<GatherInput>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        //m_transform = GetComponent<Transform>();
        m_animator = GetComponent<Animator>();
    }


    void Start()
    {
        
        idSpeed = Animator.StringToHash("speed"); // convertimos los parametros en numero asi el codigo es mas flexible y no tiene que leer 1 x 1 las letras (optimizacion)
        idIsGrounded = Animator.StringToHash("isGrounded");
        lFoot = GameObject.Find("LFoot").GetComponent<Transform>();
        rFoot = GameObject.Find("RFoot").GetComponent<Transform>();

    }

    private void Update()
    {
        SetAnimatorValues();
        
    }

    private void SetAnimatorValues()
    {
        m_animator.SetFloat(idSpeed, Mathf.Abs(m_rigidbody2D.linearVelocityX)); // abs es una formula matematica para que el numero q pases siempre sea positivo, ya que la conidicon que de idle pase a run es que speed sea mayor q 0
        m_animator.SetBool(idIsGrounded, isGrounded);
    }

    void FixedUpdate()
    {
        CheckCollision();
        Move();
        Jump();
    }

    private void CheckCollision()
    {
        HandleGround();
        HandleWall();
    }

    private void HandleWall()
    {
        isWallDetected = Physics2D.Raycast(m_transform.position, Vector2.right * direction, checkwallDistance, groundLayer);
    }

    private void HandleGround()
    {
         lFootRay = Physics2D.Raycast(lFoot.position, Vector2.down, rayLenght, groundLayer);
         rFootRay = Physics2D.Raycast(rFoot.position, Vector2.down, rayLenght, groundLayer);

        if (lFootRay || rFootRay)
        {
            isGrounded = true;
            counterExtraJumps = extrajumps;
            canDoubleJump = false;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void Move()
    {
        Flip();
        m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.ValueX, m_rigidbody2D.linearVelocityY);
    }

    private void Flip()
    {
        if (m_gatherInput.ValueX * direction < 0)
        {
            m_transform.localScale = new Vector3(-m_transform.localScale.x, 1, 1);
            direction *= -1;
        }
    }
    private void Jump()
    {
        if (m_gatherInput.IsJumping)
        {
            if (isGrounded)
            {

            m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.ValueX, jumpForce);
                canDoubleJump = true;
            }
            else if (counterExtraJumps > 0 && canDoubleJump)
            {
                m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.ValueX, jumpForce);
                counterExtraJumps--;
            }
        }
        m_gatherInput.IsJumping = false;
    }

    private void OnDrawGizmos() //dibujar gizmos
    {
        Gizmos.DrawLine(m_transform.position, new Vector2(m_transform.position.x + (checkwallDistance * direction), m_transform.position.y));
    }

}
