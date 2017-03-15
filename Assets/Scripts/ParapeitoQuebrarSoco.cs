using UnityEngine;
using System.Collections;

public class ParapeitoQuebrarSoco : MonoBehaviour {

	public GameObject VersaoQuebrada;
	public GameObject VersaoIntacta;

	public float MinimumForce = 2.0f;


	public void ParapeitoDestruir(){
		GetComponent<BoxCollider>().enabled = false;
		VersaoIntacta.SetActive(false);
		VersaoQuebrada.SetActive(true);

	}

	void OnCollisionEnter ( Collision other ){
		Rigidbody Rb = other.transform.gameObject.GetComponent<Rigidbody>();

		if(Rb.velocity.x > MinimumForce || Rb.velocity.y > MinimumForce || Rb.velocity.z > MinimumForce || Rb.velocity.x < -MinimumForce || Rb.velocity.y < -MinimumForce || Rb.velocity.z < -MinimumForce){
			ParapeitoDestruir();
		}
	}
}