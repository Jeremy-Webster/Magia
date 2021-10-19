using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MageiaGame
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory instance;
        private const int INVENTORY_SIZE = 6;

        public Item[] itemList = new Item[INVENTORY_SIZE];
        public InventorySlot[] slots = new InventorySlot[INVENTORY_SIZE];

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this);
            }
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            UpdateSlotUI();
        }

        private bool Add(Item item)
        {
            for (int i = 0;i < INVENTORY_SIZE; i++)
            {
                if (itemList[i] == null)
                {
                    itemList[i] = item;
                    return true;
                }
            }

            return false;
        }

        public void UpdateSlotUI()
        {
            for (int i = 0;i < slots.Length; i++)
            {
                slots[i].UpdateSlot();
            }
        }

        public void AddItem(Item item)
        {
            bool hasAdded = Add(item);

            if (hasAdded)
            {
                UpdateSlotUI();
            }
    }
    }
}