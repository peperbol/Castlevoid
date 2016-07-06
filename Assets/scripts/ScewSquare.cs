using UnityEngine;
using System.Collections;
using System.Linq;
public class ScewSquare : MonoBehaviour
{

    public MeshFilter meshFilter;
    public float MoveSpeed = 0.1f;
    public float visualWidth = 0.05f;
    public Color visualColor;
    public float deadZone;
    public int divisions = 2;
    bool isEnabled = false;
    int movingId = 0;
    bool movingEdge = true;
    bool usedNav;
    Transform visual;
    public Camera cam;
    bool Enabled
    {
        get
        {
            return isEnabled;
        }
        set
        {
            if (!isEnabled ^ value) return;
            isEnabled = value;
            if (!value)
            {
                OptimizeSize();
                cam.backgroundColor = Color.black;
                Destroy(visual.gameObject);
            }
            else
            {
                cam.backgroundColor = Color.white;
                visual = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
                visual.SetParent(meshFilter.transform);
                visual.GetComponent<Renderer>().material.color = visualColor;
            }
        }
    }
    void selectLeft()
    {

        Debug.Log("left");
        if (movingEdge)
        {
            switch (movingId)
            {
                case 0:
                    break;
                case 1:
                    movingEdge = false;
                    movingId = 1;
                    break;
                case 2:
                    movingId = 0;
                    break;
                case 3:
                    movingEdge = false;
                    movingId = 0;
                    break;
            }
        }
        else
        {
            switch (movingId)
            {
                case 0:
                case 1:
                    movingEdge = true;
                    movingId = 0;
                    break;
                case 2:
                    movingId = 1;
                    break;
                case 3:
                    movingId = 0;
                    break;

            }
        }
    }
    void selectRight()
    {

        Debug.Log("right");
        if (movingEdge)
        {
            switch (movingId)
            {
                case 0:
                    movingId = 2;
                    break;
                case 1:
                    movingEdge = false;
                    movingId = 2;
                    break;
                case 2:
                    break;
                case 3:
                    movingEdge = false;
                    movingId = 3;
                    break;
            }
        }
        else
        {
            switch (movingId)
            {
                case 0:
                    movingId = 3;
                    break;
                case 1:
                    movingId = 2;
                    break;
                case 2:
                case 3:
                    movingEdge = true;
                    movingId = 2;
                    break;

            }
        }
    }
    void selectUp()
    {

        Debug.Log("up");
        if (movingEdge)
        {
            switch (movingId)
            {
                case 0:
                    movingEdge = false;
                    movingId = 1;
                    break;
                case 1:
                    break;
                case 2:
                    movingEdge = false;
                    movingId = 2;
                    break;
                case 3:
                    movingId = 1;
                    break;
            }
        }
        else
        {
            switch (movingId)
            {
                case 0:
                    movingId = 1;
                    break;
                case 1:
                case 2:
                    movingEdge = true;
                    movingId = 1;
                    break;
                case 3:
                    movingId = 2;
                    break;

            }
        }
    }
    void selectDown()
    {
        Debug.Log("down");
        if (movingEdge)
        {
            switch (movingId)
            {
                case 0:
                    movingEdge = false;
                    movingId = 0;
                    break;
                case 1:
                    movingId = 3;
                    break;
                case 2:
                    movingEdge = false;
                    movingId = 3;
                    break;
                case 3:
                    break;
            }
        }
        else
        {
            switch (movingId)
            {
                case 3:
                case 0:
                    movingEdge = true;
                    movingId = 3;
                    break;
                case 1:
                    movingId = 0;
                    break;
                case 2:
                    movingId = 3;
                    break;

            }
        }
    }

