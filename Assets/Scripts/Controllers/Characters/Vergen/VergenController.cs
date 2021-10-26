using a;
using UnityEngine;

namespace Controllers.Characters.Vergen
{
    public class VergenController : BaseController
    {
        private bool _CombatEnabled;

        private VergenIdleState vergenIdleState;
        
        protected override void Start()
        {
            base.Start();
            
            vergenIdleState = new VergenIdleState(this, StateMachine, "Idle", this);
            StateMachine.Initialize(vergenIdleState);
        }

        protected override void Update()
        {
            if (_CombatEnabled)
            {
                base.Update();
            }
        }

        protected override void FixedUpdate()
        {
            if (_CombatEnabled)
            {
                base.FixedUpdate();
            }
        }

        public void StartCombat()
        {
            Debug.Log("VERGEN IS ALIVE");
            _CombatEnabled = true;
        }
    }
}