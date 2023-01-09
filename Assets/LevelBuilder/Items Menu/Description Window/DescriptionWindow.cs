using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace LevelBuilder2D
{
    public class DescriptionWindow : MonoBehaviour
    {
        [Header("UI components")]
        [SerializeField] private RectTransform layoutRoot;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;


        public static Action<ItemTemplate> onSelectItem;

        private void OnEnable()
        {
            onSelectItem += ShowItemDescription;
        }
        private void OnDisable()
        {
            onSelectItem -= ShowItemDescription;
        }

        private void ShowItemDescription(ItemTemplate itemTemplate)
        {
            nameText.text = itemTemplate.name;
            descriptionText.text = itemTemplate.description;

            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRoot);
        }
    }
}
