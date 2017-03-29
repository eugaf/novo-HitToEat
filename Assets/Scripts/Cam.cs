using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cam : MonoBehaviour {

	public GameObject[] playerLista;
	Vector3 midVector;
	public float zoomMinimo;
	public float zoomMaximo;
	public float zoomVelocidade;

	private Vector3 novaPosicao;

	void Start () {
		playerLista = GameObject.FindGameObjectsWithTag("Player");
	}

	void FixedUpdate() {
		Mover();
	}

	void Mover() {
		EncontrarPosicaoMedia();
		transform.position = Vector3.Slerp(transform.position, novaPosicao, zoomVelocidade);

	}

	void EncontrarPosicaoMedia() {
		Vector3 center = new Vector3();

		foreach (GameObject player in playerLista) {
			center += player.transform.position;
		}

		center = center / playerLista.Length;
		float distancia = -center.magnitude;

		//Verificar os limites da camera
		if (distancia < zoomMinimo) {
			distancia = zoomMinimo;
		}

		if (distancia > zoomMaximo) {
			distancia = zoomMaximo;
		}

		center = center - transform.forward * distancia;
		center.y = transform.position.y;

		novaPosicao = center;
	}
}
