using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            for (int i =1; i <= 2; i++)
            {
                string currentScene = SceneManager.GetActiveScene().name;
                if(currentScene == $"Map{i}")
                {
                    SceneManager.LoadScene($"Map{i + 1}");
                }
            }
        }
    }
}
