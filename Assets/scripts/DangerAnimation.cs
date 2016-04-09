using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DangerAnimation : MonoBehaviour
{
    public static void spawnDanger(DangerAnimation prefab, float pos) {
        RadialPosition m = Instantiate(prefab).GetComponent<RadialPosition>();

        SceneManager.MoveGameObjectToScene(m.gameObject, SceneManager.GetSceneByName("gameplay"));
        m.center = GameObject.FindGameObjectWithTag("Center").transform;
        m.Position = pos;
    }
    SpriteRenderer r;
    public float flashInterval;
    public Color flash1;
    public Color flash2;
    public float scale = 1;
    float flashTimer;
    float lifeTimer;
    bool currentFlash;
    public float stayTime;
    public float fadeTime;
    void Start()
    {
        r = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        flashTimer -= Time.deltaTime;
        if (flashTimer <= 0)
        {
            flashTimer += flashInterval;
            currentFlash = !currentFlash;
            r.color = (currentFlash) ? flash1 : flash2;
            r.transform.localScale *= (currentFlash) ? scale : 1 / scale;
        }
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= stayTime)
        {
            Color c = r.color;
                c.a = flash2.a = flash1.a = 1 - (lifeTimer - stayTime) / fadeTime;
            r.color = c;
            if (lifeTimer >= stayTime + fadeTime)
            {
                Destroy(gameObject);
            }
        }
    }
}
