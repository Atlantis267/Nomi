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
        [SerializeField] private Button _continueGameButton;
        [SerializeField] private Button _loadGameButton;


        [Header("Sences to Load")]
        [SerializeField] private SceneField _loadingScene;

        private void Start()
        {
            if (!DataPersistenceManager.instance.HasGameData())
            {
                _continueGameButton.interactable = false;
                _loadGameButton.interactable = false;
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
            Debug.Log("Continoue");
            this.DeactivateMenu();
            DataPersistenceManager.instance.SaveGame();
            SceneManager.LoadSceneAsync(_loadingScene);
        }
        public void ActivateMenu()
        {
            this.gameObject.SetActive(true);
        }

        public void DeactivateMenu()
        {
            this.gameObject.SetActive(false);
        }
    }
}
