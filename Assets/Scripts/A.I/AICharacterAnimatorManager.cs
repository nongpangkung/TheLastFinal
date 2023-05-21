using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    public class AICharacterAnimatorManager : CharacterAnimatorManager
    {
        AICharacterManager aiCharacter;

        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>();
        }

        public void AwardSoulsOnDeath()
        {
            PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
            SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();

            if (playerStats != null)
            {
                int soulsAwarded = aiCharacter.aiCharacterStatsManager.soulsAwardedOnDeath;
                playerStats.AddSouls(soulsAwarded);

                if (soulCountBar != null)
                {
                    int currentSoulCount = playerStats.currentSoulCount;
                    int increasedSouls = soulsAwarded; // The increase is equal to the souls awarded
                    soulCountBar.SetSoulCountText(currentSoulCount, increasedSouls);
                }
            }
        }

        public void AwardGoldsOnDeath()
        {
            PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
            GoldCountBar goldCountBar = FindObjectOfType<GoldCountBar>();

            if (playerStats != null)
            {
                int goldAwarded = aiCharacter.aiCharacterStatsManager.goldAwardedOnDeath;
                playerStats.AddGold(goldAwarded);

                if (goldCountBar != null)
                {
                    int currentGoldCount = playerStats.currentGoldCount;
                    int increasedGold = goldAwarded; // The increase is equal to the gold awarded
                    goldCountBar.SetGoldCountText(currentGoldCount, increasedGold);
                }
            }
        }

        public void InstantiateBossParticleFX()
        {
            BossFXTransform bossFxTransform = GetComponentInChildren<BossFXTransform>();
            GameObject phaseFX = Instantiate(aiCharacter.aiCharacterBossManager.particleFX, bossFxTransform.transform);
        }

        public void PlayWeaponTrailFX()
        {
            aiCharacter.aiCharacterEffectsManager.PlayWeaponFX(false);
        }

        public override void OnAnimatorMove()
        {
            Vector3 velocity = character.animator.deltaPosition;
            character.characterController.Move(velocity);

            if (aiCharacter.isRotatingWithRootMotion)
            {
                character.transform.rotation *= character.animator.deltaRotation;
            }
        }
    }
}