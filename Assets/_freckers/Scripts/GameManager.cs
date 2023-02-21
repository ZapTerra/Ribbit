using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freckers
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance { get; private set; }
		public uint teamCount = 2;
		public uint extraJumpCount = 1;
		public bool extraMovesInProgress;
		
		public uint currentTurn { get; private set; } = 0;

		private void Start()
		{
			currentTurn = 0;
			Instance = this;
		}

		public void nextTurn()
		{
			currentTurn = (currentTurn + 1) % teamCount;
			extraMovesInProgress = false;
			Debug.Log("Current turn: Team " + currentTurn);
		}

		public void extraJumpTime()
		{
			extraMovesInProgress = true;
		}

		public bool IsMyTurn(Froge froge)
		{
			Debug.Log("Is my turn: " + (froge.teamId == currentTurn || froge.state == Froge.State.Jumping));
			return froge.teamId == currentTurn || froge.state == Froge.State.Jumping;
		}
	}
}
