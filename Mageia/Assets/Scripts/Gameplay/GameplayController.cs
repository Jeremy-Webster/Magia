using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace MageiaGame
{
    public class GameplayController : MonoBehaviour
    {
        public static bool TransistionInProgess = false;
        public static bool EnteredRoom = false;
        public static bool ClearedRoom = false;

        public static Color COLOR_1 = new Color32(42, 59, 96, 255); // 2A3B60
        public static Color COLOR_2 = new Color32(255, 111, 114, 255); // FF6F72
        public static Color COLOR_3 = new Color32(141, 133, 146, 255); // 8D8592
        public static Color COLOR_4 = new Color32(253, 232, 210, 255); // FDE8D2

        public static Color COLOR_ROCKS = new Color32(62,68,84, 255); // 3E4454
        public static Color COLOR_FENCE = new Color32(177, 163, 190, 255); // B1A3BE
        public static Color COLOR_BRICK = new Color32(45, 49, 60, 255); // 2D313C
        public static Color COLOR_CLOUD = new Color32(236, 236, 236, 255); // ECECEC
        public static Color COLOR_GLOW = new Color32(86, 244, 255, 255); // 56F4FF

        public static Color COLOR_GRASS = new Color32(199, 248, 113, 255); // B3F871
        public static Color COLOR_STONE = new Color32(166, 157, 171, 255); // A69DAB

        [Header("Core Colors")]
        public Material core1;
        public Material core2;
        public Material core3;
        public Material core4;

        [Header("Utility Colors")]
        public Material rocks;
        public Material fence;
        public Material brick;
        public Material cloud;
        public Material glow;

        [Header("Lit Colors")]
        public Material grass;
        public Material stone;

        [Header("Stage")]
        public Stage stage;
        public TextMeshPro stageIndicator;

        [Header("Utility")]
        public GameObject forcefield;
        public GameObject portal;
        public GameObject spawnPoint;
        public Image fadeOverlay;

        public List<Stage> stageList = new List<Stage>();

        [Header("Effects")]
        public GameObject enemyAttackEffect;
        public GameObject enemyDeathEffect;
        public GameObject playerDamageEffect;

        [Header("Editor")]
        public bool drawEditorGizmos = true;

        ///////////////////////////////////////////////////////

        private Stage[] stages;

        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                          Initalization                           //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////

        private static GameplayController instance;
        public static GameplayController Instance // Assign Singlton
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GameplayController>();
                    if (Instance == null)
                    {
                        var instanceContainer = new GameObject("GameplayController");
                        instance = instanceContainer.AddComponent<GameplayController>();
                    }
                }
                return instance;
            }
        }

        void Start()
        {
            stages = new Stage[stageList.Count];
            for (int i = 0; i < stageList.Count; i++)
            {
                stages[i] = Object.Instantiate(stageList[i]);
            }

            if (stage == null && stages.Length > 0) stage = stages[0];

            core1.SetColor("_Color", COLOR_1);
            core2.SetColor("_Color", COLOR_2);
            core3.SetColor("_Color", COLOR_3);
            core4.SetColor("_Color", COLOR_4);

            rocks.SetColor("_Color", COLOR_ROCKS);
            fence.SetColor("_Color", COLOR_FENCE);
            fence.SetColor("_Color", COLOR_BRICK);
            cloud.SetColor("_Color", COLOR_CLOUD);
            glow.SetColor("_Color", COLOR_GLOW);

            grass.SetColor("_Color", COLOR_GRASS);
            stone.SetColor("_Color", COLOR_STONE);

            StartCoroutine(LoadStage(stage));
        }


        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                       GETTERS & SETTERS                          //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////

        public int GetStageNumber()
        {
            return stage.number;
        }

        public Stage GetStage()
        {
            return stage;
        }


        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                             Stage                                //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////

        public IEnumerator LoadStage(Stage stage)
        {
            Color col = fadeOverlay.color; col.a = 1f; fadeOverlay.color = col; // Keep faded while loading

            forcefield.SetActive(true);
            portal.SetActive(false);
            stageIndicator.text = stage.number.ToString();

            yield return SceneManager.LoadSceneAsync("STAGE_" + stage.number, LoadSceneMode.Additive);

            Scene loadedScene = SceneManager.GetSceneByName("STAGE_" + stage.number);
            SceneManager.SetActiveScene(loadedScene);

            this.stage = stage;

            EnteredRoom = false;

            Invoke("DelayedSpawning", 0.1f);

            StartCoroutine(Fade(1f, 0f, 0.8f)); // Fade in (Once stage is fully loaded)
        }

        private void DelayedSpawning()
        {
            stage.BeginStage();
        }

        public void ExitStage()
        {
            StartCoroutine(Fade(0f, 1f, 0.8f)); // Fade out

            // Delete spawned prefabs

            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("STAGE_" + stage.number));
        }

        public void EnemiesDefeated()
        {
            Animator anim = forcefield.GetComponent<Animator>();

            if (anim != null)
            {
                anim.SetTrigger("CLOSE");
            }

            // Pull in gold effect
            // Loop through all dropped gold, lerp posistion towards player
            // Have gold give coin when they touch the player and all enemies are defeated

            Invoke("OpenPortal", 1f);
        }

        private void OpenPortal()
        {
            forcefield.SetActive(false);
            portal.SetActive(true);
        }

        // This is triggered by the portal
        public void FinishStage()
        {
            StartCoroutine("NextStage");
        }

        public void MoveSpawnPoint(Vector3 pos)
        {
            if (spawnPoint != null)
                spawnPoint.transform.position = pos;
        }

        private IEnumerator NextStage()
        {
            TransistionInProgess = true;
            ExitStage();

            yield return new WaitForSeconds(0.3f);

            StartCoroutine(LoadStage(stages[stage.number]));

            yield return new WaitForSeconds(0.3f);

            TransistionInProgess = false;
        }

        private IEnumerator Fade(float start, float end, float duration)
        {
            Color col = fadeOverlay.color;
            float counter = 0;

            while (counter < duration)
            {
                counter += Time.deltaTime;
                col.a = Mathf.Lerp(start, end, counter / duration);
                fadeOverlay.color = col;
                yield return null;
            }
        }
    }
}