using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freckers
{
    public class KingZoneScript : MonoBehaviour
    {
        public uint teamId;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Froge froge = collision.GetComponent<Froge>();
            if (froge == null)
            {
                return;
            }

            if (froge.teamId != teamId) {
                froge.king = true;
                froge.crown.SetActive(true);
            }
        }
    }

}
