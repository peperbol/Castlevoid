using UnityEngine;
using System.Collections;
using System;

public class BomberMinion : Minion {
    bool exploded = false;
    public float explosionRadius;
    public LayerMask damageMask;
    public float timebeforeDamage = 0.1f;
    public float maxDamage;
    //public AudioClip dmgHit;
    protected override void Attack(GameObject go, Attackable a)
    {
        StartCoroutine(Explode());
    }
    protected IEnumerator Explode()
    {
        yield return new WaitForSeconds(timebeforeDamage);
        RaycastHit2D[] r = Physics2D.CircleCastAll(transform.position, explosionRadius, Vector2.one, 0, damageMask);
        for (int i = 0; i < r.Length; i++)
        {
            int dmg = Mathf.RoundToInt( maxDamage *( 1- Vector3.Distance( r[i].transform.position, transform.position)  / explosionRadius));
            Attackable a = r[i].transform.GetComponent<Attackable>();
            Debug.Log(r[i].transform.gameObject.name +" " + dmg);
            for (int j = 0; j < dmg; j++)
            {
                a.Damage(this);
            }
        }
        Destroy(gameObject);
    }
    public override void Damage(MonoBehaviour damager)
    {
        if (!exploded)
        {
            animator.SetTrigger("Dmg");

            base.Damage(damager);
        }
    }
    
}
