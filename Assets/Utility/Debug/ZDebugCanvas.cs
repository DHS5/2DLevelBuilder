using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Dhs5.Utility
{
    public class ZDebugCanvas : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform layoutRectTransform;

        [Header("Text Prefab")]
        [SerializeField] private TextMeshProUGUI textPrefab;

        private Queue<GameObject> texts = new();

        public void LogOnScreen(string log)
        {
            TextMeshProUGUI newText = Instantiate(textPrefab, layoutRectTransform);
            newText.text = log;
            newText.color = UtilityStates.OnScreendebugColor;
            texts.Enqueue(newText.gameObject);
            Invoke(nameof(DestroyText), UtilityStates.OnScreendebugTime);

            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRectTransform);
        }

        private void DestroyText()
        {
            Destroy(texts.Dequeue());
        }
    }
}
