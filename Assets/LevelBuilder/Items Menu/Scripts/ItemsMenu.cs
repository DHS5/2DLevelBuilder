using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System;
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

        [Header("Menu Template")]
        [SerializeField] private ItemsMenuTemplate menuTemplate;

        [Header("UI components")]
        [SerializeField] private GameObject itemMenu;
        [Space, Space]
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


        // ### Start Informations ###

        public ItemsMenuContent MenuContent { get; private set; }

        private LevelBuilderEnvironment env;
        public LevelBuilderEnvironment Environment
        {
            get { return env; }
            set 
            { 
                env = value;
                if (value == LevelBuilderEnvironment.DISK)
                {
                    OnSaveLevel = SaveLevelOnDisk;
                    OnLoadLevel = LoadFromDisk;
                    OnCreateLevel = CreateLevelOnDisk;
                }
                else if (value == LevelBuilderEnvironment.ASSET)
                {
                    OnSaveLevel = ModifyLevelSO;
                    OnLoadLevel = LoadLevelSO;
                    OnCreateLevel = CreateLevelSO;
                }
            }
        }
        private StartAction StartAction { get; set; }
        private LevelSO LevelSO { get; set; }
        private string LevelName { get; set; }

        public void GetStartInfos(ItemsMenuContent menuContent, StartAction sa, LevelBuilderEnvironment env, string levelName, LevelSO level)
        {
            MenuContent = menuContent;
            StartAction = sa;
            Environment = env;
            LevelName = levelName;
            LevelSO = level;
        }



        // Actions
        private LevelBuilder_InputActions inputActions;

        private event Action OnSaveLevel;
        private event Action OnLoadLevel;
        private event Action OnCreateLevel;


        private void Awake()
        {
            inputActions = new LevelBuilder_InputActions();
        }

        private void Start()
        {
            EventManager.TriggerEvent(EventManager.LevelBuilderEvent.CREATE_BUILDER);
        }

        private void OnEnable()
        {
            inputActions.LevelBuilder.Save.performed += OnControlS;

            EventManager.StartListening(EventManager.LevelBuilderEvent.CREATE_BUILDER, OnCreate);
            EventManager.StartListening(EventManager.LevelBuilderEvent.BUILDER_CREATED, OnCreated);
            EventManager.StartListening(EventManager.LevelBuilderEvent.OPEN_BUILDER, OnOpen);
            EventManager.StartListening(EventManager.LevelBuilderEvent.QUIT_BUILDER, OnQuit);
            EventManager.StartListening(EventManager.LevelBuilderEvent.SAVE_LEVEL, OnSave);
        }
        private void OnDisable()
        {
            inputActions.LevelBuilder.Save.performed -= OnControlS;

            EventManager.StopListening(EventManager.LevelBuilderEvent.CREATE_BUILDER, OnCreate);
            EventManager.StopListening(EventManager.LevelBuilderEvent.BUILDER_CREATED, OnCreated);
            EventManager.StopListening(EventManager.LevelBuilderEvent.OPEN_BUILDER, OnOpen);
            EventManager.StopListening(EventManager.LevelBuilderEvent.QUIT_BUILDER, OnQuit);
            EventManager.StopListening(EventManager.LevelBuilderEvent.SAVE_LEVEL, OnSave);
        }

        private void OnCreate()
        {
            CreateCategories();

            EventManager.TriggerEvent(EventManager.LevelBuilderEvent.BUILDER_CREATED);
        }
        private void OnCreated()
        {
            itemMenu.SetActive(false);
        }
        private void OnOpen()
        {
            itemMenu.SetActive(true);

            ActuMenu();
            InitBrushes();

            AddListeners();

            inputActions.Enable();

            if (StartAction == StartAction.CREATE) OnCreateLevel.Invoke();
            else if (StartAction == StartAction.LOAD) OnLoadLevel.Invoke();
        }
        private void OnQuit()
        {
            RemoveListeners();

            inputActions.Disable();

            itemMenu.SetActive(false);
        }
        private void OnSave()
        {
            OnSaveLevel();
        }

        #region --- Menu Creation ---
        // ### Menu Creation ###

        /// <summary>
        /// Create all the category buttons and item toggles containers
        /// </summary>
        private void CreateCategories()
        {
            CategoryButton categoryButton;

            foreach (ItemsMenuCategory c in menuTemplate.categories)
            {
                // Create button
                categoryButton = Instantiate(categoryButtonPrefab, categoryButtonsContainer.transform).GetComponent<CategoryButton>();
                categoryButton.Set(c.categoryName, c.categoryNumber, c.categoryTilemapLayer,
                    Instantiate(categoryItemsPrefab, categoryButtonsContainer.transform), rootLayout);

                CreateItems(c, categoryButton);

                categoryButtons.Add(categoryButton);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(rootLayout);
        }

        /// <summary>
        /// Create the item toggles of a category in parent
        /// </summary>
        /// <param name="category">Category of the item toggles</param>
        /// <param name="parent">Parent of the toggles</param>
        private void CreateItems(ItemsMenuCategory category, CategoryButton categoryButton)
        {
            ItemToggle itemToggle;

            foreach (ItemTemplate i in category.items)
            {
                itemToggle = Instantiate(itemTogglePrefab, categoryButton.CategoryItemsContainer.transform).GetComponent<ItemToggle>();
                itemToggle.Create(i.number, toggleGroup, categoryButton);
                categoryButton.items.Add(itemToggle);
            }
        }
        #endregion

        // ### Menu Actualization ###

        private void ActuMenu()
        {
            foreach (CategoryButton c in categoryButtons)
            {
                foreach (ItemToggle i in c.items)
                {
                    i.Set(MenuContent);
                }
                c.SetContainer(false);
            }
        }

        private void InitBrushes()
        {
            paintBrushToggle.isOn = true;
            Item item = new Item { tile = MenuContent.categories[0].tiles[0], layer = MenuContent.categories[0].tilemapLayer };
            TilemapManager.SetTileAction.Invoke(item);
            ItemToggle.OnPickTile.Invoke(item);
        }


        // ### Listeners ###

        private void AddListeners()
        {
            saveButton.onClick.AddListener(TriggerSave);
            quitButton.onClick.AddListener(TriggerQuit);

            paintBrushToggle.onValueChanged.AddListener(SetPaintBrush);
            boxBrushToggle.onValueChanged.AddListener(SetBoxBrush);
            fillBrushToggle.onValueChanged.AddListener(SetFillBrush);
            pickBrushToggle.onValueChanged.AddListener(SetPickBrush);

        }
        private void RemoveListeners()
        {
            saveButton.onClick.RemoveAllListeners();
            quitButton.onClick.RemoveAllListeners();

            paintBrushToggle.onValueChanged.RemoveAllListeners();
            boxBrushToggle.onValueChanged.RemoveAllListeners();
            fillBrushToggle.onValueChanged.RemoveAllListeners();
            pickBrushToggle.onValueChanged.RemoveAllListeners();
        }

        // # Triggers #

        private void TriggerQuit() { EventManager.TriggerEvent(EventManager.LevelBuilderEvent.QUIT_BUILDER); }
        private void TriggerSave() { Debug.Log("trigger save"); EventManager.TriggerEvent(EventManager.LevelBuilderEvent.SAVE_LEVEL); }

        // # Inputs #

        private void OnControlS(InputAction.CallbackContext ctx)
        {
            TriggerSave();
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
            LevelManager.SaveLevelToDisk(LevelManager.TilemapsToLevel(tilemapManager.Tilemaps, MenuContent, LevelName));
        }
        private void ModifyLevelSO()
        {
            LevelManager.ModifyLevelSO(LevelSO, LevelManager.TilemapsToLevel(tilemapManager.Tilemaps, MenuContent, LevelSO.level.name));
        }

        // Load
        private void LoadFromDisk()
        {
            LevelManager.LevelToTilemaps(LevelManager.LoadLevelFromDisk(LevelName), tilemapManager.Tilemaps, MenuContent);
        }
        private void LoadLevelSO()
        {
            LevelManager.LevelToTilemaps(LevelSO.level, tilemapManager.Tilemaps, MenuContent);
        }

        // Create
        private void CreateLevelOnDisk()
        {
            LevelManager.CreateLevelOnDisk(LevelManager.TilemapsToLevel(tilemapManager.Tilemaps, MenuContent, LevelName));
        }
        private void CreateLevelSO()
        {
            LevelSO = LevelManager.InstanciateLevelSO(LevelManager.TilemapsToLevel(tilemapManager.Tilemaps, MenuContent, LevelName));
        }
    }
}
