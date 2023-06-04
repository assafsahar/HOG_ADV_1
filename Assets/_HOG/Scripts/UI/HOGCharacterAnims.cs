using HOG.Core;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.Anims
{
    public class HOGCharacterAnims: HOGMonoBehaviour
    {
        public Dictionary<HOGCharacterState.CharacterStates, Sprite> StatesAnims = new Dictionary<HOGCharacterState.CharacterStates, Sprite>();

        [SerializeField] Sprite[] idleAnim;
        [SerializeField] Sprite[] attackAnim;
        [SerializeField] Sprite[] hurtAnim;
        [SerializeField] Sprite[] dieAnim;
        [SerializeField] Sprite[] defenseAnim;
        [SerializeField] Sprite[] winAnim;
        [SerializeField] Sprite[] moveAnim;
        [SerializeField] List<GameObject> hitEffects;

        public void FillDictionary(int characterType)
        {
            StatesAnims[HOGCharacterState.CharacterStates.Idle] = idleAnim[characterType];
            StatesAnims[HOGCharacterState.CharacterStates.Attack] = attackAnim[characterType];
            StatesAnims[HOGCharacterState.CharacterStates.Hurt] = hurtAnim[characterType];
            StatesAnims[HOGCharacterState.CharacterStates.Die] = dieAnim[characterType];
            StatesAnims[HOGCharacterState.CharacterStates.Defense] = defenseAnim[characterType];
            StatesAnims[HOGCharacterState.CharacterStates.Win] = winAnim[characterType];
            StatesAnims[HOGCharacterState.CharacterStates.Move] = moveAnim[characterType];
        }

        public void PlayRandomEffect(Transform parent, float effectScale)
        {
            var rand = Random.Range(1, hitEffects.Count);
            var effect = hitEffects[rand];
            InstantiateEffect(effect, parent, effectScale);
        }

        public void PlaySpecificEffect(int effectNumber, Transform parent, float effectScale)
        {
            if (hitEffects[effectNumber] == null)
            {
                return;
            }
            var effect = hitEffects[effectNumber];
            InstantiateEffect(effect, parent, effectScale);
        }

        private void InstantiateEffect(GameObject effect, Transform parent, float effectScale)
        {
            if (effect != null)
            {
                var instance = Instantiate(effect, parent);
                instance.transform.localScale = new Vector3(effectScale, effectScale, effectScale);
            }
        }
    }
}