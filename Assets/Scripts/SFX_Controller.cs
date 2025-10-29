using UnityEngine;
using UnityEngine.Audio;
using System;

public class SFX_Controller : MonoBehaviour
{
    // Hacemos el script accesible globalmente (Singleton)
    public static SFX_Controller Instance;

    // 1. Array de Clips para mapear nombres a sonidos
    [Header("Clips de Sonido")]
    public SoundClip[] clipsDeSonido;

    // El AudioSource que hemos adjuntado al SFX_Manager
    private AudioSource sfxSource;

    void Awake()
    {
        // Implementación del Singleton
        if (Instance == null)
        {
            Instance = this;
            // No destruir al cargar nuevas escenas si es un gestor de audio principal
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        sfxSource = GetComponent<AudioSource>();

        // Opcional: Validar que todos los clips tengan nombre asignado
        foreach (SoundClip s in clipsDeSonido)
        {
            if (string.IsNullOrEmpty(s.nombre))
            {
                Debug.LogError("SFX_Controller: Un clip de sonido no tiene nombre asignado.");
            }
        }
    }

    // Método principal para reproducir cualquier SFX por su nombre
    public void PlaySFX(string nombreClip)
    {
        // Buscar el clip dentro del array
        SoundClip s = Array.Find(clipsDeSonido, clip => clip.nombre == nombreClip);

        if (s == null)
        {
            Debug.LogWarning("SFX: Clip '" + nombreClip + "' no encontrado!");
            return;
        }

        // Reproducir el clip. Esto permite que varios SFX se reproduzcan a la vez.
        sfxSource.PlayOneShot(s.clip, s.volumen);
    }
}

// 2. Clase serializable para mapear el nombre y el clip en el Inspector
[System.Serializable]
public class SoundClip
{
    public string nombre; // Ejemplo: "Salto", "Danio", "Click"
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volumen = 1f;
}
