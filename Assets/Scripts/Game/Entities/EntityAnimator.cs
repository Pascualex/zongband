#nullable enable

using UnityEngine;
using System;
using System.Collections.Generic;

namespace Zongband.Game.Entities
{
    public class EntityAnimator : MonoBehaviour
    {
        [SerializeField] private Animator? animator;

        private readonly LinkedList<AnimationState> attackStates = new LinkedList<AnimationState>();

        public AnimationState Attack()
        {
            if (animator == null) throw new ArgumentNullException(nameof(animator));

            animator.SetTrigger("Attack");
            var state = new AnimationState();
            attackStates.AddLast(state);
            return state;
        }

        public void ReadyAttack()
        {
            if (animator == null) throw new ArgumentNullException(nameof(animator));

            animator.ResetTrigger("Attack");
            foreach (var state in attackStates) state.isReady = true;
        }

        public void CompleteAttack()
        {
            while (attackStates.Count > 0)
            {
                if (!attackStates.First.Value.isReady) break;
                attackStates.First.Value.isCompleted = true;
                attackStates.RemoveFirst();
            }
        }

        public class AnimationState
        {
            public bool isReady = false;
            public bool isCompleted = false;
        }
    }
}
