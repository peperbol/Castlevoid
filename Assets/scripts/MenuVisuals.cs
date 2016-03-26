using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class MenuVisuals : MonoBehaviour
{
    public float rotationSpeed;
    public float lerpvalue = 0.1f;
    float modifier = 0.0001f;
    Vector3 scale;
    float alpha;
    public Image i;
    public float power = 1;
    bool a= false;
    public bool startActive;
    // Use this for initialization
    void Start()
    {
        scale = transform.localScale;
        alpha = i.color.a;
        if (startActive) { Menu.MainManuStart(); }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime / Mathf.Pow(modifier, power)) );
        transform.localScale = scale * modifier;
        Color c = i.color;
        c.a = alpha * modifier;
        i.color = c;
    }
    public void Appear()
    {
        a = true;
        i.enabled = true;
        StartCoroutine(tweenModifer(0.00001f, 1, e => !a));
        Debug.Log("a");
    }
    public void Disappear()
    {
        a = false;
        StartCoroutine(tweenModifer(1f, 0.0000001f, e => a, () => { i.enabled = false; }));
        Debug.Log("d");
    }

    IEnumerator tweenModifer(float from, float to,Predicate<object> breakRequirement, Action a = null)
    {
        modifier = from;
        while ((modifier- to) / (from- to) > 0.002f )
        {
            if (breakRequirement(null)) yield break; 
            modifier =   modifier + (to- modifier) *  lerpvalue;
            yield return null;
        }
        modifier = to;
        if (a != null)
            a();
    }
}
