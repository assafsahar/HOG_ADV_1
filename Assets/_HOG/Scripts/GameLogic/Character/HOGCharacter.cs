using HOG.Anims;
using HOG.Core;
using HOG.Screens;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.Character
{
    public class HOGCharacter : HOGMonoBehaviour
    {
        [SerializeField] float attackStrength = 10f;
        [SerializeField] float attackRate = 10f;

        public int characterNumber = 1;

        HOGCharacterActions Actions;
        
        SpriteRenderer spriteRenderer;
        HOGCharacterAnims characterAnims;

        public void Init()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            characterAnims = GetComponent<HOGCharacterAnims>();
            characterAnims.FillDictionary();
            Actions = new HOGCharacterActions();
            CreateActionSequence();
        }
        private void Awake()
        {
            
        }
        private void Start()
        {
            
            //PlayActionSequence();
        }
       
        public void PlayAction(HOGCharacterActionBase action)
        {
            if(action == null)
            {
                return;
            }
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
            Actions.ResetList();
            
            /*Actions.AddAction(new HOGCharacterActionBase(HOGCharacterState.CharacterStates.Attack, 3));
            Actions.AddAction(new HOGCharacterActionBase(HOGCharacterState.CharacterStates.Move, 1));
            Actions.AddAction(new HOGCharacterActionBase(HOGCharacterState.CharacterStates.Defense, 1));*/


        }

        private void FinishAttackSequence()
        {
            CreateActionSequence();
            PlayAction(new HOGCharacterActionBase(HOGCharacterState.CharacterStates.Idle, 0));
            InvokeEvent(HOGEventNames.OnAttacksFinish, characterNumber);
        }

        public void Die()
        {
            PlayAction(new HOGCharacterActionBase(HOGCharacterState.CharacterStates.Die, 0));
        }

        
    }
}
