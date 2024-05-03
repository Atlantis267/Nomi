using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public interface IDataPersistence
    {
        void LoadData(GameData data);
        void SaveData(GameData data);
    }
}
