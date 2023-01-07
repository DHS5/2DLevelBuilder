using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
using Dhs5.Utility.Input;

namespace LevelBuilder2D
{
    public class RebindManager : MonoBehaviour
    {
        [Header("Player Input")]
        public PlayerInput playerInput;

        [Header("Mouse Actions")]
        [SerializeField] private InputActionReference buildAction;
        [SerializeField] private InputActionReference deleteAction;
        [SerializeField] private InputActionReference moveAction;

        [Header("Keyboard Actions")]
        [SerializeField] private InputActionReference saveAction;
        [SerializeField] private InputActionReference undoAction;
        [SerializeField] private InputActionReference redoAction;

        [Space, Space]

        [Header("UI components")]
        [SerializeField] private GameObject rebindOverlay;
        [SerializeField] private TextMeshProUGUI overlayText;
        [Space]
        // Mouse
        [SerializeField] private TMP_Dropdown mouseConfigDropdown;
        [SerializeField] private GameObject[] mouseConfigs;
        [Space]
        // Keyboard
        [SerializeField] private Button saveRebindButton;
        [SerializeField] private TextMeshProUGUI saveRebindText;
        [SerializeField] private Button undoRebindButton;
        [SerializeField] private TextMeshProUGUI undoRebindText;
        [SerializeField] private Button redoRebindButton;
        [SerializeField] private TextMeshProUGUI redoRebindText;



        InputBinding leftClickBinding = new InputBinding("<Mouse>/leftButton");
        InputBinding rightClickBinding = new InputBinding("<Mouse>/rightButton");
        InputBinding wheelClickBinding = new InputBinding("<Mouse>/middleButton");


        #region Enable / Disable
        private void OnEnable()
        {
            EventManager.StartListening(EventManager.LevelBuilderEvent.OPEN_BUILDER, OnOpenBuilder);
            EventManager.StartListening(EventManager.LevelBuilderEvent.QUIT_BUILDER, OnQuitBuilder);
            EventManager.StartListening(EventManager.LevelBuilderEvent.OPEN_HELP, OnOpenHelp);
            EventManager.StartListening(EventManager.LevelBuilderEvent.QUIT_HELP, OnCloseHelp);

            LoadRebinds();
        }
        private void OnDisable()
        {
            EventManager.StopListening(EventManager.LevelBuilderEvent.OPEN_BUILDER, OnOpenBuilder);
            EventManager.StopListening(EventManager.LevelBuilderEvent.QUIT_BUILDER, OnQuitBuilder);
            EventManager.StopListening(EventManager.LevelBuilderEvent.OPEN_HELP, OnOpenHelp);
            EventManager.StopListening(EventManager.LevelBuilderEvent.QUIT_HELP, OnCloseHelp);

            SaveRebinds();
        }

        #endregion

        #region Listeners
        // ### Listeners ###

        private void OnOpenBuilder()
        {
            AddListeners();
        }
        private void OnQuitBuilder()
        {
            RemoveListeners();
        }
        private void OnOpenHelp()
        {
            playerInput.actions.Disable();
            mouseConfigDropdown.onValueChanged.AddListener(RebindMouse);
        }
        private void OnCloseHelp()
        {
            playerInput.actions.Enable();
            mouseConfigDropdown.onValueChanged.RemoveAllListeners();
        }

        private void AddListeners()
        {
            saveRebindButton.onClick.AddListener(RebindSave);
            undoRebindButton.onClick.AddListener(RebindUndo);
            redoRebindButton.onClick.AddListener(RebindRedo);
        }
        private void RemoveListeners()
        {
            saveRebindButton.onClick.RemoveAllListeners();
            undoRebindButton.onClick.RemoveAllListeners();
            redoRebindButton.onClick.RemoveAllListeners();
        }

        #endregion

        #region Bindings

        // ### Bindings ###

        private void ActuBindingsTexts()
        {
            saveRebindText.text = InputUtility.GetCorrectBindingString(saveAction.action);
            undoRebindText.text = InputUtility.GetCorrectBindingString(undoAction.action);
            redoRebindText.text = InputUtility.GetCorrectBindingString(redoAction.action);
        }

        private void RebindMouse(int schemeIndex)
        {
            OpenMouseConfig(schemeIndex);
            switch (schemeIndex)
            {
                case 0:
                    RebindBuild(leftClickBinding);
                    RebindDelete(rightClickBinding);
                    RebindMove(wheelClickBinding);
                    break;
                case 1:
                    RebindBuild(rightClickBinding);
                    RebindDelete(wheelClickBinding);
                    RebindMove(leftClickBinding);
                    break;
                default:
                    RebindBuild(leftClickBinding);
                    RebindDelete(wheelClickBinding);
                    RebindMove(rightClickBinding);
                    break;
            }
        }

        private void RebindBuild(InputBinding newBinding) { buildAction.action.ApplyBindingOverride(newBinding); }
        private void RebindDelete(InputBinding newBinding) { deleteAction.action.ApplyBindingOverride(newBinding); }
        private void RebindMove(InputBinding newBinding) { moveAction.action.ApplyBindingOverride(newBinding); }

        private void RebindSave()
        {
            InputUtility.InteractiveRebind(saveAction.action, 1, true, saveRebindText, rebindOverlay, overlayText,
                new string[] { "<keyboard>/anyKey" }, "<Keyboard>"); //"<keyboard>/#(A)"
        }
        private void RebindUndo()
        {
            InputUtility.InteractiveRebind(undoAction.action, 1, true, undoRebindText, rebindOverlay, overlayText,
                new string[] { "<keyboard>/anyKey" }, "<Keyboard>");
        }
        private void RebindRedo()
        {
            InputUtility.InteractiveRebind(redoAction.action, 1, true, redoRebindText, rebindOverlay, overlayText,
                new string[] { "<keyboard>/anyKey" }, "<Keyboard>");
        }

        #endregion

        #region UI

        // ### UI ###

        private void OpenMouseConfig(int index)
        {
            for (int i = 0; i < mouseConfigs.Length; i++)
                mouseConfigs[i].SetActive(i == index);
        }

        #endregion

        #region Save & Load

        // ### Save & Load ###

        private void SaveRebinds()
        {
            string rebinds = playerInput.actions.SaveBindingOverridesAsJson();
            int mouseConfig = mouseConfigDropdown.value;
            PlayerPrefs.SetString("rebinds", rebinds);
            PlayerPrefs.SetInt("LevelBuilderMouseConfigIndex", mouseConfig);
        }
        private void LoadRebinds()
        {
            // Keys
            string rebinds = PlayerPrefs.GetString("rebinds");
            if (!string.IsNullOrEmpty(rebinds))
                playerInput.actions.LoadBindingOverridesFromJson(rebinds);
            ActuBindingsTexts();

            // Mouse
            int mouseConfig = PlayerPrefs.GetInt("LevelBuilderMouseConfigIndex");
            RebindMouse(mouseConfig);
            mouseConfigDropdown.value = mouseConfig;
        }

        #endregion
    }
}
