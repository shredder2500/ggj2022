using System;
using DefaultNamespace;
using UnityEngine;

namespace AI.Pathing {
public class PathObstacle : MonoBehaviour {
  [SerializeField] private bool isStatic = true;

  private PathGrid _pathGrid;

  private GridNode _node;
  
  private void Start() {
    _pathGrid = FindObjectOfType<PathGrid>();
    Debug.Assert(_pathGrid);
    _node = _pathGrid.GetNode(transform.position2D());
    _node?.Block();
  }

  private void OnDisable() {
    _node?.UnBlock();
  }

  private void Update() {
    if (isStatic) return;

    var node = _pathGrid.GetNode(transform.position2D());
    if (_node == node) return;
    
    _node?.UnBlock();
    node?.Block();
    _node = node;
  }
}
}