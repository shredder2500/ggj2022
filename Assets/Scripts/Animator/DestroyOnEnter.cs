using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace {
public class DestroyOnEnter : StateMachineBehaviour {
  [SerializeField] private bool fade;
  
  public override void OnStateEnter(UnityEngine.Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    if (!fade) {
      Destroy(animator.gameObject);
    }
    else {
      animator.GetComponent<SpriteRenderer>().DOFade(0, .3f).OnComplete(() => Destroy(animator.gameObject));
    }
  }
}
}