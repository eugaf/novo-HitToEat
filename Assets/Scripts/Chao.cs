using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chao : MonoBehaviour {

	Jogador jogadorScript;

	void Start () {
		jogadorScript = GetComponentInParent<Jogador>();
	}

	void Update () {
	}

	void OnTriggerEnter (Collider other) {
		if(other.tag == "Chao" || other.tag == "Movivel") {
			jogadorScript.noChao = true;
			jogadorScript.puloPodePular = true;
			jogadorScript._animator.ResetTrigger("NoAr");
		}
	}

	void OnTriggerStay (Collider other) {
		name = other.tag;

		if(other.tag == "Chao" || other.tag == "Movivel") {
			jogadorScript.noChao = true;
			jogadorScript.puloPodePular = true;
			jogadorScript._animator.ResetTrigger("NoAr");
		}
	}

	void OnTriggerExit (Collider other) {
		if(other.tag == "Chao" || other.tag == "Movivel") {
			jogadorScript.noChao = false;
		}
	}
}
