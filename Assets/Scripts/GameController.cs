using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

[SerializeField]
public class GameController : MonoBehaviour {

//	public static GameController singleton;

    //LASERS
	public GameObject[] lasersLista;
	public float 	laserEsperaDisparo, laserTempo;
	public bool  	laserPodeLigar;

    //JOGADORES
	public GameObject[] 			HUDVidas, jogadoresPrefab, p;
	public GameObject				mensagem;
    public List<GameObject>			jogadoresLista;
	public SkinnedMeshRenderer[]	personagensSMR;
	public Material[]				personagensMaterial;
	public int						nJogadores, prevLevel;
	public int[]					nPersonagens;

	public string[] inputP1, inputP2, inputP3, inputP4;

	public GameObject[] respawns;

	CameraCenter cam;
	Trigger trig;

    public bool gameOn = false;

	string name;

	float timer = 2f;
	public bool timerOn = false;
	void Awake () {
		//Gerenciamento da instancia do GameController
//		if (singleton == null) {
//			singleton = this;
//		}
//		else if (singleton != this) {
//			Destroy (gameObject);
//		}
		DontDestroyOnLoad(this);
	}

	void Start () {
		name = SceneManager.GetActiveScene().name;
//		
//		if(name != "Lobby" && name != "Menu") {
//			ChecarCena();
//		}
    }

	void FixedUpdate() {
		//		Invoke("ChecarCena", .1f);
		if(timerOn) {
			timer -= Time.deltaTime;
		}

		if(gameOn) {
			cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraCenter>();
			cam.AchaJogadores();
			for(int i = 0; i < 4; i++) {
				if(p[i] != null) {
					Jogador jogadorScript = p[i].GetComponent<Jogador>();
					HUD hudScript = HUDVidas[i].GetComponent<HUD>();
					Image vidas = hudScript.vidas.GetComponent<Image>();
					vidas.sprite = hudScript.vidasImg[jogadorScript.vidas];
				
					if(jogadorScript.vidas < 1) {
						Destroy(p[i].gameObject);
						p[i] = null;
						nJogadores--;
					}
				}
			}

			if(nJogadores < 2) {
				for(int i = 0; i < 4; i++) {
					if(p[i] != null) {
						Jogador jogadorScript = p[i].GetComponent<Jogador>();
						trig = GameObject.FindGameObjectWithTag("Trigger").GetComponent<Trigger>();
						int numero = i + 1;
						trig.Fim(numero);
					}
				}

				if(Input.anyKey && timer < 0) {
					prevLevel = 1;
					SceneManager.LoadScene(2);
					nJogadores = 0;
					gameOn = false;
					timerOn = false;
					timer = 2f;
				}
			}
		}

//		HUDVidas = GameObject.FindGameObjectsWithTag("HUD").OrderBy(go => go.name).ToArray();
	}

	public void StartLevel() {
		prevLevel = 0;
		SceneManager.LoadScene(2);
	}

	public void ChecarCena() {
		string name = SceneManager.GetActiveScene().name;
//      ChecarJogadores();
		CriaPersonagens();
		if (name == "Cozinha" || name == "Banheiro" || name == "Quadra" || name == "Arena") {
            lasersLista = GameObject.FindGameObjectsWithTag("Laser");
			StartCoroutine(LaserLigar());
        }
    }

