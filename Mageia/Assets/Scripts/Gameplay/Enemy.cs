using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace MageiaGame
{
    public class Enemy : Character
    {
        [Header("Enemy")]
        public string enemyName = "Enemy";
        public string prefabPath = "Prefabs/Enemy/";
        public bool melee = true;
        public bool boss = false;
        public Vector2 worth = new Vector2(50, 100);

        [Header("Attack")]
        public Vector2 damage = new Vector2(8f, 12);
        public float range = 15f;
        public float firerate = 1f;
        public float playerRealizeRange = 10f;
        public float attackRange = 5f;
        public float attackCoolTime = 2f;
        public NavMeshAgent nvAgent;

        public GameObject meleeAttackRange;

        WaitForSeconds Delay500 = new WaitForSeconds(0.5f);
        WaitForSeconds Delay250 = new WaitForSeconds(0.25f);

        private float distance = 100f;
        private bool canAttack = false;

        public enum State
        {
            Idle,
            Move,
            Attack,
            Dead
        };
        [Header("Setup")]
        public State currentState = State.Idle;

        [Header("Spawnpoint Render")]
        public Mesh highlight;


        /////////////////////////////////////////////////


        protected float attackCoolTimeCalc = 0f;


        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                          Initalization                           //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////

        protected override void Start()
        {
            base.Start();

            target = GameObject.FindGameObjectWithTag("Player");

            StartCoroutine(FSM());
            StartCoroutine(CalcCoolTime());

            nvAgent = GetComponent<NavMeshAgent>();
        }


        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                       AI State Behaviour                         //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////
        
        protected virtual IEnumerator FSM()
        {
            yield return null;

            while (!GameplayController.EnteredRoom)
            {
                yield return Delay500;
            }

            while(currentState != State.Dead)
            {
                yield return StartCoroutine(currentState.ToString());
            }
        }

        protected virtual IEnumerator Idle()
        {
            yield return null;

            if (dead) yield break;

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("IDLE"))
            {
                animator.SetTrigger("IDLE");
            }

            currentState = State.Attack;
            currentState = State.Idle;
            currentState = State.Move;
        }

        protected virtual void AttackEffect() { }

        protected override IEnumerator Attack()
        {
            yield return null;

            if (dead) yield break;

            nvAgent.stoppingDistance = 0f;
            nvAgent.isStopped = true;
            nvAgent.SetDestination(target.transform.position);

            yield return Delay250;

            if (dead) yield break;

            nvAgent.isStopped = false;
            nvAgent.speed = 30f;
            canAttack = false;

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK"))
            {
                animator.SetFloat("AttackSpeed", 1f);
                animator.SetTrigger("ATTACK");
            }

            AttackEffect();

            yield return Delay250;

            if (dead) yield break;

            nvAgent.speed = moveSpeed;
            nvAgent.stoppingDistance = attackRange;
            currentState = State.Idle;
        }

        protected virtual IEnumerator Move()
        {
            yield return null;

            if (dead) yield break;

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("MOVE") && Vector3.Distance(transform.position, target.transform.position) > 5f)
            {
                animator.SetTrigger("MOVE");
            }

            if (AttackPattern() && canAttack)
            {
                currentState = State.Attack;
            }
            else if (distance > playerRealizeRange)
            {
                nvAgent.SetDestination(transform.position - Vector3.forward * 5f);
            }
            else
            {
                nvAgent.SetDestination(target.transform.position);
            }
        }

        protected bool AttackPattern()
        {
            if (target == null) return false;
            Vector3 targetDir = new Vector3(target.transform.position.x - transform.position.x, 0f, target.transform.position.z - transform.position.z);

            Physics.Raycast(new Vector3(transform.position.x, 0.5f, transform.position.z), targetDir, out RaycastHit hit, 30f, layerMask);
            distance = Vector3.Distance(target.transform.position, transform.position);

            if (hit.transform == null) return false;
            if (hit.transform.CompareTag("Player") && distance < attackRange)
            {
                return true;
            }

            return false;
        }

        protected virtual IEnumerator CalcCoolTime()
        {
            while (true)
            {
                yield return null;
                if (!canAttack)
                {
                    attackCoolTimeCalc -= Time.deltaTime;
                    if (attackCoolTimeCalc <= 0)
                    {
                        attackCoolTimeCalc = attackCoolTime;
                        canAttack = true;
                    }
                }
            }
        }

        public void DealDamage()
        {
            Player player = Player.Instance;

            if (player != null)
            {
                if (Vector3.Distance(player.transform.position, transform.position) < 5f)
                {
                    player.TakeDamage(Random.Range(damage.x, damage.y), this);
                    meleeAttackRange.SetActive(false);
                }
            }
        }

        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                              Core                                //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////

        protected override void Death()
        {
            if (dead) return;
            base.Death();

            currentState = State.Dead;
            nvAgent.isStopped = true;
            nvAgent.speed = 0f;

            Invoke("HideHealth", 0.3f);

            SpawnGold();

            GameplayController ctrl = GameplayController.Instance;

            if (ctrl != null && ctrl.stage != null)
            {
                ctrl.stage.EnemyKilled(gameObject);
            }

            Instantiate(GameplayController.Instance.enemyDeathEffect, transform.position, Quaternion.Euler(90, 0, 0));

            Destroy(this.gameObject, 1.3f);
        }

        private void HideHealth()
        {
            if (healthBar != null)
                healthBar.gameObject.SetActive(false);
        }

        private void SpawnGold()
        {
            int gold = (int)Random.Range(worth.x, worth.y);
        }
    }
}

