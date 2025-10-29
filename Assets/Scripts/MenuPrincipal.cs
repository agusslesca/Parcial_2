using System.Collections; 
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    
    [SerializeField] private float delayBeforeSceneLoad = 0.2f;

   

    public void Play()
    {
        PlayClickSound();
        
        StartCoroutine(LoadSceneWithDelay(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void ComoJugar()
    {
        PlayClickSound();
        
        StartCoroutine(LoadSceneWithDelay("ComoJugar"));
    }


    public void Quit()
    {
        
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    // === COROUTINES (Gestionan el delay) ===

    
    IEnumerator LoadSceneWithDelay(int sceneIndex)
    {
        
        yield return new WaitForSeconds(delayBeforeSceneLoad);

        
        SceneManager.LoadScene(sceneIndex);
    }

    
    IEnumerator LoadSceneWithDelay(string sceneName)
    {
        
        yield return new WaitForSeconds(delayBeforeSceneLoad);

        SceneManager.LoadScene(sceneName);
    }

    // === FUNCIÓN AUXILIAR DE AUDIO ===

    private void PlayClickSound() //Metodo llama al SFX para reproducir el click
    {
        
        if (SFX_Controller.Instance != null)
        {
            SFX_Controller.Instance.PlaySFX("Click");
        }
    }
}