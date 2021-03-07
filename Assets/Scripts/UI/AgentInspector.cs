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

        [SerializeField] private GameManager? gameManager;
        [SerializeField] private Image? inspectorMenu;
        [SerializeField] private Slider? healthBar;
        [SerializeField] private TextMeshProUGUI? healthStat;
        [SerializeField] private TextMeshProUGUI? turnCDStat;
        [SerializeField] private TextMeshProUGUI? hostileTag;

        private Agent? fixedAgent;

        private void Awake()
        {
            if (inspectorMenu != null) inspectorMenu.gameObject.SetActive(false);
        }

        public void Refresh()
        {
            InspectAgent();
        }

        public void HandleMouseClick()
        {
            if (gameManager == null) throw new ArgumentNullException(nameof(gameManager));
            if (gameManager.board == null) throw new ArgumentNullException(nameof(gameManager.board));

            fixedAgent = gameManager.board.GetAgent(MouseTile);
        }

        private void InspectAgent()
        {
            if (gameManager == null) throw new ArgumentNullException(nameof(gameManager));
            if (gameManager.board == null) throw new ArgumentNullException(nameof(gameManager.board));
            if (inspectorMenu == null) throw new ArgumentNullException(nameof(inspectorMenu));
            if (healthBar == null) throw new ArgumentNullException(nameof(healthBar));
            if (healthStat == null) throw new ArgumentNullException(nameof(healthStat));
            if (turnCDStat == null) throw new ArgumentNullException(nameof(turnCDStat));
            if (hostileTag == null) throw new ArgumentNullException(nameof(hostileTag));

            var inspect = false;
            var agent = fixedAgent;
            if (agent == null || !agent) agent = gameManager.board.GetAgent(MouseTile);
            if (agent != null)
            {
                healthBar.maxValue = agent.MaxHealth;
                healthBar.value = agent.CurrentHealth;
                healthStat.text = agent.CurrentHealth + " / " + agent.MaxHealth;
                turnCDStat.text = agent.TurnCooldown.ToString();
                hostileTag.gameObject.SetActive(!agent.isPlayer);
                inspect = true;
            }

            inspectorMenu.gameObject.SetActive(inspect);
        }
    }
}