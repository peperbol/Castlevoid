using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
    public string[] a;
    public string[] b;
    public float timetillavailable;

    void Update()
    {
        if (timetillavailable > 0)
        {
            timetillavailable -= Time.deltaTime;
        }
        else
        {

            for (int i = 0; i < a.Length; i++)
            {
                if (Input.GetButtonDown(a[i]))
                {
                    Application.LoadLevel("world");
                }
            }
            for (int i = 0; i < b.Length; i++)
            {
                if (Input.GetButtonDown(b[i]))
                {
                    Application.LoadLevel("menu");
                }
            }
        }
    }
}
