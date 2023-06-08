using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Dhs5.Utility.EventSystem;
using Dhs5.AdvancedUI;

namespace LevelBuilder2D
{
    public class LevelMenu : MonoBehaviour
    {
        [Header("Level Assets List")]
        [SerializeField] private LevelListSO levelList;

        [Header("Menu Contents List")]
        [SerializeField] private LevelStyleList styleList;

        [Header("Items Menu")]
        [SerializeField] private ItemsMenu itemsMenu;

        [Header("UI components")]
        [SerializeField] private GameObject levelMenu;
        [Space, Space]
        [SerializeField] private GameObject environmentChoiceWindow;
        [Space]
        // Disk
        [SerializeField] private GameObject diskWindow;
        [SerializeField] private AdvancedButton diskBackButton;
        [SerializeField] private AdvancedButton diskChoiceButton;
        [SerializeField] private AdvancedButton diskDeleteButton;
        [SerializeField] private AdvancedButton diskLoadButton;
        [SerializeField] private AdvancedButton diskCreateButton;
        [SerializeField] private AdvancedDropdown diskChoiceDropdown;
        [SerializeField] private TMP_InputField diskNewLevelInput;
        [SerializeField] private ScrollListComponent diskStyleChoiceComponent;
        [Space]
        // Asset
        [SerializeField] private GameObject assetWindow;
        [SerializeField] private AdvancedButton assetBackButton;
        [SerializeField] private AdvancedButton assetChoiceButton;
        [SerializeField] private AdvancedButton assetDeleteButton;
        [SerializeField] private AdvancedButton assetLoadButton;
        [SerializeField] private AdvancedButton assetCreateButton;
        [SerializeField] private AdvancedDropdown assetChoiceDropdown;
        [SerializeField] private TMP_InputField assetNewLevelInput;
        [SerializeField] private ScrollListComponent assetStyleChoiceComponent;

        // Variables
        private List<LevelInfo> diskLevelList;
        private List<string> levelNames;

        private LevelBuilderEnvironment environment;



        private void OnEnable()
        {
            EventManager<LevelBuilderEvent>.StartListening(LevelBuilderEvent.QUIT_BUILDER, OnQuitBuilder);
            EventManager<LevelBuilderEvent>.StartListening(LevelBuilderEvent.BUILDER_CREATED, OnQuitBuilder);
        }
        private void OnDisable()
        {
            EventManager<LevelBuilderEvent>.StopListening(LevelBuilderEvent.QUIT_BUILDER, OnQuitBuilder);
            EventManager<LevelBuilderEvent>.StopListening(LevelBuilderEvent.BUILDER_CREATED, OnQuitBuilder);
        }

        private void OnQuitBuilder()
        {
            InitUI();
        }



        // ### Main Function ###

        private void SendInformation(StartAction startAction)
        {
            string name = "";
            LevelSO levelSO = null;
            ItemsMenuContent menuContent = null;
            if (startAction == StartAction.CREATE)
            {
                name = environment == LevelBuilderEnvironment.DISK ? diskNewLevelInput.text : assetNewLevelInput.text;
                menuContent = styleList.menuContents[environment == LevelBuilderEnvironment.DISK ?
                    diskStyleChoiceComponent.CurrentSelectionIndex : assetStyleChoiceComponent.CurrentSelectionIndex];
            }
            else if (startAction == StartAction.LOAD)
            {
                if (environment == LevelBuilderEnvironment.DISK)
                {
                    name = diskLevelList[diskChoiceDropdown.Value].saveName;
                    menuContent = styleList.GetByIndex(diskLevelList[diskChoiceDropdown.Value].levelStyleIndex);
                }
                else if (environment == LevelBuilderEnvironment.ASSET)
                {
                    levelSO = levelList.levels[assetChoiceDropdown.Value];
                    name = levelSO.level.name;
                    menuContent = styleList.menuContents[assetStyleChoiceComponent.CurrentSelectionIndex];
                }
            }
            itemsMenu.GetStartInfos(menuContent, startAction, environment, name, levelSO);

            levelMenu.SetActive(false);

            EventManager<LevelBuilderEvent>.TriggerEvent(LevelBuilderEvent.OPEN_BUILDER);
        }


        #region Listeners

        private void SendInfoCreate() { SendInformation(StartAction.CREATE); }
        private void SendInfoLoad() { SendInformation(StartAction.LOAD); }

        private void ActuDiskButton(string str)
        {
            diskCreateButton.Interactable = VerifyNewLevelName(str);
        }
        private void ActuAssetButton(string str)
        {
            assetCreateButton.Interactable = VerifyNewLevelName(str);
        }
        private void EnvironmentDisk() 
        { 
            environment = LevelBuilderEnvironment.DISK;
            UIStack.Show(diskWindow);
        }
        private void EnvironmentAsset() 
        {
            environment = LevelBuilderEnvironment.ASSET;
            UIStack.Show(assetWindow);
        }

