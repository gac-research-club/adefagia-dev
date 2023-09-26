using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.Shift
{
    [ExecuteInEditMode]
    public class EditorDemoPositionFix : MonoBehaviour
    {
        public List<RectTransform> objectToRepaint;

#if UNITY_EDITOR
        void Awake()
        {
            if (Application.isPlaying == true || objectToRepaint.Count == 0)
                return;

            // Rebuilding the rect in case of incorrect layout calculation
            for (int i = 0; i < objectToRepaint.Count; ++i)
            {
                if (objectToRepaint[i] != null)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(objectToRepaint[i].GetComponentInParent<RectTransform>());
                    LayoutRebuilder.ForceRebuildLayoutImmediate(objectToRepaint[i]);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(objectToRepaint[i]);
                }
            }
        }
#endif
    }
}