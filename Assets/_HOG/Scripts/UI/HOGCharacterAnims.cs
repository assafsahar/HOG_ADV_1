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
        [SerializeField] Sprite moveAnim;
        [SerializeField] List<GameObject> hitEffects;

        public Dictionary<HOGCharacterState.CharacterStates, Sprite> StatesAnims = new Dictionary<HOGCharacterState.CharacterStates, Sprite>();

        public void FillDictionary()
        {
            StatesAnims[HOGCharacterState.CharacterStates.Idle] = idleAnim;
            StatesAnims[HOGCharacterState.CharacterStates.Attack] = attackAnim;
            StatesAnims[HOGCharacterState.CharacterStates.Hurt] = hurtAnim;
            StatesAnims[HOGCharacterState.CharacterStates.Die] = dieAnim;
            StatesAnims[HOGCharacterState.CharacterStates.Defense] = defenseAnim;
            StatesAnims[HOGCharacterState.CharacterStates.Win] = winAnim;
            StatesAnims[HOGCharacterState.CharacterStates.Move] = moveAnim;
        }

        public void PlayRandomEffect(Transform parent)
        {
            var rand = Random.Range(0, hitEffects.Count-1);
            var effect = hitEffects[rand];
            if(effect != null)
            {
                Instantiate(effect, parent);
            }
        }

    }
}

