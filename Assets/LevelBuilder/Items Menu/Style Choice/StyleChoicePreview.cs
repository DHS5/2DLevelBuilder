using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelBuilder2D
{
    public class StyleChoicePreview : MonoBehaviour
    {
        [SerializeField] private Image picture;

        public Sprite previewSprite
        {
            set { picture.sprite = value; }
        }
    }
}
