using UnityEngine;
using UnityEngine.InputSystem;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

public class PlayerInput : MonoBehaviour
{
	[Header("Character Input Values")]
	public Vector2 move;
	public Vector2 look;
	public bool jump;
	public bool sprint;
	public bool roll;
	public bool attack1;
	public bool attack2;
	public bool heal;

	[Header("Movement Settings")]
	public bool analogMovement;

	[Header("Mouse Cursor Settings")]
	public bool cursorLocked = true;
	public bool cursorInputForLook = true;

    public void OnMove(InputValue value)
	{
		MoveInput(value.Get<Vector2>());
	}

	public void OnLook(InputValue value)
	{
		
		LookInput(value.Get<Vector2>());
		
	}

	public void OnJump(InputValue value)
	{
		JumpInput(value.isPressed);
	}

	public void OnSprint(InputValue value)
	{
		SprintInput(value.isPressed);
	}

	public void OnRoll(InputValue value)
    {
		RollInput(value.isPressed);
    }
	public void OnAttack1(InputValue value)
    {
		Attack1Input(value.isPressed);
    }
	public void OnAttack2(InputValue value)
	{
		Attack2Input(value.isPressed);
	}
	public void OnHeal(InputValue value)
	{
		HealInput(value.isPressed);
	}

	public void MoveInput(Vector2 newMoveDirection)
	{
		move = newMoveDirection;
	}

	public void LookInput(Vector2 newLookDirection)
	{
		look = newLookDirection;
	}

	public void JumpInput(bool newJumpState)
	{
		jump = newJumpState;
	}

	public void SprintInput(bool newSprintState)
	{
		sprint = newSprintState;
	}
	public void RollInput(bool newRollState)
	{
		roll = newRollState;
	}
	public void Attack1Input(bool newAttack1State)
    {
		attack1 = newAttack1State;
    }
	public void Attack2Input(bool newAttack2State)
	{
		attack2 = newAttack2State;
	}
	public void HealInput(bool newHealState)
	{
		heal = newHealState;
	}
    /*
	private void OnApplicationFocus(bool hasFocus)
	{
		SetCursorState(cursorLocked);
	}
	
	private void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}*/
}

