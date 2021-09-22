using System;
using Controllers;
using UnityEngine;

namespace Items
{
    public class ItemGiver : MonoBehaviour
    {
        [SerializeField] private Item[] items;
        private BaseController controller;
        
        public void Run()
        {
            foreach (var item in items)
            {
                item.Spawn(transform.position);
            }
        }
    }
}