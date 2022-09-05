using System;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TileMapHelper
{
    Tilemap _tilemap;
    Location[,] _tilesMatrix;


    internal static Vector3 ConvertLocationToWorldPosition(Location objectTilemaplocation, string tileMapName, bool center)
    {
        var tilemap = GameObject.Find(tileMapName).GetComponent<Tilemap>();
        return center ? tilemap.GetCellCenterWorld(new Vector3Int((int)objectTilemaplocation.x, (int)objectTilemaplocation.y, 1)) :
            tilemap.CellToWorld(new Vector3Int((int)objectTilemaplocation.x, (int)objectTilemaplocation.y, 1));
    }

    internal static Vector3 GetObjectTileMapPosition(Vector2 objectGlobalPosition, string tileMapName)
    {
        var tilemap = GameObject.Find(tileMapName).GetComponent<Tilemap>();
        return tilemap.WorldToLocal(objectGlobalPosition);
    }
    internal static Location[,] GenerateTilesMatrix(string tileMapName)
    {
        var tilemap = GameObject.Find(tileMapName).GetComponent<Tilemap>();

        tilemap.CompressBounds();
        var tilesMatrix = new Location[tilemap.size.x, tilemap.size.y];

        for (int i = 0; i < tilesMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < tilesMatrix.GetLength(1); j++)
            {
                var x = (int)(tilemap.origin.x + i * tilemap.cellSize.x);
                var y = (int)(tilemap.origin.y + j * tilemap.cellSize.y);
                var hastile = tilemap.HasTile(new Vector3Int(x, y));
                tilesMatrix[i, j] = new Location(x, y, !hastile);
            }
        }
        return tilesMatrix;
    }

    internal static Tilemap GetTileMap(string tileMapName)
    {
        var tilemap = GameObject.Find(tileMapName);
        if (tilemap != null)
            return tilemap.GetComponent<Tilemap>();
        else
            return null;
    }
}