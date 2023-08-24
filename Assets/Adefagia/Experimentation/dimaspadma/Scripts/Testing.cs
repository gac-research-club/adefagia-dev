using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private CharacterAction characterAction;
    
    void Start()
    {
        // TileGenerator.ShowListHighlight(Deploy());
    }

    private List<Tile> Deploy()
    {
        var deploy = new List<Tile>();
        
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                var tile = TileGenerator.GetTile(x, y);
                tile.TileHighlight.ChangeAction(characterAction);
                deploy.Add(tile);
            }
        }

        return deploy;
    }

}
