using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace MageiaGame
{
    public class ChallangeManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject challangeUIList, challangeUI;

        private Challange[] challanges;

        // Start is called before the first frame update
        void Start()
        {
            //        LoadMissionsFromFile();

            Challange mission1 = new Challange("Hallow's Eve", "1.0.0", "Jeremy Webster", "All ghosts are out. They're not after your life; They're after your candies!");
        }

        // Update is called once per frame
        void Update()
        {

        }

        public static void LoadMissionsFromFile()
        {
            string path = "/stages.json";
            string jsonString;

            jsonString = File.ReadAllText(Application.streamingAssetsPath + path);
            Challanges challangesInJson = JsonUtility.FromJson<Challanges>(jsonString);

            Debug.Log("Loading Challange File!");
            Debug.Log(jsonString);
            Debug.Log(challangesInJson);

            int i = 0;
            foreach (Challange challange in challangesInJson.challanges)
            {
                Debug.Log((i++) + "# = " + challange.ToString());
            }
        }
    }

    [System.Serializable]
    public class Challanges
    {
        public Challange[] challanges;
    }

    [System.Serializable]
    public class Challange
    {
        private string title, version, author, description;
        private Time starttime, endtime;
        private int attempts;
        private Texture2D icon, background;
        private Color bannercol;
        private Item[] rewards;

        public Challange(string title, string version, string author, string description)
        {
            this.title = title;
            this.version = version;
            this.author = author;
            this.description = description;

            Debug.Log("Created Mission!");
        }

        public Challange(string title, string version, string author, string description, Time starttime, Time endtime, int attempts, Texture2D icon, Texture2D background, Color bannercol, Item[] rewards)
        {

        }

        public Challange(string title, string version, string author, string description, Time starttime, Time endtime, int attempts, Texture2D icon, Texture2D background, Color bannercol, string rewardsJSON)
        {
            // Turn JSON rewards string into items elements
        }


        public override string ToString()
        {
            return "title: " + title + ", version: " + version + ", author: " + author;
        }
    }
}