using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InstanciarJogador : MonoBehaviour
{

    GameController gameController;

    public List<GameObject> pontosInstancia;

    //public List<string> inputJogador;

    void Awake()
    {

        // Adiciona Waypoints para serem utilizados para instanciar os jogadores
        pontosInstancia.AddRange(GameObject.FindGameObjectsWithTag("Waypoint"));

        pontosInstancia.Sort(
            delegate (GameObject way1, GameObject way2)
            {
                return way1.name.CompareTo(way2.name);
            }
        );

        gameController = FindObjectOfType<GameController>();

        // Instancia personagens para cada jogador que confirmou participação na tela de seleção
        //Debug.Log(gameController.jogadoresConfirmadosLista);
        for (int i = 0; i <= gameController.jogadoresConfirmadosLista; i++)
        {
            GameObject JogadorInstanciado = Instantiate(gameController.meshPrefabLista[gameController.meshJogadoresSelecionados[i]], new Vector3(pontosInstancia[i].transform.position.x, pontosInstancia[i].transform.position.y, pontosInstancia[i].transform.position.z), Quaternion.identity) as GameObject; // instancia prefab de mesh selecionado em ponto definido por um objeto de tag Waypoint

            JogadorInstanciado.GetComponentInChildren<Renderer>().material.mainTexture = gameController.texturasLista[gameController.meshJogadoresSelecionados[i]][gameController.texturaJogadoresSelecionados[i]];

            JogadorInstanciado.AddComponent<Jogador>(); // Adiciona script Jogador
            JogadorInstanciado.GetComponent<Jogador>().jogadorQualJogador = i;

            CameraCenter cameraZoomScript = Camera.main.GetComponent<CameraCenter>();

//            if (cameraZoomScript.player1 == null)
//            {
//                cameraZoomScript.player1 = JogadorInstanciado;
//            } else if (cameraZoomScript.player2 == null)
//            {
//                cameraZoomScript.player2 = JogadorInstanciado;
//            }
			for(int j = 0; i < cameraZoomScript.playerLista.Length; i++ ) {
				JogadorInstanciado.GetComponent<Jogador>().axisJogadorVertical = gameController.Jogadores_INPUTS[gameController.idJogadoresSelecionados[j]][0]; // primeiro [i] para qual a ID do personagem e segundo [] para qual string dentro da lista
				JogadorInstanciado.GetComponent<Jogador>().axisJogadorHorizontal = gameController.Jogadores_INPUTS[gameController.idJogadoresSelecionados[j]][1];
				JogadorInstanciado.GetComponent<Jogador>().axisJogadorPulo = gameController.Jogadores_INPUTS[gameController.idJogadoresSelecionados[j]][2];
				JogadorInstanciado.GetComponent<Jogador>().axisJogadorSocoBotao = gameController.Jogadores_INPUTS[gameController.idJogadoresSelecionados[j]][3];
				JogadorInstanciado.GetComponent<Jogador>().axisJogadorRolarBotao = gameController.Jogadores_INPUTS[gameController.idJogadoresSelecionados[j]][4];

			}
	//		cameraZoomScript.playerLista.Add(JogadorInstanciado);

            //JogadorInstanciado.GetComponent <Jogador> ().enabled = true; // Força ativação do scripts Jogador

            //Pega string dentro da lista de inputs que esta dentro da lista de inputs dos jogadores
//            JogadorInstanciado.GetComponent<Jogador>().axisJogadorVertical = gameController.Jogadores_INPUTS[gameController.idJogadoresSelecionados[i]][0]; // primeiro [i] para qual a ID do personagem e segundo [] para qual string dentro da lista
//            JogadorInstanciado.GetComponent<Jogador>().axisJogadorHorizontal = gameController.Jogadores_INPUTS[gameController.idJogadoresSelecionados[i]][1];
//            JogadorInstanciado.GetComponent<Jogador>().axisJogadorPulo = gameController.Jogadores_INPUTS[gameController.idJogadoresSelecionados[i]][2];
//            JogadorInstanciado.GetComponent<Jogador>().axisJogadorSocoBotao = gameController.Jogadores_INPUTS[gameController.idJogadoresSelecionados[i]][3];
//            JogadorInstanciado.GetComponent<Jogador>().axisJogadorRolarBotao = gameController.Jogadores_INPUTS[gameController.idJogadoresSelecionados[i]][4];

        }
    }
}
