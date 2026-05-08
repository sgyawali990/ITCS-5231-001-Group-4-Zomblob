using UnityEngine;
using System.Collections;
using System;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public CrateSpawner crateSpawner;
    [SerializeField] private ZombieSpawner spawner;
    [SerializeField] private int baseCount = 5;
    [SerializeField] private float breakBetweenWaves = 5f;
    [SerializeField] private int bossEveryNWaves = 5;
    [SerializeField] private int maxWaves = 5;
    [SerializeField] private int waveCount = 0;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI waveText;

    private int waveIndex = 0;
    private int alive = 0;
    private bool wavesActive = true;

    private void OnEnable()
    {
        spawner.EnemySpawned += OnEnemySpawned;
        EnemyHealth.EnemyDied += OnEnemyDied;
    }

    private void OnDisable()
    {
        spawner.EnemySpawned -= OnEnemySpawned;
        EnemyHealth.EnemyDied -= OnEnemyDied;
    }

    private void Start()
    {
        UpdateWaveUI();
        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
{
    while (wavesActive && waveIndex < maxWaves)
    {
        waveIndex++;
        waveCount = waveIndex;

        UpdateWaveUI();
            
        if (crateSpawner != null)
        {
                crateSpawner.SpawnRandomCrate();
        }

        bool isBossWave = waveIndex % bossEveryNWaves == 0;

        int toSpawn = baseCount + waveIndex * 2;

        if (isBossWave)
            toSpawn = Mathf.RoundToInt(toSpawn * 0.7f);

        alive = 0;

        Debug.Log($"========== WAVE {waveIndex}/{maxWaves} ==========");

        
        yield return StartCoroutine(
            spawner.BeginWaveRoutine(toSpawn, waveIndex)
        );

        
        if (isBossWave)
        {
            spawner.SpawnBoss();
        }

        
        while (alive > 0)
        {
            yield return null;
        }

        Debug.Log($"Wave {waveIndex} cleared!");

        // If final wave done -> stop loop
        if (waveIndex >= maxWaves)
            break;

        yield return new WaitForSeconds(breakBetweenWaves);
    }

}

private void UpdateWaveUI()
    {
        if (waveText != null)
            waveText.text = "Wave: " + waveIndex + " / " + maxWaves;
    }

    private void OnEnemySpawned() => alive++;
    private void OnEnemyDied() => alive = Mathf.Max(0, alive - 1);

    public void StopWaves() => wavesActive = false;
}