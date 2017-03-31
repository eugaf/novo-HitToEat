using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Laser : MonoBehaviour {

	public bool podeDisparar;
	public bool danoAtivado;
	public float startWidth;
	public float endWidth;
	public float multiplicadorWidth;
	public float alphaLaser;

	public LineRenderer Linha; //Laser

	void Start () {
		Linha = GetComponent<LineRenderer>();
		Linha.enabled = false;
		podeDisparar = false;
		danoAtivado = false;
	}

	void Update () {
		if (podeDisparar) { //Ao apertar botão aciona a função
	//		StopCoroutine("DispararLaser");
			StartCoroutine("DispararLaser");
		}
	}

	public IEnumerator DispararLaser(float tempoLaser) { //Função de disparar o laser 
		//Iniciar disparo do laser
		//Disparar Laser
		//Controlar o Tempo
		//Parar Laser quando o tempo acabar
		//TODO: Aumentar tempo do laser


		/*
		 * Switch Geral:
		  Carregar Laser: Gera o RayCast, mostra o material com alpha, aumenta a escala
		  Disparar Laser: material fica opaco, Verifica se detectou colisao durante X tempo
		  Parar Laser: desativa o dano, desativa o laser, reseta as informacoes
		*/
		if (!Linha.enabled) {
			Linha.enabled = true;

			Ray ray = new Ray (transform.position, transform.forward);
			RaycastHit hit;

			while (tempoLaser > 0) { //Enquanto podeDisparar for TRUE
				Linha.SetPosition (0, ray.origin); //Onde começa o laser
				Linha.SetWidth(startWidth, endWidth);
				Linha.GetComponent<Renderer>().material.SetColor ("_TintColor", new Color (1, 0, 0, alphaLaser)); // Muda o alpha do material do laser
				if (startWidth <= 0.25f) {
					startWidth += Time.deltaTime * multiplicadorWidth;
					endWidth = startWidth;
					alphaLaser += Time.deltaTime * multiplicadorWidth;
				}
				else {
					if (danoAtivado == false) {
						startWidth = 0.5f;
						endWidth = startWidth;
						alphaLaser = 1f;
						danoAtivado = true;
					}

					if (startWidth >= 0.3) {
						startWidth -= Time.deltaTime * multiplicadorWidth * 0.5f;
						endWidth = startWidth;
					}
				}
				if (Physics.Raycast (ray, out hit, 50)) {
					Linha.SetPosition (1, hit.point); //Laser para ao colidir com algo
					if (hit.transform.tag == "Player" && danoAtivado) {
						hit.transform.GetComponent<Jogador>().StartCoroutine(hit.transform.GetComponent<Jogador>().LaserTorrar(tempoLaser));
					}
				}
				else  {
					Linha.SetPosition (1, ray.GetPoint(50)); //Distância máxima do laser
				}

				yield return null;
				tempoLaser -= 1 * Time.deltaTime;
			}
		}

		danoAtivado = false;
		alphaLaser = 0;
		startWidth = 0;
		endWidth = startWidth;
		Linha.enabled = false;
//		GameController.singleton.StartCoroutine(GameController.singleton.LaserLigar());
	}
}
