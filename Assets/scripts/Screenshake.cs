using UnityEngine;
using System.Collections;

public class Screenshake : MonoBehaviour {

	
    public void Shake(float distance, float duration,float speed, float delay = 0, float falloff = 0)
    {
        StartCoroutine(shake(distance, duration, speed, delay, falloff));
    }
    IEnumerator shake(float distance, float duration, float speed,float delay = 0,  float falloff = 0)
    {
        yield return new WaitForSeconds(delay);
        Vector3 translation = Vector3.zero;
        float timer = duration;
        float s1 = Random.Range(speed * 0.5f, speed * 1.3f);
        float s2 = Random.Range(speed * 0.5f, speed * 1.3f);
        float t1=0, t2=0;
        falloff = distance * falloff / duration;
        while (timer > 0 || t1 != 0 || t2 != 0) {
            transform.position -= translation;
            if (timer > 0)
            {
                t1 += s1 * Time.deltaTime;
                t2 += s2 * Time.deltaTime;
                t1 %= Mathf.PI * 2;
                t2 %= Mathf.PI * 2;
            }
            else
            {
                if(t1 != 0)
                {
                    t1 += s1 * Time.deltaTime;
                    if (t1 > Mathf.PI*2) t1 = 0;
                }
                if (t2 != 0)
                {
                    t2 += s2 * Time.deltaTime;
                    if (t2 > Mathf.PI*2) t2 = 0;
                }
            }
            timer -= Time.deltaTime;
            translation = new Vector3(Mathf.Sin(t1) * distance, Mathf.Sin(t2) * distance, 0);
            distance -= falloff * Time.deltaTime;
            transform.position += translation;
            yield return null;
        }

    }
}
