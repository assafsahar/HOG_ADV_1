using HOG.Anims;
using HOG.Core;
using System;
using System.Collections;
using UI;
using UnityEngine;

namespace HOG.Character
{
    public class HOGCharacter : HOGMonoBehaviour
    {
        [SerializeField] float attackStrength = 10f;
        [SerializeField] float attackRate = 10f;
        [SerializeField] HOGCharacterAttacksScriptable characterAttacksData;

        public int characterNumber = 1;
        public bool IsDead { get; private set; } = false;

        private int characterType = 1;
        private HOGCharacterActions Actions;
        private SpriteRenderer spriteRenderer;
        private HOGCharacterAnims characterAnims;
        private int turn = 0;
        

        public void Init()
        {
            SpriteRenderer srComponent;
            var isSpriteRenderer = TryGetComponent<SpriteRenderer>(out srComponent);
            spriteRenderer = srComponent;
            HOGCharacterAnims caComponent;
            var isHOGCharacterAnims = TryGetComponent<HOGCharacterAnims>(out caComponent);
            characterAnims = caComponent;
            characterAnims.FillDictionary(characterType);
            HOGAttacksUI component;
            var attacksUI = TryGetComponent<HOGAttacksUI>(out component);           
            Actions = new HOGCharacterActions(component, characterType, characterAttacksData);
        }
        private void OnEnable()
        {
            AddListener(HOGEventNames.OnAbilityChange, ChangeFirstAction);
            AddListener(HOGEventNames.OnCharacterChange, ChangeCharacter);
            AddListener(HOGEventNames.OnTurnChange, ChangeTurn);
        }


        private void OnDisable()
        {
            RemoveListener(HOGEventNames.OnAbilityChange, ChangeFirstAction);
            RemoveListener(HOGEventNames.OnCharacterChange, ChangeCharacter);
            RemoveListener(HOGEventNames.OnTurnChange, ChangeTurn);
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
            if((int)obj == 0 || (int)obj == 1)
            {
                characterType = (int)obj;
                characterAnims.FillDictionary(characterType);
            }
            Actions.UpdateCharacterType(characterType);
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
        }
        public IEnumerator PlayActionSequence()
        {
            while(Actions.CanContinue())
            {
                PlayAction(Actions.GetAction());
                yield return new WaitForSeconds(1f);
                Actions.RemoveTempAction();
            }
            FinishAttackSequence();
            yield break;
        }

        public void CreateActionSequence()
        {
            StartIdle();
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
