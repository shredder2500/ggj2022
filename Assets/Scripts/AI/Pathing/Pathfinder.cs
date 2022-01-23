using System;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;
using UnityEngine;

namespace AI.Pathing {
public class Pathfinder {
  private readonly PathGrid _pathGrid;

  private readonly Dictionary<GridNode, float> _costSoFar = new ();
  private readonly Dictionary<GridNode, GridNode> _cameFrom = new ();
  private readonly SimplePriorityQueue<GridNode, float> _frontier = new ();

  public Pathfinder(PathGrid pathGrid) => _pathGrid = pathGrid;

  public GridNode[] GetPath(Vector2 start, Vector2 end) {
    var firstNode = _pathGrid.GetNode(start);
    var lastNode = _pathGrid.GetNode(end);

    if (firstNode == null || lastNode == null) return Array.Empty<GridNode>();
    
    _costSoFar.Clear();
    _cameFrom.Clear();
    _frontier.Clear();
    
    CalculatePathNodes(firstNode, lastNode);

    return BuildPath(lastNode).Reverse().ToArray();
  }

  private IEnumerable<GridNode> BuildPath(GridNode lastNode) {
    var current = _cameFrom.Keys.OrderBy(x => Heuristic(x, lastNode)).First();

    yield return current;

    while (_cameFrom.ContainsKey(current)) {
      var next = _cameFrom[current];
      yield return next;
      current = next;
    }
  }

  private void CalculatePathNodes(GridNode firstNode, GridNode lastNode) {
    _frontier.Enqueue(firstNode, 0);
    _costSoFar.Add(firstNode, 0);
    GridNode current = default;

    while (_frontier.Count > 0 && current != lastNode) {
      current = _frontier.Dequeue();
      if (current == lastNode)
        break;
      var currentCost = GetCostSoFar(current);

      foreach (var next in GetNeighbors(current)) {
        var newCost = currentCost + next.Cost;
        var containsNext = _costSoFar.ContainsKey(next);
        if (!containsNext || newCost < _costSoFar[next]) {
          if (!containsNext)
            _costSoFar.Add(next, newCost);
          else
            _costSoFar[next] = newCost;

          var priority = newCost + Heuristic(lastNode, next);
          _frontier.Enqueue(next, priority);
          if (containsNext)
            _cameFrom[next] = current;
          else {
            _cameFrom.Add(next, current);
          }
        }
      }
    }
  }

  private float Heuristic(GridNode a, GridNode b) => Mathf.Abs(a.X - b.X) + Mathf.Abs(a.Y - b.Y);

  private float GetCostSoFar(GridNode node) {
    if (_costSoFar.TryGetValue(node, out var cost)) {
      return cost;
    }

    return 0;
  }

  private IEnumerable<GridNode> GetNeighbors(GridNode node) => new[] {
      _pathGrid.GetNode(node.X - 1, node.Y),
      _pathGrid.GetNode(node.X + 1, node.Y),
      _pathGrid.GetNode(node.X, node.Y - 1),
      _pathGrid.GetNode(node.X, node.Y + 1),
      // _pathGrid.GetNode(node.X - 1, node.Y - 1),
      // _pathGrid.GetNode(node.X - 1, node.Y + 1),
      // _pathGrid.GetNode(node.X + 1, node.Y - 1),
      // _pathGrid.GetNode(node.X + 1, node.Y + 1)
    }
    .Where(n => n is { Blocked: false });

}
}