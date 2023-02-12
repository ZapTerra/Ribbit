using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class castClicks : MonoBehaviour {
	public Vector2 clickPos1;
	public Vector2 clickPos2;
	public Vector2 castDir;
	public float frogeRotation;
	public float clickDistanceForCast;
	public float castDistPercentage;
	public float rodRotation;
	public bool canHasCastOnRelease = false;

	private float maxCastDragDistance;
	// Start is called before the first frame update
	void Start() {
		maxCastDragDistance = Screen.width < Screen.height ? Screen.width : Screen.height / 2;
	}

	// Update is called once per frame
	void Update() {
		if (!panelOpener.paused) {
			if (Input.GetMouseButtonDown(0)) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

				if (hit.collider != null && hit.collider.transform == transform) {
					clickPos1 = Input.mousePosition;
					canHasCastOnRelease = true;
				}
			}

			if (canHasCastOnRelease == true) {
				clickPos2 = Input.mousePosition;
				clickDistanceForCast = Mathf.Clamp(Vector2.Distance(clickPos1, clickPos2), 0, maxCastDragDistance);
				rodRotation = clickDistanceForCast * -1;
				if (clickDistanceForCast > 10) {
					castDir = (clickPos2 - clickPos1).normalized;
					frogeRotation = Mathf.Atan2(castDir.y, castDir.x) * Mathf.Rad2Deg + 90f;
				}
			}

			if (Input.GetMouseButtonUp(0)) {
				rodRotation = 0;
				canHasCastOnRelease = false;
				castDistPercentage = clickDistanceForCast * (100 / maxCastDragDistance);
			}
		}
	}
}
