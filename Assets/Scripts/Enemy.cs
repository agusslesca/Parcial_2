using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    [Header("UI del menú de muerte")]
    [SerializeField] private GameObject deadPanel;   // panel “Has muerto”

    // Función auxiliar para reproducir el sonido de Daño/Hit
    private void PlayHitSound()
    {
        
        if (SFX_Controller.Instance != null)
        {
            SFX_Controller.Instance.PlaySFX("Danio");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        //  animación + bloqueo de controles desde PlayerController
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.Die();    // sigue usando tu lógica de animación / disable movimiento
        }

        // Mostrar panel de muerte
        if (deadPanel != null)
            deadPanel.SetActive(true);

        // Pausar todo
        Time.timeScale = 0f;
    }


    public void RestartLevel()
    {
        PlayHitSound();

        Time.timeScale = 1f;                                
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void BackToMainMenu()
    {
        
        PlayHitSound();

        Time.timeScale = 1f;                                 
        SceneManager.LoadScene("MenuPrincipal"); 
    }
}