using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
// Editor specific code here
using UnityEditor;
#endif

public class PowerUp : MonoBehaviour {

	public static PowerUp singleton;
	public enum powerUpType {Agilidade,Agarrar, Ima, SocoForte, Intocabilidade}
	public powerUpType meuPowerUp;
	public float powerUpDuracao;
	//Agilidade
	public float powerUpAgilidadePuloForca = 3f;
	public float powerUpAgilidadeMovimentoVelocidade = 6f;

	void Start () {
		meuPowerUp = powerUpType.Agilidade;
		singleton = this;
		Destroy(singleton.gameObject,5);
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.transform.tag == "Player") 
		{
			switch (meuPowerUp) 
			{
				case powerUpType.Agilidade:
				GameController.singleton.StartCoroutine(GameController.singleton.PowerUpAcao(singleton,meuPowerUp,other.transform.GetComponent<Jogador>(),5f));
				Destroy(gameObject);
					break;
				default:
					break;
			}
		}
	}


}

#if UNITY_EDITOR
[CustomEditor(typeof(PowerUp))]
public class MyScriptEditor : Editor
{

	public override void OnInspectorGUI()
	{
		PowerUp PowerUp = target as PowerUp;
//		PowerUp.flag = GUILayout.Toggle(PowerUp.flag, "Toggle");
//
//		if(PowerUp.flag){
//			PowerUp.i = EditorGUILayout.IntSlider("I field:", PowerUp.i , 1 , 100);
//		}
//
//		selected = EditorGUILayout.Popup("Label", selected, options); 
//		if (selected == 1) {
//			Debug.Log(selected);
//
//		}
		PowerUp.meuPowerUp = (PowerUp.powerUpType) EditorGUILayout.EnumPopup("Power Up Tipo", PowerUp.meuPowerUp);
		PowerUp.powerUpDuracao = EditorGUILayout.FloatField("Power Up Duracao", PowerUp.powerUpDuracao);

		//Passar para switch depois

		if (PowerUp.meuPowerUp == PowerUp.powerUpType.Agilidade) {
			EditorGUILayout.LabelField("Power Up Agilidade Parametros");
			PowerUp.powerUpAgilidadePuloForca = EditorGUILayout.FloatField("Pulo Forca", PowerUp.powerUpAgilidadePuloForca);
			PowerUp.powerUpAgilidadeMovimentoVelocidade = EditorGUILayout.FloatField("Velocidade Movimento", PowerUp.powerUpAgilidadeMovimentoVelocidade);
			//Adicionar o terceiro atributo
		}
//		if (GUILayout.Button("Create"))
//			InstantiatePrimitive(PowerUp.meuPowerUp);
	}
}
#endif