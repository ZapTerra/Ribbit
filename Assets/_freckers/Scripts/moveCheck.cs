using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freckers
{
	public class moveCheck : MonoBehaviour
	{
		public GameObject idiotChild;
		public bool isOnDifferentFrog = false;
		private int frogeCollideCount = 0;

		void Update()
		{
			if (frogeCollideCount > 0)
			{
				isOnDifferentFrog = true;
				GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.647f);
				idiotChild.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.647f);
			}
			else
			{
				isOnDifferentFrog = false;
				GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f, 0.647f);
				idiotChild.GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f, 0.647f);
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag("frineldlyFroge"))
			{
				frogeCollideCount++;
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.CompareTag("frineldlyFroge"))
			{
				frogeCollideCount--;
			}
		}

		private void OnDisable()
		{
			frogeCollideCount = 0;
		}
	}
}
