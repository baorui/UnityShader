using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 创建depth相机
/// by lijia
/// </summary>
public class LightCameraShadow : MonoBehaviour
{
    RenderTexture _rt;
    public Transform lightTrans;
    public Camera _camera;
    public Material material;

    Matrix4x4 sm = new Matrix4x4();

    void Start()
    {
        _camera.transform.position = lightTrans.position;
        _camera.transform.rotation = lightTrans.rotation;
        _camera.transform.parent = lightTrans;

        //_camera.orthographic = true;
        //_camera.orthographicSize = 10;

        //0.5   0       0       0.5
        //0     0.5     0       0.5
        //0     0       0.5     0.5
        //0     0       0       1
        sm.m00 = 0.5f;
        sm.m11 = 0.5f;
        sm.m22 = 0.5f;
        sm.m03 = 0.5f;
        sm.m13 = 0.5f;
        sm.m23 = 0.5f;
        sm.m33 = 1;

        _rt = new RenderTexture(1024, 1024, 0);
        _rt.wrapMode = TextureWrapMode.Clamp;
        _camera.targetTexture = _rt;
        _camera.SetReplacementShader(Shader.Find("Shadow/DepthTexture"), "RenderType");
    }

    void LateUpdate()
    {
        Matrix4x4 tm = GL.GetGPUProjectionMatrix(_camera.projectionMatrix, false) * _camera.worldToCameraMatrix;
        tm = sm * tm;
        Shader.SetGlobalMatrix("_ProjectionMatrix", tm);
        Shader.SetGlobalTexture("_DepthTexture", _rt);
    }


}
