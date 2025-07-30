using UnityEngine;
using UnityEngine.UI;

public class ToggleSoundButtonBackGround : MonoBehaviour
{
    public Sprite spriteOff;
    public Sprite spriteTransition;
    public Sprite spriteOn;
    public Image targetImage;
    public AudioSource musicSource; 

    private bool isSoundOn = true;

    void Start()
    {
        // Refleja el estado inicial
        isSoundOn = musicSource != null && musicSource.volume > 0f;
        targetImage.sprite = isSoundOn ? spriteOn : spriteOff;
    }

    public void OnClickToggleSound()
    {
        targetImage.sprite = spriteTransition;
        Invoke(nameof(ApplyToggle), 0.2f);
    }

    private void ApplyToggle()
    {
        isSoundOn = !isSoundOn;

        if (musicSource != null)
        {
            musicSource.volume = isSoundOn ? 0.1f : 0f; //  Alterna volumen
        }

        targetImage.sprite = isSoundOn ? spriteOn : spriteOff;
    }
}

