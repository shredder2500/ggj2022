using System;
using System.Collections.Generic;
using System.Linq;
using AI.Pathing;
using DefaultNamespace.AI;
using DefaultNamespace.UI;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace {
public class TowerBuilder : MonoBehaviour {
  [SerializeField] private float buildRadius;
  [SerializeField] private UnityEvent onBuild;
  [SerializeField] private TowerBuild buildPrefab;
  [SerializeField] private AudioSource audioSource;
  [SerializeField] private AudioClip cantPlaceClip;
  [SerializeField] private AudioClip placedClip;
  private TowerBuild _towerBuild;
  private Vector2 _buildPos;
  private MoneyManager _moneyManager;
  private PathGrid _grid;
  private SpawnPoint[] _spawns;
  private Transform _target;

  private void Start() {
    _moneyManager = FindObjectOfType<MoneyManager>();
    _grid = FindObjectOfType<PathGridObj>().Grid;
    _spawns = FindObjectsOfType<SpawnPoint>();
    _target = GameObject.FindWithTag("MainTarget").transform;
  }

  public void SetTower(Tower tower) {
    if (!_towerBuild)
      _towerBuild = Instantiate(buildPrefab, transform, true);
    _towerBuild.gameObject.SetActive(true);
    _towerBuild.SetTowerPrefab(tower);
  }

  public void SetBuildPos(Vector2 pos) {
    if (_towerBuild == null) return;
    _towerBuild.transform.position = new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
    _buildPos = pos - transform.position2D();
  }

  private void Update() {
    if (_towerBuild == null || !_towerBuild.isActiveAndEnabled) return;
    SetBuildPos(transform.position2D() + _buildPos);
    var reason = CanBuild();
    _towerBuild.UpdateCanBuild(string.IsNullOrEmpty(reason), reason);
  }

  private readonly List<UpgradeMenu> _upgradeMenus = new();
  public void Build() {
    if (!_towerBuild || !_towerBuild.TowerPrefab) return;
    if (!string.IsNullOrEmpty(CanBuild())) {
      audioSource.PlayOneShot(cantPlaceClip);
      return;
    }
    var tower = _towerBuild.Build();
    if (!tower) return;
    _moneyManager.Remove(_towerBuild.Cost);
    onBuild.Invoke();
    audioSource.PlayOneShot(placedClip);
    var upgradeMenu = tower.GetComponentInChildren<UpgradeMenu>();
    if (!upgradeMenu) return;
    _upgradeMenus.Add(upgradeMenu);
    
  }

  public void DisableUpgrades() {
    foreach (var upgradeMenu in _upgradeMenus) {
      upgradeMenu.enabled = false;
    }
  }

  public void EnableUpgrades() {
    foreach (var upgradeMenu in _upgradeMenus) {
      upgradeMenu.enabled = true;
    }
  }

  public void StopBuildMode() {
    if (!_towerBuild) return;
    _towerBuild.gameObject.SetActive(false);
  }

  private string CanBuild() {
    if (_moneyManager.Amount < _towerBuild.Cost) return "Not enough Coins";
    if (Vector2.Distance(_towerBuild.transform.position2D(), transform.position2D()) > buildRadius) return "To far away";
    var grid = _grid.Clone();
    var pathfinder = new Pathfinder(grid);
    var node = grid.GetNode(_towerBuild.transform.position2D());
    if (node == null) return "Out of bounds";
    if (node.Blocked) return "Blocked";
    node.Block();
    return _spawns.All(x => pathfinder.GetPath(x.transform.position2D(), _target.position2D()).Length > 0)
      ? string.Empty : "Blocks Path";
  }

  private void OnDrawGizmosSelected() {
    if (!_towerBuild || !_towerBuild.isActiveAndEnabled) return;
    var grid = _grid.Clone();
    grid.GetNode(_towerBuild.transform.position2D())?.Block();
    grid.DrawGizmos();
    var pathfinder = new Pathfinder(grid);

    foreach (var path in _spawns.Select(x => pathfinder.GetPath(x.transform.position2D(), _target.position2D()))) {
      for (var i = 0; i < path.Length - 1; i++) {
        var start = path[i];
        var end = path[i + 1];
        Gizmos.DrawLine(start.Point, end.Point);
      }
    }
  }
}
}