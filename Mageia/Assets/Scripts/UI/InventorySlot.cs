using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

namespace MageiaGame
{
    public class InventorySlot : MonoBehaviour
    {
        public GameObject icon;
        public int slotNumber;
        public SpriteAtlas atlas;

        private Item item;

        public void UpdateSlot()
        {
            item = Inventory.instance.itemList[slotNumber];

            if (item != null)
            {
                icon.GetComponent<Image>().sprite = item.GetIcon();
                icon.SetActive(true);
            }
            else
            {
                icon.SetActive(false);
            }
        }
    }

}