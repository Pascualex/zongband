#nullable enable

using UnityEngine;

using Zongband.Utils;

namespace Zongband.Game.Entities
{
    public class Entity : MonoBehaviour
    {
        public Tile tile = Tile.MinusOne;
        public bool removed = false;

        public Transform? GameModelContainer;
        [SerializeField] private GameObject? defaultGameModel;
        [SerializeField] private GameObject? gameModel;

        private void Awake()
        {
            if (GameModelContainer == null) return;
            if (defaultGameModel == null) return;

            if (gameModel != null) Destroy(gameModel);

            gameModel = Instantiate(defaultGameModel, GameModelContainer);
            gameModel.name = "GameModel";
        }

        public void ApplySO(EntitySO entitySO)
        {
            name = entitySO.name;

            if (GameModelContainer == null) return;
            if (gameModel != null) Destroy(gameModel);

            var parent = GameModelContainer;
            if (entitySO.gameModel != null) gameModel = Instantiate(entitySO.gameModel, parent);
            else if (defaultGameModel != null) gameModel = Instantiate(defaultGameModel, parent);

            if (gameModel != null) gameModel.name = "GameModel";
        }
    }
}
