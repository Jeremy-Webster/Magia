using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MageiaGame
{
    public class PauseGame : MonoBehaviour
    {
        public static bool GameIsPaused = false;
        public static bool GameIsMuted = false;
        public GameObject pauseMenuUI;

        [Header("Volume Button")]
        public Sprite volumeIcon;
        public Sprite muteIcon;
        public Image volumeImage;


        /////////////////////////////////////////


        private float initVolume;

        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                          Initalization                           //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////

        private static PauseGame instance;
        public static PauseGame Instance // Assign Singlton
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<PauseGame>();
                    if (Instance == null)
                    {
                        var instanceContainer = new GameObject("PauseGame");
                        instance = instanceContainer.AddComponent<PauseGame>();
                    }
                }
                return instance;
            }
        }

        void Start()
        {
            if (pauseMenuUI != null && pauseMenuUI.activeSelf)
            {
                Pause();
            }
        }

        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                              Core                                //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////

        public void Pause()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;
        }

        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GameIsPaused = false;
        }

        public void Home()
        {
            Time.timeScale = 1f;
            GameIsPaused = false;
            SceneManager.LoadScene("Menu");
        }

        public void Mute()
        {
            AudioSource source = Camera.main.GetComponent<AudioSource>();

            if (source.volume != 0f) initVolume = source.volume;
            GameIsMuted = !GameIsMuted;

            volumeImage.sprite = GameIsMuted ? volumeIcon : muteIcon;

            if (source != null)
            {
                source.volume = GameIsMuted ? 0f : initVolume;
            }
        }
    }

}