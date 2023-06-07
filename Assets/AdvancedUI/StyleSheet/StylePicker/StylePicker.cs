using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.AdvancedUI
{
    [System.Serializable]
    public class StylePicker
    {
        [SerializeField] private StyleSheetContainer container;
        [SerializeField] private StyleSheetType type;
        [SerializeField] private int styleSheetUID;
        [SerializeField] private string name;

        public BaseStyleSheet StyleSheet
        {
            get
            {
                if (container == null) return null;
                return container.projectStyleSheet.GetStyleSheet(styleSheetUID, type);
            }
        }

        public void SetUp(StyleSheetContainer _container, StyleSheetType _type, string _name = "Style")
        {
            container = _container;
            type = _type;
            name = _name;
        }

        public void ForceSet(StylePicker picker)
        {
            if (picker == null || picker.container == null) return;

            if (container == picker.container && type == picker.type)
                styleSheetUID = picker.styleSheetUID;
        }
    }
}
