using System;
using System.Collections;
using UnityEngine;

namespace Controllers.Froggy
{
    public class Mini_OLC : MonoBehaviour
    {
        private Rigidbody2D _rbody;
        [SerializeField] private GameObject _smoke;
        [SerializeField] private Sprite[] _pepeQC;
        
        private PepeController _pepe;

        private void Awake()
        {
            _pepe = GameObject.Find("/Characters/NPC/Pepe").GetComponent<PepeController>();
            _rbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _rbody.AddForce(new Vector2(0, 40), ForceMode2D.Impulse);
            _rbody.AddTorque(10, ForceMode2D.Impulse);

            Instantiate(_smoke, transform.position, Quaternion.Euler(0, 0, 0));
            
            //_pepe.ShowQuickChat(new Tuple<Sprite, int>(_pepeQC[0], 2));
            //_pepe.ShowQuickChat(new Tuple<Sprite, int>(_pepeQC[1], 2));
        }
    }
}