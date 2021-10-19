using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MageiaGame
{
    public class Projectile : MonoBehaviour
    {
        public GameObject muzzlePrefab;
        public GameObject impactPrefab;


        ////////////////////////////////////


        private Ability ability;
        private Vector3 target;
        private float speed;

        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                          Initalization                           //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////

        void Start()
        {
            if (muzzlePrefab != null)
            {
                GameObject effect = Instantiate(muzzlePrefab, transform.position, Quaternion.identity);
                effect.transform.forward = gameObject.transform.forward;

                RemoveEffect(effect);
            }
        }

        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                       GETTERS & SETTERS                          //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////

        public void SetAbility(Ability ability)
        {
            this.ability = ability;
        }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }

        public void SetTarget(Vector3 target)
        {
            this.target = target;
        }

        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                              Core                                //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////

        void Update()
        {
            if (speed != 0)
            {
                transform.position += transform.forward * (speed * Time.deltaTime);
            }
            if (target != null)
            {
 //               transform.rotation = Quaternion.LookRotation(target - transform.position);
            }
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("Wall") || collision.transform.CompareTag("Enemy"))
            {
                speed = 0f;

                ContactPoint contact = collision.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = contact.point;

                if (impactPrefab != null)
                {
                    GameObject effect = Instantiate(impactPrefab, pos, rot);
                    RemoveEffect(effect);
                }

                if (collision.transform.CompareTag("Enemy"))
                {
                    Enemy enemy = collision.gameObject.GetComponent<Enemy>();

                    if (enemy != null && ability != null)
                    {
                        enemy.TakeDamage(Random.Range(ability.damage.x, ability.damage.y), ability.GetCaster());
                    }
                }

                Destroy(gameObject);
            }
        }

        private void RemoveEffect(GameObject effect)
        {
            var ps = effect.GetComponent<ParticleSystem>();
            if (ps == null)
                ps = effect.transform.GetChild(0).GetComponent<ParticleSystem>();
            if (ps != null)
                Destroy(effect, ps.main.duration);
        }

    }
}
