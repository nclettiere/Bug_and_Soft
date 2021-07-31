using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

namespace Controllers.Froggy
{
    public class FroggyController : BaseController
    {
        [Header("Froggy Specific Values")]
        [SerializeField] private AudioSource jumpSFX;
        [SerializeField] private AudioSource landSFX;
        
        private float lastJumpTime = float.NegativeInfinity;
        private float jumpCooldownTime;
        private bool canFroggyDie = false;
        
        protected override void OnStart()
        {
            controllerKind = EControllerKind.Enemy;

            jumpCooldownTime = Random.Range(2.5f, 5f);

            SwitchStates(ECharacterState.Jumping);

            InvokeRepeating("MoveCejas", 0f, 7f);
        }

        protected override bool OnUpdateJumpingStateStart()
        {
            if (Time.time >= lastJumpTime)
            {
                // SFX de saltar !!!
                AudioSource.PlayClipAtPoint(jumpSFX.clip, transform.position);
                return true;
            }

            return false;
        }

        public void OnFroggyLanded()
        {
            AudioSource.PlayClipAtPoint(landSFX.clip, transform.position);
        }

        protected override void OnUpdateJumpingStateEnd()
        {
            lastJumpTime = Time.time + jumpCooldownTime;
        }

        protected override void OnEnterDeadStateStart()
        {
            rigidbody2D.constraints = RigidbodyConstraints2D.None;
            rigidbody2D.AddForce(new Vector2(3f * -facingDirection, 3f), ForceMode2D.Impulse);
            rigidbody2D.AddTorque(10f * -facingDirection, ForceMode2D.Impulse);

            StartCoroutine(DeathTiming());
        }
        
        private IEnumerator DeathTiming()
        {
            yield return new WaitForSeconds(2.5f);
            canCharacterDie = true;
        }

        /// <summary>
        ///     Metodo para empezar la animacion de mover cejas del PJ Njord
        /// </summary>
        private void MoveCejas()
        {
            GetAnimator().SetBool("EyebrowsMovement", true);
        }

        /// <summary>
        ///     Metodo para terminar la animacion de mover cejas del PJ Njord
        /// </summary>
        /// <remarks>
        ///     Es llamado al final de la animacion cejas.
        /// </remarks>
        private void AnimCejasEnded()
        {
            GetAnimator().SetBool("EyebrowsMovement", false);
        }
    }
}
