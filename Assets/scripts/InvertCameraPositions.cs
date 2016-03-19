using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class InvertCameraPositions : MonoBehaviour
{

    public Material mat;
    public Shader sh;
    public void Start() {
        //Camera.main.SetReplacementShader(sh, "");
        int smallest = Mathf.Min(Screen.currentResolution.height, Screen.currentResolution.width);
        Screen.SetResolution(smallest, smallest, true);
    }
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        //mat is the material containing your shader
        Graphics.Blit(source, destination, mat);
    }
}
