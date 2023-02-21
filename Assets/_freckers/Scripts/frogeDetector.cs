using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freckers
{
	public class frogeDetector : MonoBehaviour
	{
		public bool IsOnDifferentFrog {
			get { 
				int frogeCollideCount = 0;
				List<Collider2D> overlaps = new List<Collider2D>();
				GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D().NoFilter(), overlaps);
				foreach(Collider2D check in overlaps){
					Froge froge = check.gameObject.GetComponent<Froge>();
					if (froge == null) {
						continue;
					}
					if (!GameManager.Instance.IsMyTurn(froge))
					{
						frogeCollideCount++;
					}
				}
				return  frogeCollideCount > 0; 
			}
		}
	}
}

