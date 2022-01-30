using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AI.Pathing {
[ExecuteInEditMode]
public class PathGridObj : MonoBehaviour {
  [SerializeField] private Vector2Int size;

  private PathGrid _grid;
  public PathGrid Grid => _grid;

  private void Awake() {
    _grid = new(transform.position2D(), size);
  }

  private void OnDrawGizmosSelected() => _grid?.DrawGizmos();
}
}