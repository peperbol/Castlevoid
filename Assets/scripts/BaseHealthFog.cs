using UnityEngine;
using System.Collections;

public class BaseHealthFog : MonoBehaviour {
    public int n;
    public float radius;
    public float depth;
    Vector3[] circle;
    [Range(0,1)]
    public float split;
    public ParticleSystem p1;
    public ParticleSystem p2;
    float particleRate;

    private BaseHealthPieChart baseHealthPieChart;
    void Start () {

        var s1 = p1.shape;
        var s2 = p2.shape;
        s1.mesh = new Mesh();
        s2.mesh = new Mesh();
        particleRate = p1.emission.rate.constantMax;
        GenCircle();
        UpdateMeshes();
    }
    void GenCircle() {
        circle = new Vector3[n];
        for (int i = 0; i < n; i++)
        {
          
            float a = Mathf.PI * 2 / n * i;
            circle[i] = new Vector3(Mathf.Cos(a), Mathf.Sin(a)) * radius;
        }
    }
    void UpdateMeshes() {

        var s1 = p1.shape;
        var s2 = p2.shape;

        Mesh m1 = s1.mesh;
        Mesh m2 = s2.mesh;
        int num = Mathf.Clamp( Mathf.RoundToInt(n * split), 0, n-2) ;
        Vector3[] v1 = new Vector3[n * 2 +2];
        Vector3[] v2 = new Vector3[n * 2 +2];

        for (int i = 1; i < n + 1; i++)
        {
            if (i < num + 3)
            {
                v1[i * 2] = v1[i * 2 + 1] = circle[(n + num / 2 - i + 1) % n];
                v1[i * 2 + 1].z = depth;
            }
            else
            {
                v1[i * 2] = v1[i * 2 + 1] = Vector3.zero;
            }
        }
        v1[0] = v1[1] = Vector3.zero;
        v1[1].z = depth;
        for (int i = 1; i < n+1; i++)
        {
            if (i < n- num + 1)
            {
                v2[i * 2] = v2[i * 2 + 1] = circle[num / 2 + (n - num) - i];
                v2[i * 2 + 1].z = depth;
            }
            else {

                v2[i * 2] = v2[i * 2 + 1] = Vector3.zero;
            }
        }
        v2[0] = v2[1] = Vector3.zero;
        v2[1].z = depth;
       
        m1.vertices = v1;
        m1.triangles = genPolies(v1);
        m2.vertices = v2;
        m2.triangles = genPolies(v2);
        ;
        ;

        var e1 = p1.emission;
        var r1 = e1.rate;
        r1.constantMin = r1.constantMax = particleRate * num / Mathf.Round(n * 0.5f);
        e1.rate = r1;
        var e2 = p2.emission;
        var r2 = e2.rate;
        r2.constantMin = r2.constantMax = particleRate * (n-num) / Mathf.Round(n * 0.5f);
        e2.rate = r2;

    }
    int[] genPolies(Vector3[] v) {
        int[] tri = new int[(v.Length-2)  * 6 ];
        int p = v.Length / 2;
        int counter = 0;
        /*
        //border
        tri[counter * 3] = p * 2 - 1;
        tri[counter * 3 + 1] = 0;
        tri[counter * 3 + 2] = p * 2 - 2;
        counter++;
        tri[counter * 3] = p * 2 - 1;
        tri[counter * 3 + 1] = 1;
        tri[counter * 3 + 2] = 0;
        counter++;

        for (int i = 1; i < p; i++)
        {
            tri[counter * 3] = (i * 2) - 1;
            tri[counter * 3 + 1] = (i * 2);
            tri[counter * 3 + 2] = (i * 2) - 2;
            counter++;
            tri[counter * 3] = (i * 2) - 1;
            tri[counter * 3 + 1] = (i * 2) + 1;
            tri[counter * 3 + 2] = (i * 2);
            counter++;

        }*/
        //fill back
        for (int i = 1; i < p - 1; i++)
        {
            tri[counter * 3] = 1;
            tri[counter * 3 + 1] = ((i + 1) * 2) + 1;
            tri[counter * 3 + 2] = ((i) * 2) + 1;
            counter++;
        }
        //fill front
        for (int i = 1; i < p - 1; i++)
        {
            tri[counter * 3] = 0;
            tri[counter * 3 + 1] = ((i) * 2) ;
            tri[counter * 3 + 2] = ((i + 1) * 2) ;
            counter++;
        }
        
        return tri;
    }
    float prevsplit;
	void Update () {
	    if (baseHealthPieChart == null) baseHealthPieChart = FindObjectOfType<BaseHealthPieChart>();
	    
        split = 1 - baseHealthPieChart.target;
        if ( !Mathf.Approximately( prevsplit ,split))
        {
            prevsplit = split;
            UpdateMeshes();
        }
	}
}
