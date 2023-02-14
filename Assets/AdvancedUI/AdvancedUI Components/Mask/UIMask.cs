using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

namespace Dhs5.AdvancedUI
{
    #region Mask Content
    [System.Serializable]
    public class MaskContent
    {
        public Sprite maskSprite;
        public bool showImage;
        [ShowIf(nameof(showImage))][AllowNesting] public Sprite imageSprite;
        public bool showRawImage;
        [ShowIf(nameof(showRawImage))][AllowNesting] public Texture rawImageTexture;
    }
    #endregion

    public class UIMask : MonoBehaviour
    {
        [Header("Content")]
        [SerializeField] private MaskContent maskContent;
        public MaskContent Content { get { return maskContent; } set { maskContent = value; } }

        [Header("UI Components")]
        [SerializeField] private Image mask;
        [Space]
        [SerializeField] private Image image;
        [SerializeField] private RawImage rawImage;


        private void Awake()
        {
            SetUp();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            SetUp();   
        }
#endif

        private void SetUp()
        {
            if (mask) mask.sprite = Content.maskSprite;

            if (image)
            {
                image.enabled = Content.showImage;
                image.sprite = Content.imageSprite;
            }
            
            if (rawImage)
            {
                rawImage.enabled = Content.showRawImage;
                rawImage.texture = Content.rawImageTexture;
            }
        }
    }
}
