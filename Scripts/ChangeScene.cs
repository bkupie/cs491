using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

	public GameObject startButton;
	public string sceneName = "main-scene";
	void OnTriggerEnter( Collider other )
	{	
			Debug.Log("YO");
			startButton.GetComponent<TextMesh>().text = "Loading . . .";
			this.GetComponent<MeshRenderer>().enabled = false;
			SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
	
	}
}
