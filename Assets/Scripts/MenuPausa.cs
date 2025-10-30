using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio; // ¡Añadido! Necesario para AudioMixer

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject botonPausa;
    [SerializeField] private GameObject menuPausa;

    [Header("Control de Audio (Parcial)")]
    [Tooltip("Arrastra el archivo de tu Audio Mixer (ej. MenuPrincipal)")]
    [SerializeField] private AudioMixer masterMixer; // Referencia al Mixer

    // El nombre del parámetro LowPass expuesto en el Bus Master
    private const string CutoffParameterName = "MasterCutoff";

    // Frecuencias de corte para el efecto de Pausa
    private const float FrecuenciaNormal = 22000f; // Máximo: Sin efecto
    private const float FrecuenciaPausa = 800f;   // Amortiguado: Efecto de Pausa


    // Funcin auxiliar para reproducir el sonido de clic (ya estaba implementado)
    private void PlayClickSound()
    {
        if (SFX_Controller.Instance != null)
        {
            // Llama al clip "Click"
            SFX_Controller.Instance.PlaySFX("Click");
        }
    }

    // ----------------------------------------------------------------------
    // CONTROL DE PAUSA
    // ----------------------------------------------------------------------

    public void Pause()
    {
        PlayClickSound();

        // 1. CONTROL DE AUDIO: Aplicar el filtro Low Pass (Amortiguar el sonido)
        if (masterMixer != null)
        {
            masterMixer.SetFloat(CutoffParameterName, FrecuenciaPausa);
        }

        // 2. Control de Juego y UI
        Time.timeScale = 0f;
        botonPausa.SetActive(false);
        menuPausa.SetActive(true);
    }

    public void Resume()
    {
        PlayClickSound();

        // 1. CONTROL DE AUDIO: Restaurar la frecuencia normal (Quitar el filtro)
        if (masterMixer != null)
        {
            masterMixer.SetFloat(CutoffParameterName, FrecuenciaNormal);
        }

        // 2. Control de Juego y UI
        Time.timeScale = 1f;
        botonPausa.SetActive(true);
        menuPausa.SetActive(false);
    }

    // ----------------------------------------------------------------------
    // CONTROL DE NAVEGACIÓN
    // ----------------------------------------------------------------------

    public void Restart()
    {
        PlayClickSound();

        // CONTROL DE AUDIO: Asegurar que el filtro no quede activado al reiniciar
        if (masterMixer != null)
        {
            masterMixer.SetFloat(CutoffParameterName, FrecuenciaNormal);
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Back()
    {
        PlayClickSound();

        // CONTROL DE AUDIO: Asegurar que el filtro no quede activado al volver al menú
        if (masterMixer != null)
        {
            masterMixer.SetFloat(CutoffParameterName, FrecuenciaNormal);
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
}