using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using TMPro;

namespace Dhs5.Utility.Input
{
    /// <summary>
    /// Utility class for the new Input System
    /// </summary>
    public static class InputUtility
    {

        #region Rebind
        private static InputActionRebindingExtensions.RebindingOperation rebindOperation;

        public static Action onRebindCancelled;
        public static Action onRebindCompleted;
        public static Action onRebindConfigured;

        private static string[] problematicKeys = new string[]
            { "Arrow", "Home", "End", "Print Screen", "Delete", "Page" };

        /// <summary>
        /// Perform an interactive binding, handling UI components update.
        /// Prevent duplicates in the same action map.
        /// </summary>
        /// <param name="action">Action to rebind</param>
        /// <param name="bindingIndex">Index of the binding to rebind</param>
        /// <param name="allCompositeParts">Is the action composite ?</param>
        /// <param name="actionText">UI text displaying the action binding path</param>
        /// <param name="rebindOverlay">UI panel showing up when waiting for the player rebind</param>
        /// <param name="overlayText">UI text to display while waiting for the player rebind</param>
        /// <param name="cancelText">UI text informing the player of how to cancel the rebind</param>
        /// <param name="controlsExcluded">(OPTIONAL) Array of binding path to exclude from the interactive rebind</param>
        /// <param name="controlPath">(OPTIONAL) Part of the path required in the new binding path</param>
        /// <param name="cancelControl">(OPTIONAL) Path of the control to cancel the rebind</param>
        public static void InteractiveRebind(InputAction action, int bindingIndex, bool allCompositeParts,
            TextMeshProUGUI actionText, GameObject rebindOverlay, TextMeshProUGUI overlayText, TextMeshProUGUI cancelText,
            string[] controlsExcluded = null, string controlPath = "", string cancelControl = "<Keyboard>/escape")
        {
            InteractiveRebind(action, bindingIndex, allCompositeParts, false, actionText, rebindOverlay, overlayText, cancelText, controlsExcluded, controlPath, cancelControl);
        }

        /// <summary>
        /// Perform an interactive binding, handling UI components update.
        /// Prevent duplicates in the same action map.
        /// </summary>
        /// <param name="action">Action to rebind</param>
        /// <param name="bindingIndex">Index of the binding to rebind</param>
        /// <param name="allCompositeParts">Is the action composite ?</param>
        /// <param name="duplicate">Is the binding performed after a duplicate was entered ?</param>
        /// <param name="actionText">UI text displaying the action binding path</param>
        /// <param name="rebindOverlay">UI panel showing up when waiting for the player rebind</param>
        /// <param name="overlayText">UI text to display while waiting for the player rebind</param>
        /// <param name="cancelText">UI text informing the player of how to cancel the rebind</param>
        /// <param name="controlsExcluded">(OPTIONAL) Array of binding path to exclude from the interactive rebind</param>
        /// <param name="controlPath">(OPTIONAL) Part of the path required in the new binding path</param>
        /// <param name="cancelControl">(OPTIONAL) Path of the control to cancel the rebind</param>
        private static void InteractiveRebind(InputAction action, int bindingIndex, bool allCompositeParts, bool duplicate,
            TextMeshProUGUI actionText, GameObject rebindOverlay, TextMeshProUGUI overlayText, TextMeshProUGUI cancelText,
            string[] controlsExcluded, string controlPath = "", string cancelControl = "<Keyboard>/escape")
        {
            if (action == null) return;

            rebindOperation?.Cancel();

            void CleanUp()
            {
                rebindOperation?.Dispose();
                rebindOperation = null;
            }
            void UpdateBindingDisplay()
            {
                if (actionText != null)
                {
                    actionText.text = GetCorrectBindingString(action, bindingIndex);
                }
            }

            // Configure the rebind
            rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
                .WithCancelingThrough(cancelControl)
                .OnCancel(
                    operation =>
                    {
                        onRebindCancelled?.Invoke();

                        rebindOverlay?.SetActive(false);
                        UpdateBindingDisplay();
                        CleanUp();
                    })
                .OnComplete(
                    operation =>
                    {
                        if (CheckDuplicateBindings(action, bindingIndex, allCompositeParts))
                        {
                            action.RemoveBindingOverride(bindingIndex);
                            CleanUp();
                            InteractiveRebind(action, bindingIndex, allCompositeParts, true, actionText, rebindOverlay, overlayText, cancelText, controlsExcluded, controlPath, cancelControl);
                        }
                        else
                        {
                            onRebindCompleted?.Invoke();

                            rebindOverlay?.SetActive(false);
                            UpdateBindingDisplay();
                            CleanUp();

                            // If there's more composite parts we should bind, initiate a rebind for the next part
                            if (allCompositeParts)
                            {
                                var nextBindingIndex = bindingIndex + 1;
                                if (nextBindingIndex < action.bindings.Count && action.bindings[nextBindingIndex].isPartOfComposite)
                                    InteractiveRebind(action, nextBindingIndex, true, actionText, rebindOverlay, overlayText, cancelText, controlsExcluded, controlPath, cancelControl);
                            }
                        }
                    });

            // Exclude controls
            if (controlsExcluded != null)
            {
                for (int i = 0; i < controlsExcluded.Length; i++)
                {
                    rebindOperation.WithControlsExcluding(controlsExcluded[i]);
                }
            }
            // Control path
            if (controlPath != "")
                rebindOperation.WithControlsHavingToMatchPath(controlPath);

            onRebindConfigured?.Invoke();

            // If it's a part binding, show the name of the part in the UI.
            var partName = default(string);
            if (action.bindings[bindingIndex].isPartOfComposite)
                partName = $"Binding '{action.bindings[bindingIndex].name}'. ";

            // Open overlay and display "wait for ..."
            rebindOverlay?.SetActive(true);
            if (rebindOverlay != null && overlayText != null)
            {
                var text = !string.IsNullOrEmpty(rebindOperation.expectedControlType)
                    ? $"{partName}Waiting for {rebindOperation.expectedControlType} input..."
                    : $"{partName}Waiting for input...";
                if (duplicate) text += "\n\n ! This binding is already used !";
                overlayText.text = text;
            }
            if (cancelText != null) cancelText.text = "Cancel with " + CleanBindingPath(InputControlPath.ToHumanReadableString(cancelControl, InputControlPath.HumanReadableStringOptions.OmitDevice));

            rebindOperation.Start();
        }

