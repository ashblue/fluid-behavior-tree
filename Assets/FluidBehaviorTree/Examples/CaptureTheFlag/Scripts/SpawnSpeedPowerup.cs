using System.Collections;
using UnityEngine;

namespace Adnc.FluidBT.Examples {
    public class SpawnSpeedPowerup : MonoBehaviour {
        public GameObject speedBoostPrefab;
        
        void Start () {
            StartCoroutine(SpawnPowerup());
        }

        IEnumerator SpawnPowerup () {
            while (true) {
                yield return new WaitForSeconds(Random.Range(3, 20));

                if (FlagManager.current.speedBoost == null) {
                    FlagManager.current.speedBoost = Instantiate(
                        speedBoostPrefab, transform.position, Quaternion.identity, transform);
                }
            }
        }
    }
}