using HOG.Anims;
using HOG.Core;
using HOG.Screens;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace HOG.Character
{
    public class HOGCharacter : HOGMonoBehaviour
    {
        [SerializeField] float attackStrength = 10f;
        [SerializeField] float attackRate = 10f;

        public int characterNumber = 1;
        public int CharacterType = 1;

        HOGCharacterActions Actions;
        
        SpriteRenderer spriteRenderer;
        HOGCharacterAnims characterAnims;
        public bool IsDead { get; private set; } = false;

        public void Init()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            characterAnims = GetComponent<HOGCharacterAnims>();
            characterAnims.FillDictionary(CharacterType);
            HOGAttacksUI component;
            var attacksUI = TryGetComponent<HOGAttacksUI>(out component);
            Actions = new HOGCharacterActions(component);
            //CreateActionSequence();
        }
        private void OnEnable()
        {
            AddListener(HOGEventNames.OnAbilityChange, ChangeFirstAction);
            AddListener(HOGEventNames.OnCharacterChange, ChangeCharacter);
        }


        private void OnDisable()
        {
            RemoveListener(HOGEventNames.OnAbilityChange, ChangeFirstAction);
            RemoveListener(HOGEventNames.OnCharacterChange, ChangeCharacter);
        }

        private void ChangeCharacter(object obj)
        {
            if((int)obj == 0 || (int)obj == 1)
            {
                CharacterType = (int)obj;
                characterAnims.FillDictionary(CharacterType);
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
        }
        public IEnumerator PlayActionSequence()
        {
            while(Actions.CanContinue())
            {
                PlayAction(Actions.GetAction());
                yield return new WaitForSeconds(1f);
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
            if(characterNumber == 1)
            {
                Tuple<HOGCharacterState.CharacterStates, int> tupleData = (Tuple<HOGCharacterState.CharacterStates, int>) obj;
                Actions.AddAction((HOGCharacterState.CharacterStates)tupleData.Item1, tupleData.Item2);
                
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
