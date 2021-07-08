using System.Collections.Generic;

using Zongband.Engine.Entities;

namespace  Zongband.Engine.Boards
{
    public class Tile : IReadOnlyTile
    {
        public ITerrain Terrain { get; private set; }
        public IReadOnlyCollection<Entity> Entities => entities;

        private readonly HashSet<Entity> entities = new();

        public Tile(ITerrain terrain)
        {
            Terrain = terrain;
        }

        public bool Add(Entity entity)
        {
            entities.Add(entity);
            return true;
        }

        public bool Remove(Entity entity)
        {
            entities.Remove(entity);
            return true;
        }

        public bool SetTerrain(ITerrain terrain)
        {
            Terrain = terrain;
            return true;
        }
    }
}