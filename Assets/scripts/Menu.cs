using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public string[] a;
    public string[] b;
    public float timetillavailable;
    public float timeBeforeRestart;
    bool selected = false;
    static Menu m;
    void Start()
    {
        m = this;
        gameObject.SetActive(false);
    }
    void Update()
    {
        if (timetillavailable > 0)
        {
            timetillavailable -= Time.deltaTime;
        }
        else
        {
            if (selected) return;
            for (int i = 0; i < a.Length; i++)
            {
                if (Input.GetButtonDown(a[i]))
                {
                    StartCoroutine(Restart());
                }
            }
            for (int i = 0; i < b.Length; i++)
            {
                if (Input.GetButtonDown(b[i]))
                {
                    //Application.LoadLevel("menu");
                }
            }
        }
    }
    public static void GameOver(Base loser)
    {
        m.GetComponent<Image>().enabled = true;
        m.gameObject.SetActive(true);
        m.selected = false;
        Freezable[] freezables = GameObject.FindObjectsOfType<Object>().Where(e=> e is Freezable).Cast<Freezable>().ToArray();
        for (int i = 0; i < freezables.Length; i++)
        {
            freezables[i].Frozen = true;
        }
    }
    IEnumerator Restart() {

        m.GetComponent<Image>().enabled = false;
        selected = true;
        EndGame();
        yield return new WaitForSeconds(timeBeforeRestart);
        StartGame();

        m.gameObject.SetActive(false);
    }
    void EndGame()
    {
        Minion[] ms = GameObject.FindObjectsOfType<Minion>();
        House[] hs = GameObject.FindObjectsOfType<House>();
        Builder[] bs = GameObject.FindObjectsOfType<Builder>();
        for (int i = 0; i < ms.Length; i++)
        {
            ms[i].Health = 0;
        }
        for (int i = 0; i < hs.Length; i++)
        {
            hs[i].Health = 0;
        }
        for (int i = 0; i < bs.Length; i++)
        {
            bs[i].Health = 0;
        }
    }
    void StartGame() {
        Freezable[] freezables = GameObject.FindObjectsOfType<Object>().Where(e => e is Freezable).Cast<Freezable>().ToArray();
        for (int i = 0; i < freezables.Length; i++)
        {
            freezables[i].Frozen = false;
        }
        Builder[] bs = GameObject.FindObjectsOfType<Builder>();

        for (int i = 0; i < bs.Length; i++)
        {
            bs[i].Resources = bs[i].StartResources;
        }

        Base[] bases = GameObject.FindObjectsOfType<Base>();
        Base LostBase = bases.First(e => e.Health <= 0);
        LostBase.StartCoroutine(LostBase.Undestroy());
        for (int i = 0; i < bases.Length; i++)
        {
            bases[i].Health = bases[i].MaxHealth;
        }
    }
}
