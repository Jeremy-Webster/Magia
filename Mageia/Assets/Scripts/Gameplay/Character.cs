using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MageiaGame
{
    public class Character : MonoBehaviour
    {
        [Header("Stats")]
        public float maxHealth = 100f;
        public float moveSpeed = 5f;
        public float rotationSpeed = 0.01f;

        [Header("Abilities")]
        public List<Ability> abilitiesList = new List<Ability>();

        [Header("Weapon")]
        public GameObject weapon;
        public Transform projectileOrigin;
        public GameObject target = null;

        [Header("Sounds")]
        public AudioClip[] attackSound;
        public AudioClip[] walkSound;
        public AudioClip[] damagedSound;
        public AudioClip[] deathSound;

        public float walkVolume = 0.4f;
        public float damageVolume = 0.6f;

        [Header("Setup")]
        public HealthBar healthBar;
        public LayerMask layerMask;


        //////////////////////////////////////


        protected float health = 100;

        protected Rigidbody rb;
        protected Animator animator;

        protected bool moving = false;
        private AudioSource sound;

        protected bool dead = false;

        private Ability[] abilities;


        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                          Initalization                           //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////

        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();

            health = maxHealth;

            sound = gameObject.AddComponent<AudioSource>();
            InvokeRepeating("PlayWalkSound", 0.0f, 0.3f);

            abilities = new Ability[abilitiesList.Count];
            for (int i = 0;i < abilitiesList.Count;i++)
            {
                abilities[i] = Object.Instantiate(abilitiesList[i]);
                abilities[i].SetCaster(this);
            }
        }


        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                       GETTERS & SETTERS                          //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////

        public float GetHealth()
        {
            return health;
        }

        public void SetHealth(float health)
        {
            this.health = Mathf.Clamp(health, 0, maxHealth);
        }

        public float GetMaxHealth()
        {
            return maxHealth;
        }

        public GameObject GetTarget()
        {
            return target;
        }

        public Ability GetAbility(int num)
        {
            return this.abilities[num];
        }

        public Ability[] GetAbilities()
        {
            return abilities;
        }

        public bool IsDead()
        {
            return dead;
        }


        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                              Core                                //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////

        protected virtual IEnumerator Attack()
        {
            yield return null;

            PlayAttackSound();
        }

        public virtual void TakeDamage(float amount, Character attacker)
        {
            health = Mathf.Clamp(this.health - Mathf.Abs(amount), 0, maxHealth);

            PlayDamageSound();

            animator.SetTrigger("DMG");

            // Show damage indicator (Hit marks, and amount)

            if (health <= 0)
            {
                if (dead) return;

                if (attacker.CompareTag("Player"))
                {
                    attacker.SetHealth(attacker.GetHealth() + 10);
                }

                Death();
            }
        }

        public bool UseAbility(int num)
        {
            if (num < 0 || num >= abilities.Length) return false;

            return abilities[num].Use(target);
        }

        public bool CanUseAbility(int num)
        {
            if (num < 0 || num >= abilities.Length) return false;

            return abilities[num].CanUseAbility();
        }

        protected virtual void Death()
        {
            if (dead) return;

            animator.SetTrigger("DIE");

            // Play animations
            // Play sounds
            // Do partical effects
        }


        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                              Sound                               //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////

        private void PlayWalkSound()
        {
            if (moving && walkSound.Length > 0 && sound != null && !PauseGame.GameIsPaused && !PauseGame.GameIsMuted)
            {
                sound.volume = walkVolume;
                sound.PlayOneShot(walkSound[Random.Range(0, walkSound.Length)]);
            }
        }

        protected void PlayAttackSound()
        {
            if (attackSound.Length > 0 && sound != null && !PauseGame.GameIsPaused && !PauseGame.GameIsMuted)
            {
                sound.volume = walkVolume;
                sound.PlayOneShot(attackSound[Random.Range(0, attackSound.Length)]);
            }
        }

        protected void PlayDamageSound()
        {
            if (damagedSound.Length > 0 && sound != null && !PauseGame.GameIsPaused && !PauseGame.GameIsMuted)
            {
                sound.volume = damageVolume;
                sound.pitch = Random.Range(1.3f, 1.5f);
                sound.PlayOneShot(damagedSound[Random.Range(0, damagedSound.Length)]);
            }
        }

        protected void PlayDeathSound()
        {
            if (deathSound.Length > 0 && sound != null && !PauseGame.GameIsPaused && !PauseGame.GameIsMuted)
            {
                sound.volume = damageVolume;
                sound.pitch = Random.Range(1.3f, 1.5f);
                sound.PlayOneShot(deathSound[Random.Range(0, deathSound.Length)]);
            }
        }
    }
}