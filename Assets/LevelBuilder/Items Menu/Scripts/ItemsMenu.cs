using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace LevelBuilder2D
{
    public enum LevelBuilderEnvironment
    {
        DISK = 0,
        ASSET = 1
    }
    public enum StartAction
    {
        CREATE = 0,
        LOAD = 1
    }
    

    public class ItemsMenu : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private TilemapManager tilemapManager;

        [Header("Content")]
        [SerializeField] private ItemsMenuContent menuContent;

        [Header("UI components")]
        // Brushes
        [SerializeField] private Toggle paintBrushToggle;
        [SerializeField] private Toggle boxBrushToggle;
        [SerializeField] private Toggle fillBrushToggle;
        [SerializeField] private Toggle pickBrushToggle;
        [Space]
        // Level
        [SerializeField] private Button saveButton;
        [SerializeField] private Button quitButton;

        [Header("Containers")]
        [SerializeField] private GameObject categoryButtonsContainer;
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private RectTransform rootLayout;

        [Header("Prefabs")]
        [SerializeField] private GameObject categoryButtonPrefab;
        [SerializeField] private GameObject categoryItemsPrefab;
        [SerializeField] private GameObject itemTogglePrefab;


        private List<CategoryButton> categoryButtons = new();


        // Variables

        private LevelBuilderEnvironment env;
        public LevelBuilderEnvironment Environment
        {
            get { return env; }
            set 
            { 
                env = value;
                if (value == LevelBuilderEnvironment.DISK)
                {
                    OnSave = SaveLevelOnDisk;
                    OnLoad = LoadFromDisk;
                    OnCreate = CreateLevelOnDisk;
                }
                else if (value == LevelBuilderEnvironment.ASSET)
                {
                    OnSave = ModifyLevelSO;
                    OnLoad = LoadLevelSO;
                    OnCreate = CreateLevelSO;
                }
            }
        }

        private StartAction StartAction { get; set; }

        private LevelSO LevelSO { get; set; }

        private string LevelName { get; set; }


        // Actions
        private LevelBuilder_InputActions inputActions;

        private UnityAction OnSave;
        private UnityAction OnLoad;
        private UnityAction OnCreate;

        public static UnityAction OnStart { get; set; }
        public static UnityAction<StartAction, LevelBuilderEnvironment, string, LevelSO> GetStartInfos { get; set; }
        public static UnityAction OnQuit { get; set; }


        private void Awake()
        {
            OnStart += Enable;
            GetStartInfos += GetStartInformations;
            OnQuit += Disable;

            CreateUI();

            inputActions = new LevelBuilder_InputActions();
            inputActions.LevelBuilder.Save.performed += OnControlS;
        }

        private void Start()
        {
            OnQuit.Invoke();
        }

        private void GetStartInformations(StartAction sa, LevelBuilderEnvironment env, string levelName, LevelSO level)
        {
            StartAction = sa;
            Environment = env;
            LevelName = levelName;
            LevelSO = level;

            OnStart.Invoke();
        }

        private void Enable()
        {
            gameObject.SetActive(true);

            InitUI();

            if (StartAction == StartAction.CREATE) OnCreate.Invoke();
            else if (StartAction == StartAction.LOAD) OnLoad.Invoke();

            inputActions.Enable();
        }

        private void Disable()
        {
            gameObject.SetActive(false);

            inputActions.Disable();
        }


        // ### Menu Creation ###

        /// <summary>
        /// Create all the category buttons and item toggles containers
        /// </summary>
        private void CreateCategories()
        {
            CategoryButton categoryButton;
            GameObject categoryItemsContainer;

            foreach (ItemsMenuContentCategory c in menuContent.categories)
            {
                // Create button
                categoryButton = Instantiate(categoryButtonPrefab, categoryButtonsContainer.transform).GetComponent<CategoryButton>();
                // Create toggles container
                categoryItemsContainer = Instantiate(categoryItemsPrefab, categoryButtonsContainer.transform);
            
                categoryButton.GetInfos(c.name, c.tilemapLayer, categoryItemsContainer, rootLayout);
            
                CreateItems(c, categoryButton, categoryItemsContainer.transform);

                categoryButtons.Add(categoryButton);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(rootLayout);
        }
        private void CloseCategories()
        {
            foreach (CategoryButton categoryButton in categoryButtons)
                categoryButton.SetContainer(false);
        }

        /// <summary>
        /// Create the item toggles of a category in parent
        /// </summary>
        /// <param name="category">Category of the item toggles</param>
        /// <param name="parent">Parent of the toggles</param>
        private void CreateItems(ItemsMenuContentCategory category, CategoryButton categoryButton, Transform parent)
        {
            ItemToggle itemToggle;

            foreach (TileBase t in category.tiles)
            {
                if (t != null)
                {
                    itemToggle = Instantiate(itemTogglePrefab, parent).GetComponent<ItemToggle>();
                    itemToggle.GetInfos(toggleGroup, categoryButton, new Item { tile = t, layer = category.tilemapLayer });
                }
            }
        }

        private void InitBrushes()
        {
            paintBrushToggle.isOn = true;
            Item item = new Item { tile = menuContent.categories[0].tiles[0], layer = menuContent.categories[0].tilemapLayer };
            TilemapManager.SetTileAction.Invoke(item);
            ItemToggle.OnPickTile.Invoke(item);
        }


        // ### UI ###

        private void CreateUI()
        {
            CreateCategories();

            InitUI();
        }

        private void InitUI()
        {
            CloseCategories();
            InitBrushes();

            AddListeners();
        }


        private void AddListeners()
        {
            saveButton.onClick.RemoveAllListeners();
            saveButton.onClick.AddListener(OnSave);
            quitButton.onClick.RemoveAllListeners();
            quitButton.onClick.AddListener(OnQuit);

            paintBrushToggle.onValueChanged.RemoveAllListeners();
            paintBrushToggle.onValueChanged.AddListener(SetPaintBrush);
            boxBrushToggle.onValueChanged.RemoveAllListeners();
            boxBrushToggle.onValueChanged.AddListener(SetBoxBrush);
            fillBrushToggle.onValueChanged.RemoveAllListeners();
            fillBrushToggle.onValueChanged.AddListener(SetFillBrush);
            pickBrushToggle.onValueChanged.RemoveAllListeners();
            pickBrushToggle.onValueChanged.AddListener(SetPickBrush);

        }


        // ### Brushes ###

        private void SetPaintBrush(bool b) { TilemapManager.SetBrushAction.Invoke(Brush.PAINT); }
        private void SetBoxBrush(bool b) { TilemapManager.SetBrushAction.Invoke(Brush.BOX); }
        private void SetFillBrush(bool b) { TilemapManager.SetBrushAction.Invoke(Brush.FILL); }
        private void SetPickBrush(bool b) { TilemapManager.SetBrushAction.Invoke(Brush.PICK); }


        // ### Menu Functions ###

        // Save
        private void SaveLevelOnDisk()
        {
            LevelManager.SaveLevelToDisk(LevelManager.TilemapsToLevel(tilemapManager.Tilemaps, menuContent, LevelName));
        }
        private void ModifyLevelSO()
        {
            LevelManager.ModifyLevelSO(LevelSO, LevelManager.TilemapsToLevel(tilemapManager.Tilemaps, menuContent, LevelSO.level.name));
        }

        // Load
        private void LoadFromDisk()
        {
            LevelManager.LevelToTilemaps(LevelManager.LoadLevelFromDisk(LevelName), tilemapManager.Tilemaps, menuContent);
        }
        private void LoadLevelSO()
        {
            LevelManager.LevelToTilemaps(LevelSO.level, tilemapManager.Tilemaps, menuContent);
        }

        // Create
        private void CreateLevelOnDisk()
        {
            LevelManager.CreateLevelOnDisk(LevelManager.TilemapsToLevel(tilemapManager.Tilemaps, menuContent, LevelName));
        }
        private void CreateLevelSO()
        {
            LevelSO = LevelManager.InstanciateLevelSO(LevelManager.TilemapsToLevel(tilemapManager.Tilemaps, menuContent, LevelName));
        }


        // ### Inputs ###

        private void OnControlS(InputAction.CallbackContext ctx)
        {
            tilemapManager.PreviewHandler.HidePreview();
            OnSave();
        }
    }
}
