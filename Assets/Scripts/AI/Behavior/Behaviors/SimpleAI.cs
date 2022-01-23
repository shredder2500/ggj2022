using DefaultNamespace;
using UnityEngine;

namespace AI.Behavior {
public partial class BehaviorTree {
  public static BehaviorTree SimpleAI = Build(sel =>
    sel
      .Sequence(seq =>
        seq
          .Selector(sel2 => 
            sel2
              .MemoryContains("MainTarget")
              .Action(ai => {
                var mainTarget = GameObject.FindGameObjectWithTag("MainTarget");
                if (!mainTarget) return NodeState.Failure;
                ai.Memory.Set("MainTarget", mainTarget.transform.position2D());
                return NodeState.Success;
              }))
          .MemoryContains("MainTarget")
          .Selector(sel3 => 
            sel3
              .Sequence(seq2 => 
                seq2
                  .Action(ai => Vector2.Distance(ai.Memory.Get<Vector2>("MainTarget"), ai.transform.position2D()) < 2 ? NodeState.Success : NodeState.Failure)
                  .Attack("MainTarget"))
              .MoveTo("MainTarget"))
          ).LogWarning("AI doesn't know what to do.")
  );
}
}