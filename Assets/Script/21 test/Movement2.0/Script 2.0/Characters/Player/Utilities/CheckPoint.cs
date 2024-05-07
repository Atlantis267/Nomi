using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class CheckPoint : MonoBehaviour, IDataPersistence
    {        
        [SerializeField] private string id;

        [ContextMenu("Generate guid for id")]
        private void GenerateGuid()
        {
            id = System.Guid.NewGuid().ToString();
        }
        public CheckData checkData;
        private SpriteRenderer visual;

        private bool ischecked = false;

        public void LoadData(GameData data)
        {
            data.pointchecked.TryGetValue(id, out ischecked);
            if (ischecked)
            {
                gameObject.SetActive(false);
            }
        }

        public void SaveData(GameData data)
        {
            if (data.pointchecked.ContainsKey(id))
            {
                data.pointchecked.Remove(id);
            }
            data.pointchecked.Add(id, ischecked);
        }

        private void Awake()
        {
            visual = this.GetComponentInChildren<SpriteRenderer>();
            checkData = GameObject.FindGameObjectWithTag("Player").GetComponent<CheckData>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ischecked = true;
                gameObject.SetActive(false);
                checkData.LastPointPos = transform.position;
                checkData.LastPointRotate = transform.forward;
            }
        }
    }
}
