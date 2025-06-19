using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // LE SUMO 1 A LA ESCENA EN LA Q ESTOY
    }
    
    public void Quit()
    {
        Debug.Log("Salir...");
        Application.Quit();
    }
}
