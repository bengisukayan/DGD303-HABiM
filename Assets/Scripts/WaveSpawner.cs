using System.Collections;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public int totalWaves = 4;
    public float timeBetweenWaves = 5f;
    public int baseEnemiesPerWave = 3;

    public GameObject shooterPrefab;
    public GameObject puncherPrefab;

    public Transform[] spawnPoints;

    public TextMeshProUGUI waveText;
    public GameObject waveTextPanel;
    public GameObject waveWalls;

    private int currentWave = 0;
    private bool isSpawning = false;

    private void Start()
    {
        waveTextPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isSpawning)
        {
            StartCoroutine(SpawnWaves());
        }
    }

    private IEnumerator SpawnWaves()
    {
        isSpawning = true;
        waveTextPanel.SetActive(true);
        waveWalls.SetActive(true);

        for (currentWave = 1; currentWave <= totalWaves; currentWave++)
        {
            FindObjectOfType<AudioManager>().PlayWaveMusic(currentWave);
            waveText.text = "Wave " + currentWave;
            int enemyCount = baseEnemiesPerWave + (currentWave * 2);

            for (int i = 0; i < enemyCount; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(timeBetweenWaves);
        }

        waveText.text = "Waves Completed!";
        yield return new WaitForSeconds(3f);
        FindObjectOfType<AudioManager>().PlayBackgroundMusic();
        waveTextPanel.SetActive(false);
        waveWalls.SetActive(false);
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemyPrefab = Random.value > 0.5f ? shooterPrefab : puncherPrefab;

        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            isSpawning = false;
            waveTextPanel.SetActive(false);
        }
    }
}