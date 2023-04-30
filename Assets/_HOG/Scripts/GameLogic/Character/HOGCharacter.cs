using HOG.Anims;
using HOG.Core;
using HOG.GameLogic;
using System;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace HOG.Character
{
    public class HOGCharacter : HOGMonoBehaviour
    {
        [SerializeField] int attackStrength = 10;
        [SerializeField] int scoreMultiplier = 10;
        //[SerializeField] HOGCharacterAttacksScriptable characterAttacksData;
        [SerializeField] Transform scoreTransform;
        [SerializeField] private int characterType = 1;
        [SerializeField] private float waitTimeBetweenAttacks = 1f;

        public int characterNumber = 1;
        public bool IsDead { get; private set; } = false;

        
        private HOGCharacterActions Actions;
        private SpriteRenderer spriteRenderer;
        private HOGCharacterAnims characterAnims;
        private int turn = 0;
        private HOGScoreUI scoreComponent;
        private HOGDeckManager deckManager;


        public void Init()
        {
            SpriteRenderer srComponent;
            var isSpriteRenderer = TryGetComponent<SpriteRenderer>(out srComponent);
            spriteRenderer = srComponent;
            HOGCharacterAnims caComponent;
            var isHOGCharacterAnims = TryGetComponent<HOGCharacterAnims>(out caComponent);
            characterAnims = caComponent;
            characterAnims.FillDictionary(characterType);
                       
            
            
            var scoreUI = TryGetComponent<HOGScoreUI>(out scoreComponent);
        }
        private void OnEnable()
        {
            AddListener(HOGEventNames.OnAbilityChange, ChangeFirstAction);
            AddListener(HOGEventNames.OnCharacterChange, ChangeCharacter);
            AddListener(HOGEventNames.OnTurnChange, ChangeTurn);
            AddListener(HOGEventNames.OnGameReset, ResetScore);
            //AddListener(HOGEventNames.OnScoreSet, UpdateScore);
        }

        
        private void OnDisable()
        {
            RemoveListener(HOGEventNames.OnAbilityChange, ChangeFirstAction);
            RemoveListener(HOGEventNames.OnCharacterChange, ChangeCharacter);
            RemoveListener(HOGEventNames.OnTurnChange, ChangeTurn);
            RemoveListener(HOGEventNames.OnGameReset, ResetScore);
            //RemoveListener(HOGEventNames.OnScoreSet, UpdateScore);
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
                Actions.UpdateCharacterType(characterType);
            }
        }

        public void PreFight()
        {
            IsDead = false;
            CreateActionSequence();
        }
        public void PlayAction(HOGCharacterActionBase action)
        {
            if(action == null)
            {
                return;
            }
            IsDead = (action.ActionId == HOGCharacterState.CharacterStates.Die);
            spriteRenderer.sprite = characterAnims.StatesAnims[action.ActionId];
            var actionData = Tuple.Create(characterNumber, action.ActionStrength);
            InvokeEvent(HOGEventNames.OnAttackFinish, actionData);
            if (action.ActionId == HOGCharacterState.CharacterStates.Attack || action.ActionId == HOGCharacterState.CharacterStates.Defense || action.ActionId == HOGCharacterState.CharacterStates.Move)
            {
                SetScore(action.ActionStrength);
                ShowScore(action.ActionStrength);
            }
        }

        private void SetScore(int actionStrength)
        {
            ScoreTags scoreTag = GetScoreTagByCharacterNumber();

            HOGGameLogicManager.Instance.ScoreManager.ChangeScoreByTagByAmount(scoreTag, actionStrength * scoreMultiplier);
        }

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
            var scoreText = (HOGTweenScoreComponent)Manager.PoolManager.GetPoolable(PoolNames.ScoreToast);
            scoreText.transform.position = scoreTransform.position;
            scoreText.Init(actionStrength * scoreMultiplier);
            UpdateScoreText(scoreTag);

        }

        /*private void UpdateScore(object obj)
        {
            (ScoreTags tag, int amount) = ((ScoreTags, int))obj;
            UpdateScoreText(tag, amount);
        }*/

        private void UpdateScoreText(ScoreTags scoreTag, int amount = 0)
        {
            if(amount != 0)
            {
                UpdateScoreComponentText(amount);
            }
            var score = 0;
            HOGGameLogicManager.Instance.ScoreManager.TryGetScoreByTag(scoreTag, ref score);
            UpdateScoreComponentText(score);
        }

        private void UpdateScoreComponentText(int score)
        {
            if (scoreComponent != null)
            {
                scoreComponent.UpdateText(score.ToString());
            }
        }

        public IEnumerator PlayActionSequence()
        {
            while(Actions.CanContinue())
            {
                PlayAction(Actions.GetAction());
                yield return new WaitForSeconds(waitTimeBetweenAttacks);
                Actions.RemoveTempAction();
            }
            FinishAttackSequence();
            yield break;
        }

        public void CreateActionSequence()
        {
            StartIdle();
            var characterAttacksData = HOGGameLogicManager.Instance.UpgradeManager.GetHogAttackConfig();
            HOGAttacksUI component;
            var attacksUI = TryGetComponent<HOGAttacksUI>(out component);
            Actions = new HOGCharacterActions(component, characterType, characterAttacksData);
            Actions.ResetList();
            
        }

        public void ChangeFirstAction(object obj)
        {
            if (turn == characterNumber)
            {
                return;
            }
            if (characterNumber == 1)
            {
                Tuple<HOGCharacterState.CharacterStates, int> tupleData = (Tuple<HOGCharacterState.CharacterStates, int>) obj;
                Actions.ReplaceAction((HOGCharacterState.CharacterStates)tupleData.Item1, tupleData.Item2, true);
                
            }
            
        }

        private void FinishAttackSequence()
        {
            PlayAction(new HOGCharacterActionBase(HOGCharacterState.CharacterStates.Idle, 0));
            InvokeEvent(HOGEventNames.OnAttacksFinish, characterNumber);
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
    }
}
