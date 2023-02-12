using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : MonoBehaviour {

	public GameObject frog;
	public GameObject frogsBody;
	SkinnedMeshRenderer skinnedMeshRenderer;

	Animator anim;

	public Material blue;
	public Material balckOnRedSpot;
	public Material orangeBlackBlue;
	public Material redGreenBlack;
	public Material yellow;
	public Material yellowOnBlack;

	private void Awake() {
		anim = frog.GetComponent<Animator>();
		skinnedMeshRenderer = frogsBody.GetComponent<SkinnedMeshRenderer>();
	}


	public void Idle() {
		RootMotion();
		anim.SetTrigger("Idle");
	}

	public void Jump() {
		RootMotion();
		anim.SetTrigger("Jump");
	}

	public void Crawl() {
		RootMotion();
		anim.SetTrigger("Crawl");
	}

	public void Tongue() {
		RootMotion();
		anim.SetTrigger("Tongue");
	}

	public void Swim() {
		RootMotion();
		anim.SetTrigger("Swim");
	}

	public void Smashed() {
		RootMotion();
		anim.SetTrigger("Smashed");
	}

	public void TurnLeft() {
		anim.applyRootMotion = true;
		anim.SetTrigger("TurnLeft");
	}

	public void TurnRight() {
		anim.applyRootMotion = true;
		anim.SetTrigger("TurnRight");
	}

	void RootMotion() {
		if (anim.applyRootMotion) {
			anim.applyRootMotion = false;
		}
	}

	public void Blue() {
		skinnedMeshRenderer.material = blue;
	}
	public void BalckOnRedSpot() {
		skinnedMeshRenderer.material = balckOnRedSpot;
	}
	public void OrangeBlackBlue() {
		skinnedMeshRenderer.material = orangeBlackBlue;
	}
	public void RedGreenBlack() {
		skinnedMeshRenderer.material = redGreenBlack;
	}
	public void Yellow() {
		skinnedMeshRenderer.material = yellow;
	}
	public void YellowOnBlack() {
		skinnedMeshRenderer.material = yellowOnBlack;
	}

}