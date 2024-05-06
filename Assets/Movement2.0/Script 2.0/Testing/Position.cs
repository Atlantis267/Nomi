using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class Position : MonoBehaviour, IDataPersistence
    {        
        public void LoadData(GameData data)
        {
            this.transform.position = data.playerPosition;
        }

        public void SaveData(GameData data)
        {
            data.playerPosition = this.transform.position;
        }
    }
}
