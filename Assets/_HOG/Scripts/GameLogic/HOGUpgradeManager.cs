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
            HOGManager.Instance.ConfigManager.GetConfigAsync<HOGUpgradeManagerConfig>("upgrade_config", delegate (HOGUpgradeManagerConfig config)
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
                            upgradableTypeID = UpgradeablesTypeID.ChangeAttack,
                            CurrentLevel = 0
                        }
                    }
                };
            });
        }

        public void UpgradeItemByID(UpgradeablesTypeID typeID)
        {
            var upgradeable = GetUpgradeableByID(typeID);

            if (upgradeable != null)
            {
                var upgradeableConfig = GetHogUpgradeableConfigByID(typeID);
                HOGUpgradeableLevelData levelData = upgradeableConfig.UpgradableLevelData[upgradeable.CurrentLevel + 1];
                int amountToReduce = levelData.CoinsNeeded;
                ScoreTags coinsType = levelData.CurrencyTag;

                if (HOGGameLogicManager.Instance.ScoreManager.TryUseScore(coinsType, amountToReduce))
                {
                    upgradeable.CurrentLevel++;
                    HOGManager.Instance.EventsManager.InvokeEvent(HOGEventNames.OnUpgraded, typeID);

                    //HOGManager.Instance.SaveManager.Save(PlayerUpgradeInventoryData);
                }
                else
                {
                    Debug.LogError($"UpgradeItemByID {typeID.ToString()} tried upgrade and there is no enough");
                }
            }
        }

        public HOGUpgradeableConfig GetHogUpgradeableConfigByID(UpgradeablesTypeID typeID)
        {
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