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

using Action = Zongband.Game.Actions.Action;
using Random = UnityEngine.Random;

namespace Zongband.Game.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private AgentSO? PlayerAgentSO;
        [SerializeField] private AgentSO?[] EnemiesSOs = new AgentSO[0];
        [SerializeField] private EntitySO? BoxEntitySO;

        public PlayerController? PlayerController;
        public AIController? AIController;
        public DungeonGenerator? DungeonGenerator;
        public DungeonVisualizer? DungeonVisualizer;
        public TurnManager? TurnManager;
        public Board? Board;
        public Agent? AgentPrefab;
        public Entity? EntityPrefab;

        public Agent? LastPlayer { get; private set; }

        private Action MainAction = new NullAction();

        public void SetupExample1()
        {
            if (PlayerAgentSO == null) throw new ArgumentNullException(nameof(PlayerAgentSO));
            if (EnemiesSOs == null) throw new ArgumentNullException(nameof(EnemiesSOs));
            if (BoxEntitySO == null) throw new ArgumentNullException(nameof(BoxEntitySO));

            if (PlayerController == null) throw new ArgumentNullException(nameof(PlayerController));
            if (DungeonGenerator == null) throw new ArgumentNullException(nameof(DungeonGenerator));
            if (AIController == null) throw new ArgumentNullException(nameof(AIController));
            if (TurnManager == null) throw new ArgumentNullException(nameof(TurnManager));
            if (Board == null) throw new ArgumentNullException(nameof(Board));
            if (AgentPrefab == null) throw new ArgumentNullException(nameof(AgentPrefab));
            if (EntityPrefab == null) throw new ArgumentNullException(nameof(EntityPrefab));

            var ctx = new Action.Context(TurnManager, Board, AgentPrefab, EntityPrefab);
            var newAction = new ParallelAction();

            var dungeonData = DungeonGenerator.GenerateTestDungeon(Board.Size, 3);
            Board.Apply(dungeonData.ToBoardData());

            var playerAction = new SequentialAction();
            var playerSpawn = dungeonData.PlayerSpawn;
            var spawnPlayerAction = new SpawnAction(PlayerAgentSO, playerSpawn, ctx, true);
            playerAction.Add(spawnPlayerAction);
            playerAction.Add(new ControlAction(spawnPlayerAction.Agent, true));
            newAction.Add(playerAction);

            for (var i = 0; i < EnemiesSOs.Length; i++)
            {
                var enemySO = EnemiesSOs[i];
                if (enemySO == null) continue;
                newAction.Add(new SpawnAction(enemySO, new Tile(3 + i, 7), ctx));
            }
            newAction.Add(new CreateAction(BoxEntitySO, new Tile(3, 9), ctx));

            MainAction = newAction;
        }

        public void SetupExample2()
        {
            if (PlayerAgentSO == null) throw new ArgumentNullException(nameof(PlayerAgentSO));
            if (EnemiesSOs == null) throw new ArgumentNullException(nameof(EnemiesSOs));

            if (DungeonGenerator == null) throw new ArgumentNullException(nameof(DungeonGenerator));
            if (DungeonVisualizer == null) throw new ArgumentNullException(nameof(DungeonVisualizer));
            if (TurnManager == null) throw new ArgumentNullException(nameof(TurnManager));
            if (Board == null) throw new ArgumentNullException(nameof(Board));
            if (AgentPrefab == null) throw new ArgumentNullException(nameof(AgentPrefab));
            if (EntityPrefab == null) throw new ArgumentNullException(nameof(EntityPrefab));

            var ctx = new Action.Context(TurnManager, Board, AgentPrefab, EntityPrefab);
            var newAction = new ParallelAction();

            var dungeonData = DungeonGenerator.GenerateDungeon(Board.Size, 20, 4, 8, 4);
            DungeonVisualizer.DungeonData = dungeonData;
            if (dungeonData == null) throw new NullReferenceException();

            Board.Apply(dungeonData.ToBoardData());

            var playerAction = new SequentialAction();
            var playerSpawn = dungeonData.PlayerSpawn;
            var spawnPlayerAction = new SpawnAction(PlayerAgentSO, playerSpawn, ctx, true);
            playerAction.Add(spawnPlayerAction);
            playerAction.Add(new ControlAction(spawnPlayerAction.Agent, true));
            newAction.Add(playerAction);

            if (EnemiesSOs.Length > 0)
            {
                foreach (var spawn in dungeonData.EnemiesSpawn)
                {
                    var enemySO = EnemiesSOs[Random.Range(0, EnemiesSOs.Length)];
                    if (enemySO == null) continue;
                    newAction.Add(new SpawnAction(enemySO, spawn, ctx));
                }
            }

            MainAction = newAction;
        }

        public void GameLoop()
        {
            if (MainAction.IsCompleted) MainAction = ProcessTurns();
            else MainAction.Execute();
        }

        private Action ProcessTurns()
        {
            if (PlayerController == null) throw new ArgumentNullException(nameof(PlayerController));
            if (AIController == null) throw new ArgumentNullException(nameof(AIController));
            if (TurnManager == null) throw new ArgumentNullException(nameof(TurnManager));
            if (Board == null) throw new ArgumentNullException(nameof(Board));
            if (AgentPrefab == null) throw new ArgumentNullException(nameof(AgentPrefab));
            if (EntityPrefab == null) throw new ArgumentNullException(nameof(EntityPrefab));

            var ctx = new Action.Context(TurnManager, Board, AgentPrefab, EntityPrefab);

            var turnAction = new ParallelAction();
            var processedAgents = new HashSet<Agent>();
            Agent? agent;                               
            while (((agent = TurnManager.GetCurrent()) != null) && !processedAgents.Contains(agent))
            {
                Action? agentAction;
                if (agent.IsPlayer)
                {
                    LastPlayer = agent;
                    agentAction = PlayerController.ProduceAction(agent, ctx);
                }
                else agentAction = AIController.ProduceAction(agent, ctx);

                if (agentAction == null) break;

                agentAction.Execute();

                if (!agentAction.IsCompleted) turnAction.Add(agentAction);

                processedAgents.Add(agent);
                TurnManager.Next();

                if (!(agentAction is MoveAction) && !(agentAction is NullAction)) break;
            }

            return turnAction;
        }
    }
}
