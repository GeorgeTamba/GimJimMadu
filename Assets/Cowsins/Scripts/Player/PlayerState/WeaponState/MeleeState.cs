using UnityEngine;
using System.Collections; 

namespace cowsins
{
    public class MeleeState : WeaponBaseState
    {
        private WeaponController controller;
        private WeaponAnimator weaponAnimator;
        private float animationDuration, holsterAnimationDuration;
        private float timer;
        private bool isSwitchQueued = false;

        public MeleeState(WeaponStates currentContext, WeaponStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory) { }

        public override void EnterState()
        {
            controller = _ctx.GetComponent<WeaponController>();
            weaponAnimator = _ctx.GetComponent<WeaponAnimator>();
            timer = 0;
            controller.SecondaryMeleeAttack();
        }

        public override void UpdateState()
        {
            controller.StopAim();
            CheckSwitchState();
        }

        public override void FixedUpdateState()
        {
        }

        public override void ExitState()
        {

        }

        public override void CheckSwitchState()
        {
            timer += Time.deltaTime;
            AnimatorStateInfo stateInfo = controller.MeleeObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            animationDuration = stateInfo.length;
 
            if (!isSwitchQueued && timer >= animationDuration + holsterAnimationDuration + controller.meleeDelay)
            {
                controller.FinishMelee();

                AnimatorStateInfo holsterStateInfo = weaponAnimator.HolsterMotionObject.GetCurrentAnimatorStateInfo(0);
                holsterAnimationDuration = holsterStateInfo.length;

                _ctx.StartCoroutine(DelayedStateSwitch(holsterAnimationDuration));
                isSwitchQueued = true;
            }
        }

        private IEnumerator DelayedStateSwitch(float delay)
        {
            yield return new WaitForSeconds(delay);

            weaponAnimator.ResetParentConstraintWeight();

            SwitchState(_factory.Default());
        }

    }
}
