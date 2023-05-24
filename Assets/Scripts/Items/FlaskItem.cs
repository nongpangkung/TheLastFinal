using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace FN
{
    [CreateAssetMenu(menuName = "Items/Consumables/Flask")]
    public class FlaskItem : ConsumableItem
    {
        [Header("Flask Type")]
        public bool estusFlask;
        public bool ashenFlask;

        [Header("Recovery Amount")]
        public int healthRecoverAmount;
        public int focusPointsRecoverAmount;

        [Header("Recovery FX")]
        public GameObject recoveryFX;

        public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager, PlayerManager player)
        {
            if (currentItemAmount > 0)
            {
                base.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager, player);
                ConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
            }
            else if (currentItemAmount <= 0)
            {
                playerAnimatorManager.PlayTargetAnimation("Shrug", true);
                AudioManager.instance.Play("Shrug");
            }
        }

        private async void ConsumeItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
        {
            GameObject flask = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
            playerEffectsManager.currentParticleFX = recoveryFX;
            playerEffectsManager.amountToBeHealed = healthRecoverAmount;
            playerEffectsManager.instantiatedFXModel = flask;
            playerAnimatorManager.PlayTargetAnimation(consumeAnimation, isInteracting, true);
            weaponSlotManager.rightHandSlot.UnloadWeapon();
            currentItemAmount -= 1;
            await Task.Delay(500);
            AudioManager.instance.Play("Shrug");
            await Task.Delay(1000);
            AudioManager.instance.Play("FogWall");
        }
    }
}
