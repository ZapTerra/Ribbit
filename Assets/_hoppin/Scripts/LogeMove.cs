using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogeMove : MonoBehaviour {
	public float maxDist = 10;
	public float maxRearDist = 10;
	public bool goRight;
	public float moveSpeed = 5;
	public float randomOffset;
	public GameObject loge;
	public GameObject polyLoge;
	public Transform frogePos;

	private GameObject logeCreated;
	private float diff;
	// Start is called before the first frame update
	void Start() {
		randomOffset = Random.Range(-1.5f, 1.5f);
		polyLoge.transform.eulerAngles = new Vector3(Random.Range(0, 360), 0, 0);
		//transform.position += Vector3.right * randomOffset;
		frogePos = GameObject.Find("frogeNew").GetComponent<Transform>();
		goRight = (transform.position.y % 6) == 0 ? true : false;
	}

	// Update is called once per frame
	void Update() {
		if (transform.position.y < frogePos.position.y - maxRearDist) {
			Destroy(gameObject);
		}

		diff = transform.position.x - randomOffset - frogePos.position.x;

		if ((goRight == true ? diff > maxDist : diff < -maxDist)) {
			logeCreated = Instantiate(loge);
			logeCreated.transform.position = new Vector3(frogePos.position.x - diff, transform.position.y, 0);
			logeCreated.GetComponent<LogeMove>().moveSpeed = moveSpeed;
			logeCreated.GetComponent<LogeMove>().loge = loge;
			Destroy(gameObject);
		}

		transform.position = transform.position + Vector3.right * moveSpeed * Time.deltaTime * (goRight == true ? 1 : -1);
	}
}