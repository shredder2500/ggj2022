using System;
using System.Collections.Generic;
using System.Linq;
using AI.Pathing;
using DefaultNamespace;
using UnityEngine;

namespace AI.Behavior {
[RequireComponent(typeof(Movement))]
public class Bot : MonoBehaviour {
  [SerializeField] private AIBehavior behavior;
  private Memory _memory;
  private Pathfinder _pathfinder;
  private Movement _movement;
  
  public Memory Memory => _memory;

  private GridNode[] _path;
  private int _pathIdx;

  private void Start() {
    _memory = new Memory();
    _pathfinder = new Pathfinder(FindObjectOfType<PathGrid>());
    _movement = GetComponent<Movement>();
  }

  private void OnEnable() {
    switch (behavior) {
      case AIBehavior.Simple:
        BehaviorTree.SimpleAI.AddBot(this);
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  private void OnDisable() {
    _path = null;
    _memory.Clear();
    switch (behavior) {
      case AIBehavior.Simple:
        BehaviorTree.SimpleAI.RemoveBot(this);
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  private void Update() {
    Move();
  }

  private void Move() {
    if (_path == null || !_path.Any()) return;
    var gridNode = GetGridPoint();

    if (gridNode == null) {
      _path = null;
      _movement.SetMoveDir(Vector2.zero);
      return;
    }
    
    _movement.SetMoveDir(gridNode.Point - transform.position2D());

    GridNode GetGridPoint() {
      if (_pathIdx >= _path.Length) return null;
      var node = _path[_pathIdx];
      return Vector2.Distance(transform.position2D(), node.Point) < .1f ? GetNextGridPoint() : node;
    }

    GridNode GetNextGridPoint() {
      _pathIdx++;
      return GetGridPoint();
    }
  }

  public void MoveTo(Vector2 point) {
    _path = _pathfinder.GetPath(transform.position, point);
    _path = _path.Length > 1 ? _path : null;
    _pathIdx = 1;
  }

  public void StopMoving() => _path = null;

  private void OnDrawGizmosSelected() {
    if (_path == null) return;
    for(var i = 1; i < _path.Length - 1; i++)
      Gizmos.DrawLine(_path[i].Point, _path[i + 1].Point);
  }
}
}