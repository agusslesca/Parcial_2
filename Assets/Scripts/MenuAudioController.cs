using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MenuAudioController : MonoBehaviour
{
    // --- Campos P�blicos (SIN CAMBIOS) ---

    [Header("Configuraci�n del Audio Mixer")]
    [Tooltip("Snapshot que silencia el Bus Music (-80 dB).")]
    public AudioMixerSnapshot fanfarriaSnapshot;

    [Tooltip("Snapshot con el volumen final bajo de la M�sica Chill (-15 dB).")]
    public AudioMixerSnapshot chillSnapshot;

    [Tooltip("Duraci�n del Crossfade (Fijo a 1.0s para el efecto de fade in).")]
    public float tiempoTransicion = 1.0f;

    [Header("Fuentes de Audio")]
    public AudioSource fanfarriaSource;
    public AudioSource chillMusicSource;

    // --- L�gica del Script ---

    void Awake()
    {
        // NO APLICAMOS EL SNAPSHOT AQU�. LO APLICAREMOS DESPU�S DE LA ESPERA.
        // Esto simplifica la secuencia a un control m�s manual.

        // 1. Iniciamos la secuencia principal con una Coroutine.
        StartCoroutine(SecuenciaSecuencialSimple());
    }

    IEnumerator SecuenciaSecuencialSimple()
    {
        // 2. Esperamos un frame para garantizar la inicializaci�n.
        yield return null;

        if (fanfarriaSource == null || fanfarriaSource.clip == null || chillMusicSource == null || chillSnapshot == null)
        {
            Debug.LogError("Error: Faltan referencias clave en el Inspector.");
            yield break;
        }

        // --- FASE 1: SOLO TROMPETAS ---

        // 3. Detenemos y nos aseguramos de que la m�sica chill no est� sonando.
        chillMusicSource.Stop();

        // 4. Toca la fanfarria.
        fanfarriaSource.Play();

        // 5. Esperamos la duraci�n COMPLETA del clip de las trompetas.
        yield return new WaitForSeconds(fanfarriaSource.clip.length);

        // --- FASE 2: FADE IN DE M�SICA CHILL ---

        // 6. Primero, nos aseguramos de que el Bus Music est� silenciado (-80 dB) 
        // ANTES de que la m�sica chill empiece a sonar.
        if (fanfarriaSnapshot != null)
        {
            fanfarriaSnapshot.TransitionTo(0.0f); // Silencio instant�neo
        }

        // 7. Activamos la M�sica Chill para que el Mixer pueda controlarla.
        chillMusicSource.Play();

        // 8. Iniciamos el crossfade suave: el Bus Music sube de -80 dB a -15 dB en 1.0 segundo.
        chillSnapshot.TransitionTo(tiempoTransicion);
    }
}