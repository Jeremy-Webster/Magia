using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace MageiaGame
{
    [CreateAssetMenu(fileName = "New Item", menuName = "MageiaGame/Item")]
    public class Item : ScriptableObject
    {
        public static int ICON_SIZE = 128, ICONS_PER_SHEET = 64;
        public enum ItemType { Default, Weapon, Armor }
        public static List<Item> items = new List<Item>();

        public string itemName = "New Item";
        public string itemDescription = "New Description";
        public ItemType type = ItemType.Default;
        public string spriteName = "1";
        public SpriteAtlas atlas;

        private Sprite icon;

        public void OnEnable()
        {
            icon = atlas.GetSprite(spriteName);
        }

        public Sprite GetIcon() { return this.icon; }
    }
}