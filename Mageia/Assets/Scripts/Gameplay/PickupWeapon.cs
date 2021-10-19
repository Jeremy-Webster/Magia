using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MageiaGame
{
    public class PickupWeapon : MonoBehaviour
    {
        public bool hasWeapon = true;
        public GameObject weapon;

        private AudioSource source;

        void Start()
        {
            source = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Player player = Player.Instance;

                if (player != null && hasWeapon)
                {
                    player.weapon.SetActive(true);
                    weapon.SetActive(false);

                    if (!PauseGame.GameIsMuted && source != null) source.Play();

                    hasWeapon = false;

                    GameplayController game = GameplayController.Instance;

                    if (game != null)
                    {
                        game.EnemiesDefeated();
                    }
                }
            }
        }
    }

}