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
        private int categoryTilemapLayer = 0;

        [Header("UI components")]
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI text;

        private GameObject itemsContainer;
        private RectTransform rootLayout;

        private string CategoryName { set { text.text = value; } }



        private void Start()
        {
            // change tilemap layer

            button.onClick.AddListener(ChangeContainerState);
        }


        /// <summary>
        /// Get button useful infos
        /// </summary>
        /// <param name="name">Category name</param>
        /// <param name="layer">Tilemap layer</param>
        /// <param name="container">Item toggles container</param>
        public void GetInfos(string name, int layer, GameObject container, RectTransform layout)
        {
            CategoryName = name;
            categoryTilemapLayer = layer;
            itemsContainer = container;
            rootLayout = layout;
        }

        public void SetContainer(bool state) 
        { 
            itemsContainer.SetActive(state);
            LayoutRebuilder.ForceRebuildLayoutImmediate(rootLayout);
        }

        private void ChangeContainerState()
        {
            SetContainer(!itemsContainer.activeSelf);
        }
    }
}
