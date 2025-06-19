using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPoint : MonoBehaviour
{
    [Header("UI del menú de victoria")]
    [SerializeField] private GameObject winPanel;        // ← arrastra aquí tu panel “Win”

    [Header("Escena a cargar")]
    [SerializeField] private string nextLevelName = "Level 2";   // pon el nombre exacto de la próxima escena
    private string home = "MenuPrincipal";
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Mostrar panel de victoria
        if (winPanel != null)
            winPanel.SetActive(true);

        // Pausar el juego
        Time.timeScale = 0f;
    }

    
    public void NextLevel()
    {
        Time.timeScale = 1f;                       // quitar la pausa
        SceneManager.LoadScene(nextLevelName);
    }

    public void BackMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(home);
    }
}
