// // using UnityEngine;

// using Zongband.Games.Logic.Boards;
// using Zongband.Games.Logic.Entities;
// using Zongband.Utils;

// namespace Zongband.Games.Logic.Actions
// {
//     public class CreateAction : Action
//     {
//         public Entity Entity { get; private set; }

//         private readonly Tile Tile;
//         private readonly Context Ctx;

//         public CreateAction(EntitySO entitySO, Tile tile, Context ctx)
//         {
//             Ctx = ctx;
//             Tile = tile;

//             Entity = Spawn(entitySO);
//         }

//         protected override bool ExecuteStart()
//         {
//             if (!AddToBoard(Entity))
//             {
//                 GameObject.Destroy(Entity);
//                 return true;
//             }

//             MoveToSpawn(Entity);
//             Entity.gameObject.SetActive(true);
//             Entity.OnSpawn();
//             return true;
//         }

//         private Entity Spawn(EntitySO entitySO)
//         {
//             // TODO: var parent = Ctx.TurnManager.transform;
//             var entity = GameObject.Instantiate(Ctx.AgentPrefab);
//             entity.ApplySO(entitySO);
//             entity.gameObject.SetActive(false);
//             return entity;
//         }

//         private bool AddToBoard(Entity entity)
//         {
//             if (!Ctx.Board.IsTileAvailable(entity, Tile, false)) return false;
//             Ctx.Board.Add(entity, Tile);
//             return true;
//         }

//         private void MoveToSpawn(Entity entity)
//         {
//             var spawnPosition = entity.Tile.ToWorld(Ctx.Board.Scale, Ctx.Board.Position);
//             entity.transform.position = spawnPosition;
//         }
//     }
// }