using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    [Header("UI del menú de muerte")]
    [SerializeField] private GameObject deadPanel;   // ← arrastra aquí tu panel “Has muerto”

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // 1️⃣ Opcional: animación + bloqueo de controles desde PlayerController
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.Die();   // sigue usando tu lógica de animación / disable movimiento
        }

        // 2️⃣ Mostrar panel de muerte
        if (deadPanel != null)
            deadPanel.SetActive(true);

        // 3️⃣ Pausar todo
        Time.timeScale = 0f;
    }

    
    public void RestartLevel()
    {
        Time.timeScale = 1f;                                            // quitar la pausa
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    
    public void BackToMainMenu()
    {
        Time.timeScale = 1f;                                            // quitar la pausa
        SceneManager.LoadScene("MenuPrincipal");  // usa el nombre exacto de tu escena de menú
    }
}
