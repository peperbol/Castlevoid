using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour {
    public string[] scenes;
	// Use this for initialization
	void Awake () {

        SceneManager.LoadScene(scenes[0]);
        for (int i = 1; i < scenes.Length; i++)
        {
            SceneManager.LoadScene(scenes[i], LoadSceneMode.Additive);
        }

    }
}
