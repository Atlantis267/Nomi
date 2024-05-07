using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class sample : MonoBehaviour, IDataPersistence
    {
        public int playercount = 0;

        public void LoadData(GameData data)
        {
            this.playercount = data.playercount;
        }

        public void SaveData(GameData data)
        {
            data.playercount = this.playercount;
        }

        private void Update()
        {
            if (Input.GetKeyDown("i"))
            {
                playercount++;
            }
        }
    }
}
