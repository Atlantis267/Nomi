using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Movement
{
    public class SaveSlotsMenu : Menu
    {
        [Header("Menu Navigation")]
        [SerializeField] private MainMenu mainMenu;

        [Header("Menu Buttons")]
        [SerializeField] private Button backButton;

        [Header("Sences to Load")]
        [SerializeField] private SceneField _loadingScene;

        [Header("Confirmation Popup")]
        [SerializeField] private ConfirmationPopupMenu confirmationPopupMenu;

        private SaveSlot[] saveSlots;

        private bool isLoadingGame = false;

        private void Awake()
        {
            saveSlots = this.GetComponentsInChildren<SaveSlot>();
        }
        public void OnBackClicked()
        {
            mainMenu.ActivateMenu();
            this.DeactivateMenu();
        }
        public void OnSaveSlotClicked(SaveSlot saveSlot)
        {
            DisableMenuButtons();
            if (isLoadingGame)
            {
                DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
                SaveGameAndLoadScene();
            }
            else if (saveSlot.hasData)
            {
                confirmationPopupMenu.ActivateMenu(
                     "Starting a New Game with this slot will override the currently saved data. Are you sure?",
                      () => 
                      {
                          DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
                          DataPersistenceManager.instance.NewGame();
                          SaveGameAndLoadScene();
                      },
                      () =>
                      {
                          this.ActivateMenu(isLoadingGame);
                      }
                );
            }
            else
            {
                DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
                DataPersistenceManager.instance.NewGame();
                SaveGameAndLoadScene();
            }
        }
        private void SaveGameAndLoadScene()
        {
            DataPersistenceManager.instance.SaveGame();
            SceneManager.LoadSceneAsync(_loadingScene);
        }
        public void OnClearClicked(SaveSlot saveSlot)
        {
            DisableMenuButtons();

            confirmationPopupMenu.ActivateMenu
                (
                 "Are you sure you want to delete this saved data?",
                 () => 
                 {
                     DataPersistenceManager.instance.DeleteProfileData(saveSlot.GetProfileId());
                     ActivateMenu(isLoadingGame);
                 },
                 // function to execute if we select 'cancel'
                 () =>
                 {
                     ActivateMenu(isLoadingGame);
                 }
                );
        }
        public void ActivateMenu(bool isLoadingGame)
        {
            this.gameObject.SetActive(true);

            this.isLoadingGame = isLoadingGame;

            Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

            backButton.interactable = true;

            GameObject firstSelected = backButton.gameObject;
            foreach (SaveSlot saveSlot in saveSlots)
            {
                GameData profileData = null;
                profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
                saveSlot.SetData(profileData);
                if (profileData == null && isLoadingGame)
                {
                    saveSlot.SetInteractable(false);
                }
                else
                {
                    saveSlot.SetInteractable(true);
                    if (firstSelected.Equals(backButton.gameObject))
                    {
                        firstSelected = saveSlot.gameObject;
                    }
                }
            }

            Button firstSelectedButton = firstSelected.GetComponent<Button>();
            this.SetFirstSelected(firstSelectedButton);
        }

        public void DeactivateMenu()
        {
            this.gameObject.SetActive(false);
        }
        private void DisableMenuButtons()
        {
            foreach (SaveSlot saveSlot in saveSlots)
            {
                saveSlot.SetInteractable(false);
            }
            backButton.interactable = false;
        }
    }
}
