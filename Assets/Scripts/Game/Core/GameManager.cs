#nullable enable

using UnityEngine;
using System;
using System.Collections.Generic;

using Zongband.Game.Controllers;
using Zongband.Game.Commands;
using Zongband.Game.Generation;
using Zongband.Game.Turns;
using Zongband.Game.Boards;
using Zongband.Game.Entities;
using Zongband.Utils;

using Random = UnityEngine.Random;

namespace Zongband.Game.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private AgentSO? PlayerAgentSO;
        [SerializeField] private AgentSO[]? EnemiesSOs;
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

        private Command CurrentCommand = new NullCommand();

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

            var ctx = new Command.Context(TurnManager, Board, AgentPrefab, EntityPrefab);
            var newCommand = new ParallelCommand();

            var dungeonData = DungeonGenerator.GenerateTestDungeon(Board.Size, 3);
            Board.Apply(dungeonData.ToBoardData());

            var playerCommand = new SequentialCommand();
            var spawnPlayerCommand = new SpawnCommand(PlayerAgentSO, dungeonData.PlayerSpawn, ctx, true);
            playerCommand.Add(spawnPlayerCommand);
            playerCommand.Add(new MakePlayerCommand(spawnPlayerCommand));
            newCommand.Add(playerCommand);

            for (var i = 0; i < EnemiesSOs.Length; i++)
                newCommand.Add(new SpawnCommand(EnemiesSOs[i], new Tile(3 + i, 7), ctx));
            newCommand.Add(new SpawnCommand(BoxEntitySO, new Tile(3, 9), ctx));

            CurrentCommand = newCommand;
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

            var ctx = new Command.Context(TurnManager, Board, AgentPrefab, EntityPrefab);
            var newCommand = new ParallelCommand();

            var dungeonData = DungeonGenerator.GenerateDungeon(Board.Size, 20, 4, 8, 4);
            DungeonVisualizer.DungeonData = dungeonData;
            if (dungeonData == null) throw new NullReferenceException();

            Board.Apply(dungeonData.ToBoardData());

            var playerCommand = new SequentialCommand();
            var spawnPlayerCommand = new SpawnCommand(PlayerAgentSO, dungeonData.PlayerSpawn, ctx, true);
            playerCommand.Add(spawnPlayerCommand);
            playerCommand.Add(new MakePlayerCommand(spawnPlayerCommand));
            newCommand.Add(playerCommand);

            if (EnemiesSOs.Length > 0)
            {
                foreach (var spawn in dungeonData.EnemiesSpawn)
                {
                    var enemy = EnemiesSOs[Random.Range(0, EnemiesSOs.Length)];
                    newCommand.Add(new SpawnCommand(enemy, spawn, ctx));
                }
            }

            CurrentCommand = newCommand;
        }

        public void GameLoop()
        {
            if (CurrentCommand.IsCompleted) CurrentCommand = ProcessTurns();
            else CurrentCommand.Execute();
        }

        private Command ProcessTurns()
        {
            if (PlayerController == null) throw new ArgumentNullException(nameof(PlayerController));
            if (AIController == null) throw new ArgumentNullException(nameof(AIController));
            if (TurnManager == null) throw new ArgumentNullException(nameof(TurnManager));
            if (Board == null) throw new ArgumentNullException(nameof(Board));
            if (AgentPrefab == null) throw new ArgumentNullException(nameof(AgentPrefab));
            if (EntityPrefab == null) throw new ArgumentNullException(nameof(EntityPrefab));

            var ctx = new Command.Context(TurnManager, Board, AgentPrefab, EntityPrefab);

            var turnCommand = new ParallelCommand();
            var processedAgents = new HashSet<Agent>();
            Agent? agent;                               
            while (((agent = TurnManager.GetCurrent()) != null) && !processedAgents.Contains(agent))
            {
                Command? agentCommand;
                if (agent.IsPlayer)
                {
                    LastPlayer = agent;
                    agentCommand = PlayerController.ProduceCommand(agent, ctx);
                }
                else agentCommand = AIController.ProduceCommand(agent, ctx);

                if (agentCommand == null) break;

                agentCommand.Execute();

                if (!agentCommand.IsCompleted) turnCommand.Add(agentCommand);

                processedAgents.Add(agent);
                TurnManager.Next();

                if (!(agentCommand is MovementCommand) && !(agentCommand is NullCommand)) break;
            }

            return turnCommand;
        }
    }
}
