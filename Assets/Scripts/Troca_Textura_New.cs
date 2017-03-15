using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Troca_Textura : MonoBehaviour {

	public GameController gameController;

	public string axisHorizontal;
	public bool podeTrocarCor;
	public int idTextura;
	public int idMesh;

	public GameObject TextGameObject;

	void Start () 
	{
	
	}

	void Update () 
	{
		TextGameObject.GetComponentInChildren<Renderer> ().material.mainTexture = gameController.texturasLista[idMesh][idTextura] ;

		if (Input.GetAxis (axisHorizontal) > 0 ) 
		{
			StopCoroutine (trocaCorDireita ());
			StartCoroutine(trocaCorDireita ());
		}

		if (Input.GetAxis (axisHorizontal) < 0 )
		{
			StopCoroutine (trocaCorEsquerda ());
			StartCoroutine(trocaCorEsquerda ());
		}
	}

	public IEnumerator trocaCorDireita () // Para quando o jogador selecionar as teclas para a direita para que ele troque a cor;
	{
		if (podeTrocarCor)
		{
			podeTrocarCor = false;

			if (idTextura == GameController.singleton.texturasLista [idMesh].Count - 1)  // A lista de texturas que será pega deve ser aquela cuja mesh foi selecionada, ou seja, a id da lista dentro da lista de texturas é igual a ID da mesh
			{																		// Se a mesh estiver em ordem alfabética, a lista da lista de texturas também deve estar.
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

	public IEnumerator trocaCorEsquerda () // Para quando o jogador selecionar as teclas para a esquerda para que ele troque a cor;
	{
		if (podeTrocarCor)
		{
			podeTrocarCor = false;

			if (idTextura == 0) 
			{
				idTextura = GameController.singleton.texturasLista [idMesh].Count - 1;
			} 
			else 
			{
				idTextura -= 1;
			}
		}

		TextGameObject.GetComponentInChildren<Renderer> ().material.SetTexture("_Albedo", gameController.texturasLista[idMesh][idTextura]);

		yield return new WaitForSeconds (0.4f);

		podeTrocarCor = true;
	}
}
