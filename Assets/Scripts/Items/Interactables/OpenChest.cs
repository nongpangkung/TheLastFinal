using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace FN
{
    public class OpenChest : Interactable
    {
        Animator animator;
        OpenChest openChest;

        public Transform playerStandingPosition;
        public GameObject itemSpawner;
        public int minGold = 100;
        public int maxGold = 500;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            openChest = GetComponent<OpenChest>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
            GoldCountBar goldCountBar = FindObjectOfType<GoldCountBar>();

            Vector3 rotationDirection = transform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();

            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
            playerManager.transform.rotation = targetRotation;

            playerManager.OpenChestInteraction(playerStandingPosition);
            animator.Play("Chest Open");
            StartCoroutine(SpawnItemInChest());

            int randomGold = Random.Range(minGold, maxGold + 1);

            if (playerStats != null)
            {
                playerStats.AddGold(randomGold);

                if (goldCountBar != null)
                {
                    goldCountBar.SetGoldCountText(playerStats.currentGoldCount);
                }
            }
            AudioManager.instance.Play("OpenChest");
        }

        private IEnumerator SpawnItemInChest()
        {
            yield return new WaitForSeconds(1f);
            Instantiate(itemSpawner, transform);
            Destroy(openChest);
        }
    }
}
