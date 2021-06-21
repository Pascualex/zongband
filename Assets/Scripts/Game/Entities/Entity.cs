#nullable enable

using UnityEngine;
using System;

using Zongband.Utils;

namespace Zongband.Game.Entities
{
    public class Entity : MonoBehaviour
    {
        public Tile Tile = Tile.MinusOne;
        public bool IsAlive => IsSpawned && !IsDestroyed;

        public Transform? GameModelContainer;
        [SerializeField] private GameObject? DefaultGameModel;
        [SerializeField] private GameObject? GameModel;
        private bool IsSpawned = false;
        private bool IsDestroyed = false;

        private void Awake()
        {
            if (GameModelContainer == null) throw new ArgumentNullException(nameof(GameModelContainer));
            if (DefaultGameModel == null) throw new ArgumentNullException(nameof(DefaultGameModel));

            if (GameModel != null) Destroy(GameModel);

            GameModel = Instantiate(DefaultGameModel, GameModelContainer);
            GameModel.name = "GameModel";
        }

        private void OnDestroy()
        {
            IsDestroyed = true;
        }

        public void OnSpawn()
        {
            IsSpawned = true;
        }

        public void ApplySO(EntitySO entitySO)
        {
            if (GameModelContainer == null) throw new ArgumentNullException(nameof(GameModelContainer));
            if (DefaultGameModel == null) throw new ArgumentNullException(nameof(DefaultGameModel));

            if (GameModel != null) Destroy(GameModel);

            var parent = GameModelContainer;
            if (entitySO.GameModel != null) GameModel = Instantiate(entitySO.GameModel, parent);
            else GameModel = Instantiate(DefaultGameModel, parent);

            name = entitySO.name;
            GameModel.name = "GameModel";
        }
    }
}
