using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freckers
{
	public class Froge : MonoBehaviour
	{
		public Collider2D freckerBoardZoning;
		public uint teamId = 0;
		public Animator animator;
		public GameObject crown;
		public float jumpDist;
		public float castHeight = 5;
		public float bobeSpeed = 8;
		public float minBobeSpeed = 0.2f;
		public int jumpAgainCount = 0;
		public bool king = false;
		public GameObject explosion;
		private bool deadByFailedJumpExplosion = false;

		private float bobeTravel;
		private float defaultRotation;
		public bool earnedJump = false;

		public enum State
		{
			Jumping,
			Waiting
		}
		public State state;

		// Start is called before the first frame update
		void Start()
		{
			state = State.Waiting;
			animator.SetBool("Jumping", false);
			defaultRotation = transform.eulerAngles.z;
			GameManager.Instance.RegisterFrog(teamId);
		}

		// Update is called once per frame
		void Update()
		{
			if (jumpDist > 0)
			{
				state = State.Jumping;
				animator.SetBool("Jumping", true);
				bobeTravel = Mathf.Clamp(Time.deltaTime * bobeSpeed, 0, jumpDist);
				jumpDist -= bobeTravel;
				transform.position += transform.up * bobeTravel;
				if (jumpDist <= 0)
				{
					GameManager.Instance.GetComponent<MoveClickPoints>().dragGraphicF.SetActive(false);
					state = State.Waiting;
					animator.SetBool("Jumping", false);
					transform.eulerAngles = Vector3.forward * defaultRotation;

					if (!earnedJump || jumpAgainCount > GameManager.Instance.extraJumpCount)
					{
						FindObjectOfType<FreckersAudioTracks>().SwapClassical();
						GameObject.Find("Screen Border Effects Camera").GetComponent<Camera>().enabled = false;
						GameManager.Instance.nextTurn();
						jumpAgainCount = 0;
						animator.SetFloat("Jump Again Count", 0);
					}

					earnedJump = false;
				}
			}

			//transform.position = new Vector2(Mathf.Clamp(transform.position.x, -3.5f, 3.5f), Mathf.Clamp(transform.position.y, -3.5f, 3.5f));
			if(!freckerBoardZoning.OverlapPoint(transform.position)){
				transform.position = freckerBoardZoning.ClosestPoint(transform.position);
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag("frineldlyFroge") && jumpDist > 0)
			{
				if(collision.gameObject.GetComponent<Froge>().teamId != GetComponent<Froge>().teamId)
				{
					FindObjectOfType<FreckersAudioTracks>().SwapRiff();
					GameObject.Find("Screen Border Effects Camera").GetComponent<Camera>().enabled = true;
					Destroy(collision.gameObject);
                    if (!earnedJump)
					{
						jumpAgainCount++;
						animator.SetFloat("Jump Again Count", jumpAgainCount);
					}
					earnedJump = true;
					GameManager.Instance.extraJumpTime();
				}
			}
		}

		public void TheySetUsUpTheBombFreckers(){
			deadByFailedJumpExplosion = true;
			GameObject deathExplosion = Instantiate(explosion);
			deathExplosion.transform.position = transform.position;
			Destroy(gameObject);
			FindObjectOfType<FreckersAudioTracks>().SwapClassical();
			GameObject.Find("Screen Border Effects Camera").GetComponent<Camera>().enabled = false;
		}

		private void OnDestroy() {
			GameManager.Instance.AManIsDead(teamId);
			if(deadByFailedJumpExplosion){
				GameManager.Instance.nextTurn();
			}
		}
	}
}
