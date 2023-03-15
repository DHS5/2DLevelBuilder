using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace LevelBuilder2D
{
    public class CategoryButton : MonoBehaviour
    {
        [Header("UI components")]
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI text;

        [Header("Items container prefab")]
        [SerializeField] private GameObject containerPrefab;

        private RectTransform rootLayout;

        [HideInInspector] public List<ItemToggle> items = new();

        public string CategoryName 
        {
            get { return text.text; }
            set { text.text = value; } 
        }
        public int CategoryNumber { get; private set; }
        public int CategoryTilemapLayer { get; private set; }
        public GameObject CategoryItemsContainer { get; private set; }


        private void OnEnable()
        {
            button.onClick.AddListener(ChangeContainerState);
        }
        private void OnDisable()
        {
            button.onClick.RemoveListener(ChangeContainerState);
        }


        public void Set(string name, int number, int layer, GameObject container, RectTransform layout)
        {
            CategoryName = name;
            CategoryNumber = number;
            CategoryTilemapLayer = layer;

            rootLayout = layout;
            CategoryItemsContainer = container;
        }

        


        public void SetContainer(bool state) 
        { 
            CategoryItemsContainer.SetActive(state);
            LayoutRebuilder.ForceRebuildLayoutImmediate(rootLayout);
        }

        private void ChangeContainerState()
        {
            SetContainer(!CategoryItemsContainer.activeSelf);
        }
    }
}
