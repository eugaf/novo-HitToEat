using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Menu : MonoBehaviour {

	public GameObject[] opcoes;
	public Button[] buttons;

	public bool check = false;

	void Start () {
		for(int i = 0; i < 4; i++) {
			buttons[i] = opcoes[i].GetComponent<Button>();
		}
		buttons[0].Select();
	}

	public void AbreCreditos () {
		buttons[3].Select();
		check = true;
	}

	public void FechaCreditos () {
		buttons[0].Select();
		check = false;
	}

	public void IniciaJogo () {
		SceneManager.LoadScene("Lobby");
	}

	public void SaiJogo () {
		Application.Quit();
	}
}
