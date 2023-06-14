using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.InputSystem;
using TMPro;
using Dhs5.Utility.EventSystem;

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

        [Header("Player Inputs")]
        [SerializeField] private PlayerInput playerInput;

        [Header("Menu Template")]
        [SerializeField] private ItemsMenuTemplate menuTemplate;

        [Header("UI components")]
        [SerializeField] private GameObject itemMenu;
        [SerializeField] private GameObject worldSpaceButtons;
        [Space, Space]
        // Brushes
        [SerializeField] private Toggle paintBrushToggle;
        [SerializeField] private Toggle boxBrushToggle;
        [SerializeField] private Toggle fillBrushToggle;
        [SerializeField] private Toggle pickBrushToggle;
        [Space]
        // Level
        [SerializeField] private GameObject saveButtonAsteriskText;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button clearButton;
        [SerializeField] private Button quitButton;
        [Space]
        // World space
        [SerializeField] private Button rightRoomButton;
        [SerializeField] private Button leftRoomButton;
        [SerializeField] private Button upRoomButton;
        [SerializeField] private Button downRoomButton;

        [Header("Builder Environment")]
        [SerializeField] private SpriteRenderer builderBackground;

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
                    OnCreateLevel = SaveLevelOnDisk;
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



        private event Action OnSaveLevel;
        private event Action OnLoadLevel;
        private event Action OnCreateLevel;


        private void Start()
        {
            EventManager<LevelBuilderEvent>.TriggerEvent(LevelBuilderEvent.CREATE_BUILDER);
        }

        private void OnEnable()
        {
            EventManager<LevelBuilderEvent>.StartListening(LevelBuilderEvent.CREATE_BUILDER, OnCreate);
            EventManager<LevelBuilderEvent>.StartListening(LevelBuilderEvent.BUILDER_CREATED, OnCreated);
            EventManager<LevelBuilderEvent>.StartListening(LevelBuilderEvent.OPEN_BUILDER, OnOpen);
            EventManager<LevelBuilderEvent>.StartListening(LevelBuilderEvent.QUIT_BUILDER, OnQuit);
            EventManager<LevelBuilderEvent>.StartListening(LevelBuilderEvent.SAVE_LEVEL, OnSave);
            EventManager<LevelBuilderEvent>.StartListening(LevelBuilderEvent.DO_ACTION, OnTilemapAction);
            EventManager<LevelBuilderEvent>.StartListening(LevelBuilderEvent.UNDO_ACTION, OnTilemapAction);
        }
        private void OnDisable()
        {
            EventManager<LevelBuilderEvent>.StopListening(LevelBuilderEvent.CREATE_BUILDER, OnCreate);
            EventManager<LevelBuilderEvent>.StopListening(LevelBuilderEvent.BUILDER_CREATED, OnCreated);
            EventManager<LevelBuilderEvent>.StopListening(LevelBuilderEvent.OPEN_BUILDER, OnOpen);
            EventManager<LevelBuilderEvent>.StopListening(LevelBuilderEvent.QUIT_BUILDER, OnQuit);
            EventManager<LevelBuilderEvent>.StopListening(LevelBuilderEvent.SAVE_LEVEL, OnSave);
            EventManager<LevelBuilderEvent>.StopListening(LevelBuilderEvent.DO_ACTION, OnTilemapAction);
            EventManager<LevelBuilderEvent>.StopListening(LevelBuilderEvent.UNDO_ACTION, OnTilemapAction);
        }

        private void OnCreate()
        {
            CreateCategories();

            EventManager<LevelBuilderEvent>.TriggerEvent(LevelBuilderEvent.BUILDER_CREATED);
        }
        private void OnCreated()
        {
            itemMenu.SetActive(false);
            worldSpaceButtons.SetActive(false);
        }
        private void OnOpen()
        {
            itemMenu.SetActive(true);
            worldSpaceButtons.SetActive(true);
            saveButtonAsteriskText.SetActive(false);

            ActuMenu();
            InitBrushes();
            ActuEnvironment();

            AddListeners();

            playerInput.actions.Enable();
            playerInput.actions["Save"].performed += OnControlS;

            if (StartAction == StartAction.CREATE) OnCreateLevel.Invoke();
            else if (StartAction == StartAction.LOAD) OnLoadLevel.Invoke();
        }
        private void OnQuit()
        {
            RemoveListeners();

            playerInput.actions["Save"].performed -= OnControlS;
            playerInput.actions.Disable();

            itemMenu.SetActive(false);
            worldSpaceButtons.SetActive(false);
        }
        private void OnSave()
        {
            OnSaveLevel();
            saveButtonAsteriskText.SetActive(false);
        }
        private void OnTilemapAction()
        {
            saveButtonAsteriskText.SetActive(true);
        }

        #region --- Menu Creation ---

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
                itemToggle.Create(i, toggleGroup, categoryButton);
                categoryButton.items.Add(itemToggle);
            }
        }
        #endregion

        // ### Menu Actualization ###

        private void ActuMenu()
        {
            bool first = true;
            foreach (CategoryButton c in categoryButtons)
            {
                c.SetContainer(true);
                foreach (ItemToggle i in c.items)
                {
                    i.Set(MenuContent);
                    if (first)
                    {
                        first = false;
                        i.Toggle.isOn = true;
                        TilemapManager.onSetTile.Invoke(i.Item);
                    }
                    else i.Toggle.isOn = false;
                }
                c.SetContainer(false);
            }
        }

        private void InitBrushes()
        {
            paintBrushToggle.isOn = true;
        }

        private void ActuEnvironment()
        {
            builderBackground.sprite = MenuContent.styleBackground;
        }

        // ### Listeners ###

        private void AddListeners()
        {
            saveButton.onClick.AddListener(TriggerSave);
            clearButton.onClick.AddListener(ClearTilemaps);
            quitButton.onClick.AddListener(TriggerQuit);

            paintBrushToggle.onValueChanged.AddListener(SetPaintBrush);
            boxBrushToggle.onValueChanged.AddListener(SetBoxBrush);
            fillBrushToggle.onValueChanged.AddListener(SetFillBrush);
            pickBrushToggle.onValueChanged.AddListener(SetPickBrush);

            rightRoomButton.onClick.AddListener(RightRoom);
            leftRoomButton.onClick.AddListener(LeftRoom);
            upRoomButton.onClick.AddListener(UpRoom);
            downRoomButton.onClick.AddListener(DownRoom);

        }
        private void RemoveListeners()
        {
            saveButton.onClick.RemoveAllListeners();
            clearButton.onClick.RemoveAllListeners();
            quitButton.onClick.RemoveAllListeners();

            paintBrushToggle.onValueChanged.RemoveAllListeners();
            boxBrushToggle.onValueChanged.RemoveAllListeners();
            fillBrushToggle.onValueChanged.RemoveAllListeners();
            pickBrushToggle.onValueChanged.RemoveAllListeners();

            rightRoomButton.onClick.RemoveAllListeners();
            leftRoomButton.onClick.RemoveAllListeners();
            upRoomButton.onClick.RemoveAllListeners();
            downRoomButton.onClick.RemoveAllListeners();
        }

        // # Triggers #

        private void TriggerQuit() { EventManager<LevelBuilderEvent>.TriggerEvent(LevelBuilderEvent.QUIT_BUILDER); }
        private void TriggerSave() 
        { 
            EventManager<LevelBuilderEvent>.TriggerEvent(LevelBuilderEvent.BEFORE_SAVE);
            EventManager<LevelBuilderEvent>.TriggerEvent(LevelBuilderEvent.SAVE_LEVEL);
        }

        // # Inputs #

        private void OnControlS(InputAction.CallbackContext ctx)
        {
            TriggerSave();
        }


        // ### Brushes ###

        private void SetPaintBrush(bool b) { TilemapManager.onSetBrush.Invoke(Brush.PAINT); }
        private void SetBoxBrush(bool b) { TilemapManager.onSetBrush.Invoke(Brush.BOX); }
        private void SetFillBrush(bool b) { TilemapManager.onSetBrush.Invoke(Brush.FILL); }
        private void SetPickBrush(bool b) { TilemapManager.onSetBrush.Invoke(Brush.PICK); }

        // ### Room Changes ###

        private void RightRoom() { RoomManager.ChangeRoom(0, 1, tilemapManager.Tilemaps); }
        private void LeftRoom() { RoomManager.ChangeRoom(0, -1, tilemapManager.Tilemaps); }
        private void UpRoom() { RoomManager.ChangeRoom(1, 0, tilemapManager.Tilemaps); }
        private void DownRoom() { RoomManager.ChangeRoom(-1, 0, tilemapManager.Tilemaps); }


        // ### Menu Functions ###

        // Save
        private void SaveLevelOnDisk()
        {
            LevelManager.SaveLevelToDisk(LevelManager.TilemapsToLevel(tilemapManager.Tilemaps, MenuContent, LevelName));
        }
        private void ModifyLevelSO()
        {
#if UNITY_EDITOR
            LevelManager.ModifyLevelSO(LevelSO, LevelManager.TilemapsToLevel(tilemapManager.Tilemaps, MenuContent, LevelSO.level.name));
#endif
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
        private void CreateLevelSO()
        {
#if UNITY_EDITOR
            LevelSO = LevelManager.InstanciateLevelSO(LevelManager.TilemapsToLevel(tilemapManager.Tilemaps, MenuContent, LevelName));
#endif
        }

        // Clear
        private void ClearTilemaps()
        {
            RoomManager.BackToFirstRoom(tilemapManager.Tilemaps);
            TilemapCommandManager.Instance.Clear(tilemapManager.Tilemaps);
        }
    }
}
