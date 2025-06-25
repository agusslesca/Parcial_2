using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement; // Agregado para reiniciar el nivel

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform m_transform;
    private Rigidbody2D m_rigidbody2D;
    private GatherInput m_gatherInput;    // Lo hago así porque los listo con la m para poder usarlos rápido y siempre si son de un mismo game obj
    private Animator m_animator;

    // ANIMATOR IDS
    private int idSpeed;
    private int idIsGrounded;
    private int idIsWallDetected;
    private int idknockBack;
    private int idDeath; // <-- NUEVO: ID del Animator para la animación de muerte

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
    [SerializeField] private bool canWallSlide;
    [SerializeField] private float slideSpeed;
    [SerializeField] private Vector2 wallJumpForce;
    [SerializeField] private bool isWallJumping;
    [SerializeField] private float wallJumpDuration;

   

    [Header("Death Settings")] // <-- NUEVO: Sección para variables relacionadas con la muerte
    [SerializeField] private bool isDead; // <-- NUEVO: Indica si el jugador está muerto
    

    private void Awake()
    {
        m_gatherInput = GetComponent<GatherInput>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        //m_transform = GetComponent<Transform>(); // Esta línea está comentada, asegúrate de asignar m_transform en el Inspector o descomenta si es necesario
        m_animator = GetComponent<Animator>();
    }

    void Start()
    {
        idSpeed = Animator.StringToHash("speed");
        idIsGrounded = Animator.StringToHash("isGrounded");
        idIsWallDetected = Animator.StringToHash("isWallDetected");
        idknockBack = Animator.StringToHash("knockBack");
        idDeath = Animator.StringToHash("death"); // <-- NUEVO: Inicializamos idDeath
        lFoot = GameObject.Find("LFoot").GetComponent<Transform>();
        rFoot = GameObject.Find("RFoot").GetComponent<Transform>();

        // Asegúrate de que m_transform esté asignado, si no está configurado en el Inspector y la línea de arriba está comentada
        if (m_transform == null)
        {
            m_transform = transform; // Asigna la transformación actual si no está configurada
        }
    }

    private void Update()
    {
        // Si el jugador está muerto, deja de procesar cualquier actualización
        if (isDead) return; // <-- NUEVO: Evita actualizaciones si está muerto

        SetAnimatorValues();
    }

    private void SetAnimatorValues()
    {
        m_animator.SetFloat(idSpeed, Mathf.Abs(m_rigidbody2D.linearVelocity.x));
        m_animator.SetBool(idIsGrounded, isGrounded);
        m_animator.SetBool(idIsWallDetected, isWallDetected);
    }

    void FixedUpdate()
    {
        
        if (isDead) return; 

        CheckCollision();
        Move();
        Jump();
    }

    private void CheckCollision()
    {
        HandleGround();
        HandleWall();
        HandleWallSlide();
    }

    private void HandleWallSlide()
    {
        canWallSlide = isWallDetected;
        if (!canWallSlide) return;
        canDoubleJump = false;
        slideSpeed = m_gatherInput.Value.y < 0 ? 1 : 0.5f;
        m_rigidbody2D.linearVelocity = new Vector2(m_rigidbody2D.linearVelocity.x, m_rigidbody2D.linearVelocity.y * slideSpeed);
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
        if (isWallDetected && !isGrounded) return;
        if (isWallJumping) return;
        Flip();
        m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.Value.x, m_rigidbody2D.linearVelocity.y);
    }

    private void Flip()
    {
        if (m_gatherInput.Value.x * direction < 0)
        {
            HandleDirection();
        }
    }

    private void HandleDirection()
    {
        m_transform.localScale = new Vector3(-m_transform.localScale.x, 1, 1);
        direction *= -1;
    }

    private void Jump()
    {
        if (m_gatherInput.IsJumping)
        {
            if (isGrounded)
            {
                m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.Value.x, jumpForce);
                canDoubleJump = true;
            }
            else if (isWallDetected) wallJump();
            else if (counterExtraJumps > 0 && canDoubleJump)
            {
                DoubleJump();
            }
        }
        m_gatherInput.IsJumping = false;
    }

    private void wallJump()
    {
        m_rigidbody2D.linearVelocity = new Vector2(wallJumpForce.x * -direction, wallJumpForce.y);
        HandleDirection();
        StartCoroutine(WallJumpRoutine());
    }

    IEnumerator WallJumpRoutine()
    {
        isWallJumping = true;
        yield return new WaitForSeconds(wallJumpDuration);
        isWallJumping = false;
    }

    private void DoubleJump()
    {
        m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.Value.x, jumpForce);
        counterExtraJumps--;
    }

    
    public void Die() 
    {
        if (isDead) return; // Evita múltiples llamadas a la muerte

        
        isDead = true; 

        // Detiene el movimiento y la física del jugador
        m_rigidbody2D.linearVelocity = Vector2.zero;
        m_rigidbody2D.bodyType = RigidbodyType2D.Kinematic; 
        

        m_animator.SetTrigger(idDeath); 
       
        
    }

   
    

    private void OnDrawGizmos() // Dibujar gizmos
    {
        Gizmos.DrawLine(m_transform.position, new Vector2(m_transform.position.x + (checkwallDistance * direction), m_transform.position.y));
    }
}
