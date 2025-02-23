using HOG.Core;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.Anims
{
    public class HOGCharacterAnims: HOGMonoBehaviour
    {
        public Dictionary<HOGCharacterState.CharacterStates, string> StatesAnims = new Dictionary<HOGCharacterState.CharacterStates, string>();

        [SerializeField] string[] idleAnim;
        [SerializeField] string[] attackAnim;
        [SerializeField] string[] hurtAnim;
        [SerializeField] string[] dieAnim;
        [SerializeField] string[] defenseAnim;
        [SerializeField] string[] winAnim;
        [SerializeField] string[] attackBackAnim;
        [SerializeField] string[] attackSpeedAnim;
        [SerializeField] string[] selfHealAnim;
        [SerializeField] List<GameObject> hitEffects;

        public void FillDictionary(int characterType)
        {
            StatesAnims[HOGCharacterState.CharacterStates.Idle] = idleAnim[characterType];
            StatesAnims[HOGCharacterState.CharacterStates.Attack] = attackAnim[characterType];
            StatesAnims[HOGCharacterState.CharacterStates.Hurt] = hurtAnim[characterType];
            StatesAnims[HOGCharacterState.CharacterStates.Die] = dieAnim[characterType];
            StatesAnims[HOGCharacterState.CharacterStates.Defense] = defenseAnim[characterType];
            StatesAnims[HOGCharacterState.CharacterStates.Win] = winAnim[characterType];
            StatesAnims[HOGCharacterState.CharacterStates.AttackBack] = attackBackAnim[characterType];
            StatesAnims[HOGCharacterState.CharacterStates.AttackSpeed] = attackSpeedAnim[characterType];
            StatesAnims[HOGCharacterState.CharacterStates.SelfHeal] = selfHealAnim[characterType];
        }

        public void PlayRandomEffect(Transform parent, float effectScale)
        {
            var rand = Random.Range(0, hitEffects.Count);
            var effect = hitEffects[rand];
            InstantiateEffect(effect, parent, effectScale / (parent.localScale.y - 1));
        }

        public void PlaySpecificEffect(int effectNumber, Transform parent, float effectScale)
        {
            if (hitEffects[effectNumber] == null)
            {
                return;
            }
            var effect = hitEffects[effectNumber];
            InstantiateEffect(effect, parent, effectScale / (parent.localScale.y - 1));
        }

        private void InstantiateEffect(GameObject effect, Transform parent, float effectScale)
        {
            if (effect != null)
            {
                var instance = Instantiate(effect, transform.position, Quaternion.identity, transform);
                instance.transform.localScale = new Vector3(effectScale, effectScale, effectScale);
            }
        }
    }
}