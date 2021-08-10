using System;
using UnityEngine;

namespace Misc
{
    public class EffectController : MonoBehaviour
    {
        [SerializeField] private GameObject effectDown;
        [SerializeField] private GameObject effectBuff;
        [SerializeField] private GameObject effectSpeedUp;
        [SerializeField] private GameObject effectSlowDown;
        [SerializeField] private GameObject effectStun;
        
        [SerializeField] private bool isEffectDownActive;
        [SerializeField] private bool isEffectBuffActive;
        [SerializeField] private bool isEffectSpeedUpActive;
        [SerializeField] private bool isEffectSlowDownActive;
        [SerializeField] private bool isEffectStunActive;

        private void Update()
        {
            float effectOffset = -30f;

            if (isEffectDownActive && effectDown != null)
            {
                effectDown.transform.position = 
                    new Vector2(effectDown.transform.position.x + effectOffset, effectDown.transform.position.y);
                effectOffset += 15f;   
            }
            
            if (isEffectBuffActive && effectBuff != null)
            {
                effectBuff.transform.position = 
                    new Vector2(effectBuff.transform.position.x + effectOffset, effectBuff.transform.position.y);
                effectOffset += 15f;   
            }
            
            if (isEffectSlowDownActive && effectSlowDown != null)
            {
                effectSlowDown.transform.position = 
                    new Vector2(effectSlowDown.transform.position.x + effectOffset, effectSlowDown.transform.position.y);
                effectOffset += 15f;   
            }
            
            if (isEffectStunActive && effectStun != null)
            {
                effectStun.transform.position = 
                    new Vector2(effectStun.transform.position.x + effectOffset, effectStun.transform.position.y);
                effectOffset += 15f;   
            }
            
            if (isEffectSpeedUpActive && effectSpeedUp != null)
            {
                effectSpeedUp.transform.position = 
                    new Vector2(effectSpeedUp.transform.position.x + effectOffset, effectSpeedUp.transform.position.y);
                effectOffset += 15f;   
            }
        }

        public void SetEffectDownActive(bool isActive)
        {
            isEffectDownActive = isActive;
        }
        
        public void SetEffectBuffActive(bool isActive)
        {
            isEffectBuffActive = isActive;
        }
        
        public void SetEffectSpeedUpActive(bool isActive)
        {
            isEffectSpeedUpActive = isActive;
        }
        
        public void SetEffectSlowDownActive(bool isActive)
        {
            isEffectSlowDownActive = isActive;
        }
        
        public void SetEffectStunActive(bool isActive)
        {
            isEffectStunActive = isActive;
        }
    }
}