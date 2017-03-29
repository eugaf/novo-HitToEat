using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JogadoresVidas : MonoBehaviour {

    public static JogadoresVidas singleton;

    public float jogador1Vidas = 10f;
    public float jogador2Vidas = 10f;

    public Text jogador1Text;
    public Text jogador2Text;

    // Use this for initialization
    void Start () {
        singleton = this;
        jogador1Text.text = "Jogador 1\n" + "Vidas: " + jogador1Vidas.ToString();
        jogador2Text.text = "Jogador 2\n" + "Vidas: " + jogador2Vidas.ToString();
    }
	
    public void VidaDiminuir(float jogadorQualJogador)
    {
        if (jogadorQualJogador == 0)
        {
            if (jogador1Vidas == 1)
            {
//                GameController.singleton.ReiniciarJogo();
            } else
            {
                jogador1Vidas--;
                jogador1Text.text = "Jogador 1\n" + "Vidas: " + jogador1Vidas.ToString();
            }
        } else
        {
            if (jogador2Vidas == 1)
            {
  //              GameController.singleton.ReiniciarJogo();
            } else
            {
                jogador2Vidas--;
                jogador2Text.text = "Jogador 2\n" + "Vidas: " + jogador2Vidas.ToString();
            }
        }
    }
}
