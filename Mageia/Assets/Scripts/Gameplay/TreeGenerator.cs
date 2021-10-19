using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MageiaGame
{
    public class TreeGenerator : MonoBehaviour
    {
        [Header("Leaf")]
        public Sprite leafSprite;

        [Header("Spawning")]
        public float spawnArea = 5f;
        public float tooClose = 0.02f;
        public Vector2 leafSize = new Vector2(0.02f, 0.05f);
        public Vector2 spawnCount = new Vector2(4f, 8f);
        public Transform leafOrigin;

        private List<Vector3> leaves;

        void Start()
        {
            int toSpawn = Random.Range((int)spawnCount.x, (int)spawnCount.y);
            leaves = new List<Vector3>();

            ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
            if (ps != null)
            {
                 ParticleSystem.MainModule settings = ps.main;
                 settings.startColor = GameplayController.COLOR_2;
            }

            for (int i = 0; i < toSpawn; i++)
            {
                GameObject leaf = new GameObject("leaf");

                if (leaf != null)
                {
                    float size = Random.Range(leafSize.x, leafSize.y);
                    leaf.transform.position = GetLeafLocation();
                    leaf.transform.rotation = Quaternion.Euler(90f, 0f, Random.Range(-360, 360));
                    leaf.transform.name = "leaf " + (i > 0 ? "(" + i + ")" : "");
                    leaf.transform.SetParent(this.transform);
                    leaf.transform.localScale = new Vector3(size, size, 1f);

                    SpriteRenderer sr = leaf.AddComponent<SpriteRenderer>() as SpriteRenderer;

                    if (sr != null && leafSprite != null)
                    {
                        sr.sprite = leafSprite;
                        sr.color = GameplayController.COLOR_2;
                    }
                }
            }
        }

        // Stops leaves from being grouped too close together
        private Vector3 GetLeafLocation()
        {
            Vector3 pos;

            while (true)
            {
                pos = new Vector3(leafOrigin.position.x + Random.Range(-spawnArea, spawnArea), leafOrigin.position.y, leafOrigin.position.z + Random.Range(-spawnArea, spawnArea));

                if (leaves.Count == 0)
                {
                    leaves.Add(pos);
                    return pos;
                }
                else
                {
                    bool foundClose = false;

                    for (int i = 0; i < leaves.Count; i++)
                    {
                        if (Vector3.Distance(pos, leaves[i]) <= tooClose)
                        {
                            foundClose = true;
                        }
                    }

                    if (!foundClose)
                    {
                        leaves.Add(pos);
                        return pos;
                    }
                }
            }
        }
    }

}