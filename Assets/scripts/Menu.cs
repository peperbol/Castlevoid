using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
    public string[] a;
    public string[] b;
    void Update () {

        for (int i = 0; i < a.Length; i++)
        {
           if(Input.GetButtonDown(a[i])){
                Application.LoadLevel("world");
            }
        }
        for (int i = 0; i < b.Length; i++)
        {
            if (Input.GetButtonDown(b[i]))
            {
                Application.Quit();
            }
        }
    }
}
