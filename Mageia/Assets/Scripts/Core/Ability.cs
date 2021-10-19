using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MageiaGame
{
    [CreateAssetMenu(fileName = "New Ability", menuName = "MageiaGame/Ability")]
    public class Ability : ScriptableObject
    {
        [Header("Stats")]
        public string abilityName = "New Ability";
        public Vector2 damage = new Vector2(9f, 14f);
        public float cooldown = 1f;
        public float range = 20f;
        public float castSpeed = 1f;

        [Header("Projectile")]
        public float projectileSpeed = 20f;
        public float radius = 1f;
        public GameObject projectile;

        [Header("Sounds")]
        public AudioClip fireSound;

        //////////////////////////////

        
        private float cooldownLeft = 0f;
        private Character caster = null;


        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                       GETTERS & SETTERS                          //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////

        public void SetCaster(Character caster)
        {
            this.caster = caster;
        }

        public Character GetCaster()
        {
            return caster;
        }

        public bool CanUseAbility()
        {
            return Time.time > cooldownLeft;
        }


        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                              Core                                //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////v

        public bool Use(GameObject target)
        {
            if (!CanUseAbility() || target == null) return false;

            if (SpawnProjectile(target.transform.position))
            {
                cooldownLeft = Time.time + cooldown;

                return true;
            }

            return false;
        }

        public bool SpawnProjectile(Vector3 target)
        {
            GameObject proj;
            Projectile pr;

            if (target != null && caster != null && caster.projectileOrigin != null)
            {
                target.y = caster.projectileOrigin.position.y;
                proj = Instantiate(projectile, caster.projectileOrigin.position, Quaternion.LookRotation(target - caster.projectileOrigin.position));

                if (proj != null)
                {
                    proj.transform.name = abilityName + " Projectile";
                    pr = proj.GetComponent<Projectile>();
                    if (pr != null)
                    {
                        pr.SetSpeed(projectileSpeed);
                        pr.SetTarget(target);
                        pr.SetAbility(this);
                    }

                    Destroy(proj, 8f);
                    Debug.DrawLine(caster.projectileOrigin.position, target, Color.blue, 0.25f);

                    return true;
                }

                return false;
            }

            return false;
        }
    }
}