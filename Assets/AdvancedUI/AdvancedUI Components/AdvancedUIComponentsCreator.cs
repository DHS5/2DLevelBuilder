using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Dhs5.AdvancedUI
{
    public static class AdvancedUIComponentsCreator
    {
        private static void CreateAdvancedUIObject(string path)
        {
            GameObject go = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
            GameObject obj = PrefabUtility.InstantiatePrefab(go, Selection.activeTransform) as GameObject;
            Selection.activeGameObject = obj;
        }

        #region Images

        [MenuItem("GameObject/UI/AdvancedUI/Images/Frame", priority = 0)]
        public static void CreateSimpleImage()
        {
            CreateAdvancedUIObject("Assets/AdvancedUI/AdvancedUI Prefabs/AdvancedImage.prefab");
        }

        #endregion
        
        #region Texts

        // ### Texts ###

        [MenuItem("GameObject/UI/AdvancedUI/Texts/Text", priority = 0)]
        public static void CreateSimpleText()
        {
            CreateAdvancedUIObject("Assets/AdvancedUI/AdvancedUI Prefabs/AdvancedText.prefab");
        }

        #endregion
        
        #region Buttons

        // ### Buttons ###

        [MenuItem("GameObject/UI/AdvancedUI/Buttons/Button")]
        public static void CreateSimpleButton()
        {
            CreateAdvancedUIObject("Assets/AdvancedUI/AdvancedUI Prefabs/AdvancedButton.prefab");
        }
        
        [MenuItem("GameObject/UI/AdvancedUI/Buttons/PopUp Button")]
        public static void CreatePopupButton()
        {
            CreateAdvancedUIObject("Assets/AdvancedUI/AdvancedUI Prefabs/PopUp Button.prefab");
        }

        #endregion

        #region Toggles

        // ### Toggles ###

        [MenuItem("GameObject/UI/AdvancedUI/Toggles/Toggle")]
        public static void CreateSimpleToggle()
        {
            CreateAdvancedUIObject("Assets/AdvancedUI/AdvancedUI Prefabs/AdvancedToggle.prefab");
        }
        [MenuItem("GameObject/UI/AdvancedUI/Toggles/DropdownItem")]
        public static void CreateDropdownItemToggle()
        {
            CreateAdvancedUIObject("Assets/AdvancedUI/AdvancedUI Prefabs/DropdownItemToggle.prefab");
        }
        [MenuItem("GameObject/UI/AdvancedUI/Toggles/Switch")]
        public static void CreateSwitchToggle()
        {
            CreateAdvancedUIObject("Assets/AdvancedUI/AdvancedUI Prefabs/SwitchToggle.prefab");
        }

        #endregion

        #region Sliders

        // ### Sliders ###

        [MenuItem("GameObject/UI/AdvancedUI/Sliders/Slider")]
        public static void CreateSimpleSlider()
        {
            CreateAdvancedUIObject("Assets/AdvancedUI/AdvancedUI Prefabs/AdvancedSlider.prefab");
        }
        [MenuItem("GameObject/UI/AdvancedUI/Sliders/InputSlider")]
        public static void CreateInputSlider()
        {
            CreateAdvancedUIObject("Assets/AdvancedUI/AdvancedUI Prefabs/InputSlider.prefab");
        }

        #endregion

        #region Dropdowns

        // ### Dropdowns ###

        [MenuItem("GameObject/UI/AdvancedUI/Dropdowns/Dropdown")]
        public static void CreateSimpleDropdown()
        {
            CreateAdvancedUIObject("Assets/AdvancedUI/AdvancedUI Prefabs/AdvancedDropdown.prefab");
        }

        #endregion

        #region Scrollbars

        // ### Scrollbars ###

        [MenuItem("GameObject/UI/AdvancedUI/Scrollbars/Scrollbar")]
        public static void CreateSimpleScrollbar()
        {
            CreateAdvancedUIObject("Assets/AdvancedUI/AdvancedUI Prefabs/AdvancedScrollbar.prefab");
        }

        #endregion
        
        #region InputFields

        // ### InputFields ###

        [MenuItem("GameObject/UI/AdvancedUI/InputFields/InputField")]
        public static void CreateSimpleInutField()
        {
            CreateAdvancedUIObject("Assets/AdvancedUI/AdvancedUI Prefabs/AdvancedInputField.prefab");
        }

        #endregion

        #region ScrollViews

        // ### ScrollViews ###

        [MenuItem("GameObject/UI/AdvancedUI/ScrollViews/ScrollView")]
        public static void CreateSimpleScrollView()
        {
            CreateAdvancedUIObject("Assets/AdvancedUI/AdvancedUI Prefabs/AdvancedScrollView.prefab");
        }
        [MenuItem("GameObject/UI/AdvancedUI/ScrollViews/Vertical ScrollView")]
        public static void CreateVerticalScrollView()
        {
            CreateAdvancedUIObject("Assets/AdvancedUI/AdvancedUI Prefabs/AdvancedVerticalScrollView.prefab");
        }
        [MenuItem("GameObject/UI/AdvancedUI/ScrollViews/Horizontal ScrollView")]
        public static void CreateHorizontalScrollView()
        {
            CreateAdvancedUIObject("Assets/AdvancedUI/AdvancedUI Prefabs/AdvancedHorizontalScrollView.prefab");
        }

        #endregion

        #region ScrollList

        // ### ScrollLists ###

        [MenuItem("GameObject/UI/AdvancedUI/ScrollLists/ScrollList")]
        public static void CreateSimpleScrollList()
        {
            CreateAdvancedUIObject("Assets/AdvancedUI/AdvancedUI Prefabs/ScrollList.prefab");
        }

        #endregion

        #region Popup

        // ### Popup ###

        [MenuItem("GameObject/UI/AdvancedUI/PopUps/PopUp")]
        public static void CreateSimplePopup()
        {
            CreateAdvancedUIObject("Assets/AdvancedUI/AdvancedUI Prefabs/PopUp.prefab");
        }

        #endregion
        
        #region Mask

        // ### Mask ###

        [MenuItem("GameObject/UI/AdvancedUI/Masks/Mask")]
        public static void CreateSimpleMask()
        {
            CreateAdvancedUIObject("Assets/AdvancedUI/AdvancedUI Prefabs/UIMask.prefab");
        }

        #endregion
        
        #region Foldout

        [MenuItem("GameObject/UI/AdvancedUI/Foldout/Vertical Foldout")]
        public static void CreateVerticalFoldout()
        {
            CreateAdvancedUIObject("Assets/AdvancedUI/AdvancedUI Prefabs/VerticalFoldout.prefab");
        }
        
        [MenuItem("GameObject/UI/AdvancedUI/Foldout/Horizontal Foldout")]
        public static void CreateHorizontalFoldout()
        {
            CreateAdvancedUIObject("Assets/AdvancedUI/AdvancedUI Prefabs/HorizontalFoldout.prefab");
        }
        
        [MenuItem("GameObject/UI/AdvancedUI/Foldout/Grid Foldout")]
        public static void CreateGridFoldout()
        {
            CreateAdvancedUIObject("Assets/AdvancedUI/AdvancedUI Prefabs/GridFoldout.prefab");
        }

        #endregion
    }
}
#endif