    void Start()
    {
        MaxResolution();
        MakeMesh();
    }
    void MaxResolution()
    {
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);

    }
    void MakeMesh()
    {
        Mesh m = new Mesh();
        meshFilter.mesh = m;

        Vector3[] vertices = new Vector3[(2 + divisions) * (2 + divisions)];
        for (int x = 0; x < 2 + divisions; x++)
        {
            for (int y = 0; y < 2 + divisions; y++)
            {
                vertices[y + x * (2 + divisions)] = new Vector3(x / ((float)divisions + 1) -0.5f,  y /((float)divisions + 1)-0.5f);
            }
        }

        m.vertices = vertices;

        int[] triangles = new int[(1 + divisions) * (1 + divisions) * 6];

        for (int x = 0; x < 1 + divisions; x++)
        {
            for (int y = 0; y < 1 + divisions; y++)
            {
                triangles[(y + x * (1 + divisions)) * 6 + 0] = y   + (x  ) * (2 + divisions);
                triangles[(y + x * (1 + divisions)) * 6 + 1] = y+1 + (x+1) * (2 + divisions);
                triangles[(y + x * (1 + divisions)) * 6 + 2] = y   + (x+1) * (2 + divisions);
                triangles[(y + x * (1 + divisions)) * 6 + 3] = y   + (x  ) * (2 + divisions);
                triangles[(y + x * (1 + divisions)) * 6 + 4] = y+1 + (x  ) * (2 + divisions);
                triangles[(y + x * (1 + divisions)) * 6 + 5] = y+1 + (x+1) * (2 + divisions);
            }
        }

        m.triangles = triangles;

        Vector2[] uv = new Vector2[(2 + divisions) * (2 + divisions)];

        for (int x = 0; x < 2 + divisions; x++)
        {
            for (int y = 0; y < 2 + divisions; y++)
            {
                uv[y + x * (2 + divisions)] = new Vector2(x / ((float)divisions + 1) , y / ((float)divisions + 1) );
            }
        }

        m.uv = uv; 
       
    }
    void OptimizeSize()
    {
        
        Vector3[] v = meshFilter.mesh.vertices;

        Vector3 lt = v[divisions + 1];
        Vector3 lb = v[0];
        Vector3 rb = v[(divisions + 2) * (divisions + 1)];
        Vector3 rt = v[(divisions + 2) * (divisions + 2) - 1];
        float tb =  (lt - rt).magnitude / (lb - rb).magnitude;
        float rl =  (rb - rt).magnitude / (lt - lb).magnitude;

        // a * b^0 + a * b^1+ a * b^2 + ... + a * b^n = 1
        // a = 1 / (b^0 + b^1 + b^2 + ... + b^n)
        float btbase = 1;
        { 
            float[] e = new float[divisions + 1];
            for (int i = 0; i < divisions + 1; i++)
            {
                e[i] = Mathf.Pow(tb, i);
            }
            btbase = 1 / e.Sum();
        }
        float lrbase = 1;
        {
            float[] e = new float[divisions + 1];
            for (int i = 0; i < divisions + 1; i++)
            {
                e[i] = Mathf.Pow(rl, i);
            }
            lrbase = 1 / e.Sum();
        }
        {
            float last = 0;
            for (int y = 1; y < (divisions+1); y++)
            {
                last += btbase * Mathf.Pow(tb, y-1);
                v[y] = Vector3.Lerp(lb, lt, last);
            }
        }

        {
            float last = 0;
            for (int y = 1; y < (divisions+1); y++)
            {
                last += btbase * Mathf.Pow(tb, y-1);
                v[(divisions + 2) * (divisions + 1) + y] = Vector3.Lerp(rb, rt, last);
            }
        }

        for (int y = 0; y < (divisions+2); y++)
        {
            float last = 0;
            for (int x = 1; x < (divisions+1); x++)
            {
                last += lrbase * Mathf.Pow(rl, x-1);
                v[y + x * (2 + divisions)] = Vector3.Lerp(v[y], v[y + (divisions + 1) * (2 + divisions)], last);
            }
        }

        float ys = v.Max(e => e.y) - v.Min(e => e.y);
        meshFilter.mesh.vertices = v;
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Enabled = !Enabled;
        }
        if (Enabled)
        {

            if (Input.GetAxis("SkewNavHor") > 0)
            {
                if (!usedNav)
                    selectRight();
                usedNav = true;
            }
            else if (Input.GetAxis("SkewNavHor") < 0)
            {
                if (!usedNav)
                    selectLeft();
                usedNav = true;
            }
            else if (Input.GetAxis("SkewNavVert") > 0)
            {
                if (!usedNav)
                    selectDown();
                usedNav = true;
            }
            else if (Input.GetAxis("SkewNavVert") < 0)
            {
                if (!usedNav)
                    selectUp();
                usedNav = true;
            }
            else
            {
                usedNav = false;
            }

            if (Input.GetButtonDown("Attack1") || Input.GetButtonDown("Attack2"))
            {
                OptimizeSize();
            }

            float h = Input.GetAxis("HorizontalAny");
            float v = Input.GetAxis("VerticalAny");
            if (Mathf.Abs(v) > deadZone || Mathf.Abs(h) > deadZone)
            {
                if (movingEdge)
                {
                    MovePoint(movingId, (movingId == 1) ? -h : h, (movingId == 2) ? -v : v);
                    MovePoint((movingId + 1) % 4, (movingId == 3) ? -h : h, (movingId == 0) ? -v : v);

                }
                else
                {
                    MovePoint(movingId, Input.GetAxis("HorizontalAny"), Input.GetAxis("VerticalAny"));
                }

                OptimizeSize();
            }
            UpdateVisual();
        }
    }
    int selectedIndexToMeshIndex(int i)
    {
        switch (i)
        {
            case 0:
                return 0;
            case 1:
                return divisions + 1;
            case 2:
                return (divisions + 2) * (divisions + 2) - 1;
            case 3:
                return (divisions + 2) * (divisions + 1);
        }
        return 0;
    }
    void UpdateVisual()
    {
        if (movingEdge)
        {
            visual.localScale = new Vector3(visualWidth, visualWidth, Vector3.Distance(meshFilter.mesh.vertices[selectedIndexToMeshIndex(movingId)], meshFilter.mesh.vertices[selectedIndexToMeshIndex((movingId + 1) % 4)]));
            visual.localPosition = (meshFilter.mesh.vertices[selectedIndexToMeshIndex(movingId)] + meshFilter.mesh.vertices[selectedIndexToMeshIndex((movingId + 1) % 4)]) / 2;
            visual.LookAt(meshFilter.transform.position + Vector3.Scale(meshFilter.mesh.vertices[selectedIndexToMeshIndex(movingId)], meshFilter.transform.lossyScale));
        }
        else
        {
            visual.localScale = Vector3.one * visualWidth;
            visual.localPosition = meshFilter.mesh.vertices[selectedIndexToMeshIndex(movingId)];
        }
    }
    void MovePoint(int i, float x, float y)
    {
        Vector3[] v = meshFilter.mesh.vertices;
        int index = 0;

        v[selectedIndexToMeshIndex(i)] += new Vector3(x, y) * MoveSpeed * Time.deltaTime;
        meshFilter.mesh.vertices = v;
    }
}
