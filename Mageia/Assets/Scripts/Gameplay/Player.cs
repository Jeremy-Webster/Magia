using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

namespace MageiaGame
{
    public class Player : Character
    {

        [Header("Stats")]
        public int gold = 0;
        public TextMeshProUGUI goldText;

        [Header("Targeting")]
        private List<GameObject> targets = new List<GameObject>();

        public int currentAbility = 0;

        private MobileJoyStick joyStick;

        public GameObject obj;


        //////////////////////////////////////////


        private bool foundTarget = false;
        private float currentDistance;
        private float targetDistance;


        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                          Initalization                           //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////

        private static Player instance;
        public static Player Instance // Assign Singlton
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<Player>();
                    if (Instance == null)
                    {
                        var instanceContainer = new GameObject("Player");
                        instance = instanceContainer.AddComponent<Player>();
                    }
                }
                return instance;
            }
        }

        protected override void Start()
        {
            base.Start();
            joyStick = MobileJoyStick.Instance;

            obj = gameObject;
            goldText.SetText(gold.ToString());
        }


        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                       GETTERS & SETTERS                          //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////

        public void SetTargets(List<GameObject> targets)
        {
            this.targets = targets;
        }

        public void AddTarget(GameObject target)
        {
            targets.Add(target);
        }

        public void RemoveEnemyFromTargets(GameObject enemy)
        {
            if (targets.Contains(enemy))
            {
                targets.Remove(enemy);
            }
        }

        public void GiveGold(int amount)
        {
            gold += amount;
            goldText.SetText(gold.ToString());
        }


        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                              Core                                //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////


        void FixedUpdate()
        {
            Movement();

            if (!moving)
            {
                Targeting();
            }
        }

        private void Movement()
        {
            float moveHorizontal = 0f;
            float moveVertical = 0f;

            if (joyStick != null)
            {
                moveHorizontal += joyStick.direction.x;
                moveVertical += joyStick.direction.y;
            }
            else
            {
                joyStick = MobileJoyStick.Instance;
            }

            if (moveHorizontal != 0 || moveVertical != 0)
            {
                rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y - 1, moveVertical * moveSpeed);

                animator.SetBool("MOVE", true);
                animator.SetBool("IDLE", false);
                animator.SetBool("ATTACK", false);
                moving = true;

                rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(new Vector3(moveHorizontal, 0, moveVertical)), Time.time * rotationSpeed);
            }
            else
            {
                animator.SetBool("MOVE", false);
                animator.SetBool("IDLE", true);
                moving = false;

                if (foundTarget && target != null)
                {
                    rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(target.transform.position - transform.position), Time.time * (rotationSpeed / 2));
                }
            }
        }

        private void Targeting()
        {
            currentDistance = 100f;
            targetDistance = 100f;
            foundTarget = false;

            if (targets.Count > 0)
            {
                for (int i = 0;i < targets.Count;i++)
                {
                    if (targets[i] == null) return;
                    RaycastHit hit;
                    bool isHit = Physics.Raycast(transform.position, targets[i].transform.position - transform.position, out hit, 20f, layerMask);

                    if (isHit && hit.transform.CompareTag("Enemy"))
                    {
                        targetDistance = Vector3.Distance(transform.position, hit.point);
                        Debug.DrawRay(transform.position, targets[i].transform.position - transform.position, Color.red);

                        
                        if (targetDistance < currentDistance)
                        {
                            currentDistance = targetDistance;
                            target = targets[i];
                            foundTarget = true;
                        }
                    }
                }

                if (target != null && !moving)
                {
                    transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
                }

                if (foundTarget && target != null && !moving && CanUseAbility(currentAbility) && Vector3.Distance(transform.position, target.transform.position) <= GetAbility(0).range)
                {
//                    Quaternion lookAtEnemy = Quaternion.LookRotation(target.transform.position - transform.position);
//                    Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, lookAtEnemy, Time.deltaTime * 0.15f);

                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK"))
                    {
                        animator.SetBool("MOVE", false);
                        animator.SetBool("IDLE", false);
                        animator.SetBool("ATTACK", true);
                    }
                }
                else if (moving)
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("MOVE"))
                    {
                        animator.SetBool("MOVE", true);
                        animator.SetBool("IDLE", false);
                        animator.SetBool("ATTACK", false);
                    }
                }
                else
                {
                    animator.SetBool("MOVE", false);
                    animator.SetBool("IDLE", true);
                    animator.SetBool("ATTACK", false);
                }
            }
        }

        public void Spawn()
        {

        }

        protected override IEnumerator Attack()
        {
            StartCoroutine(base.Attack());
            yield return null;

            Ability ability = GetAbility(currentAbility);

            if (ability != null)
            {
                animator.SetFloat("AttackSpeed", ability.castSpeed);
            }
            
            UseAbility(currentAbility);
        }

        protected override void Death()
        {
            base.Death();

            Debug.Log("You have lost the game!");

            PauseGame pauseGame = PauseGame.Instance;

            if (pauseGame != null)
            {
                pauseGame.Pause();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            
            if (other.transform.CompareTag("StageTrigger"))
            {
                GameplayController ctrl = GameplayController.Instance;

                
                if (ctrl != null && ctrl.stage != null)
                {
                    GameplayController.EnteredRoom = true;
                }
            }
            else if (other.transform.CompareTag("Portal"))
            {
                GameplayController ctrl = GameplayController.Instance;

                if (ctrl != null)
                {
                    rb.velocity = new Vector3(0, 0, 0);

                    ctrl.FinishStage();
                }
            }
            else if (other.transform.CompareTag("TriggerRune"))
            {
                GameplayController ctrl = GameplayController.Instance;

                if (ctrl != null)
                {
                    ctrl.EnemiesDefeated();
                }
            }
            else if (other.transform.CompareTag("MeleeAttackRange"))
            {
                Enemy enemy = other.transform.parent.GetComponent<Enemy>();

                if (enemy != null)
                {
//                    enemy.meleeAttackRange.SetActive(false);
//                    TakeDamage(Random.Range(enemy.damage.x, enemy.damage.y), enemy);
                }
            }
        }
    }

}