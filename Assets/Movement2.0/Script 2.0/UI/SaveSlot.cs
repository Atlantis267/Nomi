using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Movement
{
    public class SaveSlot : MonoBehaviour
    {
        [Header("Profile")]
        [SerializeField] private string profileId = "";

        [Header("Content")]
        [SerializeField] private GameObject noDataContent;
        [SerializeField] private GameObject hasDataContent;
        [SerializeField] private TextMeshProUGUI percentageCompleteText;
        [SerializeField] private TextMeshProUGUI playerCountText;

        private Button saveSlotButton;

        private void Awake()
        {
            saveSlotButton = this.GetComponent<Button>();
        }


        public void SetData(GameData data)
        {
            if (data == null)
            {
                noDataContent.SetActive(true);
                hasDataContent.SetActive(false);
            }
            // there is data for this profileId
            else
            {
                noDataContent.SetActive(false);
                hasDataContent.SetActive(true);

                percentageCompleteText.text = data.GetPercentageComplete() + "% COMPLETE";
                playerCountText.text = "LEVEL COUNT: " + data.playercount;
            }
        }
        public string GetProfileId()
        {
            return this.profileId;
        }
        public void SetInteractable(bool interactable)
        {
            saveSlotButton.interactable = interactable;
        }
    }
}
