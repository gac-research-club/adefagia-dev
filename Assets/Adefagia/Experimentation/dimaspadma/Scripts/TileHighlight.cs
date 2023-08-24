using UnityEngine;

public class TileHighlight
{
   private GameObject _gameObject;
   private CharacterAction _characterAction;

   public TileHighlight(Tile tile, TileObject tileObject, Transform parent)
   {
      var highlightPrefab = tileObject.prefab;
      highlightPrefab.transform.position = tile.Position;
        
      _gameObject = Object.Instantiate(highlightPrefab, parent);
      
      // Hide first
      Hide();
   }

   public void ChangeAction(CharacterAction action)
   {
      _characterAction = action;
      _gameObject.GetComponent<TileVisual>().ChangeColor(action.color);
   }

   public void Hide()
   {
      _gameObject.SetActive(false);
   }

   public void Show()
   {
      _gameObject.SetActive(true);
   }
}
