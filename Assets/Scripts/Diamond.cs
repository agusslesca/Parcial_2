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
        // Asegura que el GameManager est� disponible (si usa el patr�n Singleton 'instance')
        if (GameManager.instance != null)
        {
            gameManager = GameManager.instance;
        }
        else
        {
            Debug.LogError("Diamond.cs: GameManager.instance no encontrado. Aseg�rate de que el GameManager est� en la escena.");
        }

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

            // =========================================================
            // === C�DIGO A�ADIDO: Reproducir el sonido "Recoger" ===
            // =========================================================
            if (SFX_Controller.Instance != null)
            {
                // El nombre "Recoger" debe coincidir EXACTAMENTE con el campo 'Nombre' en el Inspector del SFX_Controller
                SFX_Controller.Instance.PlaySFX("Recoger");
            }
            // =========================================================

            animator.SetTrigger(idPickedDiamond);// ejecutar la animacion de collect del diamond
        }
    }
}
