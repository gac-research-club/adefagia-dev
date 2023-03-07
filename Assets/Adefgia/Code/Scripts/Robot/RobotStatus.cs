using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace adefagia
{
    public class RobotStatus : MonoBehaviour
    {
        public Material selectMaterial;
        public Material hoverMaterial;

        private List<Material> _defaultMaterial;
        private List<MeshRenderer> _meshRenderer;
        
        public Robot.Robot Robot { get; set; }

        private void Awake()
        {
            _defaultMaterial = new List<Material>();
            _meshRenderer = new List<MeshRenderer>();
            
            foreach (Transform child in transform)
            {
                var partRobot = child.GetComponent<MeshRenderer>();
                _meshRenderer.Add(partRobot);
                _defaultMaterial.Add(partRobot.material);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Robot.IsSelect)
            {
                ChangeMaterial(selectMaterial);
            }
            else if (Robot.IsHover)
            {
                // Debug.Log("Clicked");
                ChangeMaterial(hoverMaterial);
            }
            else
            {
                ResetMaterial();
            }
        }
        
        private void ChangeMaterial(Material material)
        {
            foreach (var robot in _meshRenderer)
            {
                robot.material = material;
            }
        }
        
        private void ResetMaterial()
        {
            for (var i = 0; i < _defaultMaterial.Count; i++)
            {
                _meshRenderer[i].material = _defaultMaterial[0];
            }
        }
    }
}
