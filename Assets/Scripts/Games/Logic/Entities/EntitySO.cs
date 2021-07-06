﻿#nullable enable

using UnityEngine;

namespace Zongband.Games.Logic.Entities
{
    [CreateAssetMenu(fileName = "Entity", menuName = "ScriptableObjects/Entity")]
    public class EntitySO : ScriptableObject
    {
        public GameObject? GameModel = null;
    }
}
