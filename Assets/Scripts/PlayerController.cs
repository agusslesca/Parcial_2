using System;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D m_rigidbody2D;
    private GatherInput m_gatherInput;    // lo hago asi por que los listo con la m para poder usarlos rapido y siempre si son de un mismo game obj
    private Transform m_transform;
    private Animator m_animator;
    [SerializeField] private float speed;
    private int direction = 1;
    private int idSpeed;


    
    void Start()
    {
        m_gatherInput = GetComponent<GatherInput>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_transform = GetComponent<Transform>();
        m_animator = GetComponent<Animator>();
        idSpeed = Animator.StringToHash("Speed"); // convertimos los parametros en numero asi el codigo es mas flexible y no tiene que leer 1 x 1 las letras (optimizacion)
    }

    private void Update()
    {
        SetAnimatorValues();
        
    }

    private void SetAnimatorValues()
    {
        m_animator.SetFloat(idSpeed, Mathf.Abs(m_rigidbody2D.linearVelocityX)); // abs es una formula matematica para que el numero q pases siempre sea positivo, ya que la conidicon que de idle pase a run es que speed sea mayor q 0
    }

    void FixedUpdate()
    {
        Move();
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
}
