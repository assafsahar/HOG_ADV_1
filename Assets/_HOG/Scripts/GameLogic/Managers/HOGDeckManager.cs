using HOG.Core;
using System;
using System.Collections.Generic;
using HOG.UI;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace HOG.GameLogic
{
    public class HOGDeckManager : HOGMonoBehaviour
    {
        
        [SerializeField] private HOGDeckUI deckUI;
        [SerializeField] private int maxEnergy = 10;
        [SerializeField] private HOGEnergyBarUI energyBar;
        public List<ConfigurableCard> configurableCards = new List<ConfigurableCard>();
        public int CurrentEnergy { get; private set; }
        public float EnergyFillRate = 0.2f; // Energy increase per second
        public int Turn = 0;

        private Coroutine energyFillCoroutine;


        private void OnEnable()
        {
            AddListener(HOGEventNames.OnAbilityChange, DisableAllCards);
            AddListener(HOGEventNames.OnCharacterChange, DisableAllCards);
        }

        private void OnDisable()
        {
            RemoveListener(HOGEventNames.OnAbilityChange, DisableAllCards);
            RemoveListener(HOGEventNames.OnCharacterChange, DisableAllCards);
        }
        private void Awake()
        {
            CurrentEnergy = maxEnergy;
            energyBar.maxValue = maxEnergy;
            //energyBar.SetEnergy(CurrentEnergy);
            AddCardsToDeckUI();
            ShowCardLevel();
            UpdateCardButtonInteractivity();
        }

        void Start()
        {
            ShowCard(2, true, true);
            StartEnergyFillCoroutine();
        }
        public void UpdateEnergy(int amount)
        {
            CurrentEnergy += amount;
            CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0, maxEnergy);
            energyBar.SetEnergy(CurrentEnergy);
            UpdateCardButtonInteractivity();
        }

        public void DisableAllCards(object obj = null)
        {
            foreach (var card in configurableCards)
            {
                if (card != null)
                {
                    //card.CardButton.interactable = false;
                    //ShowCard(card.CardId, true, false);
                }
            }
        }
        public void EnableAllCards()
        {
            foreach (var card in configurableCards)
            {
                if (card != null)
                {
                    card.CardButton.interactable = true;
                    //ShowCard(card.CardId, true, true);
                }
            }
            UpdateCardButtonInteractivity();
        }

        public void StartEnergyFillCoroutine()
        {
            if (energyFillCoroutine == null)
            {
                energyFillCoroutine = StartCoroutine(FillEnergyBarCoroutine());
            }
        }

        public void StopEnergyFillCoroutine()
        {
            if (energyFillCoroutine != null)
            {
                StopCoroutine(energyFillCoroutine);
                energyFillCoroutine = null;
            }
        }

        public void ShowCard(int cardId, bool toShow, bool toEnable)
        {
            if (configurableCards[0] == null)
            {
                HOGDebug.LogException("Card with ID " + cardId + " not found.");
            }
            deckUI.ShowCard(cardId, toShow, toEnable,4); // Todo change 4 to be dynamic
            configurableCards[0].CardVisible = toShow;
            configurableCards[0].CardEnabled = toEnable;
        }

        public void UpdateCardLevel(int cardId, int level)
        {
            deckUI.ChangeCardLevelValue(cardId, level.ToString());
        }

        public void ScoreChanged()
        {
            if (HOGGameLogicManager.Instance.UpgradeManager.CanMakeUpgrade(UpgradeablesTypeID.ChangePower))
            {
                deckUI.ShowUpgradeButton(2, true, true);
            }
            else
            {
                deckUI.ShowUpgradeButton(2, true, false);
            }
           /* if (HOGGameLogicManager.Instance.UpgradeManager.CanMakeUpgrade(UpgradeablesTypeID.ChangeCharacter))
            {
                deckUI.ShowUpgradeButton(1, true, true);
            }
            else
            {
                deckUI.ShowUpgradeButton(1, true, false);
            }*/
        }

        private IEnumerator FillEnergyBarCoroutine()
        {
            while (true)
            {
                while (CurrentEnergy < maxEnergy)
                {
                    UpdateEnergy(1);
                    yield return new WaitForSeconds(1f / EnergyFillRate);
                }

                yield return new WaitUntil(() => CurrentEnergy < maxEnergy);
            }
        }

        private void UpdateCardButtonInteractivity()
        {
            foreach (var card in configurableCards)
            {
                if (card != null)
                {
                    bool isCardEnabled = CurrentEnergy >= card.CardCost; 
                    if(Turn != 1)
                    {
                       // card.CardButton.interactable = isCardEnabled;
                    }
                }
            }
        }

        private void ShowCardLevel()
        {
            int changePowerLevel = 1;
            var changePowerData = HOGGameLogicManager.Instance.UpgradeManager.GetUpgradeableByID(UpgradeablesTypeID.ChangePower);
            if (changePowerData != null)
            {
                changePowerLevel = changePowerData.CurrentLevel;
            }
            /*int changeCharacterLevel = 1;
            var changeCharacterData = HOGGameLogicManager.Instance.UpgradeManager.GetUpgradeableByID(UpgradeablesTypeID.ChangeCharacter);
            if (changeCharacterData != null)
            {
                changeCharacterLevel = changeCharacterData.CurrentLevel;
            }*/

            UpdateCardLevel(2, changePowerLevel);
            //UpdateCardLevel(1, changeCharacterLevel);
        }

        private void AddCardsToDeckUI()
        {
            foreach (var card in configurableCards)
            {
                if (card != null)
                {
                    if (card.CardButton != null)
                    {
                        deckUI.Cards.Add(card.CardId, card.CardButton);
                    }
                    if (card.UpgradeButton != null)
                    {
                        deckUI.UpgradeButtons.Add(card.CardId, card.UpgradeButton);
                    }
                }
            }
        }
    }
}

[Serializable]
public class ConfigurableCard
{
    public Button CardButton;
    public string CardName = "";
    public int CardId = 1;
    public bool CardEnabled = true;
    public bool CardVisible = true;
    public CardTypes CardType = CardTypes.Ability;
    public int CardValue = 0;
    public int CardCost = 1;
    public Button UpgradeButton;
}

public enum CardTypes
{
    Ability = 0,
    Character = 1
}
