﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Trigger : MonoBehaviour {

	GameController gc;
	GameObject mensagem;

	// Use this for initialization
	void Start () {
		gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		mensagem = GameObject.FindGameObjectWithTag("Mensagem");
		mensagem.SetActive(false);
		gc.ChecarCena();
	}
	
	// Update is called once per frame
	public void Fim (int i) {
		mensagem.SetActive(true);
		Text msg = mensagem.GetComponentInChildren<Text>();
		msg.text = "Player " + i + " wins!";
//		Time.timeScale = 0;
	}
}