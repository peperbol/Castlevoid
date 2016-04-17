using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class InvertCameraPositions : MonoBehaviour
{

    public Material mat;
    public Shader sh;
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        //mat is the material containing your shader
        Graphics.Blit(source, destination, mat);
    }
}
