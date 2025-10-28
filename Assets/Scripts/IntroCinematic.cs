using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroCinematic : MonoBehaviour
{
    [Header("Narración")]
    public AudioSource narrationSource; // Audio con la narración
    [TextArea(5, 10)]
    public string storyText; // Texto de la historia

    [Header("UI")]
    public TMP_Text storyTextUI;
    public Button skipButton;
    public Image fadeImage;

    [Header("Configuración")]
    public float textSpeed = 0.05f; // Velocidad con que aparecen las letras
    public float fadeDuration = 2f; // Tiempo del fade
    public string nextScene = "Level1"; // Nombre de la escena siguiente

    private bool isSkipping = false;

    void Start()
    {
        skipButton.onClick.AddListener(SkipCinematic);
        StartCoroutine(PlayCinematic());
    }

    IEnumerator PlayCinematic()
    {
        storyTextUI.text = "";
        narrationSource.Play();

        // Mostrar texto letra por letra
        foreach (char c in storyText)
        {
            if (isSkipping) yield break; // si se salta, corta
            storyTextUI.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        // Esperar hasta que termine el audio
        yield return new WaitWhile(() => narrationSource.isPlaying);

        // Iniciar fade y cambiar de escena
        yield return StartCoroutine(FadeOutAndLoad());
    }

    void SkipCinematic()
    {
        if (!isSkipping)
        {
            isSkipping = true;
            narrationSource.Stop();
            StartCoroutine(FadeOutAndLoad());
        }
    }

    IEnumerator FadeOutAndLoad()
    {
        float t = 0f;
        Color color = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(nextScene);
    }
}
