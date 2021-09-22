using System;
using Controllers;
using UnityEngine;

namespace Items
{
    public class ItemGiver : MonoBehaviour
    {
        [SerializeField] private Item item;
        private BaseController controller;

        private void Start()
        {
            controller = GetComponent<BaseController>();
        }

        private void Update()
        {
            if(controller.currentHealth <= 0)
                item.Spawn(transform.position);
        }
    }
}