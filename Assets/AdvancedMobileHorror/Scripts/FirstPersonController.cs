using System.Collections;
using UnityEngine;

namespace AdvancedHorrorFPS
{
	[RequireComponent(typeof(CharacterController))]
	public class FirstPersonController : MonoBehaviour
	{
		public float MoveSpeed = 4.0f;
		public float SprintSpeed = 6.0f;
		public float RotationSpeed = 1.0f;
		public float SpeedChangeRate = 10.0f;
		public float JumpHeight = 1.2f;
		public float Gravity = -15.0f;
		public float JumpTimeout = 0.1f;
		public float FallTimeout = 0.15f;
		public bool Grounded = true;
		public float GroundedOffset = -0.14f;
		public float GroundedRadius = 0.5f;
		public LayerMask GroundLayers;
		public float TopClamp = 90.0f;
		public float BottomClamp = -90.0f;
		private float _speed;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;
		private CharacterController _controller;
		public GameObject Camera;
		private bool canJump = true;
		public Animation animation;
		public Animation FPSHandParent;
		private bool isCrouching = false;

		private float originalHeight; // Başlangıç yüksekliği
		private bool increasingHeight = true;
		private float heightChangeSpeed = 6f; // Yükseklik değişim hızı
		private float heightDelta = 0.05f;


		private void Start()
		{
			_controller = GetComponent<CharacterController>();
			originalHeight = _controller.height;
		}

		private void Update()
		{
			JumpCrouchAndGravity();
			GroundedCheck();
			Move();
		}

		public void Jump()
        {
			if(canJump && AdvancedGameManager.Instance.canJump)
			{
				_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
				Grounded = false;
				AudioManager.Instance.Play_Jump();
				StartCoroutine(ResetJump());
			}
		}

		public void Crouch()
        {
			if(AdvancedGameManager.Instance.canCrouch)
            {
				if(isCrouching)
                {
					_controller.height = 2;
					originalHeight = 2;
					isCrouching = false;
				}
				else
                {
					_controller.height = 1.25f;
					originalHeight = 1.25f;
					isCrouching = true;
				}
            }
        }

		IEnumerator ResetJump()
		{
			canJump = false;
			yield return new WaitForSeconds(1);
			canJump = true;
        }


		private void LateUpdate()
        {
			RotationUpdate();
        }

		private void RotationUpdate()
        {
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, Camera.transform.eulerAngles.y, transform.eulerAngles.z);
        }


        private void GroundedCheck()
		{
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
		}

		public Vector2 _input;
		public Vector2 _input_look;
		[HideInInspector]
		public bool isSprinting = false;

		public void Sprint()
        {
			if (AdvancedGameManager.Instance.canSprint)
			{
				if (isSprinting)
				{
					isSprinting = false;
				}
				else
				{
					isSprinting = true;
				}
			}
        }

		private float Stamina = 100;

		private void Move()
		{
			float targetSpeed = MoveSpeed;
			if(AdvancedGameManager.Instance.canSprint)
            {
				if (AdvancedGameManager.Instance.controllerType == ControllerType.PcAndConsole)
				{
					if(Input.GetKey(KeyCode.LeftShift))
                    {
						isSprinting = true;
					}
					else if (Input.GetKeyUp(KeyCode.LeftShift))
					{
						isSprinting = false;
					}
				}
				if(isSprinting && Stamina > 0)
                {
					targetSpeed = SprintSpeed;
					Stamina = Stamina - Time.deltaTime * 25;
				}
				else if(!isSprinting && Stamina < 100)
                {
					Stamina = Stamina + Time.deltaTime * 10;
					if (Stamina > 100) Stamina = 100;
				}
				if(Stamina <0)
                {
					AudioManager.Instance.Play_Audio_StaminaBreathing();
					Stamina = 0;
				}
				GameCanvas.Instance.Slider_Stamina.fillAmount = (Stamina / 100f);
			}


			if(isCrouching)
            {
				targetSpeed = (MoveSpeed / 1.5f);
            }


			if (AdvancedGameManager.Instance.controllerType == ControllerType.Mobile)
			{
				_input = new Vector2(SimpleJoystick.Instance.HorizontalValue, SimpleJoystick.Instance.VerticalValue);
			}
			else
            {
				_input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			}
			if (_input == Vector2.zero) targetSpeed = 0.0f;
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
			float speedOffset = 0.1f;
			float inputMagnitude = 1f;
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			Vector3 inputDirection = new Vector3(_input.x, 0.0f, _input.y).normalized;
			if (_input != Vector2.zero)
			{
				inputDirection = transform.right * _input.x + transform.forward * _input.y;
				AudioManager.Instance.Play_Player_Walk();
				if (!FPSHandParent.isPlaying) FPSHandParent.Play("WalkingHandAnimation");

				if (increasingHeight)
				{
					_controller.height += heightDelta * Time.deltaTime * heightChangeSpeed;
					if (_controller.height >= originalHeight + heightDelta)
					{
						increasingHeight = false;
					}
				}
				else
				{
					_controller.height -= heightDelta * Time.deltaTime * heightChangeSpeed;
					if (_controller.height <= originalHeight - heightDelta)
					{
						increasingHeight = true;
					}
				}



			}
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		}

		private void JumpCrouchAndGravity()
		{
			if(Input.GetKeyUp(KeyCode.Space) && AdvancedGameManager.Instance.canJump && AdvancedGameManager.Instance.controllerType == ControllerType.PcAndConsole)
            {
				Jump();
			}
			if ((Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl) && AdvancedGameManager.Instance.canCrouch && AdvancedGameManager.Instance.controllerType == ControllerType.PcAndConsole))
			{
				Crouch();
			}
			if (Grounded)
			{
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}
			}

			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}
	}
}