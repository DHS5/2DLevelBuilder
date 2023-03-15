using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dhs5.AdvancedUI;

namespace LevelBuilder2D
{
    public class StyleChoicePreview : ScrollListObject
    {
        [SerializeField] private Image picture;

        public override string GetName<T>(T objectToGetNameFrom)
        {
            ItemsMenuContent style = objectToGetNameFrom as ItemsMenuContent;

            return style != null ? style.styleName : string.Empty;
        }

        protected override void SetUp<T>(T objectToSetUpFrom)
        {
            ItemsMenuContent style = objectToSetUpFrom as ItemsMenuContent;

            picture.sprite = style.stylePreview;
        }
    }
}
