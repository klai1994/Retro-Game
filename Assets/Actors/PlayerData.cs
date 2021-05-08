﻿using UnityEngine;

namespace Game
{
    public class PlayerData : ActorStats
    {
        static PlayerData playerData;

        public static PlayerData GetPlayerData()
        {
            if (playerData == null)
            {
                playerData = ScriptableObject.CreateInstance<PlayerData>();
            }
            return playerData;
        }

        public override void Init(string name)
        {
            Init(name);
        }
    }
}
