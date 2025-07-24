using UnityEngine;

public class TestKnocked : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().KnockBack();
        }
    }
}
