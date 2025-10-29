using System;
using System.Collections;

using UnityEngine;


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
    private int idDeath;
    private int idKnockBack;

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

    [Header("Knock settings")]
    [SerializeField] private bool isKnocked;
    [SerializeField] private bool canBeKnocked;
    [SerializeField] private Vector2 knockedPower;
    [SerializeField] private float knockedDuration;

   

    [Header("Death Settings")] 
    [SerializeField] private bool isDead; 
    

    private void Awake()
    {
        m_gatherInput = GetComponent<GatherInput>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        //m_transform = GetComponent<Transform>(); 
        m_animator = GetComponent<Animator>();
        
    }

    void Start()
    {
        idSpeed = Animator.StringToHash("speed");
        idIsGrounded = Animator.StringToHash("isGrounded");
        idIsWallDetected = Animator.StringToHash("isWallDetected");
        idDeath = Animator.StringToHash("death"); // <-- NUEVO: Inicializamos idDeath
        idKnockBack = Animator.StringToHash("knockBack");
        lFoot = GameObject.Find("LFoot").GetComponent<Transform>();
        rFoot = GameObject.Find("RFoot").GetComponent<Transform>();

        
        if (m_transform == null)
        {
            m_transform = transform; 
        }
    }

    private void Update()
    {
        // Si el jugador está muerto, deja de procesar cualquier actualización
        if (isDead) return; 

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
        if (isKnocked) return; // si es nockeadco no quiero q haga nada lo de abajo
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
                PlayJumpSound();

            }
            else if (isWallDetected) 
            {
                wallJump();
                PlayJumpSound();

            }
            else if (counterExtraJumps > 0 && canDoubleJump)
            {
                DoubleJump();
                PlayJumpSound();



            }
        }
        m_gatherInput.IsJumping = false;
    }

    // === NUEVA FUNCIÓN AÑADIDA PARA CENTRALIZAR LA LLAMADA AL MANAGER ===
    private void PlayJumpSound()
    {
        // El nombre "Salto" debe coincidir EXACTAMENTE con el campo 'Nombre' en el Inspector del SFX_Controller
        if (SFX_Controller.Instance != null)
        {
            SFX_Controller.Instance.PlaySFX("Salto");
        }
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

    public void KnockBack()
    {
        PlayHitSound();
        StartCoroutine(KnockBackRoutine());
        m_rigidbody2D.linearVelocity = new Vector2(knockedPower.x * -direction, knockedPower.y);
        m_animator.SetTrigger(idKnockBack);
    }

    // === Nueva función auxiliar para centralizar la llamada al Manager ===
    private void PlayHitSound()
    {
        // El nombre "Danio" debe coincidir EXACTAMENTE con el campo 'Nombre' en el Inspector.
        if (SFX_Controller.Instance != null)
        {
            // Usamos "Danio" o "Hit" dependiendo de cómo lo llamaste en tu SFX_Controller
            SFX_Controller.Instance.PlaySFX("Danio");
        }
    }

    private IEnumerator KnockBackRoutine()
    {
        isKnocked = true;
        canBeKnocked = false;
        yield return new WaitForSeconds(knockedDuration);
        isKnocked = false;
        canBeKnocked = true;

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
