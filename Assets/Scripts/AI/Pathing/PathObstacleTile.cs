using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AI.Pathing {
public class PathObstacleTile : Tile {
  

#if UNITY_EDITOR

  [MenuItem("Assets/Create/ObstacleTile")]
  public static void Create() {
    string path = EditorUtility.SaveFilePanelInProject("Save Obstacle Tile", "New Obstacle Tile", "Asset",
      "Save Obstacle Tile", "Assets/Art/Tiles");
    if (path == "")
      return;
    AssetDatabase.CreateAsset(CreateInstance<PathObstacleTile>(),path);
  }
  
  #endif
}
}