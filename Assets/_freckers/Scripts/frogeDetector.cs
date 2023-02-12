using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freckers
{
	public class frogeDetector : MonoBehaviour
	{
		public MoveClickPoints moveClickPoints;
		public bool IsOnDifferentFrog { get { return frogeCollideCount > 0; } }
		public static int frogeCollideCount = 0;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			Froge froge = collision.gameObject.GetComponent<Froge>();
			if (froge == null) {
				return;
			}

			if (!GameManager.Instance.IsMyTurn(froge))
			{
				frogeCollideCount++;
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			Froge froge = collision.gameObject.GetComponent<Froge>();
			if (froge == null)
			{
				return;
			}

			if (!GameManager.Instance.IsMyTurn(froge))
			{
				frogeCollideCount--;
			}
		}
	}
}

