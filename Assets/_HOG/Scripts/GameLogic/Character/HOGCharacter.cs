using HOG.Anims;
using HOG.Core;
using HOG.GameLogic;
using System;
using System.Collections;
using UnityEngine;

namespace HOG.Character
{
    public class HOGCharacter : HOGMonoBehaviour
    {
        public int characterNumber = 1;
        private bool isDead = false;
        private bool isWin = false;

        [SerializeField] int scoreMultiplier = 10;
        [SerializeField] Transform scoreTransform;
        [SerializeField] private int characterType = 1;
        [SerializeField] private float waitTimeBetweenAttacks = 1f;
        [SerializeField] HOGDeckManager deckManager;
        private HOGCharacterActions actions;
        //private SpriteRenderer spriteRenderer;
        private Animator animator;
        private HOGCharacterAnims characterAnims;
        private int turn = 0;
        //private HOGScoreUI scoreComponent;
        public bool IsDead
        {
            get { return isDead; }
            set { isDead = value; }
        }
        public bool IsWin
        {
            get { return isWin; }
            set { isWin = value; }
        }
        private void OnEnable()
        {
            AddListener(HOGEventNames.OnAbilityChange, ChangeAction);
            AddListener(HOGEventNames.OnCharacterChange, ChangeCharacter);
            AddListener(HOGEventNames.OnTurnChange, ChangeTurn);
            AddListener(HOGEventNames.OnGameReset, ResetScore);
            AddListener(HOGEventNames.OnUpgraded, UpdateUpgradeData);
        }

        private void OnDisable()
        {
            RemoveListener(HOGEventNames.OnAbilityChange, ChangeAction);
            RemoveListener(HOGEventNames.OnCharacterChange, ChangeCharacter);
            RemoveListener(HOGEventNames.OnTurnChange, ChangeTurn);
            RemoveListener(HOGEventNames.OnGameReset, ResetScore);
            RemoveListener(HOGEventNames.OnUpgraded, UpdateUpgradeData);
        }

        public void Init()
        {
            /*priteRenderer srComponent;
            var isSpriteRenderer = TryGetComponent<SpriteRenderer>(out srComponent);
            spriteRenderer = srComponent;*/
            HOGCharacterAnims caComponent;
            var isHOGCharacterAnims = TryGetComponent<HOGCharacterAnims>(out caComponent);
            characterAnims = caComponent;
            characterAnims.FillDictionary(characterType);
            //var scoreUI = TryGetComponent<HOGScoreUI>(out scoreComponent);
            Animator aComponent;
            if(TryGetComponent<Animator>(out aComponent))
            {
                animator = aComponent;
            }
        }

        public void PreFight()
        {
            IsDead = false;
            IsWin = false;
            CreateActionSequence();
        }
        public void PlayAction(HOGCharacterActionBase action)
        {
            if (action == null)
            {
                return;
            }
            IsDead = (action.ActionId == HOGCharacterState.CharacterStates.Die);
            IsWin = (action.ActionId == HOGCharacterState.CharacterStates.Win);
            //spriteRenderer.sprite = characterAnims.StatesAnims[action.ActionId];
            animator.SetTrigger(characterAnims.StatesAnims[action.ActionId]);
            if (IsDead || IsWin)
            {
                return;
            }

            var actionData = Tuple.Create(characterNumber, action);
            InvokeEvent(HOGEventNames.OnAttack, actionData);
            if (action.ActionId == HOGCharacterState.CharacterStates.Attack || action.ActionId == HOGCharacterState.CharacterStates.Defense || action.ActionId == HOGCharacterState.CharacterStates.AttackBack)
            {
                //SetScore(action.ActionStrength);
                //ShowScore(action.ActionStrength);
                //NotifyDeckManagerOnScore();
            }
        }

        public IEnumerator PlayActionSequence()
        {
            while (actions.CanContinue())
            {
                PlayAction(actions.GetAction());
                yield return new WaitForSeconds(waitTimeBetweenAttacks);
                actions.RemoveTempAction();
            }
            HOGDebug.Log($"{characterNumber} play action sequence");
            FinishAttackSequence();
            yield break;
        }

