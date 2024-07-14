using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freckers
{
    public class KingZoneScript : MonoBehaviour
    {
        public List<uint> excludeTeams;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Froge froge = collision.GetComponent<Froge>();
            if (froge == null)
            {
                return;
            }

            if (!excludeTeams.Contains(froge.teamId)) {
                froge.king = true;
                froge.crown.SetActive(true);
            }
        }
    }

}
