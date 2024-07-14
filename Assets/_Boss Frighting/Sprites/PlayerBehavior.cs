using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

	private Animator _anim;
	public PlayerInputs daputs;
	void Start() {
		_anim = transform.parent.gameObject.GetComponent<Animator>();
	}

	void Update() {
		if (daputs.inputDirection != PlayerInputs.Direction.Neutral) {
			_anim.Play(daputs.inputDirection.ToString());
			Debug.Log(daputs.inputDirection.ToString());
			daputs.inputDirection = PlayerInputs.Direction.Neutral;
		}
	}
}
