using adefagia.Graph;
using Unity.VisualScripting;
using UnityEngine;

namespace adefagia.Collections
{
    public interface ISelectableObject
    {
        public void Hover(bool value);
        public void Selected(bool value);
    }

    public enum SelectType
    {
        Robot,
        Grid
    }
    
    public class SelectableObject<T> where T: ISelectableObject
    {

        private T _objectHover;
        private T _objectSelect;

        // private MonoBehaviour _manager;
        
        private Select _selectComponent;

        public SelectableObject(MonoBehaviour manager)
        {
            // _manager = manager;
            _selectComponent = manager.GetComponent<Select>();
        }

        public void ChangeHover(T newObject)
        {
            // First time initialize
            if (_objectHover.IsUnityNull())
            {
                _objectHover = newObject;
                _objectHover.Hover(true);
            }
            else
            {
                // different grid disable old grid hover
                if (newObject.Equals(_objectHover)) return;
                
                _objectHover.Hover(false);
                _objectHover = newObject;
                _objectHover.Hover(true);
            }
        }
        
        public void NotHover()
        {
            // If grid hover already null, break
            if (_objectHover.IsUnityNull()) return;
            
            // Disable hover
            _objectHover.Hover(false);
            
            // Delete grid hover
            _objectHover = default;
        }
        
        public void ChangeSelect(T newObject)
        {
            // First time initialize
            if (_objectSelect.IsUnityNull())
            {
                _objectSelect = newObject;
                _objectSelect.Selected(true);
            }
            else
            {
                // different grid disable old grid hover
                if (newObject.Equals(_objectSelect)) return;
                
                _objectSelect.Selected(false);
                _objectSelect = newObject;
                _objectSelect.Selected(true);
            }
        }
        
        
        public Select GetSelectComponent()
        {
            return _selectComponent;
        }

        public T GetSelect()
        {
            return _objectSelect;
        }
    }
}