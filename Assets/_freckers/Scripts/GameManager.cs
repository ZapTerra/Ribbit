using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;

namespace Freckers
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance { get; private set; }

		public int numberOfTeams = 3;
		public List<Color> teamBackgroundColors;
		public float buttonLockDelay = 1.5f;
		public List<ButtonPadlock> buttonsToLock;
		public Animator enGardeAnimator;
		public List<int> livingFrogCounts;
		public List<bool> deadTeams;
		public int extraJumpCount = 1;
		public bool extraMovesInProgress;
		public bool gameHasBegun = false;

		public Camera cameraThatControlsBackgroundColor;
		
		public int currentTurn { get; private set; } = 0;

		private void Awake()
		{
			Debug.Log("Build problems? Maybe try uncommenting header");
			currentTurn = 0;
			if(Instance != null)
			{
			//Destroy the previous instance.
			Destroy(Instance);
			}
			//Set the new instance to this object.
			Instance = this;

			for(int i = 0; i < numberOfTeams; i++){
				deadTeams.Add(false);
				livingFrogCounts.Add(0);
			}
		}

		public void StartGame(){
			gameHasBegun = true;
			enGardeAnimator.SetTrigger("EnGarde");
			StartCoroutine(LockButtons());
		}

		public IEnumerator LockButtons(){
			yield return new WaitForSeconds(buttonLockDelay);
			gameHasBegun = true;
			foreach(ButtonPadlock b in buttonsToLock){
				b.Lock();
			}
		}

		public void RegisterFrog(uint teamNumber){
			livingFrogCounts[(int)teamNumber]++;
		}

		public void AManIsDead(uint teamNumber){
			int team = (int)teamNumber;
			livingFrogCounts[team]--;
			if(livingFrogCounts[team] <= 0){
				deadTeams[team] = true;
			}
		}

		public void nextTurn()
		{
			currentTurn = (currentTurn + 1) % numberOfTeams;
			if(deadTeams[currentTurn]){
				if(!deadTeams.Contains(false)){
					Debug.Log("There is not a life left unclaimed in this barren hellscape of conflict");
					cameraThatControlsBackgroundColor.backgroundColor = Color.black;
					return;
				}
				nextTurn();
				return;
			}
			extraMovesInProgress = false;
			Debug.Log("Current turn: Team " + currentTurn);
			cameraThatControlsBackgroundColor.backgroundColor = teamBackgroundColors[currentTurn];
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
