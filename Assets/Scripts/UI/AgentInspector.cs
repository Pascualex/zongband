#nullable enable

using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
            if (gameManager == null) return;

            var board = gameManager.board;
            if (board == null) return;

            fixedAgent = board.GetAgent(MouseTile);
        }

        private void InspectAgent()
        {
            if (gameManager == null) return;
            if (inspectorMenu == null) return;
            if (healthBar == null) return;
            if (healthStat == null) return;
            if (turnCDStat == null) return;
            if (hostileTag == null) return;

            var board = gameManager.board;
            if (board == null) return;

            var inspect = false;

            var agent = fixedAgent;
            if (agent == null || !agent) agent = board.GetAgent(MouseTile);
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