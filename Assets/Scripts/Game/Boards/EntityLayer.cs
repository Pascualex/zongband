﻿using UnityEngine;
using System;

using Zongband.Game.Entities;
using Zongband.Utils;

namespace Zongband.Game.Boards
{
    public class EntityLayer : Layer
    {
        private Entity[][] entities;

        public EntityLayer(Vector2Int size, float scale) : base(size, scale)
        {
            entities = new Entity[size.y][];
            for (int i = 0; i < size.y; i++)
            {
                entities[i] = new Entity[size.x];
            }
        }

        public void Add(Entity entity, Vector2Int at)
        {
            if (entity == null) throw new ArgumentNullException();
            if (!IsPositionEmpty(at)) throw new ArgumentOutOfRangeException();

            entities[at.y][at.x] = entity;
            entities[at.y][at.x].position = at;
        }

        public void Move(Entity entity, Vector2Int to)
        {
            if (!CheckEntityPosition(entity)) throw new NotInTileException(entity);

            Move(entity.position, to);
        }

        public void Move(Vector2Int from, Vector2Int to)
        {
            if (IsPositionEmpty(from)) throw new EmptyTileException(from);
            if (!IsPositionEmpty(to)) throw new NotEmptyTileException(to);

            entities[to.y][to.x] = entities[from.y][from.x];
            entities[from.y][from.x] = null;
            entities[to.y][to.x].position = to;
        }

        public void Remove(Entity entity)
        {
            if (!CheckEntityPosition(entity)) throw new NotInTileException(entity);

            Remove(entity.position);
        }

        public void Remove(Vector2Int at)
        {
            if (IsPositionEmpty(at)) throw new EmptyTileException(at);

            entities[at.y][at.x].removed = true;
            entities[at.y][at.x] = null;
        }

        public bool IsPositionEmpty(Vector2Int position)
        {
            if (!IsPositionValid(position)) throw new ArgumentOutOfRangeException();

            return entities[position.y][position.x] == null;
        }

        public bool CheckEntityPosition(Entity entity)
        {
            if (entity == null) throw new ArgumentNullException();
            if (!Checker.Range(entity.position, size)) throw new ArgumentOutOfRangeException();

            return entities[entity.position.y][entity.position.x] == entity;
        }
    }
}
