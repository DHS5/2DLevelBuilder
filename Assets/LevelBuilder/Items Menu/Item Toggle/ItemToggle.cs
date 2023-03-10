using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System;
using Dhs5.Utility.Tilemaps;

namespace LevelBuilder2D
{
    public struct Item
    {
        public Item(TileBase _tile, int _layer)
        {
            tile = _tile;
            layer = _layer;
        }

        public TileBase tile;
        public int layer;

        public bool Equal(Item item)
        {
            if (tile == item.tile && layer == item.layer) return true;
            return false;
        }
    }

    public class ItemToggle : MonoBehaviour
    {
        [Header("UI components")]
        [SerializeField] private Toggle toggle;
        [SerializeField] private Image image;

        private Item item;
        private ItemTemplate itemTemplate;
        private CategoryButton categoryButton;


        // Static Action
        public static Action<Item> OnPickTile;

        public Toggle Toggle { get { return toggle; } }

        private ToggleGroup toggleGroup;
        public ToggleGroup Group 
        { 
            get { return toggleGroup; }
            set 
            { 
                toggle.group = value;
                toggleGroup = value;
            } 
        }
        public int ItemNumber { get { return itemTemplate.number; } }

        public Item Item
        {
            get { return item; }
            set
            {
                item = value;
                image.sprite = value.tile.GetSprite();
            }
        }
        

        private void OnEnable()
        {
            toggle.onValueChanged.AddListener(AssignTile);
        }
        private void OnDisable()
        {
            toggle.onValueChanged.RemoveListener(AssignTile);
        }
        private void OnDestroy()
        {
            OnPickTile -= IsPickedTile;
        }


        public void Create(ItemTemplate template, ToggleGroup group, CategoryButton button)
        {
            itemTemplate = template;
            Group = group;
            categoryButton = button;

            OnPickTile += IsPickedTile;
        }
        public void Set(ItemsMenuContent menuContent)
        {
            TileBase t = menuContent.categories[categoryButton.CategoryNumber].tiles[ItemNumber];
            Item = new(t, categoryButton.CategoryTilemapLayer);

            gameObject.SetActive(t != null);
        }


        private void AssignTile(bool value)
        {
            if (value)
            {
                TilemapManager.onSetTile.Invoke(Item);
                DescriptionWindow.onSelectItem.Invoke(itemTemplate);
            }
        }

        private void IsPickedTile(Item item)
        {
            if (Item.Equal(item))
            {
                categoryButton.SetContainer(true);
                toggle.isOn = true;
            }
        }
    }
}
