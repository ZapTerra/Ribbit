using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freckers {
	public class MoveClickPoints : MonoBehaviour {
		public Collider2D freckerBoardZoning;
		public GameObject targetFroge;
		public GameObject dragGraphic1;
		public GameObject dragGraphic2;
		public GameObject dragGraphicCenter;
		public GameObject dragGraphicF;
		public GameObject frogeDetector;
		public GameObject gameCamera;
		private Vector2 clickPos1;
		private Vector2 clickPos2;
		private Vector2 moveDir;
		private Vector2 fPos;
		private Vector3 dragRotation;
		private Vector3 cameraPositionSave = new Vector3(0, 0, -10);
		private Vector3 targetFrogePosition;
		private Vector3 cameraPos;
		public float maxMoveDistance = 2f;
		public float distToNoticeCast = .8f;
		public float timeToDisableDragF = 1f;
		public float maxDragAngle;
		private float changeX;
		private float changeY;
		private float clickDistanceForCast;
		private bool canHasCastOnRelease = false;

		private float clickDistanceForMove;

		void Update() {
			//If frog disappears for any reason
			if(canHasCastOnRelease && targetFroge == null){
				clickDistanceForCast = 0;
				canHasCastOnRelease = false;
			}

			cameraPos = gameCamera.transform.position;
			if (Input.GetMouseButtonDown(0)) {
				Collider2D[] hits = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
				Froge froge = null;
				foreach(Collider2D hit in hits){
					froge = hit?.GetComponent<Froge>();
					if(froge != null){
						break;
					}
				}
				//Yes, this is messy. No, I do not care enough to clean it up and make sure there aren't bugs.
				if (froge != null) {
					if (froge.state != Froge.State.Jumping && GameManager.Instance.IsMyTurn(froge) && !GameManager.Instance.extraMovesInProgress || froge.jumpAgainCount > 0) {
						cameraPositionSave = cameraPos;
						targetFroge = froge.gameObject;
						targetFrogePosition = targetFroge.transform.position;
						Debug.Log(targetFroge.GetComponent<Froge>().king);
						maxDragAngle = targetFroge.GetComponent<Froge>().king ? 180 : 45;
						clickPos1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
						dragGraphic1.SetActive(true);
						dragGraphic1.transform.position = clickPos1;
						dragGraphic2.SetActive(true);
						dragGraphicCenter.SetActive(true);
						dragGraphicF.SetActive(true);
						frogeDetector.SetActive(true);
						canHasCastOnRelease = true;
					}
				}
			}

			if (cameraPos != cameraPositionSave && !canHasCastOnRelease) {
				gameCamera.transform.position += (cameraPositionSave - cameraPos) * .25f;
			}

			if (canHasCastOnRelease) {
				clickPos2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				dragRotation = -targetFroge.transform.up;
				moveDir = ClampMagnitudeSquare((clickPos2 - clickPos1), maxMoveDistance, targetFroge.transform.eulerAngles.z);

				if (Vector3.Angle(dragRotation, moveDir) > maxDragAngle) {
					// Clamp rotation to max in direction of mouse around a circle.
					moveDir = Quaternion.Euler(0, 0, Vector2.SignedAngle(dragRotation, moveDir) >= 0 ? maxDragAngle : -maxDragAngle) * dragRotation * Mathf.Clamp((clickPos2 - clickPos1).magnitude, 0, Mathf.Sqrt(maxMoveDistance * 2));
				}

				clickDistanceForCast = moveDir.magnitude;

				dragGraphic2.transform.position = clickPos1 + moveDir;


				fPos = (targetFrogePosition * Vector2.one - moveDir);
				//frogeDetector.transform.position = new Vector2(Mathf.Clamp(fPos.x, -3.5f, 3.5f), fPos.y);
				if(freckerBoardZoning.OverlapPoint(fPos)){
					frogeDetector.transform.position = fPos;
				}else{
					frogeDetector.transform.position = freckerBoardZoning.ClosestPoint(fPos);
				}

				fPos -= moveDir * (frogeDetector.GetComponent<frogeDetector>().IsOnDifferentFrog ? 1 : 0);
				//dragGraphicF.transform.position = new Vector2(Mathf.Clamp(fPos.x, -3.5f, 3.5f), Mathf.Clamp(fPos.y, -3.5f, 3.5f));
				if(freckerBoardZoning.OverlapPoint(fPos)){
					dragGraphicF.transform.position = fPos;
				}else{
					dragGraphicF.transform.position = freckerBoardZoning.ClosestPoint(fPos);
				}

				//move camera dynamically
				var cameraTarget = (targetFrogePosition * Vector2.one - moveDir);
				cameraTarget = new Vector2(Mathf.Clamp(cameraTarget.x, -3.5f, 3.5f), Mathf.Clamp(cameraTarget.y, -3.5f, 3.5f));
				cameraTarget -= (Vector2)targetFrogePosition;
				cameraTarget = -cameraTarget * .6f;
				if (Mathf.Abs((targetFrogePosition + (Vector3)cameraTarget).x) > Mathf.Abs(targetFrogePosition.x) || Mathf.Abs(cameraTarget.x) < Mathf.Abs(cameraPos.x)) {
					changeX = (cameraTarget - (Vector2)cameraPos).x * .1f;
				} else {
					changeX = 0;
				}
				if (Mathf.Abs((targetFrogePosition + (Vector3)cameraTarget).y) > Mathf.Abs(targetFrogePosition.y) || Mathf.Abs(cameraTarget.y) < Mathf.Abs(cameraPos.y)) {
					changeY = (cameraTarget - (Vector2)cameraPos).y * .1f;
				} else {
					changeY = 0;
				}
				gameCamera.transform.position += new Vector3(changeX, changeY);
				//lol
				//cameraPositionSave + ((Vector3)cameraTarget - cameraPositionSave) * (((((Vector3)cameraTarget - cameraPos).magnitude) / ((Vector3)cameraTarget - cameraPositionSave).magnitude) + .1f);

				dragGraphicCenter.transform.position = (dragGraphic1.transform.position + dragGraphic2.transform.position) / 2;
				dragGraphicCenter.transform.localScale = new Vector3(dragGraphicCenter.transform.localScale.x, moveDir.magnitude, 1);
				dragGraphicCenter.transform.up = moveDir.normalized;
			} else if (dragGraphic1.activeInHierarchy) {
				dragGraphic1.SetActive(false);
				dragGraphic2.SetActive(false);
				dragGraphicCenter.SetActive(false);
				frogeDetector.SetActive(false);
				//was not necessary but is now because of explosions
				dragGraphicF.SetActive(false);
			}

			if (!Input.GetMouseButton(0)) {
				if (canHasCastOnRelease && clickDistanceForCast > distToNoticeCast && !dragGraphicF.GetComponent<moveCheck>().isOnDifferentFrog) {
					Debug.Log("The game starts and the buttons are padlocked from here");
					if(!GameManager.Instance.gameHasBegun){
						GameManager.Instance.StartGame();
					}
					targetFroge.GetComponent<Froge>().jumpDist = clickDistanceForCast * (frogeDetector.GetComponent<frogeDetector>().IsOnDifferentFrog ? 2 : 1);
					targetFroge.transform.up = -moveDir.normalized;
				} else if (clickDistanceForCast < distToNoticeCast) {
					dragGraphicF.SetActive(false);
				}
				clickDistanceForCast = 0;
				canHasCastOnRelease = false;
			}
		}

		Vector2 ClampMagnitudeSquare(Vector2 vector, float squareLength, float heading) {
			// Rotate the input vector by the frog's heading (rotation)
			Vector2 clampVector = Rotate(vector, -heading * Mathf.Deg2Rad);

			if (Mathf.Abs(clampVector.x) < squareLength && Mathf.Abs(clampVector.y) < squareLength) {
				// If the rotated vector is within the square, return it as is
				return vector;
			}

			if (Mathf.Abs(clampVector.x) > Mathf.Abs(clampVector.y)) {
				clampVector.y = (Mathf.Abs(clampVector.y) / Mathf.Abs(clampVector.x)) * Mathf.Sign(clampVector.y) * squareLength;
				clampVector.x = squareLength * Mathf.Sign(clampVector.x);
			} else {
				clampVector.x = (Mathf.Abs(clampVector.x) / Mathf.Abs(clampVector.y)) * Mathf.Sign(clampVector.x) * squareLength;
				clampVector.y = squareLength * Mathf.Sign(clampVector.y);
			}

			// Rotate the vector back to the world space
			clampVector = Rotate(clampVector, heading * Mathf.Deg2Rad);

			return clampVector;
		}

		private Vector2 Rotate(Vector2 v, float delta) {
			return new Vector2(
				v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
				v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
			);
		}
	}
}
