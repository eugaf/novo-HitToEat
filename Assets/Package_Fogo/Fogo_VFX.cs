using UnityEngine;
using System.Collections;

public class Fogo_VFX : MonoBehaviour {

	ParticleSystem psFogo;
	ParticleSystem psFaisca;
	ParticleSystem psFumaca;

	public Vector3 MultiplicadorEscalaLocal; //Multiplicador para aumentar escala
	public Vector3 EscalaMaxima; //Escala máxima que o objeto poderá chegar

	public bool aumentaEixoX; //Para limitar no eixo X
	public bool aumentaEixoY; //Para limitar no eixo Y
	public bool aumentaEixoZ; //Para limitar no eixo Z
	public bool aumentaEscala; //Para verificar se o objeto aumenta ou não a escala
	public bool podeAumentar;

	public float delayFogo; //Tempo até o fogo ser ativado
	public float multiplicadorEmissoes; //Multiplicador para o numero de emissoes de particulas

	void Start()
	{
		psFogo = GetComponentInChildren <ParticleSystem> ();
		psFaisca = GetComponentInChildren <ParticleSystem> ().transform.GetChild (0).GetComponent<ParticleSystem> ();
		psFumaca = GetComponentInChildren <ParticleSystem> ().transform.GetChild (1).GetComponent<ParticleSystem> ();
		podeAumentar = false;

	}

	void Update()
	{
		/*if (Input.GetKeyDown(KeyCode.T))
		{
			psFogo.Play ();

			if (aumentaEscala)
			{
				podeAumentar = true;
			}
		}*/

		if (podeAumentar)
		{
			transform.localScale += MultiplicadorEscalaLocal * Time.deltaTime;

			var emission = psFogo.emission;
			var emission2 = psFumaca.emission;
			var emission3 = psFaisca.emission;

			var rate = emission.rate;
			var rate2 = emission2.rate;
			var rate3 = emission3.rate;

			rate.constantMax += multiplicadorEmissoes * Time.deltaTime;
			rate2.constantMax += multiplicadorEmissoes * Time.deltaTime;
			rate3.constantMax += (multiplicadorEmissoes * Time.deltaTime) / 5;

			emission.rate = rate;
			emission2.rate = rate2;
			emission3.rate = rate3;
		}
		if (aumentaEixoX) 
		{
			if (transform.localScale.x > EscalaMaxima.x) 
			{
				podeAumentar = false;
			}
		}
		if (aumentaEixoY) 
		{
			if (transform.localScale.y > EscalaMaxima.y) 
			{
				podeAumentar = false;
			}
		}
		if (aumentaEixoZ) 
		{
			if (transform.localScale.z > EscalaMaxima.z) 
			{
				podeAumentar = false;
			}
		}
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "Fogo") 
		{
			if (!psFogo.isPlaying) 
			{
				psFogo.Play ();
				
				if (aumentaEscala)
				{
					podeAumentar = true;
				}
			}
		}
	}
}
