using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Loader : MonoBehaviour {

	GameController gcScript;

	private bool loadScene = true;
	public int scene;
	[SerializeField]
	public Text loadingText;

	void Start () {
		gcScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		loadingText.text = "Loading...";
		StartCoroutine(LoadNewScene());

	}
	
	// Update is called once per frame
	void Update () {
//		if((Input.anyKey) && !loadScene) {
//			loadScene = true;
//			loadingText.text = "Loading...";
//			StartCoroutine(LoadNewScene());
//		}

		if(loadScene) {
			loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));
		}
	}

	IEnumerator LoadNewScene () {
		yield return new WaitForSeconds(3);
		AsyncOperation async = Application.LoadLevelAsync(scene);
		while(!async.isDone) {
			yield return null;
		}

		yield return async;
	}
}
