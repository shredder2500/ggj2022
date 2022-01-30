using System;
using UnityEngine;

namespace DefaultNamespace {
public class Reward : MonoBehaviour {
  [SerializeField] private int reward = 1;
  public void RewardPlayer() {
    FindObjectOfType<MoneyManager>().Add(reward);
  }
}
}