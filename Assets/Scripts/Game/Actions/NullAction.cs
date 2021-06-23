#nullable enable

using UnityEngine;

namespace Zongband.Game.Actions
{
    public class NullAction : Action
    {
        public NullAction()
        {
            IsCompleted = true;
        }

        protected override bool ExecuteStart()
        {
            return true;
        }
    }
}