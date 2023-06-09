using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    public class CharacterSoundFXManager : MonoBehaviour
    {
        CharacterManager character;
        PlayerManager player;
        AudioSource audioSource;

        //ATTACKING GRUNTS

        //TAKING DAMAGE GRUNTS

        [Header("Taking Damage Sounds")]
        public AudioClip[] takingDamageSounds;
        private List<AudioClip> potentialDamageSounds;
        private AudioClip lastDamageSoundPlayed;

        [Header("Weapon Whooshes")]
        private List<AudioClip> potentialWeaponWhooshes;
        private AudioClip lastWeaponWhoosh;

        ////FOOT STEP SOUNDS

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            character = GetComponent<CharacterManager>();
        }

        public virtual void PlayRandomDamageSoundFX()
        {
            potentialDamageSounds = new List<AudioClip>();

            foreach (var damageSound in takingDamageSounds)
            {
                //If the potential damage sound has not been played before, we add it as a potential (Stops repeated damage sounds)
                if (damageSound != lastDamageSoundPlayed)
                {
                    potentialDamageSounds.Add(damageSound);
                }
            }

            int randomValue = Random.Range(0, potentialDamageSounds.Count);
            lastDamageSoundPlayed = takingDamageSounds[randomValue];
            audioSource.PlayOneShot(takingDamageSounds[randomValue], 0.4f);
        }

        public virtual void PlayRandomWeaponWhoosh()
        {
            potentialWeaponWhooshes = new List<AudioClip>();

            if (character.isUsingRightHand)
            {
                foreach (var whooshSound in character.characterInventoryManager.rightWeapon.weaponWhooshes)
                {
                    if (whooshSound != lastWeaponWhoosh)
                    {
                        potentialWeaponWhooshes.Add(whooshSound);
                    }
                }

                int randomValue = Random.Range(0, potentialWeaponWhooshes.Count);
                lastWeaponWhoosh = character.characterInventoryManager.rightWeapon.weaponWhooshes[randomValue];
                audioSource.PlayOneShot(character.characterInventoryManager.rightWeapon.weaponWhooshes[randomValue]);
            }          
            else
            {
                foreach (var whooshSound in character.characterInventoryManager.leftWeapon.weaponWhooshes)
                {
                    if (whooshSound != lastWeaponWhoosh)
                    {
                        potentialWeaponWhooshes.Add(whooshSound);
                    }
                }

                int randomValue = Random.Range(0, potentialDamageSounds.Count);
                lastWeaponWhoosh = character.characterInventoryManager.leftWeapon.weaponWhooshes[randomValue];
                audioSource.PlayOneShot(character.characterInventoryManager.leftWeapon.weaponWhooshes[randomValue]);
            }
        }
    }
}
