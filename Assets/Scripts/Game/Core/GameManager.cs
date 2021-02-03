﻿using UnityEngine;
using System;

using Zongband.Game.AI;
using Zongband.Game.Boards;
using Zongband.Game.Turns;
using Zongband.Game.Actions;
using Zongband.Game.Entities;

namespace Zongband.Game.Core
{
    public class GameManager : MonoBehaviour
    {
        public Agent playerPrefab;
        public Agent agentPrefab;
        public Entity entityPrefab;
        public TileSO floorTile;
        public TileSO wallTile;

        public Board board;
        public TurnManager turnManager;
        public AgentAI aiController;

        public Agent playerAgent { get; private set; }

        private void Awake()
        {
            if (playerPrefab == null) throw new NullReferenceException();
            if (agentPrefab == null) throw new NullReferenceException();
            if (entityPrefab == null) throw new NullReferenceException();
            if (floorTile == null) throw new NullReferenceException();
            if (wallTile == null) throw new NullReferenceException();

            if (board == null) throw new NullReferenceException();
            if (turnManager == null) throw new NullReferenceException();
            if (aiController == null) throw new NullReferenceException();
        }

        public void SetupExample()
        {
            playerAgent = Spawn(playerPrefab.GetEntity(), new Vector2Int(3, 3)).GetAgent();
            Spawn(agentPrefab.GetEntity(), new Vector2Int(3, 5));
            Spawn(agentPrefab.GetEntity(), new Vector2Int(4, 5));
            Spawn(agentPrefab.GetEntity(), new Vector2Int(5, 5));
            Spawn(agentPrefab.GetEntity(), new Vector2Int(6, 5));
            Spawn(entityPrefab, new Vector2Int(3, 7));

            Vector2Int upRight = board.size - Vector2Int.one;
            Vector2Int downRight = new Vector2Int(board.size.x - 1, 0);
            Vector2Int downLeft = Vector2Int.zero;
            Vector2Int upLeft = new Vector2Int(0, board.size.y - 1);
            board.ModifyBoxTerrain(downLeft, upRight, floorTile);
            board.ModifyBoxTerrain(upLeft, upRight + new Vector2Int(0, -1), wallTile);
            board.ModifyBoxTerrain(upRight, downRight + new Vector2Int(-1, 0), wallTile);
            board.ModifyBoxTerrain(downRight, downLeft + new Vector2Int(0, 1), wallTile);
            board.ModifyBoxTerrain(downLeft, upLeft + new Vector2Int(1, 0), wallTile);

            if (!IsPlayerTurn()) ProcessAITurns();
        }

        public void ProcessPlayerTurn(ActionPack actionPack)
        {
            if (!IsPlayerTurn()) throw new IsNotPlayerTurnException();

            ApplyActionPack(actionPack);
            turnManager.Next();

            if (!IsPlayerTurn()) ProcessAITurns();
        }

        private void ProcessAITurns()
        {
            if (IsPlayerTurn()) throw new IsPlayerTurnException();

            while (!IsPlayerTurn())
            {
                Agent agent = turnManager.GetCurrent();

                ActionPack actionPack = aiController.GenerateActionPack(agent, board);
                ApplyActionPack(actionPack);

                turnManager.Next();
            }
        }

        private bool IsPlayerTurn()
        {
            if (playerAgent == null) throw new NullReferenceException();

            return turnManager.GetCurrent() == playerAgent;
        }

        private void ApplyActionPack(ActionPack actionPack)
        {
            if (actionPack == null) throw new ArgumentNullException();

            foreach (MovementAction movementAction in actionPack.GetMovementActions())
            {
                ApplyAction(movementAction);
            }
        }

        private void ApplyAction(MovementAction movementAction)
        {
            if (movementAction == null) throw new ArgumentNullException();

            if (movementAction.absolute) board.Move(movementAction.entity, movementAction.movement);
            else board.Displace(movementAction.entity, movementAction.movement);
        }

        private Entity Spawn(Entity entityPrefab, Vector2Int at)
        {
            if (entityPrefab == null) throw new ArgumentNullException();
            if (!board.IsPositionValid(at)) throw new ArgumentOutOfRangeException();

            Entity entity = Instantiate(entityPrefab, turnManager.transform);
            board.Add(entity, at);
            if (entity.IsAgent()) turnManager.Add(entity.GetAgent());
            return entity;
        }
    }
}
