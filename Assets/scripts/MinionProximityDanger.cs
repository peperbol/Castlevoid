using UnityEngine;
using System.Collections;
using System.Linq;

public class MinionProximityDanger : MonoBehaviour
{



    public float dangerPlayProximity;
    public DangerAnimation dangerPrefab;
    public float dangerTimeout;
    private float dangerTimeoutTimer;
    public Base team;
    
    void Update() { }

    public void OnTriggerEnter2D(Collider2D c)
    {

        Debug.Log(c.gameObject.name);
        Minion m = c.GetComponent<Minion>();
        if (m)
        {
            if (dangerTimeoutTimer <= 0 && RadialPosition.RadialDistance(FindObjectsOfType<Builder>().First(e => e.team == team).position, m.position) > dangerPlayProximity)
            {
                DangerAnimation.spawnDanger(dangerPrefab, m.position);
                dangerTimeoutTimer = dangerTimeout;
                StartCoroutine(CountdownDanger());
            }
        }
    }


    IEnumerator CountdownDanger()
    {
        while (dangerTimeoutTimer > 0)
        {
            dangerTimeoutTimer -= Time.deltaTime;
            yield return null;
        }
    }
}
