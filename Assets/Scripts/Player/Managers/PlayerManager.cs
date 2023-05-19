using FM;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FN
{
    public class PlayerManager : CharacterManager, IDataPersistence
    {
        [Header("Camera")]
        public CameraHandler cameraHandler;

        [Header("Input")]
        public InputHandler inputHandler;

        [Header("UI")]
        public UIManager uiManager;

        [Header("Player")]
        public PlayerStatsManager playerStatsManager;
        public PlayerWeaponSlotManager playerWeaponSlotManager;
        public PlayerEquipmentManager playerEquipmentManager;
        public PlayerCombatManager playerCombatManager;
        public PlayerInventoryManager playerInventoryManager;
        public PlayerEffectsManager playerEffectsManager;
        public PlayerAnimatorManager playerAnimatorManager;
        public PlayerLocomotionManager playerLocomotionManager;

        [Header("Interactables")]
        InteractableUI interactableUI;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;
        public FlaskItem flaskItem;

        public string currentSceneName;

        protected override void Awake()
        {
            base.Awake();
            cameraHandler = FindObjectOfType<CameraHandler>();
            uiManager = FindObjectOfType<UIManager>();
            interactableUI = FindObjectOfType<InteractableUI>();
            inputHandler = GetComponent<InputHandler>();
            animator = GetComponent<Animator>();

            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        }


        public void LoadData(GameData data)
        {
            this.currentSceneName = SceneManager.GetActiveScene().name; // Update the current scene name

            this.playerStatsManager.currentGoldCount = data.gold;
            this.playerStatsManager.currentSoulCount = data.soul;
            this.playerStatsManager.healthLevel = data.healthLevel;
            this.playerStatsManager.staminaLevel = data.staminaLevel;
            this.playerStatsManager.poiseLevel = data.poiseLevel;

            //Equipment
            playerInventoryManager.rightWeapon = ItemDataBase.Instance.GetWeaponItemByID(data.currentRightHandWeaponID);
            playerInventoryManager.leftWeapon = ItemDataBase.Instance.GetWeaponItemByID(data.currentLeftHandWeaponID);
            playerWeaponSlotManager.LoadBothWeaponsOnSlots();

            EquipmentItem headEquipment = ItemDataBase.Instance.GetEquipmentItemByID(data.currentHeadGearItemID);

            if (headEquipment != null)
            {
                playerInventoryManager.currentHelmetEquipment = headEquipment as HelmetEquipment;
            }

            EquipmentItem armorEquipment = ItemDataBase.Instance.GetEquipmentItemByID(data.currentArmorGearItemID);

            if (armorEquipment != null)
            {
                playerInventoryManager.currentBodyEquipment = armorEquipment as BodyEquipment;
            }

            EquipmentItem legEquipment = ItemDataBase.Instance.GetEquipmentItemByID(data.currentLegGearItemID);

            if (legEquipment != null)
            {
                playerInventoryManager.currentLegEquipment = legEquipment as LegEquipment;
            }

            EquipmentItem handEquipment = ItemDataBase.Instance.GetEquipmentItemByID(data.currentHandGearItemID);

            if (handEquipment != null)
            {
                playerInventoryManager.currentHandEquipment = handEquipment as HandEquipment;
            }

            // Inventory
            for (int i = 0; i < data.weaponsRSlots; i++)
            {
                if (data.weaponsRSlotItems.Length > i && data.weaponsRSlotItems[i] != 0)
                {
                    playerInventoryManager.weaponsInRightHandSlots[i] = ItemDataBase.Instance.GetWeaponItemByID(data.weaponsRSlotItems[i]);
                }
                else
                {
                    playerInventoryManager.weaponsInRightHandSlots[i] = null; // Slot is empty
                }
            }

            for (int i = 0; i < data.weaponsLSlots; i++)
            {
                if (data.weaponsLSlotItems.Length > i && data.weaponsLSlotItems[i] != 0)
                {
                    playerInventoryManager.weaponsInLeftHandSlots[i] = ItemDataBase.Instance.GetWeaponItemByID(data.weaponsLSlotItems[i]);
                }
                else
                {
                    playerInventoryManager.weaponsInLeftHandSlots[i] = null; // Slot is empty
                }
            }

            // Clear the existing weapon inventory
            playerInventoryManager.weaponsInventory.Clear();

            // Load weapon inventory data
            foreach (int weaponID in data.weaponsInventorySlotItems)
            {
                WeaponItem weapon = ItemDataBase.Instance.GetWeaponItemByID(weaponID); // Retrieve the weapon data
                playerInventoryManager.weaponsInventory.Add(weapon); // Add the weapon to the inventory
            }
        }

        public void SaveData(ref GameData data)
        {
            // Leave this method empty

            data.gold = this.playerStatsManager.currentGoldCount;
            data.soul = this.playerStatsManager.currentSoulCount;
            data.healthLevel = this.playerStatsManager.healthLevel;
            data.staminaLevel = this.playerStatsManager.staminaLevel;
            data.poiseLevel = this.playerStatsManager.poiseLevel;

            //Equipment
            data.currentRightHandWeaponID = playerInventoryManager.rightWeapon.itemID;
            data.currentLeftHandWeaponID = playerInventoryManager.leftWeapon.itemID;

            data.currentHeadGearItemID = playerInventoryManager.currentHelmetEquipment != null ? playerInventoryManager.currentHelmetEquipment.itemID : -1;
            data.currentArmorGearItemID = playerInventoryManager.currentBodyEquipment != null ? playerInventoryManager.currentBodyEquipment.itemID : -1;
            data.currentLegGearItemID = playerInventoryManager.currentLegEquipment != null ? playerInventoryManager.currentLegEquipment.itemID : -1;
            data.currentHandGearItemID = playerInventoryManager.currentHandEquipment != null ? playerInventoryManager.currentHandEquipment.itemID : -1;

            // Inventory
            data.weaponsRSlots = playerInventoryManager.weaponsInRightHandSlots.Length;
            data.weaponsLSlots = playerInventoryManager.weaponsInLeftHandSlots.Length;

            data.weaponsRSlotItems = new int[data.weaponsRSlots];
            for (int i = 0; i < data.weaponsRSlots; i++)
            {
                if (playerInventoryManager.weaponsInRightHandSlots[i] != null)
                {
                    data.weaponsRSlotItems[i] = playerInventoryManager.weaponsInRightHandSlots[i].itemID;
                }
                else
                {
                    data.weaponsRSlotItems[i] = 0; // Slot is empty
                }
            }

            data.weaponsLSlotItems = new int[data.weaponsLSlots];
            for (int i = 0; i < data.weaponsLSlots; i++)
            {
                if (playerInventoryManager.weaponsInLeftHandSlots[i] != null)
                {
                    data.weaponsLSlotItems[i] = playerInventoryManager.weaponsInLeftHandSlots[i].itemID;
                }
                else
                {
                    data.weaponsLSlotItems[i] = 0; // Slot is empty
                }
            }

            // Save weapon inventory data
            data.weaponsInventorySlotItems = new List<int>();

            foreach (WeaponItem weapon in playerInventoryManager.weaponsInventory)
            {
                data.weaponsInventorySlotItems.Add(weapon.itemID);
            }
        }

        protected override void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = animator.GetBool("isInteracting");
            canDoCombo = animator.GetBool("canDoCombo");
            canRotate = animator.GetBool("canRotate");
            isInvulnerable = animator.GetBool("isInvulnerable");
            isFiringSpell = animator.GetBool("isFiringSpell");
            isPerformingFullyChargedAttack = animator.GetBool("isPerformingFullyChargedAttack");
            animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);
            animator.SetBool("isDead", isDead);
            animator.SetBool("isBlocking", isBlocking);

            inputHandler.TickInput();
            playerLocomotionManager.HandleRollingAndSprinting();
            playerLocomotionManager.HandleJumping();
            playerStatsManager.RegenerateStamina();
            playerLocomotionManager.HandleGroundedMovement();
            playerLocomotionManager.HandleRotation();

            CheckForInteractableObject();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            playerEffectsManager.HandleAllBuildUpEffects();
        }

        private void LateUpdate()
        {
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.a_Input = false;
            inputHandler.inventory_Input = false;
            inputHandler.level_Input = false;

            float delta = Time.deltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation();
            }
        }

        #region Player Interactions

        public void CheckForInteractableObject()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f))
            {
                if (hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if (interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;
                        interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);

                        if (inputHandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
            else
            {
                if (interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if (itemInteractableGameObject != null && inputHandler.a_Input)
                {
                    itemInteractableGameObject.SetActive(false);
                }
            }
        }

        public void OpenChestInteraction(Transform playerStandsHereWhenOpeningChest)
        {
            playerLocomotionManager.rigidbody.velocity = Vector3.zero; //Stops the player from ice skating
            transform.position = playerStandsHereWhenOpeningChest.transform.position;
            playerAnimatorManager.PlayTargetAnimation("Open Chest", true);
        }

        public void PassThroughFogWallInteraction(Transform fogWallEntrance)
        {
            playerLocomotionManager.rigidbody.velocity = Vector3.zero; //Stops the player from ice skating

            Vector3 rotationDirection = fogWallEntrance.transform.forward;
            Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = turnRotation;
            //Rotate over time so it does not look as rigid

            playerAnimatorManager.PlayTargetAnimation("Pass Through Fog", true);
        }

        #endregion
    }
}