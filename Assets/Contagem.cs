using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Contagem : MonoBehaviour {

	Text m_Text;

	void Start()
	{
		m_Text = GetComponent<Text>();
		StartCoroutine(IniciarContagem());
	}

	public IEnumerator IniciarContagem()
	{
		int i = 5;
		do {
			m_Text.text = i.ToString();
			yield return new WaitForSeconds(1);
			i--;
		} while (i >= 0);
		GameController.singleton.TrocaCena();
		yield return null;
	}
}
