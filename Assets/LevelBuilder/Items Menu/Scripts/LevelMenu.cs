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

        [Header("UI components")]
        [SerializeField] private GameObject environmentChoiceWindow;
        // Disk
        [SerializeField] private GameObject diskWindow;
        [SerializeField] private Button diskBackButton;
        [SerializeField] private Button diskChoiceButton;
        [SerializeField] private Button diskDeleteButton;
        [SerializeField] private Button diskLoadButton;
        [SerializeField] private Button diskCreateButton;
        [SerializeField] private TMP_Dropdown diskChoiceDropdown;
        [SerializeField] private TMP_InputField diskNewLevelInput;

        // Asset
        [SerializeField] private GameObject assetWindow;
        [SerializeField] private Button assetBackButton;
        [SerializeField] private Button assetChoiceButton;
        [SerializeField] private Button assetDeleteButton;
        [SerializeField] private Button assetLoadButton;
        [SerializeField] private Button assetCreateButton;
        [SerializeField] private TMP_Dropdown assetChoiceDropdown;
        [SerializeField] private TMP_InputField assetNewLevelInput;


        // Readonly


        // Variables
        private string[] diskLevelList;


        private LevelBuilderEnvironment environment;


        /// <summary>
        /// Index of the levelSO in the levels list
        /// </summary>
        public int LevelSO { get; set; }


        private void Awake()
        {
            ItemsMenu.OnQuit += InitUI;
        }



        // ### Functions ###

        private void SendInformation(StartAction startAction)
        {
            if (startAction == StartAction.CREATE)
            {
                string name = environment == LevelBuilderEnvironment.DISK ? diskNewLevelInput.text : assetNewLevelInput.text;
                ItemsMenu.GetStartInfos.Invoke(startAction, environment, name, null);
            }
            else if (startAction == StartAction.LOAD)
            {
                if (environment == LevelBuilderEnvironment.DISK)
                {
                    ItemsMenu.GetStartInfos.Invoke(startAction, environment, diskLevelList[diskChoiceDropdown.value], null);
                }
                else if (environment == LevelBuilderEnvironment.ASSET)
                {
                    LevelSO levelSO = levelList.levels[assetChoiceDropdown.value];
                    ItemsMenu.GetStartInfos.Invoke(startAction, environment, levelSO.level.name, levelSO);
                }
            }

            gameObject.SetActive(false);
        }
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
            gameObject.SetActive(true);

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
