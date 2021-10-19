using UnityEngine;
using System.Collections;

namespace MageiaGame
{
    [CreateAssetMenu(fileName = "New Chapter", menuName = "MageiaGame/Chapter")]
    public class Chapter: ScriptableObject
    {
        [Header("Display")]
        public string title;
        public string description;
        public Sprite icon;

        [Header("Setup")]
        public Stage[] stages;
        public Stage[] bossStages;
        public Stage[] relicStage;
    }
}

