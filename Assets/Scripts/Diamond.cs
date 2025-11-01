using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Diamond : MonoBehaviour
{
    // Referencia privada al AudioSource del tintineo. Se obtendrá en Awake().
    private AudioSource tintineoSource;

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

        // --- SOLUCIÓN DE SONIDO PERSISTENTE: OBTENER EL AUDIO SOURCE ---
        // 1. Busca el AudioSource en el objeto actual (el diamante).
        tintineoSource = GetComponent<AudioSource>();

        // 2. Si no lo encuentra, busca en los objetos hijos.
        if (tintineoSource == null)
        {
            tintineoSource = GetComponentInChildren<AudioSource>();
        }
        // ----------------------------------------------------------------
    }

    private void Start()
    {
        if (GameManager.instance != null)
        {
            gameManager = GameManager.instance;
        }
        else
        {
            Debug.LogError("Diamond.cs: GameManager.instance no encontrado. Asegúrate de que el GameManager esté en la escena.");
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
            // 1. DETENER SONIDO AMBIENTAL 3D (Se detiene el loop antes del SFX de recogida)
            if (tintineoSource != null && tintineoSource.isPlaying)
            {
                tintineoSource.Stop(); // <-- LÍNEA CLAVE
            }

            // 2. REPRODUCIR SFX DE RECOLECCIÓN (2D)
            if (SFX_Controller.Instance != null)
            {
                SFX_Controller.Instance.PlaySFX("Recoger");
            }

            m_rigibody2D.simulated = false;
            gameManager.AddDiamond();

            animator.SetTrigger(idPickedDiamond);// ejecutar la animacion de collect del diamond
        }
    }
}
