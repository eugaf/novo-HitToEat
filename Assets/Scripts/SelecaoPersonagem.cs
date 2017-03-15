using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SelecaoPersonagem : MonoBehaviour {

    GameController gameController;

	//public Color[] coresDisponiveis; // lista com as cores possíveis para selecionar
	public List<GameObject> listaPersonagens; // Lista com os prefabs das meshes que podem ser selecionadas

	//public Text nomeJogador; // Nome utilizado para a TAG
	//public Text modo; // Modo de start, mesh, cor e confirmar 
	//public GameObject toolTip_JaSelecionado;
	public GameObject toolTip_Comecar;
	public GameObject toolTip_Confirmado;
	//public GameObject toolTip_Confirmado;


	public string selecionar; // String do input de ataque
	public string retornar; // String do input de pular
	public string comecar; // String do input de rolar
	public string axisHorizontal; // String do input horizontal
	public string axisVertical; // String do input vertical

	public bool podeParticipar; // Entrar na seleção de mesh
	bool podeSelecionar; // Usar botão de selecionar novamente
	bool podeRetornar; // Usar o botão de Retornar novamente
	bool podeTrocarCor; // Selecao da cor
	bool podeTrocarPersonagem; // Selecao da mesh
	bool personagemSelecionado; // Personagem locked para começar partida
	bool podeUsarPersonagem; // Pode selecionar a mesh pois não foi selecionada por nenhum jogador

	public int jogadorID; // Para saber qual jogador é
	public int idTextura; //ID para o array de cores. Cada ID representa uma cor específica.
	public int idPersonagem; //ID para o array de personagens. Cada ID representa um personagem específico.
	int idPersonagemAtual; //Armazena a id antiga para poder desabilitar a mesh antiga!!

	void Awake ()
	{
		gameController = FindObjectOfType<GameController> ();

		podeTrocarPersonagem = true; 
		podeTrocarCor = false;
		podeParticipar = false;
		personagemSelecionado = false;

		//toolTip_JaSelecionado.SetActive (false);
		toolTip_Comecar.SetActive (false);
		toolTip_Confirmado.SetActive (false);

		listaPersonagens.Sort (
			delegate (GameObject mesh1, GameObject mesh2) 
			{
				return mesh1.name.CompareTo (mesh2.name);
			}
		);


//		nomeJogador.text = gameObject.transform.parent.name;

		foreach (GameObject mesh in listaPersonagens) // Desabilita todas as mesh's dentro da lista e muda suas cores
		{
			mesh.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
		}
	}

	void Start () 
	{
		listaPersonagens [idPersonagem].gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
	}

	void Update () 
	{
		if (!podeParticipar) {

			toolTip_Comecar.SetActive (true);

			listaPersonagens [idPersonagem].GetComponentInChildren<Renderer> ().material.color = Color.black;

//			modo.text = "Pressione START";
		}

		foreach (GameObject mesh in listaPersonagens) // 
		{
			mesh.GetComponentInChildren<Renderer> ().material.mainTexture = gameController.texturasLista[idPersonagem][idTextura];
		}

		if (Input.GetAxis (comecar) != 0 && podeTrocarPersonagem == true && podeTrocarCor == false) 
		{
//			modo.text = "Troca PERSONAGEM";
			podeParticipar = true;
		}

		if (Input.GetAxis (selecionar) == 0) 
		{
			podeSelecionar = true;
		}

		if (Input.GetAxis (retornar) == 0) 
		{
			podeRetornar = true;
		}

		if (podeParticipar) 
		{
			listaPersonagens [idPersonagem].GetComponentInChildren<Renderer> ().material.color = Color.white;

			toolTip_Comecar.SetActive (false);

			if (Input.GetAxis (selecionar) != 0) 
			{
				if (podeTrocarPersonagem == true && podeTrocarCor == false) 
				{

					foreach (int ID in gameController.meshJogadoresSelecionados)
					{
						if (idPersonagem == ID) 
						{
							podeUsarPersonagem = false;

							listaPersonagens [idPersonagem].GetComponentInChildren<Renderer> ().material.color = Color.black;

//							toolTip_JaSelecionado.SetActive (true);
							break;
						}

						podeUsarPersonagem = true;
					}

					if (gameController.meshJogadoresSelecionados.Count == 0) 
					{
						podeUsarPersonagem = true;
					}

					if (podeUsarPersonagem) 
					{
						gameController.meshJogadoresSelecionados.Add (idPersonagem);
						podeSelecionar = false;
						podeTrocarPersonagem = false;
						podeTrocarCor = true;
//						modo.text = "Trocar COR";
					}
				}

				if (podeTrocarPersonagem == false && podeTrocarCor == true && podeSelecionar) 
				{
					podeTrocarCor = false;
					personagemSelecionado = true;
					gameController.numeroJogadores++;
					gameController.jogadoresConfirmadosLista ++;
					gameController.texturaJogadoresSelecionados.Add (idTextura);
					gameController.idJogadoresSelecionados.Add (jogadorID);

//					modo.text = "CONFIRMADO";

					toolTip_Confirmado.SetActive (true);

                    // Debug.Log (personagemSelecionado);
				}
			}
			
			if (Input.GetAxis (retornar) != 0) 
			{
				if (personagemSelecionado && podeRetornar) 
				{
					podeRetornar = false;
					personagemSelecionado = false;
					gameController.numeroJogadores--;
					gameController.jogadoresConfirmadosLista --;
					gameController.texturaJogadoresSelecionados.Remove (idTextura);
					gameController.idJogadoresSelecionados.Remove (jogadorID);
					podeTrocarCor = true;

					toolTip_Confirmado.SetActive (false);
//					modo.text = "Trocar COR";
				}

				if (podeTrocarPersonagem == false && podeTrocarCor == true && podeRetornar) // Retornar para seleção de mesh
				{
					podeRetornar = false;
					podeTrocarCor = false;

					gameController.meshJogadoresSelecionados.Remove (idPersonagem);

					podeTrocarPersonagem = true;
//					modo.text = "Troca PERSONAGEM";
				}

				if (podeTrocarPersonagem == true && podeTrocarCor == false && podeRetornar) 
				{
					podeRetornar = false;
					podeParticipar = false;
//					modo.text = "Pressione START";
				}
			}
			
			if (Input.GetAxis (axisHorizontal) > 0 && podeTrocarPersonagem == true && podeTrocarCor == false) //&& idSelecao == 1) 
			{
				listaPersonagens [idPersonagem].GetComponentInChildren<Renderer> ().material.color = Color.white;
//				toolTip_JaSelecionado.SetActive (false);
				StopCoroutine (trocaPersonagemDireita ());
				StartCoroutine(trocaPersonagemDireita ());
			}
			
			if (Input.GetAxis (axisHorizontal) < 0 && podeTrocarPersonagem == true && podeTrocarCor == false) //&& idSelecao == 1) 
			{
				listaPersonagens [idPersonagem].GetComponentInChildren<Renderer> ().material.color = Color.white;
//				toolTip_JaSelecionado.SetActive (false);
				StopCoroutine (trocaPersonagemEsquerda ());
				StartCoroutine(trocaPersonagemEsquerda ());
			}
			
			if (Input.GetAxis (axisHorizontal) > 0 && podeTrocarCor == true && podeTrocarPersonagem == false) //&& idSelecao == 1) 
			{
				listaPersonagens [idPersonagem].GetComponentInChildren<Renderer> ().material.color = Color.white;
//				toolTip_JaSelecionado.SetActive (false);
				StopCoroutine (trocaCorDireita ());
				StartCoroutine(trocaCorDireita ());
			}
			
			if (Input.GetAxis (axisHorizontal) < 0 && podeTrocarCor == true && podeTrocarPersonagem == false) //&& idSelecao == 1) 
			{
				listaPersonagens [idPersonagem].GetComponentInChildren<Renderer> ().material.color = Color.white;
//				toolTip_JaSelecionado.SetActive (false);
				StopCoroutine (trocaCorEsquerda ());
				StartCoroutine(trocaCorEsquerda ());
			}
		}
	}

	//FOI FEITO SEPARADO PARA CADA TECLA POIS SE JUNTAR TODAS EM UMA COROUTINE APRESENTAVA ERROS QUANDO A DELAY E A PRÓPRIA TROCA

	public IEnumerator trocaCorDireita () // Para quando o jogador selecionar as teclas para a direita para que ele troque a cor;
	{
		if (podeTrocarCor)
		{
			podeTrocarCor = false;

			if (idTextura == gameController.texturasLista[idPersonagem].Count - 1) 
			{
				idTextura = 0;
			} 
			else 
			{
				idTextura += 1;
			}
		}

		yield return new WaitForSeconds (0.4f);

		podeTrocarCor = true;
	}

	// Para quando o jogador selecionar as teclas para a esquerda para que ele troque a cor;
	public IEnumerator trocaCorEsquerda () 
	{
		if (podeTrocarCor)
		{
			podeTrocarCor = false;

			if (idTextura == 0) 
			{
				idTextura = gameController.texturasLista[idPersonagem].Count - 1;
			} 
			else 
			{
				idTextura -= 1;
			}
		}

		yield return new WaitForSeconds (0.4f);

		podeTrocarCor = true;
	}

	// Para quando o jogador selecionar as teclas para a esquerda para que ele troque o personagem;
	public IEnumerator trocaPersonagemDireita ()
	{
		if (podeTrocarPersonagem)
		{
			podeTrocarPersonagem = false;

			idPersonagemAtual = idPersonagem;

			if (idPersonagem == listaPersonagens.Count - 1)
			{
				idPersonagem = 0;
			} 
			else
			{
				idPersonagem += 1;
			}

			listaPersonagens [idPersonagemAtual].gameObject.GetComponentInChildren<SkinnedMeshRenderer> ().enabled = false;
			listaPersonagens [idPersonagem].gameObject.GetComponentInChildren<SkinnedMeshRenderer> ().enabled = true;

		}

		yield return new WaitForSeconds (0.4f);

		podeTrocarPersonagem = true;
	}

	public IEnumerator trocaPersonagemEsquerda () // Para quando o jogador selecionar as teclas para a esquerda para que ele troque o personagem;
	{
		if (podeTrocarPersonagem)
		{
			podeTrocarPersonagem = false;

			idPersonagemAtual = idPersonagem;

			if (idPersonagem == 0) 
			{
				idPersonagem = listaPersonagens.Count - 1;
			} 
			else 
			{
				idPersonagem -= 1;
			}

			listaPersonagens [idPersonagemAtual].gameObject.GetComponentInChildren<SkinnedMeshRenderer> ().enabled = false;
			listaPersonagens [idPersonagem].gameObject.GetComponentInChildren<SkinnedMeshRenderer> ().enabled = true;
		}

		yield return new WaitForSeconds (0.4f);

		podeTrocarPersonagem = true;
	}
}
