using HOG.Anims;
using HOG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.Character
{
    public class HOGCharacter : MonoBehaviour
    {
        HOGCharacterActions Actions;
        [SerializeField] float attackStrength = 10f;
        [SerializeField] float attackRate = 10f;
        HOGCharacterHealth health;
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
            PlayActionSequence();
        }
        public void PlayAction(HOGCharacterActionBase action)
        {
            if(action == null)
            {
                return;
            }
            spriteRenderer.sprite = characterAnims.StatesAnims[action.ActionId];
        }
        public void PlayActionSequence()
        {
            //PlayAction(Actions.GetAction());
        }

        public void CreateActionSequence()
        {
            HOGCharacterActionBase action = new HOGCharacterActionBase(HOGCharacterState.CharacterStates.Attack);
            Actions.AddAction(action);
        }
    }
}
