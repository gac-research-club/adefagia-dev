using System.Collections.Generic;
using Adefagia.SelectObject;
using UnityEngine;

namespace Adefagia.RobotSystem
{
    public class RobotStatus : MonoBehaviour
    {
        public Material selectMaterial;
        public Material hoverMaterial;

        private List<Material> _defaultMaterial;
        private List<MeshRenderer> _meshRenderer;

        public float health;
        
        public Robot Robot { get; set; }

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
            health = Robot.CurrentHealth;
            
            if (Robot.IsSelect)
            {
                ChangeMaterial(selectMaterial);
            }
            else if (Robot.IsHover)
            {
                // Debug.Log("Hover");
                ChangeMaterial(hoverMaterial);
                AddOutlineComponent(Robot.OutlineStyle);
            }
            else
            {
                ResetMaterial();
                RemoveOutlineComponent();
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
        
        /*--------------------------------------------------------------------------
        * Add outline component to gameObject
        *--------------------------------------------------------------------------*/
        private void AddOutlineComponent(OutlineScriptableObject outlineStyle)
        {
            var outline = gameObject.GetComponent<Outline>();

            // if outline not found, create first
            if (outline == null)
            {
                gameObject.AddComponent<Outline>();
                
                outline = gameObject.GetComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineVisible;
                outline.OutlineColor = Color.red;
                outline.OutlineWidth = 8f;
            }
            else
            {
                outline.enabled = true;
            }
        }
        
        private void RemoveOutlineComponent()
        {
            var outline = gameObject.GetComponent<Outline>();

            if (outline != null)
            {
                outline.enabled = false;
            }
        }
    }
}
