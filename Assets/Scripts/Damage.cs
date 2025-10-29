using UnityEngine;

public class Damage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (SFX_Controller.Instance != null)
            {
                collision.GetComponent<PlayerController>().KnockBack();
                SFX_Controller.Instance.PlaySFX("Danio"); // Llama al clip que nombraste "Danio"
            }
            
        }
    }
}
