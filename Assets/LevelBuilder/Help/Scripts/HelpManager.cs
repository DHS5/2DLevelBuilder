using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

namespace LevelBuilder2D
{
    public class HelpManager : MonoBehaviour
    {
        [Header("UI components")]
        [SerializeField] private GameObject helpUI;
        [Space]
        [SerializeField] private GameObject helpWindow;
        [Space]
        [SerializeField] private Button openHelpButton;
        [SerializeField] private Button quitHelpButton;

        #region Enable / Disable
        private void OnEnable()
        {
            EventManager.StartListening(EventManager.LevelBuilderEvent.OPEN_BUILDER, OnOpenBuilder);
            EventManager.StartListening(EventManager.LevelBuilderEvent.QUIT_BUILDER, OnQuitBuilder);
        }
        private void OnDisable()
        {
            EventManager.StopListening(EventManager.LevelBuilderEvent.OPEN_BUILDER, OnOpenBuilder);
            EventManager.StopListening(EventManager.LevelBuilderEvent.QUIT_BUILDER, OnQuitBuilder);
        }
        #endregion

        #region Listeners
        // ### Listeners ###

        private void OnOpenBuilder()
        {
            helpUI.gameObject.SetActive(true);
            helpWindow.gameObject.SetActive(false);
            AddListeners();
        }
        private void OnQuitBuilder()
        {
            helpUI.gameObject.SetActive(false);
            RemoveListeners();
        }

        #endregion

        #region Buttons
        // ### Buttons ###

        private void AddListeners()
        {
            openHelpButton.onClick.AddListener(OnOpenHelp);
            quitHelpButton.onClick.AddListener(OnQuitHelp);
        }
        private void RemoveListeners()
        {
            openHelpButton.onClick.RemoveAllListeners();
            quitHelpButton.onClick.RemoveAllListeners();
        }

        // Listeners

        private void OnOpenHelp()
        {
            helpWindow.SetActive(true);
            EventManager.TriggerEvent(EventManager.LevelBuilderEvent.OPEN_HELP);
        }
        private void OnQuitHelp()
        {
            helpWindow.SetActive(false);
            EventManager.TriggerEvent(EventManager.LevelBuilderEvent.QUIT_HELP);
        }

        #endregion
    }
}
