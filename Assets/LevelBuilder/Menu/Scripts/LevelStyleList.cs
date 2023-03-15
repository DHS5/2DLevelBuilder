using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelBuilder2D
{
    [CreateAssetMenu(fileName = "Level Style List", menuName = "Items Menu/Style List")]
    public class LevelStyleList : ScriptableObject
    {
        public List<ItemsMenuContent> menuContents;


        public ItemsMenuContent GetByIndex(int i)
        {
            return menuContents.Find(item => item.styleIndex == i);
        }


        private void OnValidate()
        {
            CleanStyles();
            SortStyles();
        }
        private void OnEnable()
        {
            CleanStyles();
            SortStyles();
        }


        private void CleanStyles()
        {
            menuContents.RemoveAll(x => x == null);
            menuContents.TrimExcess();
        }

        private void SortStyles()
        {
            menuContents.TrimExcess();
            menuContents.Sort(delegate (ItemsMenuContent c1, ItemsMenuContent c2)
            {
                if (c1.styleIndex == c2.styleIndex)
                {
                    Debug.LogError("Items Menu Contents can't have the same style index !");
                    return 0;
                }
                else return c1.styleIndex.CompareTo(c2.styleIndex);
            });
        }
    }
}
