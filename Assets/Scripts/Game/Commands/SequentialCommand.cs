#nullable enable

using UnityEngine;
using System.Collections.Generic;

namespace Zongband.Game.Commands
{
    public class SequentialCommand : CombinedCommand
    {
        private readonly Queue<Command> Commands = new Queue<Command>();

        protected override bool ExecuteStart()
        {
            return ProcessUntilNotCompleted();
        }

        protected override bool ExecuteUpdate()
        {
            return ProcessUntilNotCompleted(); 
        }

        private bool ProcessUntilNotCompleted()
        {
            while (Commands.Count != 0)
            {
                Commands.Peek().Execute();
                if (!Commands.Peek().IsCompleted) return false;
                Commands.Dequeue();
            }
            return true;
        }

        public override void Add(Command command)
        {
            base.Add(command);
            if (!command.IsCompleted) Commands.Enqueue(command);
        }
    }
}