using UnityEngine;
using System.Collections;

public class Teste_ControladorFogo : MonoBehaviour {

	public float tempoComecarFogo; // Delay até começar o incêndio
	public GameObject Fogo_Fogao_1;
	public GameObject Fogo_Fogao_2;
	public ParticleSystem Fogo_Fogao_1_Inicio;
	public ParticleSystem Fogo_Fogao_2_Inicio;

	bool podeComecar;

	// Use this for initialization
	void Start () 
	{
		Invoke ("Fogo", tempoComecarFogo);
	}
	
	// Update is called once per frame
	void Update () 
	{
			
	}

	void Fogo()
	{
		Fogo_Fogao_1_Inicio.Stop ();
		Fogo_Fogao_2_Inicio.Stop ();

		Fogo_Fogao_1.GetComponentInChildren<ParticleSystem>().Play ();
		Fogo_Fogao_2.GetComponentInChildren<ParticleSystem>().Play ();
			
		if (Fogo_Fogao_1.GetComponent<Fogo_VFX>().aumentaEscala && Fogo_Fogao_2.GetComponent<Fogo_VFX>().aumentaEscala)
		{
			Fogo_Fogao_1.GetComponent<Fogo_VFX> ().podeAumentar = true;
			Fogo_Fogao_2.GetComponent<Fogo_VFX> ().podeAumentar = true;
		}
	}
}
