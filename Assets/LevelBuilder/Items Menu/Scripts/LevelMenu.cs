using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LevelBuilder2D
{
    public class LevelMenu : MonoBehaviour
    {
        [Header("Level Assets List")]
        [SerializeField] private LevelListSO levelList;

        [Header("Menu Contents List")]
        [SerializeField] private LevelStyleList styleList; // --> list

        [Header("Items Menu")]
        [SerializeField] private ItemsMenu itemsMenu;

        [Header("UI components")]
        [SerializeField] private GameObject levelMenu;
        [Space, Space]
        [SerializeField] private GameObject environmentChoiceWindow;
        [Space]
        // Disk
        [SerializeField] private GameObject diskWindow;
        [SerializeField] private Button diskBackButton;
        [SerializeField] private Button diskChoiceButton;
        [SerializeField] private Button diskDeleteButton;
        [SerializeField] private Button diskLoadButton;
        [SerializeField] private Button diskCreateButton;
        [SerializeField] private TMP_Dropdown diskChoiceDropdown;
        [SerializeField] private TMP_InputField diskNewLevelInput;
        [Space]
        // Asset
        [SerializeField] private GameObject assetWindow;
        [SerializeField] private Button assetBackButton;
        [SerializeField] private Button assetChoiceButton;
        [SerializeField] private Button assetDeleteButton;
        [SerializeField] private Button assetLoadButton;
        [SerializeField] private Button assetCreateButton;
        [SerializeField] private TMP_Dropdown assetChoiceDropdown;
        [SerializeField] private TMP_InputField assetNewLevelInput;


        // Variables
        private string[] diskLevelList;

        private LevelBuilderEnvironment environment;



        private void OnEnable()
        {
            EventManager.StartListening(EventManager.LevelBuilderEvent.QUIT_BUILDER, OnQuitBuilder);
            EventManager.StartListening(EventManager.LevelBuilderEvent.BUILDER_CREATED, OnQuitBuilder);
        }
        private void OnDisable()
        {
            EventManager.StopListening(EventManager.LevelBuilderEvent.QUIT_BUILDER, OnQuitBuilder);
            EventManager.StopListening(EventManager.LevelBuilderEvent.BUILDER_CREATED, OnQuitBuilder);
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
            if (startAction == StartAction.CREATE)
            {
                name = environment == LevelBuilderEnvironment.DISK ? diskNewLevelInput.text : assetNewLevelInput.text;
                
            }
            else if (startAction == StartAction.LOAD)
            {
                if (environment == LevelBuilderEnvironment.DISK)
                {
                    name = diskLevelList[diskChoiceDropdown.value];
                }
                else if (environment == LevelBuilderEnvironment.ASSET)
                {
                    levelSO = levelList.levels[assetChoiceDropdown.value];
                    name = levelSO.level.name;
                }
            }
            itemsMenu.GetStartInfos(styleList.menuContents[0], startAction, environment, name, levelSO);

            levelMenu.SetActive(false);

            EventManager.TriggerEvent(EventManager.LevelBuilderEvent.OPEN_BUILDER);
        }


        // ### Listeners ###

        private void SendInfoCreate() { SendInformation(StartAction.CREATE); }
        private void SendInfoLoad() { SendInformation(StartAction.LOAD); }

        private void ActuDiskButton(string str)
        {
            diskCreateButton.interactable = VerifyNewLevelName(str);
        }
        private void ActuAssetButton(string str)
        {
            assetCreateButton.interactable = VerifyNewLevelName(str);
        }
        private void EnvironmentDisk() 
        { 
            environment = LevelBuilderEnvironment.DISK;
            diskWindow.SetActive(true);
            environmentChoiceWindow.SetActive(false);
        }
        private void EnvironmentAsset() 
        { 
            environment = LevelBuilderEnvironment.ASSET;
            assetWindow.SetActive(true);
            environmentChoiceWindow.SetActive(false);
        }

        private void BackFromDisk()
        {
            diskWindow.SetActive(false);
            environmentChoiceWindow.SetActive(true);
        }
        private void BackFromAsset()
        {
            assetWindow.SetActive(false);
            environmentChoiceWindow.SetActive(true);
        }
        private void DeleteFromDisk()
        {
            LevelManager.DeleteLevelFromDisk(diskLevelList[diskChoiceDropdown.value]);
            FillDiskDropdown();
        }
        private void DeleteFromAsset()
        {
            LevelManager.DeleteLevelSO(levelList.levels[assetChoiceDropdown.value], levelList);
            FillAssetDropdown();
        }


        // ### UI ###

        private void InitUI()
        {
            levelMenu.SetActive(true);

#if UNITY_EDITOR
            FullEnvironment();
            FillAssetDropdown();
            AddAssetListeners();
            ActuAssetButton(assetNewLevelInput.text);
#else
            OnlyDiskEnvironment();
#endif
            AddDiskListeners();

            FillDiskDropdown();
            ActuDiskButton(diskNewLevelInput.text);
        }

        private void FullEnvironment()
        {
            environmentChoiceWindow.SetActive(true);

            diskWindow.SetActive(false);
            assetWindow.SetActive(false);
        }
        private void OnlyDiskEnvironment()
        {
            environmentChoiceWindow.SetActive(false);
            diskBackButton.gameObject.SetActive(false);

            diskWindow.SetActive(true);
        }

        private void FillDiskDropdown()
        {
            diskLevelList = LevelManager.GetLevelList();
            diskChoiceDropdown.ClearOptions();

            if (diskLevelList != null)
                diskChoiceDropdown.AddOptions(new List<string>(diskLevelList));
            else
            {
                diskChoiceDropdown.interactable = false;
                diskLoadButton.interactable = false;
                diskDeleteButton.interactable = false;
            }
        }
        private void FillAssetDropdown()
        {
            assetChoiceDropdown.ClearOptions();

            if (levelList.levels != null)
            {
                List<string> assetNamesList = new();

                foreach (LevelSO l in levelList.levels)
                {
                    assetNamesList.Add(l.level.name);
                }

                assetChoiceDropdown.AddOptions(new List<string>(assetNamesList));
            }
            else
            {
                assetChoiceDropdown.interactable = false;
                assetLoadButton.interactable = false;
                assetDeleteButton.interactable = false;
            }
        }
        private void AddDiskListeners()
        {
            diskBackButton.onClick.RemoveAllListeners();
            diskBackButton.onClick.AddListener(BackFromDisk);

            diskDeleteButton.onClick.RemoveAllListeners();
            diskDeleteButton.onClick.AddListener(DeleteFromDisk);

            diskChoiceButton.onClick.RemoveAllListeners();
            diskChoiceButton.onClick.AddListener(EnvironmentDisk);

            diskCreateButton.onClick.RemoveAllListeners();
            diskCreateButton.onClick.AddListener(SendInfoCreate);

            diskLoadButton.onClick.RemoveAllListeners();
            diskLoadButton.onClick.AddListener(SendInfoLoad);

            diskNewLevelInput.onValueChanged.RemoveAllListeners();
            diskNewLevelInput.onValueChanged.AddListener(ActuDiskButton);
        }
        private void AddAssetListeners()
        {
            assetBackButton.onClick.RemoveAllListeners();
            assetBackButton.onClick.AddListener(BackFromAsset);

            assetDeleteButton.onClick.RemoveAllListeners();
            assetDeleteButton.onClick.AddListener(DeleteFromAsset);

            assetChoiceButton.onClick.RemoveAllListeners();
            assetChoiceButton.onClick.AddListener(EnvironmentAsset);

            assetCreateButton.onClick.RemoveAllListeners();
            assetCreateButton.onClick.AddListener(SendInfoCreate);

            assetLoadButton.onClick.RemoveAllListeners();
            assetLoadButton.onClick.AddListener(SendInfoLoad);

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
                foreach (string str in diskLevelList)
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
