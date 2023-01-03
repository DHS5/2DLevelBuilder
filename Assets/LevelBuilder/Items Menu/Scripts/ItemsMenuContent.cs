using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelBuilder2D
{
    [CreateAssetMenu(fileName = "Items Menu Content", menuName = "Items Menu/Content")]
    public class ItemsMenuContent : ScriptableObject
    {
        public string styleName;
        public int styleIndex;

        public Sprite stylePreview;
        public Sprite styleBackground;

        public ItemsMenuTemplate template;

        public ItemsMenuContentCategory[] categories;
        
        public void BuildCategories()
        {
            int size = -1;
            foreach (ItemsMenuCategory c in template.categories)
            {
                if (c.categoryNumber > size) size = c.categoryNumber;
            }

            size++;
            ItemsMenuContentCategory[] newCategories = new ItemsMenuContentCategory[size];

            if (categories == null)
            {
                categories = newCategories;
                foreach (ItemsMenuCategory c in template.categories)
                {
                    categories[c.categoryNumber] = new(c);
                }
                return;
            }

            for (int i = 0; i < Mathf.Min(categories.Length, size); i++)
            {
                newCategories[i] = categories[i];
            }
            categories = newCategories;
            

            foreach (ItemsMenuCategory c in template.categories)
            {
                categories[c.categoryNumber].GetCategoryInfos(c);
            }
        }


        public Dictionary<TileBase, Vector2> BuildDictionnary()
        {
            Dictionary<TileBase, Vector2> dico = new();
            TileBase tile;

            for (int c = 0; c < categories.Length; c++)
            {
                for (int i = 0; i < categories[c].tiles.Length; i++)
                {
                    tile = categories[c].tiles[i];
                    if (tile != null && !dico.ContainsKey(tile))
                        dico.Add(tile, new Vector2(c, i));
                }
            }

            return dico;
        }
    }

    [System.Serializable]
    public class ItemsMenuContentCategory
    {
        public string name;
        public int number;
        public int tilemapLayer;
        public TileBase[] tiles;


        public ItemsMenuContentCategory(ItemsMenuCategory category)
        {
            GetCategoryInfos(category);
        }

        public void GetCategoryInfos(ItemsMenuCategory category)
        {
            name = category.categoryName;
            number = category.categoryNumber;
            tilemapLayer = category.categoryTilemapLayer;
            ActuTilesArray(category);
        }

        private void ActuTilesArray(ItemsMenuCategory category)
        {
            int size = -1;
            foreach (ItemTemplate i in category.items)
            {
                if (i.number > size) size = i.number;
            }

            size++;
            TileBase[] newTiles = new TileBase[size];

            if (tiles == null)
            {
                tiles = newTiles;
                return;
            }
            for (int i = 0; i < Mathf.Min(tiles.Length, size); i++)
            {
                newTiles[i] = tiles[i];
            }
            tiles = newTiles;
        }
    }
}

