using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour {

	public GameObject 				pPos, p, startButton, cancelButton, colorButton;
	public GameObject[]				listaPersonagens, listaPaletas;
	public Material[]				alienMaterials, astronautaMaterials, galinhaMaterials,  pedroMaterials, zumbiMaterials;
	public SkinnedMeshRenderer[]	personagemMaterial;
	public int 						nListaPersonagens = 3, nMaterial, status;
	public string					horizontal, a, b;

	private float timer = 0, timerCall = .2f;
	private bool once = false;


	//Status 0 = Sem seleção; Status 1 = Seleciona personagem; Status 2 = Seleciona paleta

	// Use this for initialization
	void Start () {
		status = 0;
		CriaPersonagemLobby();
//		for(int i = 0 ; i < 4; i++) {
//			jogadoresMaterials[i] = p.GetComponentInChildren<SkinnedMeshRenderer>();
//			jogadoresMaterials [i].material = baseMaterials[i];
//		}
	}

	void FixedUpdate() {
		if(Input.GetAxisRaw(a) > 0.1f && !once && status < 2) {
			once = true;
			status++;
		}

		if(Input.GetAxisRaw(b) > 0.1f && !once && status > 0) {
			once = true;
			status--;
		}

		switch (status) {
		case 0:
			startButton.SetActive(true);
			cancelButton.SetActive(false);
			colorButton.SetActive(false);
			break;
		case 1:
			startButton.SetActive(false);
			cancelButton.SetActive(true);
			colorButton.SetActive(true);
			break;
		case 2:
			startButton.SetActive(true);
			cancelButton.SetActive(true);
			colorButton.SetActive(false);
			break;
		}

		CdInicial ();
		if(status > 0) {
			if(Input.GetAxisRaw(horizontal) < -0.1f && !once && timer > 2.8f) {
				once = true;
				if(status == 1) {
					MudaPersonagemEsquerda();
					nMaterial = 0;
				} else if(status == 2) {
					MudaPaletaEsquerda(nListaPersonagens);
				}
			} else if(Input.GetAxisRaw(horizontal) > 0.1f && !once && timer > 2.8f) {
				once = true;
				if(status == 1) {
					MudaPersonagemDireita();
					nMaterial = 0;
				} else if(status == 2) {
					MudaPaletaDireita(nListaPersonagens);
				}
			}
		}

		Once();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void Once () {

		if(once) {
			timerCall -= Time.deltaTime;
		}

		if(timerCall < 0) {
			once = false;
			timerCall = .2f;
		}
	}

	void MudaPersonagemEsquerda () {
		Destroy(p.gameObject);
		nListaPersonagens = nListaPersonagens - 1;
		if(nListaPersonagens < 0) {
			nListaPersonagens = 5;
		}
		p = Instantiate(listaPersonagens[nListaPersonagens].gameObject, pPos.transform.position, pPos.transform.rotation);
		EscolhePaleta(nListaPersonagens);
	}

	void MudaPersonagemDireita () {
		Destroy(p.gameObject);
		nListaPersonagens = nListaPersonagens + 1;
		if(nListaPersonagens > 5) {
			nListaPersonagens = 0;
		}
		p = Instantiate(listaPersonagens[nListaPersonagens].gameObject, pPos.transform.position, pPos.transform.rotation);
		EscolhePaleta(nListaPersonagens);
	}

	void MudaPaletaEsquerda (int i) {
		switch(i) {
		case 0:
			break;
		case 1:
			break;
		case 2:
			break;
		case 3:
			break;
		case 4:
			break;
		case 5:
			break;
		}
	}

	void MudaPaletaDireita (int i) {

	}

	void EscolhePaleta(int i) {
		for(int j = 0; j < 6; j++) {
			if(j == i) {
				listaPaletas[j].SetActive(true);
			} else {
				listaPaletas[j].SetActive(false);
			}
		}
	}

	void CriaPersonagemLobby () {
		p = Instantiate(listaPersonagens[3].gameObject, pPos.transform.position, pPos.transform.rotation);
	}

	void CdInicial () {
		if(timer < 2.8f) {
			timer += Time.deltaTime;
			p.transform.position = pPos.transform.position;
		}
	}
}
