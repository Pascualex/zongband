#nullable enable

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

using Zongband.Game.Core;
using Zongband.Game.Entities;
using Zongband.Utils;

namespace Zongband.UI
{
    public class AgentInspector : MonoBehaviour
    {
        public Tile MouseTile { private get; set; } = Tile.MinusOne;

        [SerializeField] private GameManager? GameManager;
        [SerializeField] private Image? InspectorMenu;
        [SerializeField] private Slider? HealthBar;
        [SerializeField] private TextMeshProUGUI? HealthStat;
        [SerializeField] private TextMeshProUGUI? TurnCDStat;
        [SerializeField] private TextMeshProUGUI? HostileTag;

        private Agent? FixedAgent;

        private void Awake()
        {
            if (InspectorMenu != null) InspectorMenu.gameObject.SetActive(false);
        }

        public void Refresh()
        {
            InspectAgent();
        }

        public void LockAgent()
        {
            if (GameManager == null) throw new ArgumentNullException(nameof(GameManager));
            if (GameManager.Board == null) throw new ArgumentNullException(nameof(GameManager.Board));

            FixedAgent = GameManager.Board.GetAgent(MouseTile);
        }

        private void InspectAgent()
        {
            if (GameManager == null) throw new ArgumentNullException(nameof(GameManager));
            if (GameManager.Board == null) throw new ArgumentNullException(nameof(GameManager.Board));
            if (InspectorMenu == null) throw new ArgumentNullException(nameof(InspectorMenu));
            if (HealthBar == null) throw new ArgumentNullException(nameof(HealthBar));
            if (HealthStat == null) throw new ArgumentNullException(nameof(HealthStat));
            if (TurnCDStat == null) throw new ArgumentNullException(nameof(TurnCDStat));
            if (HostileTag == null) throw new ArgumentNullException(nameof(HostileTag));

            var inspect = false;
            var agent = FixedAgent;
            if (agent == null || !agent) agent = GameManager.Board.GetAgent(MouseTile);
            if (agent != null)
            {
                HealthBar.maxValue = agent.MaxHealth;
                HealthBar.value = agent.CurrentHealth;
                HealthStat.text = agent.CurrentHealth + " / " + agent.MaxHealth;
                TurnCDStat.text = agent.TurnCooldown.ToString();
                HostileTag.gameObject.SetActive(!agent.IsPlayer);
                inspect = true;
            }

            InspectorMenu.gameObject.SetActive(inspect);
        }
    }
}