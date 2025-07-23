using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class Diamond : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D m_rigibody2D;
    [SerializeField] private Animator animator;
    private int idPickedDiamond;
    private int idDiamondIndex;

    private void Awake()
    {
        m_rigibody2D = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        idPickedDiamond = Animator.StringToHash("pickedDiamond");
        idDiamondIndex = Animator.StringToHash("diamondIndex");
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        SetRandomDiamond();
    }

    private void SetRandomDiamond() //Randomizar los colores de los diamonds
    {
        var randomDiamondIndex = Random.Range(0, 7);
        animator.SetFloat(idDiamondIndex, randomDiamondIndex);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //spriteRenderer.enabled = false;
            m_rigibody2D.simulated = false;
            gameManager.AddDiamond();
            animator.SetTrigger(idPickedDiamond);// ejecutar la animacion de collect del diamond
        }
    }
}
