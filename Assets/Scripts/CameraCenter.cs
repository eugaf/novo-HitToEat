using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraCenter : MonoBehaviour {

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
		Zoom();
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

		center.x = center.x / playerLista.Length;
		center.y = center.y / playerLista.Length;
		center.z = center.z / playerLista.Length;
		float distancia = -center.magnitude;

		//Verificar os limites da camera
		if (distancia < zoomMinimo) {
			distancia = zoomMinimo;
		}

		if (distancia > zoomMaximo) {
			distancia = zoomMaximo;
		}

		center = center - transform.forward * distancia;
//		center.z = center.z*2;
		center.y = transform.position.y;

		novaPosicao = center;
	}

	void Zoom() {
		float requiredSize = EncontrarTamanhoNecessario();
		Vector3 newPosition = Camera.main.transform.position;
		newPosition.y = requiredSize;
		Camera.main.transform.position = newPosition;
	}

	float EncontrarTamanhoNecessario() {
		// Find the position the camera rig is moving towards in its local space.
		Vector3 desiredLocalPos = transform.InverseTransformPoint(novaPosicao);

		// Start the camera's size calculation at zero.
		float size = 0f;

		// Go through all the targets...
		for (int i = 0; i < playerLista.Length; i++) {
			// Otherwise, find the position of the target in the camera's local space.
			Vector3 targetLocalPos = transform.InverseTransformPoint(playerLista[i].transform.position);

			// Find the position of the target from the desired position of the camera's local space.
			Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

			// Choose the largest out of the current size and the distance of the tank 'up' or 'down' from the camera.
			size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

			// Choose the largest out of the current size and the calculated size based on the tank being to the left or right of the camera.
			size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / Camera.main.aspect);
		}

		// Add the edge buffer to the size.
		size += 9f;

		// Make sure the camera's size isn't below the minimum.
		size = Mathf.Max (size, zoomMinimo);

		return size;
	}
}
