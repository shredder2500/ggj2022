using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI.Behavior;
using DefaultNamespace.AI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace DefaultNamespace {
public class EnemySpawner : MonoBehaviour {
  [SerializeField] private int totalWaves = 15;
  [SerializeField] private float secondsBetweenWaves = 10;
  [SerializeField] private Spawn[] spawns;
  [SerializeField] private SpawnPoint[] spawnPoints;

  [SerializeField] private UnityEvent<float> onWaveCountdown;
  [FormerlySerializedAs("onWaveNumber")] [SerializeField] private UnityEvent<int, int> onWaveStart;
  [SerializeField] private UnityEvent<int, int> onEnemyCount;
  [SerializeField] private UnityEvent onCompleted;
  [SerializeField] private UnityEvent onWaveComplete;

  private int _wave;
  private bool _skip;

  public event Action OnWaveStart;
  public event Action OnWaveEnd;

  private void Start() {
    StartCoroutine(SpawnWaves());
  }

  public void SkipToNextWave() {
    _skip = true;
    Debug.Log("Skipping to next wave");
  }

  private IEnumerator WaitForNextWave() {
    var time = 0f;
    do {
      yield return null;
      time += Time.deltaTime;
      onWaveCountdown.Invoke(secondsBetweenWaves - time);
    } while (time < secondsBetweenWaves && !_skip);
    onWaveCountdown.Invoke(0);
    _skip = false;
  }

  private IEnumerator SpawnWaves() {
    while (_wave < totalWaves) {
      yield return StartCoroutine(WaitForNextWave());
      onWaveStart.Invoke(_wave, totalWaves);
      OnWaveStart?.Invoke();
      var spawnCounts = spawns.ToDictionary(x => x,
        x => (int)Mathf.Lerp(x.StartAmount, x.EndAmount, x.SpawnCurve.Evaluate(_wave / (float)totalWaves)));
      var totalEnemies = spawnCounts.Values.Aggregate(0, (acc, curr) => acc + curr);
      var enemies = totalEnemies;
      onEnemyCount.Invoke(enemies, totalEnemies);

      var spawnCoroutines = spawns.Select(spawn => StartCoroutine(DoSpawn(spawn, spawnCounts[spawn], () => {
        enemies -= 1;
        onEnemyCount.Invoke(enemies, totalEnemies);
      }))).ToArray();

      foreach (var spawnCoroutine in spawnCoroutines) {
        yield return spawnCoroutine;
      }

      while (enemies > 0) {
        yield return null;
      }

      onWaveComplete.Invoke();
      OnWaveEnd?.Invoke();
      _wave += 1;
    }
    
    onCompleted.Invoke();

    IEnumerator DoSpawn(Spawn spawn, int amount, Action onDeath) {
      var spawned = 0;
      var spawnRate = Mathf.Lerp(spawn.StartSpawnRate, spawn.EndSpawnRate,
        spawn.SpawnRateCurve.Evaluate(_wave / (float)totalWaves));
      while (spawned < amount) {
        yield return new WaitForSeconds(spawnRate);
        spawned += 1;
        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        spawnPoint.Spawn(spawn.Prefab).GetComponent<Health>().OnDeath += onDeath;
      }
    }
  }

  [Serializable]
  public class Spawn {
    [SerializeField] private Enemy prefab;
    [SerializeField] private int startAmount;
    [SerializeField] private int endAmount;
    [SerializeField] private float startSpawnRate;
    [SerializeField] private float endSpawnRate;
    [SerializeField] private AnimationCurve spawnCurve;
    [SerializeField] private AnimationCurve spawnRateCurve;

    public Enemy Prefab => prefab;
    public int StartAmount => startAmount;
    public int EndAmount => endAmount;
    public float StartSpawnRate => startSpawnRate;
    public float EndSpawnRate => endSpawnRate;
    public AnimationCurve SpawnCurve => spawnCurve;
    public AnimationCurve SpawnRateCurve => spawnRateCurve;
  }
}
}