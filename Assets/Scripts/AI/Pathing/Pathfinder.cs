using System;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;
using UnityEngine;

namespace AI.Pathing {
public class Pathfinder {
  private readonly PathGrid _pathGrid;

  public Pathfinder(PathGrid pathGrid) => _pathGrid = pathGrid;

  private float Heuristic(GridNode a, GridNode b) => Vector2.Distance(a.Point, b.Point);

  private IEnumerable<GridNode> GetNeighbors(GridNode node) => new[] {
      _pathGrid.GetNode(node.X - 1, node.Y),
      _pathGrid.GetNode(node.X + 1, node.Y),
      _pathGrid.GetNode(node.X, node.Y - 1),
      _pathGrid.GetNode(node.X, node.Y + 1),
      _pathGrid.GetNode(node.X - 1, node.Y - 1),
      _pathGrid.GetNode(node.X - 1, node.Y + 1),
      _pathGrid.GetNode(node.X + 1, node.Y - 1),
      _pathGrid.GetNode(node.X + 1, node.Y + 1)
    }
    .Where(n => n is { Blocked: false });
  
//   function reconstruct_path(cameFrom, current)
  private GridNode[] ReconstructPath(Dictionary<GridNode, GridNode> cameFrom, GridNode endNode) {
    var current = endNode;
    // total_path := {current}
    IEnumerable<GridNode> totalPath = new[] { current };
    // while current in cameFrom.Keys:
    while (cameFrom.ContainsKey(current)) {
      // current := cameFrom[current]
      current = cameFrom[current];
      // total_path.prepend(current)
      totalPath = totalPath.Prepend(current);
    }
    // return total_path
    return totalPath.ToArray();
  }

  public GridNode[] GetPath(Vector2 start, Vector2 end) {
    GetPath(start, end, out var path);
    return path;
  }

  public bool GetPath(Vector2 start, Vector2 end, out GridNode[] path) =>
    GetPath(_pathGrid.GetNode(start), _pathGrid.GetNode(end), out path);
  // A* finds a path from start to goal.
  public bool GetPath(GridNode start, GridNode end, out GridNode[] path) {
    if (start == null || end == null) {
      path = Array.Empty<GridNode>();
      return false;
    }
    // // The set of discovered nodes that may need to be (re-)expanded.
    // // Initially, only the start node is known.
    // // This is usually implemented as a min-heap or priority queue rather than a hash-set.
    // openSet := {start}
    var openSet = new SimplePriorityQueue<GridNode, float>();
    openSet.Enqueue(start, 0);
    // // For node n, cameFrom[n] is the node immediately preceding it on the cheapest path from start
    // // to n currently known.
    // cameFrom := an empty map
    var cameFrom = new Dictionary<GridNode, GridNode>();
    // // For node n, gScore[n] is the cost of the cheapest path from start to n currently known.
    // gScore := map with default value of Infinity
    // gScore[start] := 0
    var gScore = new Dictionary<GridNode, float>();
    gScore.Add(start, 0);
    
    // // For node n, fScore[n] := gScore[n] + h(n). fScore[n] represents our current best guess as to
    // // how short a path from start to finish can be if it goes through n.
    // fScore := map with default value of Infinity
    // fScore[start] := h(start)
    var fScore = new Dictionary<GridNode, float>();
    fScore.Add(start, gScore[start] + Heuristic(start, end));

    // while openSet is not empty
    while (openSet.Count > 0) {
      //   // This operation can occur in O(1) time if openSet is a min-heap or a priority queue
      //   current := the node in openSet having the lowest fScore[] value
      // openSet.Remove(current)
      var current = openSet.Dequeue();
      // if current = goal
      if (current == end) {
        // return reconstruct_path(cameFrom, current)
        path = ReconstructPath(cameFrom, current);
        return true;
      }

      // for each neighbor of current
      foreach (var neighbor in GetNeighbors(current)) {
        // // d(current,neighbor) is the weight of the edge from current to neighbor
        // // tentative_gScore is the distance from start to the neighbor through current
        // tentative_gScore := gScore[current] + d(current, neighbor)
        var tentative_gScore = gScore[current] + D(current, neighbor);
        // if tentative_gScore < gScore[neighbor]
        if (!gScore.TryGetValue(neighbor, out var score)) {
          // // This path to neighbor is better than any previous one. Record it!
          // cameFrom[neighbor] := current
          cameFrom.Add(neighbor, current);
          // gScore[neighbor] := tentative_gScore
          gScore.Add(neighbor, tentative_gScore);
          // fScore[neighbor] := tentative_gScore + h(neighbor)
          fScore.Add(neighbor, tentative_gScore + Heuristic(neighbor, end));
          // if neighbor not in openSet
          // openSet.add(neighbor)
          openSet.Enqueue(neighbor, fScore[neighbor]);
        }
        else if (tentative_gScore < score) {
          // // This path to neighbor is better than any previous one. Record it!
          // cameFrom[neighbor] := current
          cameFrom[neighbor] = current;
          // gScore[neighbor] := tentative_gScore
          gScore[neighbor] = tentative_gScore;
          // fScore[neighbor] := tentative_gScore + h(neighbor)
          fScore[neighbor] = tentative_gScore + Heuristic(neighbor, end);
          // if neighbor not in openSet
          if (!openSet.Contains(neighbor)) {
            // openSet.add(neighbor)
            openSet.Enqueue(neighbor, tentative_gScore + Heuristic(neighbor, end));
          }
        }
      }
    }

    // // Open set is empty but goal was never reached
      // return failure
      path = Array.Empty<GridNode>();
      return false;
      float D(GridNode current, GridNode neighbor) => Vector2.Distance(current.Point, neighbor.Point) * neighbor.Cost;
    }
  }
}