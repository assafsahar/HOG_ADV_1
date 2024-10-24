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

        public Button HitArea;

        public Button topBtn;
        public Button bottomBtn;
        public Button leftBtn;
        public Button rightBtn;

        public Image glow_topSymbol;
        public Image glow_bottomSymbol;
        public Image glow_leftSymbol;
        public Image glow_rightSymbol;


        public Image card;

        private Image selectedSymbol;

        public Image popUp;
        public Button closeBtn;
        public Animator animator;


        // the following methods are called from the UI buttons (editor)

        private Vector2 startTouchPosition, endTouchPosition;

       /* void Update()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startTouchPosition = Input.GetTouch(0).position;
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                endTouchPosition = Input.GetTouch(0).position;

                // 
                Vector2 direction = endTouchPosition - startTouchPosition;

                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    // right/left
                    if (direction.x > 0)
                        SwipeDirection("right");
                    else
                        SwipeDirection("left");
                }
                else
                {
                    // up / down
                    if (direction.y > 0)
                        SwipeDirection("up");
                    else
                        SwipeDirection("down");
                }
            }
        }*/

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
            HitArea.onClick.AddListener(() => OnCardClick());

            topBtn.onClick.AddListener(() => OnSymbolClick(glow_topSymbol, CardSwipeDirections.up));
            bottomBtn.onClick.AddListener(() => OnSymbolClick(glow_bottomSymbol, CardSwipeDirections.down));
            leftBtn.onClick.AddListener(() => OnSymbolClick(glow_leftSymbol, CardSwipeDirections.left));
            rightBtn.onClick.AddListener(() => OnSymbolClick(glow_rightSymbol, CardSwipeDirections.right));

            topBtn.interactable = false;
            bottomBtn.interactable = false;
            leftBtn.interactable = false;
            rightBtn.interactable = false;

            closeBtn.GetComponent<Button>().onClick.AddListener(() => OnCloseCard());

            animator = GetComponent<Animator>();
        }
        public void SwipeDirection(string direction)
        {

            ResetGlow();


            switch (direction)
            {
                case "up":
                    selectedSymbol = topSymbol;
                    break;
                case "down":
                    selectedSymbol = bottomSymbol;
                    break;
                case "left":
                    selectedSymbol = leftSymbol;
                    break;
                case "right":
                    selectedSymbol = rightSymbol;
                    break;
            }
        }
            private void OnCloseCard()
        {
            popUp.gameObject.SetActive(false);
            closeBtn.gameObject.SetActive(false);

            animator.SetBool("cardReverse", true);


            topBtn.interactable = false;
            bottomBtn.GetComponent<Button>().interactable = false;
            leftBtn.GetComponent<Button>().interactable = false;
            rightBtn.GetComponent<Button>().interactable = false;

            // WaitForAnimation(1.0f);
            HitArea.interactable = true;
           

        }
        private void OnCardClick()
        {
            Debug.Log("Anat"+gameObject.GetComponent<RectTransform>().rect);

            popUp.gameObject.SetActive(true);
            closeBtn.gameObject.SetActive(true);

            animator.SetBool("cardClicked", true);
            animator.SetBool("cardReverse", false);
            // WaitForAnimation(1.0f);

            topBtn.interactable = true;
            bottomBtn.GetComponent<Button>().interactable = true;
            leftBtn.GetComponent<Button>().interactable = true;
            rightBtn.GetComponent<Button>().interactable = true;

            HitArea.interactable = false;


        }
        IEnumerator WaitForAnimation(float duration)
        {
            yield return new WaitForSeconds(duration);
            Debug.Log("Animation Finished after waiting!");
        }
        private void OnSymbolClick(Image symbol, CardSwipeDirections direction)
        {
            ChangeAttack(0, 10);
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
                //InvokeEvent(HOGEventNames.OnAbilityChange, new Tuple<HOGCharacterState.CharacterStates, int>(HOGCharacterState.CharacterStates.Attack, amount));
                InvokeEvent(HOGEventNames.OnAbilityChange, new Tuple<HOGCharacterState.CharacterStates, int>(HOGCharacterState.CharacterStates.Defense, 20));
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


            symbol.gameObject.SetActive(true);
        }

        // reset effect on all symbols 
        private void ResetGlow()
        {
            RemoveOutline(glow_topSymbol);
            RemoveOutline(glow_bottomSymbol);
            RemoveOutline(glow_leftSymbol);
            RemoveOutline(glow_rightSymbol);
        }

        
        private void RemoveOutline(Image symbol)
        {
            symbol.gameObject.SetActive(false); 
        }
    }
}
enum CardSwipeDirections
{
    right=0,
    left=1,
    up=2,
    down=3
}
