#nullable enable

using UnityEngine;
using System.Collections.Generic;

namespace Zongband.Game.Commands
{
    public class ParallelCommand : CombinedCommand
    {
        private readonly LinkedList<Command> Commands = new LinkedList<Command>();

        protected override bool ExecuteStart()
        {
            return ProcessAll();
        }

        protected override bool ExecuteUpdate()
        {
            return ProcessAll();
        }

        private bool ProcessAll()
        {
            var node = Commands.First;
            while (node != null)
            {
                var next = node.Next;
                node.Value.Execute();
                if (node.Value.IsCompleted) Commands.Remove(node);
                node = next;
            }
            return Commands.Count == 0;
        }

        public override void Add(Command command)
        {
            base.Add(command);
            if (!command.IsCompleted) Commands.AddLast(command);
        }
    }
}