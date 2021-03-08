#nullable enable

using UnityEngine;
using System;
using System.Collections.Generic;

using Zongband.Game.Controllers;
using Zongband.Game.Actions;
using Zongband.Game.Generation;
using Zongband.Game.Turns;
using Zongband.Game.Boards;
using Zongband.Game.Entities;
using Zongband.Utils;

using Action=Zongband.Game.Actions.Action;

namespace Zongband.Game.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private AgentSO? playerAgentSO;
        [SerializeField] private AgentSO? fastAgentSO;
        [SerializeField] private AgentSO? normalAgentSO;
        [SerializeField] private AgentSO? slowAgentSO;
        [SerializeField] private AgentSO? notRoamerAgentSO;
        [SerializeField] private EntitySO? boxEntitySO;

        public PlayerController? playerController;
        public AIController? aiController;
        public DungeonGenerator? dungeonGenerator;
        public TurnManager? turnManager;
        public Board? board;
        public Agent? agentPrefab;
        public Entity? entityPrefab;

        public Agent? LastPlayer { get; private set; }

        private Action currentAction = new NullAction();

        public void SetupExample1()
        {
            if (playerAgentSO == null) throw new ArgumentNullException(nameof(playerAgentSO));
            if (fastAgentSO == null) throw new ArgumentNullException(nameof(fastAgentSO));
            if (normalAgentSO == null) throw new ArgumentNullException(nameof(normalAgentSO));
            if (slowAgentSO == null) throw new ArgumentNullException(nameof(slowAgentSO));
            if (notRoamerAgentSO == null) throw new ArgumentNullException(nameof(notRoamerAgentSO));
            if (boxEntitySO == null) throw new ArgumentNullException(nameof(boxEntitySO));

            if (playerController == null) throw new ArgumentNullException(nameof(playerController));
            if (dungeonGenerator == null) throw new ArgumentNullException(nameof(dungeonGenerator));
            if (aiController == null) throw new ArgumentNullException(nameof(aiController));
            if (turnManager == null) throw new ArgumentNullException(nameof(turnManager));
            if (board == null) throw new ArgumentNullException(nameof(board));
            if (agentPrefab == null) throw new ArgumentNullException(nameof(agentPrefab));
            if (entityPrefab == null) throw new ArgumentNullException(nameof(entityPrefab));

            var ctx = new Action.Context(turnManager, board, agentPrefab, entityPrefab);
            var newAction = new ParallelAction();

            var boardData = dungeonGenerator.GenerateTestDungeon(board.Size, 2);
            if (boardData == null) throw new NullReferenceException();
            board.Apply(boardData);

            var playerAction = new SequentialAction();
            var spawnPlayerAction = new SpawnAction(playerAgentSO, new Tile(3, 3), ctx, true);
            playerAction.Add(spawnPlayerAction);
            playerAction.Add(new MakePlayerAction(spawnPlayerAction));
            newAction.Add(playerAction);

            // newAction.Add(new SpawnAction(fastAgentSO, new Tile(3, 5), ctx));
            newAction.Add(new SpawnAction(normalAgentSO, new Tile(4, 5), ctx));
            newAction.Add(new SpawnAction(normalAgentSO, new Tile(5, 5), ctx));
            newAction.Add(new SpawnAction(slowAgentSO, new Tile(6, 5), ctx));
            newAction.Add(new SpawnAction(notRoamerAgentSO, new Tile(9, 5), ctx));
            newAction.Add(new SpawnAction(notRoamerAgentSO, new Tile(10, 6), ctx));
            newAction.Add(new SpawnAction(notRoamerAgentSO, new Tile(11, 5), ctx));
            newAction.Add(new SpawnAction(boxEntitySO, new Tile(3, 7), ctx));

            currentAction = newAction;
        }

        public void SetupExample2()
        {
            if (playerAgentSO == null) throw new ArgumentNullException(nameof(playerAgentSO));

            if (dungeonGenerator == null) throw new ArgumentNullException(nameof(dungeonGenerator));
            if (turnManager == null) throw new ArgumentNullException(nameof(turnManager));
            if (board == null) throw new ArgumentNullException(nameof(board));
            if (agentPrefab == null) throw new ArgumentNullException(nameof(agentPrefab));
            if (entityPrefab == null) throw new ArgumentNullException(nameof(entityPrefab));

            var ctx = new Action.Context(turnManager, board, agentPrefab, entityPrefab);
            var newAction = new ParallelAction();

            var boardData = dungeonGenerator.GenerateDungeon(board.Size, 200, 4, 20, 2);
            if (boardData == null) throw new NullReferenceException();

            board.Apply(boardData);

            var playerAction = new SequentialAction();
            var spawnPlayerAction = new SpawnAction(playerAgentSO, boardData.PlayerSpawn, ctx, true);
            playerAction.Add(spawnPlayerAction);
            playerAction.Add(new MakePlayerAction(spawnPlayerAction));
            newAction.Add(playerAction);

            currentAction = newAction;
        }

        public void GameLoop()
        {
            if (currentAction.IsCompleted) currentAction = ProcessTurns();
            else currentAction.Process();
        }

        public Action ProcessTurns()
        {
            if (playerController == null) throw new ArgumentNullException(nameof(playerController));
            if (aiController == null) throw new ArgumentNullException(nameof(aiController));
            if (turnManager == null) throw new ArgumentNullException(nameof(turnManager));
            if (board == null) throw new ArgumentNullException(nameof(board));
            if (agentPrefab == null) throw new ArgumentNullException(nameof(agentPrefab));
            if (entityPrefab == null) throw new ArgumentNullException(nameof(entityPrefab));

            var ctx = new Action.Context(turnManager, board, agentPrefab, entityPrefab);

            var turnAction = new ParallelAction();
            var processedAgents = new HashSet<Agent>();
            Agent? agent;
            while (((agent = turnManager.GetCurrent()) != null) && !processedAgents.Contains(agent))
            {
                Action? agentAction;
                if (agent.isPlayer)
                {
                    LastPlayer = agent;
                    agentAction = playerController.ProduceAction(agent, ctx);
                }
                else agentAction = aiController.ProduceAction(agent, ctx);

                if (agentAction == null) break;

                agentAction.Process();

                if (!agentAction.IsCompleted) turnAction.Add(agentAction);

                processedAgents.Add(agent);
                turnManager.Next();

                if (!(agentAction is MovementAction) && !(agentAction is NullAction)) break;
            }

            return turnAction;
        }
    }
}
