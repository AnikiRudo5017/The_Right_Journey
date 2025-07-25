using UnityEngine;
using UnityEngine.SceneManagement;

public class teleport : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("Map1");
        }
    }
}
