using System;
using System.Collections.Generic;
using System.Linq;
using HOG.Character;
using HOG.Core;
using UnityEngine;

namespace HOG.GameLogic
{
    public class HOGUpgradeManager
    {
        public HOGPlayerUpgradeInventoryData PlayerUpgradeInventoryData; //Player Saved Data
        public HOGUpgradeManagerConfig UpgradeConfig = new HOGUpgradeManagerConfig(); //From cloud
        public HOGUpgradableAttacksConfig UpgradeAttacksConfig = new HOGUpgradableAttacksConfig();

        //MockData
        //Load From Save Data On Device (Future)
        //Load Config From Load
        public HOGUpgradeManager()
        {
            HOGManager.Instance.ConfigManager.GetConfigAsync<HOGUpgradeManagerConfig>("UpgradableConfig", delegate (HOGUpgradeManagerConfig config)
            {
                UpgradeConfig = config;
            });
            HOGManager.Instance.ConfigManager.GetConfigAsync<HOGUpgradableAttacksConfig>("UpgradableAttacks", delegate (HOGUpgradableAttacksConfig attacksConfig)
            {
                UpgradeAttacksConfig = attacksConfig;
            });

            HOGManager.Instance.SaveManager.Load(delegate (HOGPlayerUpgradeInventoryData data)
            {
                PlayerUpgradeInventoryData = data ?? new HOGPlayerUpgradeInventoryData
                {
                    Upgradeables = new List<HOGUpgradeableData>(){new HOGUpgradeableData
                        {
                            upgradableTypeID = UpgradeablesTypeID.ChangePower,
                            CurrentLevel = 1
                        }
                    }
                };
            });
        }

        public bool CanMakeUpgrade(UpgradeablesTypeID typeID)
        {
            return UpgradeItemByID(typeID, false);
        }
        public bool UpgradeItemByID(UpgradeablesTypeID typeID, bool makeTheUpgrade = true)
        {
            var upgradeable = GetUpgradeableByID(typeID);

            if (upgradeable != null)
            {
                var upgradeableConfig = GetHogUpgradeableConfigByID(typeID);
                if(upgradeableConfig.UpgradableLevelData.Count <= upgradeable.CurrentLevel)
                {
                    return false;
                }
                HOGUpgradeableLevelData levelData = upgradeableConfig.UpgradableLevelData[upgradeable.CurrentLevel];
                int amountToReduce = levelData.CoinsNeeded;
                ScoreTags coinsType = levelData.CurrencyTag;
                int newLevel = levelData.Level;

                if (HOGGameLogicManager.Instance.ScoreManager.TryUseScore(coinsType, amountToReduce, makeTheUpgrade))
                {
                    if (makeTheUpgrade)
                    {
                        upgradeable.CurrentLevel++;
                        HOGManager.Instance.EventsManager.InvokeEvent(HOGEventNames.OnUpgraded, (coinsType, amountToReduce, newLevel, (int)typeID));
                        HOGManager.Instance.SaveManager.Save(PlayerUpgradeInventoryData);
                    }
                    return true;
                }
                else
                {
                    if (makeTheUpgrade)
                    {
                        Debug.LogError($"UpgradeItemByID {typeID.ToString()} tried upgrade and there is no enough");
                    }
                    return false;
                }
            }
            else
            {
                HOGDebug.Log("failed because upgradable was null");
                return false;
            }
        }

        
        public HOGUpgradeableConfig GetHogUpgradeableConfigByID(UpgradeablesTypeID typeID)
        {
            HOGUpgradeManagerConfig hOGUpgradeManagerConfig = UpgradeConfig;
            HOGUpgradeableConfig upgradeableConfig = UpgradeConfig.UpgradeableConfigs.FirstOrDefault(upgradable => upgradable.UpgradableTypeID == typeID);
            return upgradeableConfig;
        }
        public HOGUpgradableAttacksConfig GetHogAttackConfig()
        {
            //HOGAttacksConfig attackConfig = UpgradeAttacksConfig.UpgradableAttacks.FirstOrDefault(upgradable => upgradable.CharacterType == typeID);
            HOGUpgradableAttacksConfig attackConfig = UpgradeAttacksConfig;
            HOGDebug.Log($"GetHogAttackConfig {attackConfig.ToString()}");
            return attackConfig;
        }
        //.UpgradableAttacks.FirstOrDefault(upgradable => upgradable.CharacterType == typeID)
        public int GetPowerByIDAndLevel(UpgradeablesTypeID typeID, int level)
        {
            var upgradeableConfig = GetHogUpgradeableConfigByID(typeID);
            var power = upgradeableConfig.UpgradableLevelData[level].Power;
            return power;
        }

        public HOGUpgradeableData GetUpgradeableByID(UpgradeablesTypeID typeID)
        {
            var upgradeable = PlayerUpgradeInventoryData.Upgradeables.FirstOrDefault(x => x.upgradableTypeID == typeID);
            return upgradeable;
        }
    }


    //Per Player Owned Item
    [Serializable]
    public class HOGUpgradeableData
    {
        public UpgradeablesTypeID upgradableTypeID;
        public int CurrentLevel;
    }

    //Per Level in Item config
    [Serializable]
    public struct HOGUpgradeableLevelData
    {
        public int Level;
        public int CoinsNeeded;
        public ScoreTags CurrencyTag;
        public int Power;
    }

    //Per Item Config
    [Serializable]
    public class HOGUpgradeableConfig
    {
        public UpgradeablesTypeID UpgradableTypeID;
        public List<HOGUpgradeableLevelData> UpgradableLevelData;
    }
    public class HOGAttacksConfig
    {
        public int CharacterType;
        public List<HOGCharacterAction> CharacterActions;
    }

    //All config for upgradeable
    [Serializable]
    public class HOGUpgradeManagerConfig
    {
        public List<HOGUpgradeableConfig> UpgradeableConfigs;
    }
    public class HOGUpgradableAttacksConfig
    {
        public List<HOGAttacksConfig> UpgradableAttacks;
    }

    //All player saved data
    [Serializable]
    public class HOGPlayerUpgradeInventoryData: IHOGSaveData
    {
        public List<HOGUpgradeableData> Upgradeables;
    }

    [Serializable]
    public enum UpgradeablesTypeID
    {
        ChangeAttack = 0,
        ChangeCharacter = 1,
        ChangePower = 2,
        DoubleAttack = 3,
        Combo = 4
        
    }


    
}