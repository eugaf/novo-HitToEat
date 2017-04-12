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
	public GameObject[] 			HUDVidas, jogadoresPrefab;
    public List<GameObject>			jogadoresLista;
	public SkinnedMeshRenderer[]	personagemMaterial;
	public int						nJogadores;

    //INPUTS
    public List<string> inputsPlayer1;
	public List<string> inputsPlayer2;
	public List<string> inputsPlayer3;
	public List<string> inputsPlayer4;
	public List<List<string>> Jogadores_INPUTS;

	public GameObject[] respawns;

    //CONFIRMAR
    bool podeLigar;

	string name;

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

	void Update() {
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
		
//	public void TrocaCena() {
//		SceneManager.LoadScene(1);
//        Invoke("ChecarCena", 3);
//   }

//    public void ReiniciarJogo() {
//        SceneManager.LoadScene(0);
//        Destroy(gameObject);
//    }
	public void PreCena () {
		Invoke("ChecarCena", .1f);
	}


	public void ChecarCena() {
		string name = SceneManager.GetActiveScene().name;
//        ChecarJogadores();
		CriaPersonagens();
		if (name == "Cozinha" || name == "Banheiro" || name == "Quadra" || name == "Arena") {
            lasersLista = GameObject.FindGameObjectsWithTag("Laser");
			StartCoroutine(LaserLigar());
        }
    }

	void CriaPersonagens() {
		respawns = GameObject.FindGameObjectsWithTag("Respawn");
		GameObject local = respawns[Random.Range(0, respawns.Length)];
		Debug.Log(respawns.Length);
		jogadoresPrefab[0].tag = "Player";
		Instantiate (jogadoresPrefab[0].gameObject, local.transform.position, local.transform.rotation);
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
	//	HUDVidas = GameObject.FindGameObjectsWithTag("HUD").OrderBy(go => go.name).ToArray();

//		for(int i = jogadoresLista.Count; i < HUDVidas.Length; i++) {
//			HUDVidas[i].SetActive(false);
//		}
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
