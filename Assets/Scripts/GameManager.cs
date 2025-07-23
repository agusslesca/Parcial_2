using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager instance; //lo puedo usar en todo los scripts haciendolo static la instancia
    [SerializeField] private PlayerController _playerController;
    public PlayerController PlayerController { get => _playerController;}

    [SerializeField] private int _diamondCollected;
    public int DiamondCollected { get => _diamondCollected;  }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void AddDiamond () =>  _diamondCollected ++; //hacerlo con la flecha nos permite hacer metodos de una sola linea

    
}
