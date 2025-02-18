using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem.XR;

namespace cowsins
{
    public class WeaponAnimator : MonoBehaviour
    {
        [SerializeField] private ParentConstraint parentConstraint;

        [SerializeField] private Animator holsterMotionObject;

        public Animator HolsterMotionObject
        {
            get { return holsterMotionObject; }
        }


        private PlayerMovement player;
        private WeaponController wc;
        private InteractManager interactManager;
        private Rigidbody rb;
        private WeaponStates weaponStates; 

        void Start()
        {
            player = GetComponent<PlayerMovement>();
            wc = GetComponent<WeaponController>();
            interactManager = GetComponent<InteractManager>();
            rb = GetComponent<Rigidbody>();
            weaponStates = GetComponent<WeaponStates>();    
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (wc.inventory[wc.currentWeapon] == null) return;

            Animator currentAnimator = wc.inventory[wc.currentWeapon].GetComponentInChildren<Animator>();

            if (player.wallRunning && !wc.Reloading)
            {
                CowsinsUtilities.PlayAnim("walking", currentAnimator);
                return;

            }
            if (wc.Reloading || wc.shooting || player.isCrouching || !player.grounded || rb.velocity.magnitude < 0.1f || wc.isAiming
                || currentAnimator.GetCurrentAnimatorStateInfo(0).IsName("Unholster")
                || currentAnimator.GetCurrentAnimatorStateInfo(0).IsName("reloading")
                || currentAnimator.GetCurrentAnimatorStateInfo(0).IsName("shooting"))
            {
                CowsinsUtilities.StopAnim("walking", currentAnimator);
                CowsinsUtilities.StopAnim("running", currentAnimator);
                return;
            }

            if (rb.velocity.magnitude > player.crouchSpeed && !wc.shooting && player.currentSpeed < player.runSpeed && player.grounded && !interactManager.inspecting) CowsinsUtilities.PlayAnim("walking", currentAnimator);
            else CowsinsUtilities.StopAnim("walking", currentAnimator);

            if (player.currentSpeed >= player.runSpeed && player.grounded) CowsinsUtilities.PlayAnim("running", currentAnimator);
            else CowsinsUtilities.StopAnim("running", currentAnimator);

            if(weaponStates.CurrentState != weaponStates._States.Inspect())
            {
                CowsinsUtilities.StopAnim("inspect", currentAnimator);
                CowsinsUtilities.StopAnim("finishedInspect", currentAnimator);
            }    
        }

        public void StopWalkAndRunMotion()
        {
            if (!wc) return; // Ensure there is a reference for the Weapon Controller before running the following code
            Animator weapon = wc.inventory[wc.currentWeapon].GetComponentInChildren<Animator>();
            CowsinsUtilities.StopAnim("inspect", weapon);
            CowsinsUtilities.StopAnim("walking", weapon);
            CowsinsUtilities.StopAnim("running", weapon);
        }

        public void HideWeapon()
        {
            CowsinsUtilities.PlayAnim("hit", holsterMotionObject);
        }

        public void ShowWeapon()
        {
            CowsinsUtilities.PlayAnim("finished", holsterMotionObject);
            Invoke(nameof(ResetParentConstraintWeight), .2f);
        }

        public void SetParentConstraintSource(Transform transform)
        {
            if(parentConstraint.sourceCount > 0) parentConstraint.RemoveSource(0);

            ConstraintSource source = new ConstraintSource
            {
                sourceTransform = transform,
                weight = .5f                 
            };

            parentConstraint.AddSource(source);
        }

        public void SetParentConstraintWeight(float weight) => parentConstraint.weight = weight;

        public void ResetParentConstraintWeight()
        {
            if (player.Climbing) return; 
            StartCoroutine(ResetParentConstraintWeightCoroutine());
        }

        private IEnumerator ResetParentConstraintWeightCoroutine()
        {
            SetParentConstraintWeight(0);
            SetParentConstraintSource(wc.id?.HeadBone ?? null);

            // Gradually increase weight
            float targetWeight = 0.5f;
            float weightIncrementDuration = 1f;
            float elapsedTime = 0f;

            while (elapsedTime < weightIncrementDuration)
            {
                elapsedTime += Time.deltaTime;
                float weight = Mathf.Lerp(0, targetWeight, elapsedTime / weightIncrementDuration);
                SetParentConstraintWeight(weight);
                yield return null;
            }

            // Ensure the final weight is exactly 0.5
            SetParentConstraintWeight(targetWeight);
        }    
    }

}