using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMVP : MonoBehaviour
{
    public Material material;
    public Camera worldCamera;

    void Start()
    {
        Matrix4x4 P = GL.GetGPUProjectionMatrix(worldCamera.projectionMatrix, true);
        Matrix4x4 V = Camera.main.worldToCameraMatrix;
        Matrix4x4 M = GetComponent<Renderer>().localToWorldMatrix;
        Matrix4x4 MV = V * M;
        Matrix4x4 MVP = P * V * M;
        material.SetMatrix("_MATRIX_MVP", MVP);
    }
}
