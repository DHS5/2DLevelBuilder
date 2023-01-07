using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using TMPro;

namespace Dhs5.Utility.Input
{
    public static class InputUtility
    {

        #region Rebind
        private static InputActionRebindingExtensions.RebindingOperation rebindOperation;

        public static Action onRebindCancelled;
        public static Action onRebindCompleted;
        public static Action onRebindConfigured;

        private static string[] problematicKeys = new string[]
            { "Arrow", "Home", "End", "Print Screen", "Delete", "Page" };

        public static void InteractiveRebind(InputAction action, int bindingIndex, bool allCompositeParts,
            TextMeshProUGUI actionText, GameObject rebindOverlay, TextMeshProUGUI overlayText,
            string[] controlsExcluded, string controlPath = "", string cancelControl = "<Keyboard>/escape")
        {
            InteractiveRebind(action, bindingIndex, allCompositeParts, false, actionText, rebindOverlay, overlayText, controlsExcluded, controlPath, cancelControl);
        }

        private static void InteractiveRebind(InputAction action, int bindingIndex, bool allCompositeParts, bool duplicate,
            TextMeshProUGUI actionText, GameObject rebindOverlay, TextMeshProUGUI overlayText,
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
                    actionText.text = GetCorrectBindingString(action);
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
                            InteractiveRebind(action, bindingIndex, allCompositeParts, true, actionText, rebindOverlay, overlayText, controlsExcluded, controlPath, cancelControl);
                        }
                        else
                        {
                            onRebindCompleted?.Invoke();

                            rebindOverlay?.SetActive(false);
                            UpdateBindingDisplay();
                            CleanUp();

                            // If there's more composite parts we should bind, initiate a rebind
                            // for the next part.
                            if (allCompositeParts)
                            {
                                var nextBindingIndex = bindingIndex + 1;
                                if (nextBindingIndex < action.bindings.Count && action.bindings[nextBindingIndex].isPartOfComposite)
                                    InteractiveRebind(action, nextBindingIndex, true, actionText, rebindOverlay, overlayText, controlsExcluded);
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

            rebindOperation.Start();
        }

        // # Helpers #

        private static bool CheckDuplicateBindings(InputAction action, int bindingIndex, bool isComposite)
        {
            InputBinding newBinding = action.bindings[bindingIndex];
            foreach (InputBinding b in action.actionMap.bindings)
            {
                if (b.action == newBinding.action) continue;
                if (b.effectivePath == newBinding.effectivePath && (b.name != "modifier" || newBinding.name != "modifier"))
                {
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
        public static string GetCorrectBindingString(InputAction action)
        {
            string Clean(string text)
            {
                foreach (string key in problematicKeys)
                {
                    if (text.Contains(key))
                    {
                        return text.Remove(text.IndexOf('['));
                    }
                }
                return "";
            }

            string cleanText;
            string text = "";//, InputControlPath.HumanReadableStringOptions.UseShortNames);
            if (action.bindings[0].isComposite)
            {
                string[] textParts = action.GetBindingDisplayString().Split('+');
                for (int i = 0; i < textParts.Length; i++)
                {
                    cleanText = Clean(InputControlPath.ToHumanReadableString(action.bindings[i + 1].effectivePath));
                    if (cleanText != "") textParts[i] = cleanText;
                    text += textParts[i] + " + ";
                }
                text = text.Remove(text.LastIndexOf(" + "));
                return text;
            }
            // Clean
            text = Clean(InputControlPath.ToHumanReadableString(action.bindings[0].effectivePath));
            if (text != "") return text;

            return action.GetBindingDisplayString();
        }

        #endregion
    }
}
