using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Dhs5.Utility.EventSystem;

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
            EventManager<LevelBuilderEvent>.StartListening(LevelBuilderEvent.OPEN_BUILDER, OnOpenBuilder);
            EventManager<LevelBuilderEvent>.StartListening(LevelBuilderEvent.QUIT_BUILDER, OnQuitBuilder);
        }
        private void OnDisable()
        {
            EventManager<LevelBuilderEvent>.StopListening(LevelBuilderEvent.OPEN_BUILDER, OnOpenBuilder);
            EventManager<LevelBuilderEvent>.StopListening(LevelBuilderEvent.QUIT_BUILDER, OnQuitBuilder);
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
            EventManager<LevelBuilderEvent>.TriggerEvent(LevelBuilderEvent.OPEN_HELP);
        }
        private void OnQuitHelp()
        {
            helpWindow.SetActive(false);
            EventManager<LevelBuilderEvent>.TriggerEvent(LevelBuilderEvent.QUIT_HELP);
        }

        #endregion
    }
}
