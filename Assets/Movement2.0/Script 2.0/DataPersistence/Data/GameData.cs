using System;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    [Serializable]
    public class GameData
    {

        public long lastUpdated;

        public int playercount;
        public Vector3 playerPosition;

        public SerializableDictionary<string, bool> pointchecked;

        public Vector3 lastCheckPointPos;
        public Vector3 lastCheckPointRotate;
        public GameData()
        {
            this.playercount = 0;
            playerPosition = Vector3.zero;
            lastCheckPointPos = Vector3.zero;
            lastCheckPointRotate = Vector3.zero;

            pointchecked = new SerializableDictionary<string, bool>();
        }
        public int GetPercentageComplete()
        {
            int totalChecked = 0;
            foreach (bool ischecked in pointchecked.Values)
            {
                if (ischecked)
                {
                    totalChecked++;
                }
            }
            int percentageCompleted = -1;
            if (pointchecked.Count != 0)
            {
                percentageCompleted = (totalChecked * 100 / pointchecked.Count);
            }
            return percentageCompleted;
        }
    }
}
