using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

	Jogador		jogadorScript;
	public GameObject 	selecionaJogador;
	public Text vidasText;

	// Use this for initialization
	void Start () {
		jogadorScript = selecionaJogador.GetComponent<Jogador>();
		vidasText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	//	vidasText.text = "Vidas: " + jogadorScript.vidas;
	}
}
