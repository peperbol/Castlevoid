using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]
public class AudioPlay : MonoBehaviour {
    public float ttl;
    public static void PlaySound(AudioClip clip, float volume = 1) {
        AudioPlay o = Instantiate(Resources.Load<AudioPlay>("Audio"));
        o.GetComponent<AudioSource>().PlayOneShot(clip, volume);
    }
    void Update() {
        ttl -= Time.deltaTime;
        if (ttl <=0) Destroy(gameObject);
    }
}
