using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

namespace LevelBuilder2D
{
    public class RebindManager : MonoBehaviour
    {
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
        // Mouse
        [SerializeField] private Button buildRebindButton;
        [SerializeField] private TextMeshProUGUI buildRebindText;
        [SerializeField] private Button deleteRebindButton;
        [SerializeField] private TextMeshProUGUI deleteRebindText;
        [SerializeField] private Button moveRebindButton;
        [SerializeField] private TextMeshProUGUI moveRebindText;
        [Space]
        // Keyboard
        [SerializeField] private Button saveRebindButton;
        [SerializeField] private TextMeshProUGUI saveRebindText;
        [SerializeField] private Button undoRebindButton;
        [SerializeField] private TextMeshProUGUI undoRebindText;
        [SerializeField] private Button redoRebindButton;
        [SerializeField] private TextMeshProUGUI redoRebindText;


        private InputActionRebindingExtensions.RebindingOperation rebindingOperation;


        // ### Main Function ###

        private void Rebind(InputActionReference actionToRemap, TextMeshProUGUI actionText)
        {
            actionToRemap.action.Disable();

            rebindingOperation = actionToRemap.action.PerformInteractiveRebinding()
                .OnMatchWaitForAnother(0.2f).OnComplete(
                operation =>
                {
                    actionText.text = InputControlPath.ToHumanReadableString
                    (actionToRemap.action.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);

                    rebindingOperation.Dispose();
                    actionToRemap.action.Enable();
                }).Start();
        }

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
            AddListeners();
        }
        private void OnQuitBuilder()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            buildRebindButton.onClick.AddListener(RebindBuild);
            //deleteRebindButton.onClick.AddListener(RebindDelete);
            //moveRebindButton.onClick.AddListener(RebindMove);
            //saveRebindButton.onClick.AddListener(RebindSave);
            //undoRebindButton.onClick.AddListener(RebindUndo);
            //redoRebindButton.onClick.AddListener(RebindRedo);
        }
        private void RemoveListeners()
        {
            buildRebindButton.onClick.RemoveAllListeners();
            //deleteRebindButton.onClick.RemoveAllListeners();
            //moveRebindButton.onClick.RemoveAllListeners();
            //saveRebindButton.onClick.RemoveAllListeners();
            //undoRebindButton.onClick.RemoveAllListeners();
            //redoRebindButton.onClick.RemoveAllListeners();
        }

        private void RebindBuild() { Rebind(buildAction, buildRebindText); }
        private void RebindDelete() { Rebind(deleteAction, deleteRebindText); }
        private void RebindMove() { Rebind(moveAction, moveRebindText); }
        private void RebindSave() { Rebind(saveAction, saveRebindText); }
        private void RebindUndo() { Rebind(undoAction, undoRebindText); }
        private void RebindRedo() { Rebind(redoAction, redoRebindText); }

        #endregion
    }
}
