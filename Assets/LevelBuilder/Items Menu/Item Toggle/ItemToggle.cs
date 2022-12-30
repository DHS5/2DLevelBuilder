using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

namespace LevelBuilder2D
{
    public struct Item
    {
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
        private CategoryButton categoryButton;


        // Static Action
        public static UnityAction<Item> OnPickTile;

        public ToggleGroup Group { set { toggle.group = value; } }

        public Item Item
        {
            get { return item; }
            set
            {
                item = value;
                image.sprite = value.tile.GetSprite();
            }
        }

        private void Start()
        {
            toggle.onValueChanged.AddListener(AssignTile);
        }


        public void GetInfos(ToggleGroup group, CategoryButton button, Item tile)
        {
            Group = group;
            Item = tile;
            categoryButton = button;

            OnPickTile += IsPickedTile;
        }


        private void AssignTile(bool value)
        {
            if (value)
            {
                TilemapManager.SetTileAction.Invoke(Item);
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
