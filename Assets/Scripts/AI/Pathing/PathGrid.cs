using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AI.Pathing {
public class PathGrid : MonoBehaviour {
  [SerializeField] private Vector2Int size = new Vector2Int(100, 100);

  public Vector2Int Size => size;
  private Dictionary<Vector2Int, GridNode> _nodes;

  private void Awake() {
    
    _nodes = Enumerable.Range(0, size.x * size.y).Select(i => {
      var y = i / size.x;
      var x = i - y * size.x;
      return new GridNode(x, y, 0, false);
    }).ToDictionary(n => new Vector2Int(n.X, n.Y));
  }

  public GridNode GetNode(Vector2 point) => GetNode(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y));

  public GridNode GetNode(int x, int y) => _nodes.TryGetValue(new(x, y), out var node) ? node : null;

  private void OnDrawGizmosSelected() {
    for (var x = -.5f; x < size.x; x++) {
      for (var y = -.5f; y < size.y; y++) {
        if (_nodes != null) {
          var node = GetNode(new(x + .5f, y + .5f));
          if (node is { Blocked: true }) {
            var dColor = Gizmos.color;
            Gizmos.color = Color.red;
            Gizmos.DrawCube(node.Point, Vector3.one);
            Gizmos.color = dColor;
          }
        }

        if (x + 1 < size.x)
          Gizmos.DrawLine(new (x, y, 0), new (x + 1, y, 0));
        if (y + 1 < size.y)
          Gizmos.DrawLine(new (x, y, 0), new (x, y + 1, 0));
      }
    }
  }
}
}