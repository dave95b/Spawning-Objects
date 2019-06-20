using System;
using System.Collections.Generic;

namespace Core
{
    public class ActionSource<TTarget>
    {
        private readonly Dictionary<TTarget, Action> actions = new Dictionary<TTarget, Action>();
        private readonly Func<TTarget, Action> factory;

        public ActionSource(Func<TTarget, Action> factory)
        {
            this.factory = factory;
        }

        public Action this[TTarget target]
        {
            get
            {
                if (actions.TryGetValue(target, out var action))
                    return action;

                action = factory(target);
                actions[target] = action;

                return action;
            }
        }
    }
}