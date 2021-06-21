#nullable enable

using UnityEngine;

namespace Zongband.Game.Commands
{
    public class NullCommand : Command
    {
        public NullCommand()
        {
            IsCompleted = true;
        }

        protected override bool ExecuteStart()
        {
            return true;
        }
    }
}