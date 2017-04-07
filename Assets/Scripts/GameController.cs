using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

[SerializeField]
public class GameController : MonoBehaviour {

//	public static GameController singleton;

	public GameObject[] pPos, p;
	public GameObject[] listaPersonagens;

    //LASERS
	public GameObject[] lasersLista;
	public float 	laserEsperaDisparo, laserTempo;
	public bool  	laserPodeLigar;

    //JOGADORES
	public GameObject[] HUDVidas;
    public List<GameObject> jogadoresLista;

    //INPUTS
    public List<string> inputsPlayer1;
	public List<string> inputsPlayer2;
	public List<string> inputsPlayer3;
	public List<string> inputsPlayer4;
	public List<List<string>> Jogadores_INPUTS;

    //CONFIRMAR
    bool podeLigar;

	void Awake () {
		//Gerenciamento da instancia do GameController
//		if (singleton == null) {
//			singleton = this;
//		}
//		else if (singleton != this) {
//			Destroy (gameObject);
//		}
		DontDestroyOnLoad(this);

//		if(cenaAtual.name != "Fim") {
//			Debug.Log("Cena ativa: " + cenaAtual.name);
		//		}
		CriaPersonagensLobby(0);
	}

	void Start () {
		if(Application.loadedLevelName != "Lobby" || Application.loadedLevelName != "Menu") {
			ChecarCena();
		}

    }

	void Update() {
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
//			Invoke("Start", .5f);
			SceneManager.LoadScene(0);
			Start();
		} else if(Input.GetKeyDown(KeyCode.Alpha2)) {
			SceneManager.LoadScene(1);
			ChecarCena();
//			Invoke("Start", .5f);
		} else if(Input.GetKeyDown(KeyCode.Alpha3)) {
			SceneManager.LoadScene(2);
//			Invoke("Start", .5f);
		} else if(Input.GetKeyDown(KeyCode.Alpha4)) {
			SceneManager.LoadScene(3);
//			Invoke("Start", .5f);
		}

		p[0].transform.position = pPos[0].transform.position;



//		for(int i = 0; i < jogadoresLista.Count; i++) {
//			Jogador jogadorScript = jogadoresLista[i].GetComponent<Jogador>();
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

	void CriaPersonagensLobby (int numeroPersonagens) {
		switch (numeroPersonagens) {
		case 0:
			p[0] = Instantiate(listaPersonagens[3].gameObject, pPos[0].transform.position, pPos[0].transform.rotation);
			break;
		}
	}
		
	public void TrocaCena() {
		SceneManager.LoadScene(1);
        Invoke("ChecarCena", 3);
   }

    public void ReiniciarJogo() {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }



    public void ChecarCena() {
        //Debug.Log("CHECK");
        ChecarJogadores();
		if (SceneManager.GetActiveScene().name == "Cozinha" || SceneManager.GetActiveScene().name == "Banheiro" || SceneManager.GetActiveScene().name == "Quadra" || SceneManager.GetActiveScene().name == "Arena") {
            lasersLista = GameObject.FindGameObjectsWithTag("Laser");
            //StartCoroutine(CriarNovoPowerUp());
			StartCoroutine(LaserLigar());
        }
    }

    public void ChecarJogadores() {
        jogadoresLista.Clear();
        jogadoresLista.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        //Deixar em ordem alfabetica a lista de jogadores
        jogadoresLista.Sort(
            delegate (GameObject Jogador1, GameObject Jogador2) {
                return Jogador1.name.CompareTo(Jogador2.name);
            }
        );
	//	HUDVidas = GameObject.FindGameObjectsWithTag("HUD").OrderBy(go => go.name).ToArray();

//		for(int i = jogadoresLista.Count; i < HUDVidas.Length; i++) {
//			HUDVidas[i].SetActive(false);
//		}
    }

	//COROUTINES
	public IEnumerator LaserLigar() {
		if (SceneManager.GetActiveScene().name == "Cozinha" || SceneManager.GetActiveScene().name == "Banheiro" || SceneManager.GetActiveScene().name == "Quadra" || SceneManager.GetActiveScene().name == "Arena") {
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
