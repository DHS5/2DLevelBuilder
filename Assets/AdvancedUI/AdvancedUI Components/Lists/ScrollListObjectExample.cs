using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Dhs5.AdvancedUI
{
    public class ScrollListObjectExample : ScrollListObject
    {
        [SerializeField] private TextMeshProUGUI text;

        protected override void SetUp<T>(T objectToSetUpFrom)
        {
            text.text = Index.ToString();
        }

        public override string GetName<T>(T objectToGetNameFrom)
        {
            return Index.ToString();
        }
    }
}
