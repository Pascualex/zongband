#nullable enable

using UnityEngine;
using System.Collections.Generic;

using Zongband.Game.Controllers;
using Zongband.Game.Actions;
using Zongband.Game.Turns;
using Zongband.Game.Boards;
using Zongband.Game.Entities;
using Zongband.Utils;

namespace Zongband.Game.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private BoardSO? boardSO;
        [SerializeField] private TerrainSO? floorTerrainSO;
        [SerializeField] private TerrainSO? wallTerrainSO;
        [SerializeField] private AgentSO? playerAgentSO;
        [SerializeField] private AgentSO? fastAgentSO;
        [SerializeField] private AgentSO? normalAgentSO;
        [SerializeField] private AgentSO? slowAgentSO;
        [SerializeField] private AgentSO? notRoamerAgentSO;
        [SerializeField] private EntitySO? boxEntitySO;

        public PlayerController? playerController;
        public AIController? aiController;
        public TurnManager? turnManager;
        public Board? board;
        public Agent? agentPrefab;
        public Entity? entityPrefab;

        public Agent? LastPlayer { get; private set; }

        private Action currentAction = new NullAction();

        public void SetupExample()
        {
            if (boardSO == null) return;
            if (floorTerrainSO == null) return;
            if (wallTerrainSO == null) return;
            if (playerAgentSO == null) return;
            if (fastAgentSO == null) return;
            if (normalAgentSO == null) return;
            if (slowAgentSO == null) return;
            if (notRoamerAgentSO == null) return;
            if (boxEntitySO == null) return;

            if (playerController == null) return;
            if (aiController == null) return;
            if (turnManager == null) return;
            if (board == null) return;
            if (agentPrefab == null) return;
            if (entityPrefab == null) return;

            var context = new Action.Context(turnManager, board, agentPrefab, entityPrefab);

            var newAction = new ParallelAction();

            var playerAction = new SequentialAction();
            var spawnPlayerAction = new SpawnAction(playerAgentSO, new Tile(3, 3), context, true);
            playerAction.Add(spawnPlayerAction);
            playerAction.Add(new MakePlayerAction(spawnPlayerAction));
            newAction.Add(playerAction);

            // newAction.Add(new SpawnAction(fastAgentSO, new Tile(3, 5), context));
            newAction.Add(new SpawnAction(normalAgentSO, new Tile(4, 5), context));
            newAction.Add(new SpawnAction(normalAgentSO, new Tile(5, 5), context));
            newAction.Add(new SpawnAction(slowAgentSO, new Tile(6, 5), context));
            newAction.Add(new SpawnAction(notRoamerAgentSO, new Tile(9, 5), context));
            newAction.Add(new SpawnAction(notRoamerAgentSO, new Tile(10, 6), context));
            newAction.Add(new SpawnAction(notRoamerAgentSO, new Tile(11, 5), context));
            newAction.Add(new SpawnAction(boxEntitySO, new Tile(3, 7), context));

            currentAction = newAction;

            // TODO: maybe terrain modification should also be done through actions
            var upRight = new Tile(board.Size.x - 1, board.Size.y - 1);
            var downRight = new Tile(board.Size.x - 1, 0);
            var downLeft = Tile.Zero;
            var upLeft = new Tile(0, board.Size.y - 1);
            board.ModifyBoxTerrain(downLeft, upRight, floorTerrainSO);
            board.ModifyBoxTerrain(upLeft, upRight + new Tile(0, -1), wallTerrainSO);
            board.ModifyBoxTerrain(upRight, downRight + new Tile(-1, 0), wallTerrainSO);
            board.ModifyBoxTerrain(downRight, downLeft + new Tile(0, 1), wallTerrainSO);
            board.ModifyBoxTerrain(downLeft, upLeft + new Tile(1, 0), wallTerrainSO);
        }

        public void GameLoop()
        {
            if (currentAction.IsCompleted) currentAction = ProcessTurns();
            else currentAction.Process();
        }

        public Action ProcessTurns()
        {
            if (playerController == null) return new NullAction();
            if (aiController == null) return new NullAction();
            if (turnManager == null) return new NullAction();
            if (board == null) return new NullAction();
            if (agentPrefab == null) return new NullAction();
            if (entityPrefab == null) return new NullAction();

            var context = new Action.Context(turnManager, board, agentPrefab, entityPrefab);

            var turnAction = new ParallelAction();
            var processedAgents = new HashSet<Agent>();
            Agent? agent;
            while (((agent = turnManager.GetCurrent()) != null) && !processedAgents.Contains(agent))
            {
                Action? agentAction;
                if (agent.isPlayer)
                {
                    LastPlayer = agent;
                    agentAction = playerController.ProduceAction(agent, context);
                }
                else agentAction = aiController.ProduceAction(agent, context);

                if (agentAction == null) break;

                agentAction.Process();

                if (!agentAction.IsCompleted) turnAction.Add(agentAction);

                processedAgents.Add(agent);
                turnManager.Next();

                if (!(agentAction is MovementAction)) break;
            }

            return turnAction;
        }
    }
}
