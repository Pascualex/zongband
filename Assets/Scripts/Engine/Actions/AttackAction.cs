// // using UnityEngine;
// using System;

// using Zongband.Engine.Entities;

// namespace  Zongband.Engine.Actions
// {
//     public class AttackAction : Action
//     {
//         private readonly Agent Attacker;
//         private readonly Agent Target;
//         private readonly Parameters Prms;
//         private readonly Context Ctx;

//         public AttackAction(Agent attacker, Agent target, Parameters prms, Context ctx)
//         {
//             Attacker = attacker;
//             Target = target;
//             Prms = prms;
//             Ctx = ctx;
//         }

//         protected override bool ExecuteStart()
//         {
//             if (!CheckAlive(Target)) return true;

//             Damage();
//             CheckTargetDeath();
//             return true;
//         }

//         private void Damage()
//         {
//             var damage = (double)Prms.BaseDamage;
//             damage += Attacker.Attack * Prms.AttackMultiplier;
//             Target.Damage((int)Math.Round(damage));
//         }

//         private void CheckTargetDeath()
//         {
//             if (Target.Health == 0)
//             {
//                 Ctx.Board.Remove(Target);
//                 Ctx.TurnManager.Remove(Target);
//                 GameObject.Destroy(Target.gameObject);
//             }
//         }

//         [Serializable]
//         public class Parameters
//         {
//             public int BaseDamage = 0;
//             public float AttackMultiplier = 0f;

//             public void OnValidate()
//             {
//                 BaseDamage = Math.Max(0, BaseDamage);
//                 AttackMultiplier = Math.Max(0f, AttackMultiplier);
//             }
//         }
//     }
// }