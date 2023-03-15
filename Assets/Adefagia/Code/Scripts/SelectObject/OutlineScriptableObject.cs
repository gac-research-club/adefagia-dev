using UnityEngine;

namespace Adefagia.SelectObject
{
    [CreateAssetMenu(fileName = "OutlineStyle", menuName = "ScriptableObjects/OutlineStyle")]
    public class OutlineScriptableObject : ScriptableObject
    {
        public Outline.Mode outlineMode;
        public Color outlineColor;
        [Range(0f, 10f)] public float outlineWidth;
    }
}