	void CriaPersonagens() {
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraCenter>();
		respawns = GameObject.FindGameObjectsWithTag("Respawn");
//		mensagem = GameObject.FindGameObjectWithTag("Mensagem");


		HUDVidas = GameObject.FindGameObjectsWithTag("HUD").OrderBy(go => go.name).ToArray();
		for(int i = nJogadores; i < HUDVidas.Length; i++) {
			HUDVidas[i].SetActive(false);
		}

		for(int i = 0; i < nJogadores; i++) {
			personagensSMR[i] = jogadoresPrefab[i].GetComponentInChildren<SkinnedMeshRenderer>();
			personagensSMR[i].material = personagensMaterial[i];

			GameObject local = respawns[Random.Range(0, respawns.Length)];
			p[i] = Instantiate (jogadoresPrefab[i].gameObject, local.transform.position, local.transform.rotation);
			p[i].tag = "Player";
			p[i].transform.localScale = new Vector3(.3f, .3f, .3f);

			Jogador jogadorScript = p[i].GetComponent<Jogador>();
			jogadorScript.nJogador = i+1;
			jogadorScript.enabled = true;
			CapsuleCollider caps = p[i].GetComponent<CapsuleCollider>();
			caps.enabled = true;
			Rigidbody rb = p[i].GetComponent<Rigidbody>();
			rb.useGravity = true;

			switch(i) {
			case 0:
				for(int j = 0; j < 5; j++) {
					jogadorScript.axisJogadorVertical = inputP1[0];
					jogadorScript.axisJogadorHorizontal = inputP1[1];
					jogadorScript.axisJogadorPulo = inputP1[2];
					jogadorScript.axisJogadorSocoBotao = inputP1[3];
					jogadorScript.axisJogadorRolarBotao = inputP1[4];
				}
				break;
			case 1:
				for(int j = 0; j < 5; j++) {
					jogadorScript.axisJogadorVertical = inputP2[0];
					jogadorScript.axisJogadorHorizontal = inputP2[1];
					jogadorScript.axisJogadorPulo = inputP2[2];
					jogadorScript.axisJogadorSocoBotao = inputP2[3];
					jogadorScript.axisJogadorRolarBotao = inputP2[4];
				}
				break;
			case 2:
				for(int j = 0; j < 5; j++) {
					jogadorScript.axisJogadorVertical = inputP3[0];
					jogadorScript.axisJogadorHorizontal = inputP3[1];
					jogadorScript.axisJogadorPulo = inputP3[2];
					jogadorScript.axisJogadorSocoBotao = inputP3[3];
					jogadorScript.axisJogadorRolarBotao = inputP3[4];
				}
				break;
			case 3:
				for(int j = 0; j < 5; j++) {
					jogadorScript.axisJogadorVertical = inputP4[0];
					jogadorScript.axisJogadorHorizontal = inputP4[1];
					jogadorScript.axisJogadorPulo = inputP4[2];
					jogadorScript.axisJogadorSocoBotao = inputP4[3];
					jogadorScript.axisJogadorRolarBotao = inputP4[4];
				}
				break;
			}

//			Jogador jogadorScript = p[i].GetComponent<Jogador>();
//			HUD hudScript = HUDVidas[i].GetComponent<HUD>();
//			Image vidas = hudScript.vidas.GetComponent<Image>();

			HUD hudScript = HUDVidas[i].GetComponent<HUD>();
//			mensagem.SetActive(false);
			Image icone = hudScript.icones.GetComponent<Image>();
			icone.sprite = hudScript.iconesImg[nPersonagens[i]];

			gameOn = true;
		}

		cam.AchaJogadores();

//		for(int i = 0; i < nJogadores; i++) {
//			Jogador jogadorScript = jogadoresPrefab[i].GetComponent<Jogador>();
//			Text[] vidasText = HUDVidas[i].GetComponentsInChildren<Text>();
//			for(int j =0; j < vidasText.Length; j++) {
//				if(vidasText[j].tag == "Vidas") {
//					vidasText[j].text = jogadorScript.vidas.ToString();
//				}
//
//				if(jogadorScript.vidas <= 0) {
//					Application.LoadLevel(3);
//				}
//			}
//		}
	}

    public void ChecarJogadores() {
//        jogadoresLista.Clear();
//        jogadoresLista.AddRange(GameObject.FindGameObjectsWithTag("Player"));
//        //Deixar em ordem alfabetica a lista de jogadores
//        jogadoresLista.Sort(
//            delegate (GameObject Jogador1, GameObject Jogador2) {
//                return Jogador1.name.CompareTo(Jogador2.name);
//            }
//        );

    }

	//COROUTINES
	public IEnumerator LaserLigar() {
		string name = SceneManager.GetActiveScene().name;

		if (name == "Cozinha" || name == "Banheiro" || name == "Quadra" || name == "Arena") {
            yield return new WaitForSeconds(laserEsperaDisparo);
            int i = Random.Range(0, lasersLista.Length);
			if (!lasersLista[i].GetComponent<Laser>().Linha.enabled) {
				StartCoroutine(lasersLista[i].GetComponent<Laser>().DispararLaser(laserTempo));
			}
            yield return new WaitForSeconds(1 * Time.deltaTime);
            StartCoroutine(LaserLigar());
        }
	}
}
