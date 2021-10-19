using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MageiaGame
{
    public class EnemyBoximonFiery : Enemy
    {

        protected override void Start()
        {
            base.Start();

            attackCoolTimeCalc = attackCoolTime;

            StartCoroutine(ResetAtkArea());
        }

        IEnumerator ResetAtkArea()
        {
            while (true)
            {
                yield return null;
                if (!meleeAttackRange.activeInHierarchy && currentState == State.Attack)
                {
                    yield return new WaitForSeconds(attackCoolTime);
                    meleeAttackRange.SetActive(true);
                }
            }
        }

        protected override void AttackEffect() {
            GameObject attackEffect = Instantiate(GameplayController.Instance.enemyAttackEffect, transform.position, Quaternion.Euler(90, 0, 0));
            Destroy(attackEffect, 1.5f);

            Camera.main.GetComponent<CameraFollow>().ScreenShake(0.03f, 0.15f);
        }
    }
}

