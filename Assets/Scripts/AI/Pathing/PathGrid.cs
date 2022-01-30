using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI.Pathing {
public class PathGrid {
  private Dictionary<Vector2Int, GridNode> _nodes;

  public PathGrid(Vector2 center, Vector2Int size) {
    var startX = Mathf.RoundToInt(center.x) - size.x / 2;
    var startY = Mathf.RoundToInt(center.y) - size.y / 2;
    _nodes = Enumerable.Range(0, size.x * size.y).Select(i => {
      var y = i / size.x;
      var x = i - y * size.x;
      return new GridNode(startX + x, startY + y, 1, false);
    }).ToDictionary(n => new Vector2Int(n.X, n.Y));
  }
  
  public GridNode GetNode(Vector2 point) => GetNode(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y));

  public GridNode GetNode(int x, int y) => _nodes.TryGetValue(new(x, y), out var node) ? node : null;

  public PathGrid Clone() =>
    new(_nodes.ToDictionary(x => x.Key, x => new GridNode(x.Value.X, x.Value.Y, x.Value.Cost, x.Value.Blocked)));

  private PathGrid(Dictionary<Vector2Int, GridNode> nodes) => _nodes = nodes;

  public void DrawGizmos() {
    var dColor = Gizmos.color;
    foreach (var node in _nodes.Values) {
      
      if (node.Blocked) {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new (node.Point.x, node.Point.y), Vector3.one); 
        Gizmos.color = dColor;
      }
      else {
        Gizmos.DrawWireCube(new (node.Point.x, node.Point.y), Vector3.one);
      }
    }

  }
}
}