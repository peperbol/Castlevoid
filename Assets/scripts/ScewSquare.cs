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
        m.vertices = new Vector3[]
            {
                new Vector3(-0.5f,-0.5f),
                new Vector3(-0.5f,0.5f),
                new Vector3(0.5f,0.5f),
                new Vector3(0.5f,-0.5f),
                new Vector3(0,0)
            };
        m.triangles = new int[] {
            4, 0, 1,
            4, 1, 2,
            4, 2, 3,
            4, 3, 0
            };
        m.uv = new Vector2[]
           {
               new Vector2(0,0),
               new Vector2(0,1),
               new Vector2(1,1),
               new Vector2(1,0),
               new Vector2(0.5f,0.5f)
           };

    }
    void OptimizeSize()
    {
        Vector3[] v = meshFilter.mesh.vertices;
        float ys = v.Max(e => e.y) - v.Min(e => e.y);
        Vector3 center = new Vector3(v.Average(e => e.x), v.Average(e => e.y));
        for (int i = 0; i < v.Length; i++)
        {
            v[i] -= center;
            v[i] = v[i] / ys;
        }
        v[4] = Vector3.zero;
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
            }
            UpdateVisual();
        }
    }
    void UpdateVisual()
    {
        if (movingEdge)
        {
            visual.localScale = new Vector3(visualWidth, visualWidth, Vector3.Distance(meshFilter.mesh.vertices[movingId], meshFilter.mesh.vertices[(movingId + 1) % 4]));
            visual.localPosition = (meshFilter.mesh.vertices[movingId] + meshFilter.mesh.vertices[(movingId + 1) % 4]) / 2;
            visual.LookAt(meshFilter.transform.position + Vector3.Scale(meshFilter.mesh.vertices[movingId], meshFilter.transform.lossyScale));
        }
        else
        {
            visual.localScale = Vector3.one * visualWidth;
            visual.localPosition = meshFilter.mesh.vertices[movingId];
        }
    }
    void MovePoint(int i, float x, float y)
    {
        Vector3[] v = meshFilter.mesh.vertices;
        v[i] += new Vector3(x, y) * MoveSpeed * Time.deltaTime;
        meshFilter.mesh.vertices = v;
    }
}
