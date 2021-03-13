#nullable enable

using UnityEngine;
using System.Collections.Generic;

namespace Zongband.Game.Actions
{
    public class ParallelAction : CombinedAction
    {
        private readonly LinkedList<Action> Actions = new LinkedList<Action>();

        protected override bool ProcessStart()
        {
            return ProcessAll();
        }

        protected override bool ProcessUpdate()
        {
            return ProcessAll();
        }

        private bool ProcessAll()
        {
            var node = Actions.First;
            while (node != null)
            {
                var next = node.Next;
                node.Value.Process();
                if (node.Value.IsCompleted) Actions.Remove(node);
                node = next;
            }
            return Actions.Count == 0;
        }

        public override void Add(Action action)
        {
            base.Add(action);
            if (!action.IsCompleted) Actions.AddLast(action);
        }
    }
}