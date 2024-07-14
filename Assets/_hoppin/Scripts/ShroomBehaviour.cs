using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomBehaviour : MonoBehaviour
{
	public float spinSpeed;
	public float bobSpeed;
	public float bobAmount;
	public float randomOffset;
	public bool randBool;
	public static int shroomCount;
	public GameObject model;
	public GameObject getParticles;
	private bool hasBeenPickedUp = false;
	private Vector3 lockedPos;
	// Start is called before the first frame update
	void Start()
	{
		if (Random.Range(1, 7) < 6)
		{
			randBool = false;
		}
		else
		{
			randBool = true;
		}

		// SpriteRenderer[] spriteRenderers = gameObject.GetComponents<SpriteRenderer>();
		// foreach (var r in spriteRenderers)
		// {
		// 	r.enabled = randBool;
		// }
		model.SetActive(randBool);
		
		BoxCollider[] boxColliders = gameObject.GetComponents<BoxCollider>();
		foreach (var b in boxColliders)
		{
			b.enabled = randBool;
		}

		randomOffset = Random.Range(-1f, 1f);
		transform.position += new Vector3(randomOffset, 0);
		lockedPos = model.transform.localPosition;

	}

	// Update is called once per frame
	void Update()
	{
		model.transform.localPosition = lockedPos + Vector3.up * Mathf.Sin(Time.time * bobSpeed) * bobAmount;
		model.transform.localEulerAngles = model.transform.localEulerAngles + Vector3.up * Time.deltaTime * spinSpeed;
	}

	void OnTriggerEnter(Collider otherObject)
	{
		if (otherObject.gameObject.CompareTag("Player") && hasBeenPickedUp == false)
		{
			Debug.Log("Vibrate, shroom pickup");
			Vibration.VibratePeek();
			PlayerMoney.MONEY++;
			PlayerMoney.saveMoney();
			hasBeenPickedUp = true;
			PlayerPrefs.Save();
			Instantiate(getParticles, transform).transform.parent = null;
			Destroy(gameObject);
		}
	}
}
