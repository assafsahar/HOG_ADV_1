using HOG.Core;
using HOG.GameLogic;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HOG.Components
{
    public class HOGCardClicker : HOGMonoBehaviour
    {
        private HOGDeckManager deckManager;

        public Image topSymbol;
        public Image bottomSymbol;
        public Image leftSymbol;
        public Image rightSymbol;

        public Image card;

        private Image selectedSymbol;

        public Image popUp;
        public Button closeBtn;
        public Animator animator;

        // the following methods are called from the UI buttons (editor)


        private void Awake()
        {
            deckManager = FindObjectOfType<HOGDeckManager>();
            if(deckManager == null)
            {
                HOGDebug.LogException("HOGDeckManager not found. Make sure it exists in the scene.");
            }
        }
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => OnCardClick());

            topSymbol.GetComponent<Button>().onClick.AddListener(() => OnSymbolClick(topSymbol));
            bottomSymbol.GetComponent<Button>().onClick.AddListener(() => OnSymbolClick(bottomSymbol));
            leftSymbol.GetComponent<Button>().onClick.AddListener(() => OnSymbolClick(leftSymbol));
            rightSymbol.GetComponent<Button>().onClick.AddListener(() => OnSymbolClick(rightSymbol));

            topSymbol.GetComponent<Button>().interactable = false;
            bottomSymbol.GetComponent<Button>().interactable = false;
            leftSymbol.GetComponent<Button>().interactable = false;
            rightSymbol.GetComponent<Button>().interactable = false;

            closeBtn.GetComponent<Button>().onClick.AddListener(() => OnCloseCard());

            animator = GetComponent<Animator>();
        }
        private void OnCloseCard()
        {
            popUp.gameObject.SetActive(false);
            closeBtn.gameObject.SetActive(false);

            animator.SetBool("cardReverse", true);


            topSymbol.GetComponent<Button>().interactable = false;
            bottomSymbol.GetComponent<Button>().interactable = false;
            leftSymbol.GetComponent<Button>().interactable = false;
            rightSymbol.GetComponent<Button>().interactable = false;

             WaitForAnimation(1.0f);
            GetComponent<Button>().interactable = true;
           

        }
        private void OnCardClick()
        {
            popUp.gameObject.SetActive(true);
            closeBtn.gameObject.SetActive(true);

            animator.SetBool("cardClicked", true);
            animator.SetBool("cardReverse", false);
            // WaitForAnimation(1.0f);

            topSymbol.GetComponent<Button>().interactable = true;
            bottomSymbol.GetComponent<Button>().interactable = true;
            leftSymbol.GetComponent<Button>().interactable = true;
            rightSymbol.GetComponent<Button>().interactable = true;

            GetComponent<Button>().interactable = false;


        }
        IEnumerator WaitForAnimation(float duration)
        {
            yield return new WaitForSeconds(duration);
            Debug.Log("Animation Finished after waiting!");
        }
        private void OnSymbolClick(Image symbol)
        {
           
            ApplyGlowEffect(symbol);
        }

        public void OnChangeAttackButtonClicked()
        {
            int cardCost = deckManager.configurableCards[0].CardCost; // Set the desired card cost
            int cardValue = deckManager.configurableCards[0].CardValue;
            ChangeAttack(cardValue, cardCost);
        }

        public void OnCharacterNumberButtonClicked()
        {
            int cardCost = deckManager.configurableCards[1].CardCost; // Set the desired card cost
            int cardValue = deckManager.configurableCards[1].CardValue;
            CharacterNumber(cardValue, cardCost);
        }

        public void ChangeAttack(int cardValue, int cardCost)
        {

            if (deckManager != null && deckManager.CurrentEnergy >= cardCost)
            {
                deckManager.UpdateEnergy(-cardCost);
                var amount = HOGGameLogicManager.Instance.UpgradeManager.GetUpgradeableByID(UpgradeablesTypeID.ChangePower).CurrentLevel;
                InvokeEvent(HOGEventNames.OnAbilityChange, new Tuple<HOGCharacterState.CharacterStates, int>(HOGCharacterState.CharacterStates.Attack, amount));
            }
        }

        public void CharacterNumber(int cardValue, int cardCost)
        {
            if (deckManager != null && deckManager.CurrentEnergy >= cardCost)
            {
                deckManager.UpdateEnergy(-cardCost);
                var num = HOGGameLogicManager.Instance.UpgradeManager.GetUpgradeableByID(UpgradeablesTypeID.ChangeCharacter).CurrentLevel;
                var character = HOGGameLogicManager.Instance.UpgradeManager.GetCharacterByIDAndLevel(UpgradeablesTypeID.ChangeCharacter, num - 1);
                InvokeEvent(HOGEventNames.OnCharacterChange, character);
            }
        }

        public void OnUpgradePress(int upgradeId)
        {
            UpgradeablesTypeID upgradable = (UpgradeablesTypeID)upgradeId;
            HOGGameLogicManager.Instance.UpgradeManager.UpgradeItemByID(upgradable);
        }


        public void ApplyGlowEffect(Image symbol)
        {
            ResetGlow();  

           
            Outline outline = symbol.GetComponent<Outline>();
            if (outline == null)
            {
                outline = symbol.gameObject.AddComponent<Outline>();
            }

           
            outline.effectColor = Color.yellow;  
            outline.effectDistance = new Vector2(5, 5); 
        }

        // reset effect on all symbols 
        private void ResetGlow()
        {
            RemoveOutline(topSymbol);
            RemoveOutline(bottomSymbol);
            RemoveOutline(leftSymbol);
            RemoveOutline(rightSymbol);
        }

        
        private void RemoveOutline(Image symbol)
        {
            Outline outline = symbol.GetComponent<Outline>();
            if (outline != null)
            {
                Destroy(outline); 
            }
        }
    }
}
