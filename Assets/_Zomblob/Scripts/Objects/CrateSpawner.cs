using UnityEngine;

public class CrateSpawner : MonoBehaviour
{
    public GameObject MysteryBarrelPrefab;
    public Transform[] spawnPoint;
    private GameObject currentObject;

    public void SpawnRandomCrate ()
    {
        if(currentObject != null)
        {
            Destroy(currentObject);
        }

        int randomPoint = Random.Range(0, spawnPoint.Length);

        currentObject = Instantiate(
            MysteryBarrelPrefab,
            spawnPoint[randomPoint].position,
            Quaternion.identity
        );
    }
} 