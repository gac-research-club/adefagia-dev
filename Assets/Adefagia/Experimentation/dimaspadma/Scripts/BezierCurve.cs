using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class BezierCurve : MonoBehaviour
{
    public Transform start;
    public Transform end;
    
    public float amount = 4f;
    [SerializeField] private float width = 1;
    // [Range(0f,1f)] 
    // [SerializeField] private float slider;
    private Vector3 curve;
    private Vector3 curve2;
    
    private bool _hasCall;
    
    private float _x;

    private Mesh _mesh;
    private List<Vector3> _points;
    private List<int> _triIndices;
    private List<Vector2> _uvs;

    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

    private void Start()
    {
        _mesh = new Mesh
        {
            name = "Procedural Quad"
        };

        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        
        ChangeAmount(5);
    }

    private void Update()
    {
        Trejactory();
    }

    private void Trejactory()
    {
        _mesh.Clear();

        var distance = Vector3.Distance(start.position, end.position);
        var up = new Vector3(0, distance, 0);
        curve = start.position + up;
        curve2 = end.position + up;
        
        
        for (int i = 0; i < amount+1; i++)
        {
            var position = DOCurve.CubicBezier.GetPointOnSegment(
                start.position, 
                curve, 
                end.position, 
                curve2, 
                i/amount);

            var offset = Vector3.Cross((end.position - start.position).normalized * width, Vector3.up);

            _points[2*i] = position - offset;
            _points[2*i + 1] = position + offset;

            _uvs[2 * i] = new Vector2(i, 1);
            _uvs[2 * i + 1] = new Vector2(i, 0);

            var iStart = 6 * i;
            
            if (i >= amount) continue;
            
            _triIndices[iStart] = 2 * i + 1;
            _triIndices[iStart+1] = 2 * i;
            _triIndices[iStart+2] = 2 * i + 2;

            _triIndices[iStart+3] = _triIndices[iStart];
            _triIndices[iStart + 4] = _triIndices[iStart + 2];
            _triIndices[iStart + 5] = 2 * i + 3;

        }
        
        _mesh.SetVertices(_points);
        _mesh.triangles = _triIndices.ToArray();
        _mesh.SetUVs(0, _uvs);
        
        _mesh.RecalculateNormals();

        _meshFilter.sharedMesh = _mesh;
    }

    public void ChangeAmount(int newAmount)
    {
        amount = newAmount;
        
        _points = new List<Vector3>();
        var pointAmount = ((int)amount + 1) * 2;
        for (int i = 0; i < pointAmount; i++)
        {
            _points.Add(Vector3.zero);
        }
        
        _triIndices = new List<int>();
        var triIndexAmount = (int)amount * 2 * 3;
        for (int i = 0; i < triIndexAmount; i++)
        {
            _triIndices.Add(0);
        }

        _uvs = new List<Vector2>();
        var uvAmount = ((int)amount + 1) * 2;
        for (int i = 0; i < uvAmount; i++)
        {
            _uvs.Add(Vector2.zero);
        }
    }

    public void Show()
    {
        _meshRenderer.enabled = true;
    }

    public void Hide()
    {
        _meshRenderer.enabled = false;
    }

    private void OnlyOne(float deltaTime)
    {
        if (_hasCall) return;
        
        Debug.Log(deltaTime);

        _hasCall = true;
    }
}