        public void CreateActionSequence()
        {
            StartIdle();
            //HOGDebug.Log(characterNumber);
            var characterAttacksData = HOGGameLogicManager.Instance.UpgradeManager.GetHogAttackConfig();
            actions = new HOGCharacterActions(characterType, characterAttacksData);
            actions.ResetList();
        }


        public void ChangeAction(object obj)
        {
            if (turn == characterNumber)
            {
                return;
            }
            if (characterNumber == 1)
            {
                Tuple<HOGCharacterState.CharacterStates, int> tupleData = (Tuple<HOGCharacterState.CharacterStates, int>)obj;
                actions.ReplaceAction(tupleData.Item1, tupleData.Item2, true);
            }
        }

        public void Die()
        {
            PlayAction(new HOGCharacterActionBase(HOGCharacterState.CharacterStates.Die, 0));
        }

        public void StartIdle()
        {
            if (IsDead)
            {
                return;
            }
            PlayAction(new HOGCharacterActionBase(HOGCharacterState.CharacterStates.Idle, 0));
        }

        public void PlayHit()
        {
            if (IsDead)
            {
                return;
            }
            PlayAction(new HOGCharacterActionBase(HOGCharacterState.CharacterStates.Hurt, 0));
        }


        private void ResetScore(object obj)
        {
            ShowScore(1);
        }

        private void ChangeTurn(object obj)
        {
            turn = (int)obj;
        }

        private void ChangeCharacter(object obj)
        {
            if(turn == characterNumber)
            {
                return;
            }
            if (characterNumber == 1)
            {
                if ((int)obj == 0 || (int)obj == 1)
                {
                    characterType = (int)obj;
                    characterAnims.FillDictionary(characterType);
                }
                actions.UpdateCharacterType(characterType);
            }
        }

        /*private void SetScore(int actionStrength)
        {
            ScoreTags scoreTag = GetScoreTagByCharacterNumber();
            HOGGameLogicManager.Instance.ScoreManager.ChangeScoreByTagByAmount(scoreTag, actionStrength * scoreMultiplier);
        }*/

        private ScoreTags GetScoreTagByCharacterNumber()
        {
            ScoreTags scoreTag = 0;
            if (characterNumber == 1)
            {
                scoreTag = ScoreTags.Character1Score;
            }
            else if (characterNumber == 2)
            {
                scoreTag = ScoreTags.Character2Score;
            }
            return scoreTag;
        }

        private void ShowScore(int actionStrength)
        {
            ScoreTags scoreTag = GetScoreTagByCharacterNumber();
            // update score toast
            /*var scoreText = (HOGTweenScoreComponent)Manager.PoolManager.GetPoolable(PoolNames.ScoreToast);
            scoreText.transform.position = scoreTransform.position;
            scoreText.Init(actionStrength * scoreMultiplier);*/
            // update score label
            //UpdateScoreLabel(scoreTag);
        }

        private void NotifyDeckManagerOnScore()
        {
            if (deckManager != null)
            {
                deckManager.ScoreChanged();
            }
        }

        private void UpdateUpgradeData(object obj)
        {
            (ScoreTags tag, int amount, int level, int cardId) = ((ScoreTags, int, int, int))obj;
            //UpdateScoreLabel(tag, amount);
            UpdateUpgradeLevel(cardId, level);
        }

        private void UpdateUpgradeLevel(int cardId, int level)
        {
            if (deckManager != null)
            {
                deckManager.UpdateCardLevel(cardId, level);
            }
        }

        private void FinishAttackSequence()
        {
            PlayAction(new HOGCharacterActionBase(HOGCharacterState.CharacterStates.Idle, 0));
            InvokeEvent(HOGEventNames.OnAttacksFinish, characterNumber);
        }

        public void PlayWin()
        {
            PlayAction(new HOGCharacterActionBase(HOGCharacterState.CharacterStates.Win, 0));
        }
    }
}
