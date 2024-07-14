using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
	public float tapSensitivity = .2f;
	public float blockSensitivity = .2f;
	public enum Direction
    {
		Neutral,
		Fire,
		Roll,
		Loll,
		Reload,
		Taunt,
		Ublock,
		Rblock,
		Lblock,
		Up,
		Down,
		Left,
		Right,
		Lup,
		Rup,
		Ldown,
		Rdown
    }
	public Direction inputDirection;
	private Touch playerTouch;
	private float timeTouchStarted;
	private float timeTouchEnded;
	private Vector2 touchStartPosition;
	private Vector2 touchEndPosition;
	private float x;
	private float y;
	private bool awaitingInputDecision = false;
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		if (timeTouchEnded < Time.time - 1)
        {
			inputDirection = Direction.Neutral;
		}

		if (Input.GetMouseButtonDown(0))
		{
			timeTouchStarted = Time.time;
			touchStartPosition = Input.mousePosition;
			awaitingInputDecision = true;
		}
		else if (Input.GetMouseButtonUp(0) && awaitingInputDecision)
		{
			timeTouchEnded = Time.time;
			touchEndPosition = Input.mousePosition;
			float x = touchEndPosition.x - touchStartPosition.x;
			float y = touchEndPosition.y - touchStartPosition.y;
			if (Mathf.Abs(x) < tapSensitivity && Mathf.Abs(y) < tapSensitivity)
			{
				inputDirection = touchEndPosition.y < Screen.height / 3 ?
					Direction.Reload :
					touchEndPosition.x < Screen.width / 3 ?
					Direction.Loll :
					touchEndPosition.x > Screen.width * 2 / 3 ?
					Direction.Roll :
					touchEndPosition.y < Screen.height * 2 / 3 ?
					Direction.Fire :
					Direction.Taunt;
			}
			else
			{
				inputDirection =
					(Mathf.Abs(x / y) > 2) ?
					(Mathf.Sign(x) == 1 ? Direction.Right : Direction.Left)
					:
					(Mathf.Abs(x / y) < .5) ?
					(Mathf.Sign(y) == 1 ? Direction.Up : Direction.Down)
					:
					Mathf.Sign(y) == 1 ?
					Mathf.Sign(x) == 1 ? Direction.Rup : Direction.Lup
					:
					Mathf.Sign(x) == 1 ? Direction.Rdown : Direction.Ldown;
			}
			awaitingInputDecision = false;
		}
		if(Time.time > timeTouchStarted + blockSensitivity && Input.GetMouseButton(0) && awaitingInputDecision)
        {
			float x = Input.mousePosition.x - touchStartPosition.x;
			float y = Input.mousePosition.y - touchStartPosition.y;
			if (Mathf.Abs(x) < tapSensitivity && Mathf.Abs(y) < tapSensitivity)
			{
				inputDirection = Input.mousePosition.y > Screen.height * 2 / 3 ? Direction.Ublock : Input.mousePosition.x > Screen.width / 2 ? Direction.Rblock : Direction.Lblock;
				awaitingInputDecision = false;
				timeTouchEnded = Time.time;
			}
		}
	}
}
