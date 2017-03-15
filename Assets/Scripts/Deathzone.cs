using UnityEngine;
using System.Collections;

public class Deathzone : MonoBehaviour {
	void OnTriggerEnter(Collider other){
		if (other.tag == "Player") {
			other.transform.position = Vector3.zero;
		}
	}
}
