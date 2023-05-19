using FM;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace FN
{
    public class CharacterStatsManager : MonoBehaviour
    {
        public GameObject DamageText;
        CharacterManager character;

        [Header("NAME")]
        public string characterName = "Nameless";

        [Header("Team I.D")]
        public int teamIDNumber = 0;

        [Header("STATS")]
        public int maxHealth;
        public int currentHealth;
        public float maxStamina;
        public float currentStamina;

        [Header("CURRENCY")]
        public int currentSoulCount = 0;
        public int soulsAwardedOnDeath = 50;

        public int currentGoldCount = 0;
        public int goldAwardedOnDeath = 50;

        [Header("CHARACTER LEVEL")]
        public int playerLevel = 1;

        [Header("STAT LEVELS")]
        public int healthLevel = 10;
        public int staminaLevel = 10;
        public int poiseLevel = 10;

        [Header("Poise")]
        public float totalPoiseDefence; //The TOTAL poise during damage calculation
        public float offensivePoiseBonus; //The poise you GAIN during an attack with a weapon
        public float armorPoiseBonus; //The poise you GAIN from wearing what ever you have equipped
        public float totalPoiseResetTime = 15;
        public float poiseResetTimer = 0;

        [Header("Armor Absorptions")]
        public float physicalDamageAbsorptionHead;
        public float physicalDamageAbsorptionBody;
        public float physicalDamageAbsorptionLegs;
        public float physicalDamageAbsorptionHands;

        [Header("Blocking Absorptions")]
        public float blockingPhysicalDamageAbsorption;
        public float blockingFireDamageAbsorption;
        public float blockingStabilityRating;

        //Fire Absorption
        //Lightning Absorption
        //Magic Absorption
        //Dark Absorption

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {
            HandlePoiseResetTimer();
        }

        private void Start()
        {

        }

        public virtual void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation, CharacterManager enemyCharacterDamagingMe)
        {
            if (character.isDead)
                return;

            float finalDamage = physicalDamage + fireDamage; // + magicDamage + lightningDamage + darkDamage

            if (enemyCharacterDamagingMe.isPerformingFullyChargedAttack)
            {
                finalDamage = finalDamage + 30; // Increase the base damage by 30
            }

            bool isPlayer = (this is AICharacterStatsManager); // Check if the current state is PlayerState

            if (isPlayer)
            {
                Vector3 Offset = new Vector3(0, 2, 0);

                if (DamageText != null)
                {
                    var go = Instantiate(DamageText, transform.position + Offset, Quaternion.identity);
                    go.transform.LookAt(Camera.main.transform);
                    go.transform.Rotate(0f, 180f, 0f); // Rotate the text 180 degrees around the y-axis
                    go.transform.Translate(Vector3.right * 0.5f); // Adjust the position to the right by 0.5 units

                    var textMesh = go.GetComponent<TextMeshPro>();

                    float randomValue = Random.value;

                    // Set the color and modify damage based on the random value
                    if (randomValue < 0.5f)
                    {
                        textMesh.color = Color.white;
                        // No modification, damage remains the same
                        finalDamage = finalDamage;
                    }
                    else if (randomValue < 0.75f)
                    {
                        textMesh.color = Color.yellow;
                        // Multiply damage by 1.5
                        finalDamage = finalDamage * 1.5f;
                    }
                    else
                    {
                        textMesh.color = Color.red;
                        // Multiply damage by 2
                        finalDamage = finalDamage * 2f;
                    }

                    textMesh.text = finalDamage.ToString();
                }
            }

            currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                character.isDead = true;
            }

            character.characterSoundFXManager.PlayRandomDamageSoundFX();
        }


        public virtual void TakeDamageAfterBlock(int physicalDamage, int fireDamage, CharacterManager enemyCharacterDamagingMe)
        {
            if (character.isDead)
                return;

            float finalDamage = physicalDamage + fireDamage; // + magicDamage + lightningDamage + darkDamage

            if (enemyCharacterDamagingMe.isPerformingFullyChargedAttack)
            {
                finalDamage = finalDamage * 2; //WHAT EVER DAMAGE MULTIPLIER YOU WANT GOES HERE
            }

            currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                character.isDead = true;
            }

            //PLAY BLOCKING NOISE
        }

        public virtual void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
        {
            if (character.isDead)
                return;

            float finalDamage = physicalDamage + fireDamage;

            bool isPlayer = (this is AICharacterStatsManager); // Check if the current state is PlayerState

            if (isPlayer)
            {
                Debug.Log("isPlayer");
                float randomValue = Random.value;

                if (randomValue < 0.5f)
                {
                    // No modification, damage remains the same
                    finalDamage = finalDamage;
                }
                else if (randomValue < 0.75f)
                {
                    // Multiply damage by 1.5
                    finalDamage = finalDamage * 1.5f;
                }
                else
                {
                    // Multiply damage by 2
                    finalDamage = finalDamage * 2f;
                }
            }

            // multiply the final damage by 0.85 if the character has armor
            if (totalPoiseDefence > 0)
            {
                finalDamage *= 0.85f;
            }

            int damageTaken = Mathf.RoundToInt(finalDamage);
            currentHealth -= damageTaken;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                character.isDead = true;
            }
        }


        public virtual void TakePoisonDamage(int damage)
        {
            currentHealth = currentHealth - damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                character.isDead = true;
            }
        }

        public virtual void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else
            {
                totalPoiseDefence = armorPoiseBonus;
            }
        }

        public virtual void DeductStamina(float staminaToDeduct)
        {
            currentStamina = currentStamina - staminaToDeduct;
        }

        public virtual int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public virtual float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        public virtual float SetMaxPoisePointsFromPoiseLevel()
        {
            totalPoiseDefence = poiseLevel * 100;
            return totalPoiseDefence;
        }

    }
}