using HOG.Anims;
using HOG.Core;
using HOG.Screens;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.Character
{
    public class HOGCharacter : HOGMonoBehaviour
    {
        [SerializeField] int characterNumber = 1;
        [SerializeField] float attackStrength = 10f;
        [SerializeField] float attackRate = 10f;
        [SerializeField] HOGCharacterHealth health;

        HOGCharacterActions Actions;
        
        SpriteRenderer spriteRenderer;
        HOGCharacterAnims characterAnims;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            characterAnims = GetComponent<HOGCharacterAnims>();
            characterAnims.FillDictionary();
            Actions = new HOGCharacterActions();
            CreateActionSequence();
        }
        private void Start()
        {
            health = new HOGCharacterHealth();
            //PlayActionSequence();
        }
        public void TakeDamage(int amount)
        {
            health.TakeDamage(amount);
        }
        public void PlayAction(HOGCharacterActionBase action)
        {
            if(action == null)
            {
                return;
            }
            spriteRenderer.sprite = characterAnims.StatesAnims[action.ActionId];
        }
        public IEnumerator PlayActionSequence()
        {
            while(Actions.GetAttacksCount() > 0)
            {
                PlayAction(Actions.GetAction());
                yield return new WaitForSeconds(1f);
            }
            FinishAttackSequence();
            yield break;
        }

        public void CreateActionSequence()
        {
            Actions.AddAction(new HOGCharacterActionBase(HOGCharacterState.CharacterStates.Idle));
            Actions.AddAction(new HOGCharacterActionBase(HOGCharacterState.CharacterStates.Attack));
            Actions.AddAction(new HOGCharacterActionBase(HOGCharacterState.CharacterStates.Move));
            Actions.AddAction(new HOGCharacterActionBase(HOGCharacterState.CharacterStates.Defense));
            

        }

        private void FinishAttackSequence()
        {
            CreateActionSequence();
            InvokeEvent(HOGEventNames.OnAttacksFinish, characterNumber);
        }

        
    }
}
