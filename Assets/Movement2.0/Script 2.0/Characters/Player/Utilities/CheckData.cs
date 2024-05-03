using UnityEngine;

namespace Movement
{
    public class CheckData : MonoBehaviour, IDataPersistence
    {
        public Vector3 LastPointPos;
        public Vector3 LastPointRotate;
        public void LoadData(GameData data)
        {
            this.LastPointPos = data.lastCheckPointPos;
            this.LastPointRotate = data.lastCheckPointRotate;
        }
        public void SaveData(GameData data)
        {
            data.lastCheckPointPos = this.LastPointPos;
            data.lastCheckPointRotate = this.LastPointRotate;
        }
    }
}
