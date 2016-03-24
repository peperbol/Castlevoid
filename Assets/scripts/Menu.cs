using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public string[] a;
    public string[] b;
    public float timeTillAvailable;
    private float availableTimer;
    public float timeBeforeRestart;
    bool restartMenu = false;
    bool mainMenu = true;
    static Menu m;

    public bool RestartMenu
    {
        get
        {
            return restartMenu;
        }
        set
        {
            restartMenu = value;
            if (restartMenu)
            {
                MainMenu = false;
            }
        }
    }

    public bool MainMenu
    {
        get
        {
            return mainMenu;
        }

        set
        {
            mainMenu = value;
            if (mainMenu)
            {
                RestartMenu = false;
            }
        }
    }

    void Start()
    {
        m = this;
    }

    public void OnEnable()
    {
        availableTimer = timeTillAvailable;
    }

    void Update()
    {
        if (timeTillAvailable > 0)
        {
            timeTillAvailable -= Time.deltaTime;
        }
        else
        {
            if (RestartMenu)
            {
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
                        StartCoroutine(GoToMenu());
                    }
                }
            }
            else if (MainMenu)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    if (Input.GetButtonDown(a[i]))
                    {
                        StartCoroutine(StartGameFromMenu());
                    }
                }
            }
        }
    }
    public static void GameOver(Base loser)
    {
        m.GetComponent<Image>().enabled = true;
        m.gameObject.SetActive(true);
        m.RestartMenu = true;
        Freeze();
    }
    public static void Freeze()
    {
        Freezable[] freezables = GameObject.FindObjectsOfType<Object>().Where(e => e is Freezable).Cast<Freezable>().ToArray();
        for (int i = 0; i < freezables.Length; i++)
        {
            freezables[i].Frozen = true;
        }
    }

    public static void UnFreeze()
    {
        Freezable[] freezables = GameObject.FindObjectsOfType<Object>().Where(e => e is Freezable).Cast<Freezable>().ToArray();
        for (int i = 0; i < freezables.Length; i++)
        {
            freezables[i].Frozen = false;
        }
    }
    private void Disable()
    {
        m.GetComponent<Image>().enabled = false;
        mainMenu = false;
        RestartMenu = false;
    }
    IEnumerator StartGameFromMenu()
    {
        Disable();
        SceneManager.UnloadScene("menu");
        SceneManager.LoadScene("gameplay", LoadSceneMode.Additive);
        yield return new WaitForSeconds(timeBeforeRestart);

    }

    IEnumerator Restart()
    {
        Disable();
        EndGame();
        yield return new WaitForSeconds(timeBeforeRestart);
        StartGame();

        m.gameObject.SetActive(false);
    }
    IEnumerator GoToMenu()
    {
        Disable();
        EndGame();
        yield return new WaitForSeconds(timeBeforeRestart);
        SceneManager.UnloadScene("gameplay");
        SceneManager.LoadScene("menu", LoadSceneMode.Additive);
        MainMenu = true;

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
    void StartGame()
    {
        UnFreeze();
        Builder[] bs = GameObject.FindObjectsOfType<Builder>();

        for (int i = 0; i < bs.Length; i++)
        {
            bs[i].Resources = bs[i].StartResources;
        }

        Base[] bases = GameObject.FindObjectsOfType<Base>();

        if (bases.Length > 0)
        {
            Base LostBase = bases.First(e => e.Health <= 0);
            if (LostBase)
                LostBase.StartCoroutine(LostBase.Undestroy());
            for (int i = 0; i < bases.Length; i++)
            {
                bases[i].Health = bases[i].MaxHealth;
            }
        }
    }
}
