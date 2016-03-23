using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour {
    public string[] scenes;
	// Use this for initialization
	void Awake () {
        for (int i = 0; i < scenes.Length; i++)
        {
            SceneManager.LoadScene(scenes[i], LoadSceneMode.Additive);
        }

        SceneManager.UnloadScene("Start");
    }
}
