using HOG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.Anims
{
    public class HOGCharacterAnims: HOGMonoBehaviour
    {
        [SerializeField] Sprite idleAnim;
        [SerializeField] Sprite attackAnim;
        [SerializeField] Sprite hurtAnim;
        [SerializeField] Sprite dieAnim;
        [SerializeField] Sprite defenseAnim;
        [SerializeField] Sprite winAnim;

        public Dictionary<HOGCharacterState.CharacterStates, Sprite> StatesAnims = new Dictionary<HOGCharacterState.CharacterStates, Sprite>();

        public void FillDictionary()
        {
            StatesAnims[HOGCharacterState.CharacterStates.Idle] = idleAnim;
            StatesAnims[HOGCharacterState.CharacterStates.Attack] = attackAnim;
            StatesAnims[HOGCharacterState.CharacterStates.Hurt] = hurtAnim;
            StatesAnims[HOGCharacterState.CharacterStates.Die] = dieAnim;
            StatesAnims[HOGCharacterState.CharacterStates.Defense] = defenseAnim;
            StatesAnims[HOGCharacterState.CharacterStates.Win] = winAnim;
        }

    }
}

