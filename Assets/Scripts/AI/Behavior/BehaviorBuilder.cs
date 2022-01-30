using System;
using System.Linq;

namespace AI.Behavior {
public abstract class BehaviorBuilder {
  private BehaviorBuilder[] _childBuilders;

  protected Node[] ChildNodes => _childBuilders.Select(x => x.BuildNode()).ToArray();
  public static BehaviorBuilder Selector(params BehaviorBuilder[] builders) {
    return new SelectorBuilder {
      _childBuilders = builders
    };
  }

  public static BehaviorBuilder Sequence(params BehaviorBuilder[] builders) {
    return new SequenceBuilder {
      _childBuilders = builders
    };
  }

  protected abstract Node BuildNode();

  public static implicit operator BehaviorTree(BehaviorBuilder builder) => new (builder.BuildNode());
}

public class SelectorBuilder : BehaviorBuilder {
  protected override Node BuildNode() => new Selector(ChildNodes);
}

public class SequenceBuilder : BehaviorBuilder {
  protected override Node BuildNode() => new Sequence(ChildNodes);
}

public class ActionNodeBuilder : BehaviorBuilder {
  private readonly Func<Bot, NodeState> _func;

  public ActionNodeBuilder(Func<Bot, NodeState> func) => _func = func;

  public ActionNodeBuilder(Action action) => _func = _ => {
    action();
    return NodeState.Success;
  };

  public ActionNodeBuilder(Action<Bot> action) => _func = bot => {
    action(bot);
    return NodeState.Success;
  };

  public ActionNodeBuilder(Func<NodeState> action) => _func = _ => action(); 

  protected override Node BuildNode() => new ActionNode(_func);
}
}