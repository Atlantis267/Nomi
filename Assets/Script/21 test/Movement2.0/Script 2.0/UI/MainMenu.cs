using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Movement
{
    public class MainMenu : Menu
    {
        [Header("Menu Navigation")]
        [SerializeField] private SaveSlotsMenu saveSlotsMenu;


        [Header("Main Meau Object")]
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button continueGameButton;
        [SerializeField] private Button loadGameButton;


        [Header("Sences to Load")]
        [SerializeField] private SceneField _loadingScene;

        private void Start()
        {
            DisableButtonsDependingOnData();
        }
        private void DisableButtonsDependingOnData()
        {
            if (!DataPersistenceManager.instance.HasGameData())
            {
                continueGameButton.interactable = false;
                loadGameButton.interactable = false;
            }
        }
        public void OnNewGameClicked()
        {
            saveSlotsMenu.ActivateMenu(false);
            this.DeactivateMenu();
        }

        public void OnLoadGameClicked()
        {
            saveSlotsMenu.ActivateMenu(true);
            this.DeactivateMenu();
            
        }
        public void OnContinueGameClicked()
        {
            DisableMenuButtons();
            DataPersistenceManager.instance.SaveGame();
            SceneManager.LoadSceneAsync(_loadingScene);
        }
        public void OnQuitClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        private void DisableMenuButtons()
        {
            newGameButton.interactable = false;
            continueGameButton.interactable = false;
        }
        public void ActivateMenu()
        {
            this.gameObject.SetActive(true);
            DisableButtonsDependingOnData();
        }

        public void DeactivateMenu()
        {
            this.gameObject.SetActive(false);
        }
    }
}
