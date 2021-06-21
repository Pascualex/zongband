#nullable enable

using UnityEngine;

namespace Zongband.Game.Commands
{
    public abstract class CombinedCommand : Command
    {
        public CombinedCommand()
        {
            IsCompleted = true;
        }

        public virtual void Add(Command command)
        {
            if (!command.IsCompleted) IsCompleted = false;
        }
    }
}