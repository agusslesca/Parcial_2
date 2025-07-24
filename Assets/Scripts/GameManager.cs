using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //lo puedo usar en todo los scripts haciendolo static la instancia
    [SerializeField] private PlayerController _playerController;
    public PlayerController PlayerController { get => _playerController; }

    [SerializeField] private int _diamondCollected;
    public int DiamondCollected { get => _diamondCollected; }

    private int _totalDiamonds;
    public int TotalDiamonds => _totalDiamonds;

    [SerializeField] private TMP_Text contadorDiamantesTexto;

    private void Start()
    {
        // Contar todos los diamantes con el tag "Item"
        _totalDiamonds = GameObject.FindGameObjectsWithTag("Item").Length;
        ActualizarContadorDiamantes();
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void AddDiamond ()
    {
        _diamondCollected++; //hacerlo con la flecha nos permite hacer metodos de una sola linea
        ActualizarContadorDiamantes();
    }

    private void ActualizarContadorDiamantes()
    {
        if (contadorDiamantesTexto != null)
        {
            if (_diamondCollected >= _totalDiamonds)
            {
                //cambia el texto a color verde cuando completa los diamantes
                contadorDiamantesTexto.color = Color.green;
            }
            else
            {
                //color normal (no completado)
                contadorDiamantesTexto.color = Color.white;
            }
            contadorDiamantesTexto.text = $"Diamantes: {_diamondCollected} / {_totalDiamonds}";
        }
    }

    
}
