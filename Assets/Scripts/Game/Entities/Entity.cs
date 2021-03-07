#nullable enable

using UnityEngine;
using System;

using Zongband.Utils;

namespace Zongband.Game.Entities
{
    public class Entity : MonoBehaviour
    {
        public Tile tile = Tile.MinusOne;

        public Transform? gameModelContainer;
        [SerializeField] private GameObject? defaultGameModel;
        [SerializeField] private GameObject? gameModel;

        private void Awake()
        {
            if (gameModelContainer == null) throw new ArgumentNullException(nameof(gameModelContainer));
            if (defaultGameModel == null) throw new ArgumentNullException(nameof(defaultGameModel));

            if (gameModel != null) Destroy(gameModel);

            gameModel = Instantiate(defaultGameModel, gameModelContainer);
            gameModel.name = "GameModel";
        }

        public void ApplySO(EntitySO entitySO)
        {
            if (gameModelContainer == null) throw new ArgumentNullException(nameof(gameModelContainer));
            if (defaultGameModel == null) throw new ArgumentNullException(nameof(defaultGameModel));

            if (gameModel != null) Destroy(gameModel);

            var parent = gameModelContainer;
            if (entitySO.gameModel != null) gameModel = Instantiate(entitySO.gameModel, parent);
            else gameModel = Instantiate(defaultGameModel, parent);

            name = entitySO.name;
            gameModel.name = "GameModel";
        }
    }
}