        private void DeleteFromDisk()
        {
            LevelManager.DeleteLevelFromDisk(diskLevelList[diskChoiceDropdown.Value]);
            FillDiskLevelList();
            FillDiskDropdown();
        }
        private void DeleteFromAsset()
        {
#if UNITY_EDITOR
            LevelManager.DeleteLevelSO(levelList.levels[assetChoiceDropdown.Value], levelList);
            FillAssetDropdown();
#endif
        }
        #endregion

        // ### UI ###

        private void InitUI()
        {
            levelMenu.SetActive(true);

            FillDiskLevelList();
#if UNITY_EDITOR
            FullEnvironment();
            FillAssetDropdown();
            AddAssetListeners();
            ActuAssetButton(assetNewLevelInput.text);
            CreateAssetStyleChoiceList();
#else
            OnlyDiskEnvironment();
#endif
            AddDiskListeners();

            CreateDiskStyleChoiceList();
            FillDiskDropdown();
            ActuDiskButton(diskNewLevelInput.text);
        }

        private void FullEnvironment()
        {
            UIStack.Clear(environmentChoiceWindow);

            // Safety
            diskWindow.SetActive(false);
            assetWindow.SetActive(false);
        }
        private void OnlyDiskEnvironment()
        {
            environmentChoiceWindow.SetActive(false);
            assetWindow.SetActive(false);
            diskBackButton.gameObject.SetActive(false);

            UIStack.Clear(diskWindow);
        }
        private void CreateAssetStyleChoiceList()
        {
            assetStyleChoiceComponent.CreateList(styleList.menuContents);
        }
        private void CreateDiskStyleChoiceList()
        {
            diskStyleChoiceComponent.CreateList(styleList.menuContents);
        }

        private void FillDiskLevelList()
        {
            diskLevelList = LevelManager.GetLevelList();
            levelNames = LevelManager.GetLevelNamesList();
        }
        private void FillDiskDropdown()
        {
            diskChoiceDropdown.ClearOptions();

            if (diskLevelList != null && diskLevelList.Count > 0)
            {
                diskChoiceDropdown.SetOptions(new List<string>(levelNames));
                diskChoiceDropdown.Interactable = true;
                diskLoadButton.Interactable = true;
                diskDeleteButton.Interactable = true;
            }
            else
            {
                diskChoiceDropdown.Interactable = false;
                diskLoadButton.Interactable = false;
                diskDeleteButton.Interactable = false;
            }
        }
        private void FillAssetDropdown()
        {
            assetChoiceDropdown.ClearOptions();

            if (levelList.levels != null && levelList.levels.Count > 0)
            {
                List<string> assetNamesList = new();

                foreach (LevelSO l in levelList.levels)
                {
                    assetNamesList.Add(l.level.name);
                }

                assetChoiceDropdown.SetOptions(new List<string>(assetNamesList));

                assetChoiceDropdown.Interactable = true;
                assetLoadButton.Interactable = true;
                assetDeleteButton.Interactable = true;
            }
            else
            {
                assetChoiceDropdown.Interactable = false;
                assetLoadButton.Interactable = false;
                assetDeleteButton.Interactable = false;
            }
        }
        private void AddDiskListeners()
        {
            diskBackButton.OnClick -= UIStack.Back;
            diskBackButton.OnClick += UIStack.Back;

            diskDeleteButton.OnClick -= DeleteFromDisk;
            diskDeleteButton.OnClick += DeleteFromDisk;

            diskChoiceButton.OnClick -= EnvironmentDisk;
            diskChoiceButton.OnClick += EnvironmentDisk;

            diskCreateButton.OnClick -= SendInfoCreate;
            diskCreateButton.OnClick += SendInfoCreate;

            diskLoadButton.OnClick -= SendInfoLoad;
            diskLoadButton.OnClick += SendInfoLoad;

            diskNewLevelInput.onValueChanged.RemoveAllListeners();
            diskNewLevelInput.onValueChanged.AddListener(ActuDiskButton);
        }
        private void AddAssetListeners()
        {
            assetBackButton.OnClick -= UIStack.Back;
            assetBackButton.OnClick += UIStack.Back;

            assetDeleteButton.OnClick -= DeleteFromAsset;
            assetDeleteButton.OnClick += DeleteFromAsset;

            assetChoiceButton.OnClick -= EnvironmentAsset;
            assetChoiceButton.OnClick += EnvironmentAsset;

            assetCreateButton.OnClick -= SendInfoCreate;
            assetCreateButton.OnClick += SendInfoCreate;

            assetLoadButton.OnClick -= SendInfoLoad;
            assetLoadButton.OnClick += SendInfoLoad;

            assetNewLevelInput.onValueChanged.RemoveAllListeners();
            assetNewLevelInput.onValueChanged.AddListener(ActuAssetButton);
        }


        // ### Helpers ###

        private bool VerifyNewLevelName(string name)
        {
            name.Trim();
            if (name.Length < 5) return false;

            if (environment == LevelBuilderEnvironment.DISK && diskLevelList != null)
            {
                foreach (string str in levelNames)
                {
                    if (str == name) return false;
                }
            }
            else if (environment == LevelBuilderEnvironment.ASSET && levelList.levels != null)
            {
                foreach (LevelSO l in levelList.levels)
                {
                    if (l.level.name == name) return false;
                }
            }

            return true;
        }
    }
}
