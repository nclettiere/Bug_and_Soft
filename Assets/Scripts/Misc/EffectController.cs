using System;
using UnityEngine;

namespace Misc
{
    public class EffectController : MonoBehaviour
    {
        private int activeEffects;
        [SerializeField] private GameObject effectBuff;
        private float effectBuffLifetime;

        [Header("Effects Indicators")] [SerializeField]
        private GameObject effectDown;

        private float effectDownLifetime;
        [SerializeField] private GameObject effectSlowDown;
        private float effectSlowDownLifetime;
        [SerializeField] private GameObject effectSpeedUp;
        private float effectSpeedUpLifetime;
        [SerializeField] private GameObject effectStun;
        private float effectStunLifetime;

        private void Update()
        {
            if (effectDown.activeSelf && Time.time >= effectDownLifetime)
                EndEffect(effectDown);
            if (effectBuff.activeSelf && Time.time >= effectBuffLifetime)
                EndEffect(effectBuff);
            if (effectSpeedUp.activeSelf && Time.time >= effectSpeedUpLifetime)
                EndEffect(effectSpeedUp);
            if (effectSlowDown.activeSelf && Time.time >= effectSlowDownLifetime)
                EndEffect(effectSlowDown);
            if (effectStun.activeSelf && Time.time >= effectStunLifetime)
                EndEffect(effectStun);
        }

        private void EndEffect(GameObject go)
        {
            go.SetActive(false);
            activeEffects--;

            // Recalculate other effects position
            effectDown.transform.localPosition = new Vector2(0f, effectDown.transform.localPosition.y - 1f);
            effectBuff.transform.localPosition = new Vector2(0f, effectBuff.transform.localPosition.y - 1f);
            effectSpeedUp.transform.localPosition = new Vector2(0f, effectSpeedUp.transform.localPosition.y - 1f);
            effectSlowDown.transform.localPosition = new Vector2(0f, effectSlowDown.transform.localPosition.y - 1f);
            effectStun.transform.localPosition = new Vector2(0f, effectStun.transform.localPosition.y - 1f);
        }

        public void SetEffectDownActive(float lifeTime = 0f)
        {
            if (Time.time >= effectDownLifetime)
            {
                effectDown.SetActive(true);
                effectDown.transform.localPosition = GetEffectPosition(effectDown.transform);
                activeEffects++;
            }

            effectDownLifetime = Time.time + lifeTime;
        }

        public void SetEffectBuffActive(float lifeTime = 0f)
        {
            if (Time.time >= effectBuffLifetime)
            {
                effectBuff.SetActive(true);
                effectBuff.transform.localPosition = GetEffectPosition(effectBuff.transform);
                activeEffects++;
            }

            effectBuffLifetime = Time.time + lifeTime;
        }

        public void SetEffectSpeedUpActive(float lifeTime = 0f)
        {
            if (Time.time >= effectSpeedUpLifetime)
            {
                effectSpeedUp.SetActive(true);
                effectSpeedUp.transform.localPosition = GetEffectPosition(effectSpeedUp.transform);
                activeEffects++;
            }

            effectSpeedUpLifetime = Time.time + lifeTime;
        }

        public void SetEffectSlowDownActive(float lifeTime = 0f)
        {
            if (Time.time >= effectSlowDownLifetime)
            {
                effectSlowDown.SetActive(true);
                effectSlowDown.transform.localPosition = GetEffectPosition(effectSlowDown.transform);
                activeEffects++;
            }

            effectSlowDownLifetime = Time.time + lifeTime;
        }

        public void SetEffectStunActive(float lifeTime = 0f)
        {
            if (Time.time >= effectStunLifetime)
            {
                effectStun.SetActive(true);
                effectStun.transform.localPosition = GetEffectPosition(effectStun.transform);
                activeEffects++;
            }

            effectStunLifetime = Time.time + lifeTime;
        }

        private Vector2 GetEffectPosition(Transform transform)
        {
            Debug.Log("activeEffects" + activeEffects);
            return new Vector2(transform.localPosition.x, transform.localPosition.y + activeEffects);
        }
    }
}