using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour {

	public GameObject 				pPos, p, startButton, cancelButton, colorButton;
	public GameObject[]				listaPersonagens, listaPaletas;
	public Material[]				alienMaterials, astronautaMaterials, galinhaMaterials,  pedroMaterials, zumbiMaterials;
	public SkinnedMeshRenderer		personagemMaterial;
	public int 						nListaPersonagens = 3, nMaterial, status, nPlayer;
	public string					horizontal, a, b;

	private float 	timer = 0, timerCall = .2f;
	private bool 	once = false, cont = false;

	GameController gcScript;


	//Status 0 = Sem seleção; Status 1 = Seleciona personagem; Status 2 = Seleciona paleta

	void Awake () {

		for(int i = 0; i < listaPersonagens.Length; i++) {
			listaPersonagens[i].transform.localScale = new Vector3(.59f, .59f, .59f);
			Jogador jogadorScript = listaPersonagens[i].GetComponent<Jogador>();
			jogadorScript.enabled = false;
			Rigidbody rb = listaPersonagens[i].GetComponent<Rigidbody>();
			rb.useGravity = false;
			CapsuleCollider caps = listaPersonagens[i].GetComponent<CapsuleCollider>();
			caps.enabled = false;
			SkinnedMeshRenderer smr = listaPersonagens[i].GetComponentInChildren<SkinnedMeshRenderer>();
			switch(i) {
			case 0:
				smr.material = alienMaterials[nMaterial];
				break;
			case 1:
				smr.material = astronautaMaterials[nMaterial];
				break;
			case 2:
				smr.material = galinhaMaterials[nMaterial];
				break;
			case 3:
				smr.material = pedroMaterials[nMaterial];
				break;
			case 4:
				smr.material = zumbiMaterials[nMaterial];
				break;
			}
		}
	}

	// Use this for initialization
	void Start () {
		gcScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		CriaPersonagemLobby();
		status = 0;
//		for(int i = 0 ; i < 4; i++) {
//			jogadoresMaterials[i] = p.GetComponentInChildren<SkinnedMeshRenderer>();
//			jogadoresMaterials [i].material = baseMaterials[i];
//		}
	}

	void FixedUpdate() {
		if(Input.GetAxisRaw(a) > 0.1f && !once && status < 3) {
			once = true;
			status++;
			if(status >= 1 && !cont) {
				gcScript.nJogadores += 1;
				gcScript.jogadoresPrefab[nPlayer] = listaPersonagens[3].gameObject;
				cont = true;
			}
			if(status == 3) {
				gcScript.StartLevel();
			}
		}

		if(Input.GetAxisRaw(b) > 0.1f && !once && status > 0) {
			once = true;
			status--;
			if(status == 0 && cont) {
				gcScript.nJogadores -= 1;
				cont = false;
			}
		}

		switch (status) {
		case 0:
			startButton.SetActive(true);
			cancelButton.SetActive(false);
			colorButton.SetActive(false);
			break;
		case 1:
			personagemMaterial = p.GetComponentInChildren<SkinnedMeshRenderer>();
			startButton.SetActive(false);
			cancelButton.SetActive(true);
			colorButton.SetActive(true);
			break;
		case 2:
			personagemMaterial = p.GetComponentInChildren<SkinnedMeshRenderer>();
			startButton.SetActive(true);
			cancelButton.SetActive(true);
			colorButton.SetActive(false);
			break;
		}

		CdInicial ();
		MudarSelecao();
		Once();
	}

	void MudarSelecao () {
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
			nListaPersonagens = 4;
		}

		p = Instantiate(listaPersonagens[nListaPersonagens].gameObject, pPos.transform.position, pPos.transform.rotation);
		gcScript.jogadoresPrefab[nPlayer] = listaPersonagens[nListaPersonagens];
		gcScript.nPersonagens[nPlayer] = nListaPersonagens;
		ArrumaPaleta(nListaPersonagens);
		EscolhePaleta(nListaPersonagens);
	}

	void MudaPersonagemDireita () {
		Destroy(p.gameObject);
		nListaPersonagens = nListaPersonagens + 1;
		if(nListaPersonagens > 4) {
			nListaPersonagens = 0;
		}

		p = Instantiate(listaPersonagens[nListaPersonagens].gameObject, pPos.transform.position, pPos.transform.rotation);
		gcScript.jogadoresPrefab[nPlayer] = listaPersonagens[nListaPersonagens];
		gcScript.nPersonagens[nPlayer] = nListaPersonagens;
		ArrumaPaleta(nListaPersonagens);
		EscolhePaleta(nListaPersonagens);
	}

	void ArrumaPaleta (int i) {
		personagemMaterial = p.GetComponentInChildren<SkinnedMeshRenderer>();

		switch(i) {
		case 0:
			personagemMaterial.material = alienMaterials[nMaterial];
			gcScript.personagensMaterial[nPlayer] = personagemMaterial.material;
			break;
		case 1:
			personagemMaterial.material = astronautaMaterials[nMaterial];
			gcScript.personagensMaterial[nPlayer] = personagemMaterial.material;
			break;
		case 2:
			personagemMaterial.material = galinhaMaterials[nMaterial];
			gcScript.personagensMaterial[nPlayer] = personagemMaterial.material;
			break;
		case 3:
			personagemMaterial.material = pedroMaterials[nMaterial];
			gcScript.personagensMaterial[nPlayer] = personagemMaterial.material;
			break;
		case 4:
			personagemMaterial.material = zumbiMaterials[nMaterial];
			gcScript.personagensMaterial[nPlayer] = personagemMaterial.material;
			break;
		}

	}

	void MudaPaletaEsquerda (int i) {
		nMaterial -= 1;

		if(nMaterial < 0) {
			nMaterial = 3;
		} 

		switch(i) {
		case 0:
			personagemMaterial.material = alienMaterials[nMaterial];
			gcScript.personagensMaterial[nPlayer] = personagemMaterial.material;
			break;
		case 1:
			personagemMaterial.material = astronautaMaterials[nMaterial];
			gcScript.personagensMaterial[nPlayer] = personagemMaterial.material;
			break;
		case 2:
			personagemMaterial.material = galinhaMaterials[nMaterial];
			gcScript.personagensMaterial[nPlayer] = personagemMaterial.material;
			break;
		case 3:
			personagemMaterial.material = pedroMaterials[nMaterial];
			gcScript.personagensMaterial[nPlayer] = personagemMaterial.material;
			break;
		case 4:
			personagemMaterial.material = zumbiMaterials[nMaterial];
			gcScript.personagensMaterial[nPlayer] = personagemMaterial.material;
			break;
		}
	}

	void MudaPaletaDireita (int i) {
		nMaterial += 1;

		if(nMaterial > 3) {
			nMaterial = 0;
		} 

		switch(i) {
		case 0:
			personagemMaterial.material = alienMaterials[nMaterial];
			gcScript.personagensMaterial[nPlayer] = personagemMaterial.material;
			break;
		case 1:
			personagemMaterial.material = astronautaMaterials[nMaterial];
			gcScript.personagensMaterial[nPlayer] = personagemMaterial.material;
			break;
		case 2:
			personagemMaterial.material = galinhaMaterials[nMaterial];
			gcScript.personagensMaterial[nPlayer] = personagemMaterial.material;
			break;
		case 3:
			personagemMaterial.material = pedroMaterials[nMaterial];
			gcScript.personagensMaterial[nPlayer] = personagemMaterial.material;
			break;
		case 4:
			personagemMaterial.material = zumbiMaterials[nMaterial];
			gcScript.personagensMaterial[nPlayer] = personagemMaterial.material;
			break;
		}
	}

	void EscolhePaleta(int i) {
		for(int j = 0; j < 5; j++) {
			if(j == i) {
				listaPaletas[j].SetActive(true);
			} else {
				listaPaletas[j].SetActive(false);
			}
		}
	}

	void CriaPersonagemLobby () {
		p = Instantiate(listaPersonagens[3].gameObject, pPos.transform.position, pPos.transform.rotation);
		SkinnedMeshRenderer skr = p.GetComponentInChildren<SkinnedMeshRenderer>();
		skr.material = pedroMaterials[0];
		ArrumaPaleta(3);
		gcScript.nPersonagens[nPlayer] = nListaPersonagens;
//		gcScript.jogadoresPrefab[nPlayer] = listaPersonagens[3].gameObject;
	}

	void CdInicial () {
		if(timer < 2.8f) {
			timer += Time.deltaTime;
			p.transform.position = pPos.transform.position;
		}
	}
}