        // # Helpers #

        /// <summary>
        /// Check if the new binding of action is a duplicate
        /// </summary>
        /// <param name="action">Action currently rebinded</param>
        /// <param name="bindingIndex">Index of the binding to rebind</param>
        /// <param name="isComposite">Is the action to rebind a composite ?</param>
        /// <returns>Whether the new binding is a duplicate</returns>
        private static bool CheckDuplicateBindings(InputAction action, int bindingIndex, bool isComposite)
        {
            return CheckDuplicateBindings(action, bindingIndex, isComposite, out InputAction conflictedAction, out InputBinding conflictedBinding);
        }
        /// <summary>
        /// Check if the new binding of action is a duplicate
        /// </summary>
        /// <param name="action">Action currently rebinded</param>
        /// <param name="bindingIndex">Index of the binding to rebind</param>
        /// <param name="isComposite">Is the action to rebind a composite ?</param>
        /// <param name="conflictedAction">OUT the action that is in conflict with the new binding</param>
        /// <param name="conflictedBinding">OUT the binding that is in conflict with the new binding</param>
        /// <returns>Whether the new binding is a duplicate</returns>
        private static bool CheckDuplicateBindings(InputAction action, int bindingIndex, bool isComposite, out InputAction conflictedAction, out InputBinding conflictedBinding)
        {
            conflictedAction = null;
            conflictedBinding = new();
            InputBinding newBinding = action.bindings[bindingIndex];
            foreach (InputBinding b in action.actionMap.bindings)
            {
                if (b.action == newBinding.action) continue;
                if (b.effectivePath == newBinding.effectivePath && (b.name != "modifier" || newBinding.name != "modifier"))
                {
                    conflictedAction = action.actionMap.FindAction(b.action);
                    conflictedBinding = b;
                    return true;
                }
            }
            if (isComposite)
            {
                for (int i = 1; i < bindingIndex; i++)
                {
                    if (action.bindings[i].effectivePath == newBinding.effectivePath)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Resets an action to its default binding
        /// </summary>
        /// <param name="action">Action to reset</param>
        /// <param name="bindingIndex">Index of the binding to reset</param>
        public static void ResetToDefault(InputAction action, int bindingIndex)
        {
            void ResetDuplicate(int currentBindingIndex)
            {
                if (CheckDuplicateBindings(action, currentBindingIndex, false, out InputAction conflictedAction, out InputBinding conflictedBinding))
                {
                    ResetToDefault(conflictedAction, conflictedAction.GetBindingIndex(conflictedBinding.id));
                }
            }

            if (action.bindings[bindingIndex].isComposite)
            {
                // It's a composite. Remove overrides from part bindings.
                for (var i = bindingIndex + 1; i < action.bindings.Count && action.bindings[i].isPartOfComposite; ++i)
                {
                    action.RemoveBindingOverride(i);
                    ResetDuplicate(i);
                }
            }
            else
            {
                action.RemoveBindingOverride(bindingIndex);
                ResetDuplicate(bindingIndex);
            }
        }

        /// <summary>
        /// Returns the correct (human readable) path of an action binding corresponding to his gears
        /// </summary>
        /// <param name="action">Action to get the binding path from</param>
        /// <param name="bindingIndex">Index of the binding to get the path from</param>
        /// <returns>Human readable binding</returns>
        public static string GetCorrectBindingString(InputAction action, int bindingIndex)
        {
            string cleanText;
            string text = "";//, InputControlPath.HumanReadableStringOptions.UseShortNames);
            if (action.bindings[bindingIndex].isComposite)
            {
                string[] textParts = action.GetBindingDisplayString(bindingIndex).Split('+');
                for (int i = 0; i < textParts.Length; i++)
                {
                    cleanText = CleanBindingPath(InputControlPath.ToHumanReadableString(action.bindings[bindingIndex + i + 1].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice));
                    if (cleanText != "") textParts[i] = cleanText;
                    text += textParts[i] + " + ";
                }
                text = text.Remove(text.LastIndexOf(" + "));
                return text;
            }
            // Clean
            text = CleanBindingPath(InputControlPath.ToHumanReadableString(action.bindings[bindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice));
            if (text != "") return text;

            return action.GetBindingDisplayString();
        }
        /// <summary>
        /// Returns the good path of a binding given error keys found by experience
        /// </summary>
        /// <param name="text">Path</param>
        /// <returns>Cleaned of errors path</returns>
        private static string CleanBindingPath(string text)
        {
            foreach (string key in problematicKeys)
            {
                if (text.Contains(key))
                {
                    if (text.Contains('['))
                        text = text.Remove(text.IndexOf('['));
                    return text;
                }
            }
            return "";
        }

        // ### Extension Methods ###

        /// <summary>
        /// Get the index of a binding in an action bindings array
        /// </summary>
        /// <param name="action">Action to get the bindings array from</param>
        /// <param name="binding">Binding of which to find the index</param>
        /// <returns>Binding index (-1 if not found)</returns>
        /// <exception cref="ArgumentNullException">If action is null</exception>
        public static int GetBindingIndex(this InputAction action, InputBinding binding)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            for (int i = 0; i < action.bindings.Count; i++)
            {
                if (action.bindings[i].id == binding.id)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// Get the index of a binding in an action bindings array given the binding ID
        /// </summary>
        /// <param name="action">Action to get the bindings array from</param>
        /// <param name="bindingId">Binding ID of the searched for binding</param>
        /// <returns>Binding index (-1 if not found)</returns>
        /// <exception cref="ArgumentNullException">If action is null</exception>
        public static int GetBindingIndex(this InputAction action, Guid bindingId)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            for (int i = 0; i < action.bindings.Count; i++)
            {
                if (action.bindings[i].id == bindingId)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// Rebind an action manually
        /// </summary>
        /// <param name="action">Action to rebind</param>
        /// <param name="bindingIndex">Index of the binding to rebind</param>
        /// <param name="overridePath">New binding path</param>
        public static void ManualRebind(this InputAction action, int bindingIndex, string overridePath)
        {
            InputBinding newBinding = action.bindings[bindingIndex];
            newBinding.overridePath = overridePath;
            action.ApplyBindingOverride(bindingIndex, newBinding);
        }

        #endregion
    }
}
