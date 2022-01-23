using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace {
public class PlayerSpawn : MonoBehaviour {
  [SerializeField] private Transform[] playerSpawnLocations = new Transform[4];
  
  public void PlayerSpawned(PlayerInput input) {
    Debug.Log("Player Joined. Spawning");
    var mainTarget = GameObject.FindWithTag("MainTarget").transform;
    input.transform.position = playerSpawnLocations[input.playerIndex].position;
  }
}
}