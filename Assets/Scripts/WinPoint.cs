using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para gestionar escenas

public class WinPoint : MonoBehaviour
{
    [SerializeField] private string nextLevelName = "Level 2"; 

    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Jugador tocó el punto de victoria! Cargando siguiente nivel...");
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        // Carga la escena 
        SceneManager.LoadScene(nextLevelName);
    }
}