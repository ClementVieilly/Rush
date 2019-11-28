///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 28/11/2019 16:27
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Rush {
	public class Field : MonoBehaviour {
	
		private void Start () {
		}
        private void Update() {
            transform.Rotate(Vector3.up, 50 * Time.deltaTime);

        }

    }
}