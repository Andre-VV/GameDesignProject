using UnityEngine;

public class EnemyDeathPickupSpawner : MonoBehaviour
{
    public GameObject[] pickupPrefabs;
    
    public float chanceToSpawnNothing = 0.5f;

    public void spawnRandomPickup()
    {
        if (pickupPrefabs.Length == 0 || pickupPrefabs == null)
        {
            Debug.LogWarning("No pickup prefabs assigned to EnemyDeathPickupSpawner.");
            return;
        }
        

        chanceToSpawnNothing = Mathf.Clamp01(chanceToSpawnNothing);

        if (Random.value < chanceToSpawnNothing)
        {
            return;
        }

        int randomIndex = Random.Range(0, pickupPrefabs.Length);
        GameObject pickupToSpawn = pickupPrefabs[randomIndex];
        
        Instantiate(pickupToSpawn, transform.position, Quaternion.Euler(0f,0f,0f));
    }
}
