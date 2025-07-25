using UnityEngine;

public class HealBottle : MonoBehaviour
{
    [SerializeField, Range(0, 10)] private int heal;
    [SerializeField] private GameObject healEffectPrefabs;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        playerController.RestoreHP(heal);
        GameManager.Instance.AudioManager.Play("Heal");
        Debug.Log($"GameObject: {other.gameObject.name}");
        Destroy(this.gameObject);
        GameObject newPrefab = GameObject.Instantiate(healEffectPrefabs, other.transform.position, Quaternion.identity);
        Destroy(newPrefab, 0.5f);
    }


}
