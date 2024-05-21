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
        [SerializeField] private GameObject blur;
        [SerializeField] private Animator fade;
        [SerializeField] private GameObject fadeimage;


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
            blur.SetActive(true);
            this.DeactivateMenu();
        }

        public void OnLoadGameClicked()
        {
            saveSlotsMenu.ActivateMenu(true);
            blur.SetActive(true);
            this.DeactivateMenu();
            
        }
        public void OnContinueGameClicked()
        {
            StartCoroutine(OnContinueClicked());
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

        IEnumerator OnContinueClicked()
        {
            DisableMenuButtons();
            fadeimage.SetActive(true);
            yield return null;
            fade.SetTrigger("Fade");
            yield return new WaitForSeconds(1f);
            DataPersistenceManager.instance.SaveGame();
            SceneManager.LoadSceneAsync(_loadingScene);
        }
    }
}
