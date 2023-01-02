using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LevelBuilder2D
{
    public class StyleChoiceComponent : MonoBehaviour
    {
        private enum MoveDirection { LEFT = -1, RIGHT = 1 }

        [Header("Parameters")]
        [SerializeField] private float moveTime;
        [SerializeField] private int moveSteps;
        [SerializeField] private int moveDistance;

        [Header("UI Components")]
        // Preview Display
        [SerializeField] private List<StyleChoicePreview> previews = new();
        [Space]
        // Texts and Buttons
        [SerializeField] private TextMeshProUGUI styleNameText;
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;

        [Header("Content")]
        [SerializeField] private LevelStyleList levelStyles;


        private Vector3 translation;
        private Vector3 completeTranslation;

        private int currentStyleNumber = 0;
        private int CurrentStyleNumber
        {
            get { return GetIndex(currentStyleNumber); }
            set { currentStyleNumber = value; }
        }
        public ItemsMenuContent CurrentStyle
        {
            get { return levelStyles.menuContents[CurrentStyleNumber]; }
        }


        private void Awake()
        {
            translation = new Vector3(moveDistance / moveSteps, 0, 0);
            completeTranslation = new Vector3(moveDistance * previews.Count, 0, 0);

            SetUpPreviews();
        }
        private void OnEnable()
        {
            leftButton.onClick.AddListener(Left);
            rightButton.onClick.AddListener(Right);
        }
        private void OnDisable()
        {
            leftButton.onClick.RemoveAllListeners();
            rightButton.onClick.RemoveAllListeners();
        }

        // ### Initializers ###

        private void SetUpPreviews()
        {
            styleNameText.text = CurrentStyle.styleName;
            for (int i = 0; i < previews.Count / 2 + 1; i++)
            {
                previews[i].previewSprite = levelStyles.menuContents[GetIndex(i)].stylePreview;
            }
            int j = -1;
            for (int i = previews.Count / 2 + 1; i < previews.Count; i++)
            {
                previews[i].previewSprite = levelStyles.menuContents[GetIndex(j)].stylePreview;
                j--;
            }
        }

        // ### Listeners ###
        private void Left()
        {
            OnDisable();

            CurrentStyleNumber++;
            styleNameText.text = CurrentStyle.styleName;

            StyleChoicePreview preview = previews[0];
            previews.Remove(preview);
            previews.Add(preview);

            previews[2].previewSprite =
                levelStyles.menuContents[GetIndex(CurrentStyleNumber + 2)].stylePreview;

            StartCoroutine(MoveCR(MoveDirection.LEFT));
        }
        private void Right()
        {
            OnDisable();

            CurrentStyleNumber--;
            styleNameText.text = CurrentStyle.styleName;

            StyleChoicePreview preview = previews[previews.Count - 1];
            previews.Remove(preview);
            previews.Insert(0, preview);

            previews[3].previewSprite =
                levelStyles.menuContents[GetIndex(CurrentStyleNumber - 2)].stylePreview;

            StartCoroutine(MoveCR(MoveDirection.RIGHT));
        }

        // ### Helpers ###

        /// <summary>
        /// Move the previews in the chosen direction
        /// </summary>
        /// <param name="direction">LEFT : -1 / RIGHT : 1</param>
        /// <returns></returns>
        private IEnumerator MoveCR(MoveDirection direction)
        {
            for (int i = 0; i < moveSteps; i++)
            {
                foreach (StyleChoicePreview s in previews)
                {
                    s.gameObject.transform.Translate((int)direction * translation);
                }
                yield return new WaitForSeconds(moveTime / moveSteps);
            }

            int index = direction == MoveDirection.LEFT ? 2 : 3;
            previews[index].transform.Translate(-(int)direction * completeTranslation);

            OnEnable();
        }

        private int GetIndex(int index)
        {
            while (index < 0) index += levelStyles.menuContents.Length;

            return index % levelStyles.menuContents.Length;
        }
    }
}
