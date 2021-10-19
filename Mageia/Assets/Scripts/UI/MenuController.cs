using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MageiaGame
{
    public class MenuController : MonoBehaviour
    {
        Chapter curChap;
        Chapter[] chapters;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartGame()
        {
            SceneManager.LoadScene("Game",LoadSceneMode.Single);
        }
    }
}