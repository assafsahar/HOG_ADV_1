using HOG.Character;
using HOG.Core;
using HOG.GameLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HOG.Test
{
    
    public class HOGTester : HOGMonoBehaviour
    {
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                InvokeEvent(HOGEventNames.OnAbilityChange, new Tuple<HOGCharacterState.CharacterStates, int>(HOGCharacterState.CharacterStates.Defense,4));
                /*var bullet = Manager.PoolManager.GetPoolable("BulletsPool");
                poolables.Enqueue(bullet);*/
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                InvokeEvent(HOGEventNames.OnAbilityChange, new Tuple<HOGCharacterState.CharacterStates, int>(HOGCharacterState.CharacterStates.Attack,3));

            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                InvokeEvent(HOGEventNames.OnAbilityChange, new Tuple<HOGCharacterState.CharacterStates, int>(HOGCharacterState.CharacterStates.AttackBack,5));

            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                InvokeEvent(HOGEventNames.OnCharacterChange, 0);

            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                InvokeEvent(HOGEventNames.OnCharacterChange, 1);

            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                /*var bullet = poolsables.Dequeue();
                Manager.PoolManager.ReturnPoolable(bullet);*/
                //var attackConfigTest = HOGGameLogicManager.Instance.UpgradeManager.GetHogAttackConfig();
            }

        }
        
        // the following methods are called from the UI buttons (editor)
        public void ChangeAttack(int amount)
        {
            InvokeEvent(HOGEventNames.OnAbilityChange, new Tuple<HOGCharacterState.CharacterStates, int>(HOGCharacterState.CharacterStates.Attack, amount));
        }

        public void changeCharacter(int characterNumber)
        {
            InvokeEvent(HOGEventNames.OnCharacterChange, characterNumber-1);
        }
    
    }
}

