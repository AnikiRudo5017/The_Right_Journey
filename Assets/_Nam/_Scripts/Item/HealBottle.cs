using UnityEngine;

public class HealBottle : MonoBehaviour
{
    [SerializeField,Range(0,10)] private int heal;
    [SerializeField] private GameObject healEffectPrefabs;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Health healthPlayer = GetComponent<Health>();
            //healthPlayer.HealPlayer(heal)
            //Destroy(this.gameObject);
            //Spawn hi?u ?ng vào chân nhân v?t tronh 1 giây r m?t
            GameManager.Instance.AudioManager.Play("Heal");
            Debug.Log($"GameObject: {other.gameObject.name}");
            Destroy(this.gameObject);
            GameObject newPrefab = GameObject.Instantiate(healEffectPrefabs, other.transform.position, Quaternion.identity);
            Destroy(newPrefab,0.5f);
        }
    }

    
}
