using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomBehaviour : MonoBehaviour
{
	public float randomOffset;
	public bool randBool;
	public static int shroomCount;
	private bool hasBeenPickedUp = false;
	private Vector3 lockedPos;
	// Start is called before the first frame update
	void Start()
	{
		if (Random.Range(1, 13) < 12)
		{
			randBool = false;
		}
		else
		{
			randBool = true;
		}

		SpriteRenderer[] spriteRenderers = gameObject.GetComponents<SpriteRenderer>();
		foreach (var r in spriteRenderers)
		{
			r.enabled = randBool;
		}
		BoxCollider[] boxColliders = gameObject.GetComponents<BoxCollider>();
		foreach (var b in boxColliders)
		{
			b.enabled = randBool;
		}

		randomOffset = Random.Range(-1f, 1f);
		transform.position += new Vector3(randomOffset, 0);
		lockedPos = transform.localPosition;

	}

	// Update is called once per frame
	void Update()
	{
		transform.localPosition = lockedPos;
	}

	void OnTriggerEnter(Collider otherObject)
	{
		if (otherObject.gameObject.CompareTag("Player") && hasBeenPickedUp == false)
		{
			Debug.Log("Vibrate, shroom pickup");
			Vibration.VibratePop();
			PlayerMoney.MONEY++;
			PlayerMoney.saveMoney();
			hasBeenPickedUp = true;
			PlayerPrefs.Save();
			Destroy(gameObject);
		}
	}
}
