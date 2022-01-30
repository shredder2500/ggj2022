using System;
using System.Linq;
using AI.Behavior;
using AI.Pathing;
using UnityEngine;

namespace DefaultNamespace.AI {
public class SpawnPoint : MonoBehaviour {
  [SerializeField] private PathGridObj grid;
  [SerializeField] private Transform target;

  private Pathfinder _pathfinder;
  private GridNode[] _path;

  private void Start() {
    _pathfinder = new Pathfinder(grid.Grid);
  }

  private bool HasValidPath() => _path is { Length: > 0 } && _path.All(x => !x.Blocked);

  private void GeneratePath() => _pathfinder.GetPath(transform.position2D(), target.position2D(), out _path);

  public Bot Spawn(Enemy botPrefab) {
    if (!HasValidPath()) {
      Debug.Log("Generating path for spawn point");
      GeneratePath();
    }
    var bot = Instantiate(botPrefab, transform.position2D(), Quaternion.identity);
    bot.Memory.Set("Path", _path);
    bot.Memory.Set("Target", target);
    bot.Memory.Set("PathComplete", (0, 0f));
    bot.Memory.Set("PathStart", bot.transform.position2D());
    bot.Init(grid.Grid, 0);
    return bot;
  }
  
  private void OnDrawGizmosSelected() {
    if (_path == null) return;
    for(var i = 0; i < _path.Length - 1; i++)
      Gizmos.DrawLine(_path[i].Point, _path[i + 1].Point);
  }
}
}