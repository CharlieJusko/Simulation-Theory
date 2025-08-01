using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
	public class Inputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
        public bool dash;
        public bool shoot1;
		public bool shoot2;
		public bool activate;
		public bool pause;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		public void OnMove(InputValue value) {
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value) {
			if(cursorInputForLook) {
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value) {
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value) {
			SprintInput(value.isPressed);
		}

        public void OnDash(InputValue value) {
            DashInput(value.isPressed);
        }

        public void OnShoot1(InputValue value) {
			Shoot1Input(value.isPressed);
		}

		public void OnShoot2(InputValue value) {
			Shoot2Input(value.isPressed);
		}

		public void OnActivate(InputValue value) {
			ActivateInput(value.isPressed);
		}

		public void OnPause(InputValue value) {
			PauseInput(value.isPressed);
		}

		public void MoveInput(Vector2 newMoveDirection) { 
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection) {
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState) {
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState) {
			sprint = newSprintState;
		}

        public void DashInput(bool newDashState) {
            dash = newDashState;
        }

        public void Shoot1Input(bool newShoot1State) {
			shoot1 = newShoot1State;
		}

		public void Shoot2Input(bool newShoot2State) {
			shoot2 = newShoot2State;
		}

		public void ActivateInput(bool newActivateState) {
			activate = newActivateState;
		}

		public void PauseInput(bool newPauseState) {
			pause = newPauseState;
		}
	}
}