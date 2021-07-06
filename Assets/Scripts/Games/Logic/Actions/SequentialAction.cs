// #nullable enable

// using UnityEngine;
// using System.Collections.Generic;

// namespace Zongband.Games.Logic.Actions
// {
//     public class SequentialAction : CombinedAction
//     {
//         private readonly Queue<Action> Actions = new Queue<Action>();

//         protected override bool ExecuteStart()
//         {
//             return ProcessUntilNotCompleted();
//         }

//         protected override bool ExecuteUpdate()
//         {
//             return ProcessUntilNotCompleted(); 
//         }

//         private bool ProcessUntilNotCompleted()
//         {
//             while (Actions.Count != 0)
//             {
//                 Actions.Peek().Execute();
//                 if (!Actions.Peek().IsCompleted) return false;
//                 Actions.Dequeue();
//             }
//             return true;
//         }

//         public override void Add(Action action)
//         {
//             base.Add(action);
//             if (!action.IsCompleted) Actions.Enqueue(action);
//         }
//     }
// }