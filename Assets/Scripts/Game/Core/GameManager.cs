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

using Random = UnityEngine.Random;
using Action = Zongband.Game.Actions.Action;

namespace Zongband.Game.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private AgentSO? playerAgentSO;
        [SerializeField] private AgentSO[]? enemiesSOs;
        [SerializeField] private EntitySO? boxEntitySO;

        public PlayerController? playerController;
        public AIController? aiController;
        public DungeonGenerator? dungeonGenerator;
        public DungeonVisualizer? dungeonVisualizer;
        public TurnManager? turnManager;
        public Board? board;
        public Agent? agentPrefab;
        public Entity? entityPrefab;

        public Agent? LastPlayer { get; private set; }

        private Action currentAction = new NullAction();

        public void SetupExample1()
        {
            if (playerAgentSO == null) throw new ArgumentNullException(nameof(playerAgentSO));
            if (enemiesSOs == null) throw new ArgumentNullException(nameof(enemiesSOs));
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

            var dungeonData = dungeonGenerator.GenerateTestDungeon(board.Size, 2);
            board.Apply(dungeonData.ToBoardData());

            var playerAction = new SequentialAction();
            var spawnPlayerAction = new SpawnAction(playerAgentSO, dungeonData.playerSpawn, ctx, true);
            playerAction.Add(spawnPlayerAction);
            playerAction.Add(new MakePlayerAction(spawnPlayerAction));
            newAction.Add(playerAction);

            for (var i = 0; i < enemiesSOs.Length; i++)
                newAction.Add(new SpawnAction(enemiesSOs[i], new Tile(3 + i, 5), ctx));
            newAction.Add(new SpawnAction(boxEntitySO, new Tile(3, 7), ctx));

            currentAction = newAction;
        }

        public void SetupExample2()
        {
            if (playerAgentSO == null) throw new ArgumentNullException(nameof(playerAgentSO));
            if (enemiesSOs == null) throw new ArgumentNullException(nameof(enemiesSOs));

            if (dungeonGenerator == null) throw new ArgumentNullException(nameof(dungeonGenerator));
            if (dungeonVisualizer == null) throw new ArgumentNullException(nameof(dungeonVisualizer));
            if (turnManager == null) throw new ArgumentNullException(nameof(turnManager));
            if (board == null) throw new ArgumentNullException(nameof(board));
            if (agentPrefab == null) throw new ArgumentNullException(nameof(agentPrefab));
            if (entityPrefab == null) throw new ArgumentNullException(nameof(entityPrefab));

            var ctx = new Action.Context(turnManager, board, agentPrefab, entityPrefab);
            var newAction = new ParallelAction();

            var dungeonData = dungeonGenerator.GenerateDungeon(board.Size, 20, 4, 8, 4);
            dungeonVisualizer.DungeonData = dungeonData;
            if (dungeonData == null) throw new NullReferenceException();

            board.Apply(dungeonData.ToBoardData());

            var playerAction = new SequentialAction();
            var spawnPlayerAction = new SpawnAction(playerAgentSO, dungeonData.playerSpawn, ctx, true);
            playerAction.Add(spawnPlayerAction);
            playerAction.Add(new MakePlayerAction(spawnPlayerAction));
            newAction.Add(playerAction);

            if (enemiesSOs.Length > 0)
            {
                foreach (var spawn in dungeonData.enemiesSpawn)
                {
                    var enemy = enemiesSOs[Random.Range(0, enemiesSOs.Length)];
                    newAction.Add(new SpawnAction(enemy, spawn, ctx));
                }
            }

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
