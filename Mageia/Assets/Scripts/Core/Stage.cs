using UnityEngine;
using System.Collections.Generic;

namespace MageiaGame
{
    [CreateAssetMenu(fileName = "New Stage", menuName = "MageiaGame/Stage")]
    public class Stage : ScriptableObject
    {
        public string title = "New Stage";
        public int number = 1;
        public StageType type = StageType.Normal;
        public enum StageType {Normal, Boss, Shop}

        [Header("Spawning")]
        public Vector3 spawnPoint;
        public Vector3 spawnOffset = new Vector3(0f, 0.17458349f, 0f);

        [System.Serializable]
        public struct Enemies {
            public GameObject prefab;
            public Vector3 position;
            public Quaternion rotation;
        }
        public List<Enemies> enemiesToSpawn = new List<Enemies>();

        [Header("Camera")]
        public float cameraMinZ = -4.16f;
        public bool drawEditorGizmos = true;

        [Header("Setup")]
        public Chapter chapter;


        ////////////////////////////////////////////

        private bool loaded = false;
        private List<GameObject> enemies = new List<GameObject>();

        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                       GETTERS & SETTERS                          //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////

        public bool IsLoaded()
        {
            return loaded;
        }

        public void SetLoaded(bool loaded)
        {
            this.loaded = loaded;
        }

        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                              Core                                //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////

        public void BeginStage()
        {
            SpawnPlayer();
            SpawnEnemies();
        }

        public void SpawnPlayer()
        {
            Player player = Player.Instance;

            GameplayController ctrl = GameplayController.Instance;
            ctrl.MoveSpawnPoint(spawnPoint);

            if (player != null)
            {
                GameObject obj = player.obj;
                player.Spawn();

                if (obj != null)
                {
                    obj.transform.position = spawnPoint + spawnOffset;
                    obj.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                }

                CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();

                if (camFollow != null)
                {
                    camFollow.minZ = spawnPoint.z + cameraMinZ;
                    camFollow.ForceMove();
                }
            }
        }

        private void SpawnEnemies()
        {
            foreach (Enemies enemyToSpawn in enemiesToSpawn)
            {
                GameObject enemy = Instantiate(enemyToSpawn.prefab, enemyToSpawn.position, enemyToSpawn.rotation);

                enemies.Add(enemy);

                Player player = Player.Instance;

                if (player != null)
                    player.AddTarget(enemy);

            }
        }

        public void EnemyKilled(GameObject enemy)
        {
            if (enemies.Contains(enemy))
            {
                Rigidbody rb = enemy.GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;

                Collider collider = enemy.GetComponent<Collider>();
                if (collider != null) Destroy(collider);
                enemies.Remove(enemy);

                Player player = Player.Instance;
                if (player != null) player.RemoveEnemyFromTargets(enemy);

                if (enemies.Count <= 0)
                {
                    FinishStage();
                }
            }
        }

        public void FinishStage()
        {
            GameplayController ctrl = GameplayController.Instance;

            if (ctrl != null) ctrl.EnemiesDefeated();

        }

        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                             Editor                               //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////

        private void OnDrawGizmosSelected()
        {
            if (drawEditorGizmos)
            {
                foreach (Enemies spawnnedEnemy in enemiesToSpawn)
                {
                    if (spawnnedEnemy.prefab != null)
                    {
                        Enemy enemy = spawnnedEnemy.prefab.GetComponent<Enemy>();

                        if (enemy != null && enemy.highlight != null)
                        {
                            Gizmos.color = Color.black;
                            Gizmos.DrawWireMesh(enemy.highlight, spawnnedEnemy.position, spawnnedEnemy.rotation);
                            Gizmos.color = Color.yellow;
                            Gizmos.DrawWireSphere(spawnnedEnemy.position, enemy.playerRealizeRange);
                            Gizmos.color = Color.red;
                            Gizmos.DrawWireSphere(spawnnedEnemy.position, enemy.attackRange);
                        }
                    }
                }
            }
        }

        public void GenerateEnemySpawners()
        {
            enemiesToSpawn.Clear();

            foreach (GameObject spawner in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Enemy enemy = spawner.transform.GetComponent<Enemy>();

                GameObject marker = Instantiate(Resources.Load("Prefabs/Spawner") as GameObject, spawner.transform.position + new Vector3(0f,1f,0f), spawner.transform.rotation);
                marker.transform.name = enemy.name + " Spawner";

                if (enemy != null)
                {
                    Enemies properties = new Enemies();
                    properties.prefab = Resources.Load(enemy.prefabPath) as GameObject;
                    properties.position = spawner.transform.position;
                    properties.rotation = spawner.transform.rotation;

                    enemiesToSpawn.Add(properties);

                    DestroyImmediate(spawner);
                }
            }
        }
    }
}