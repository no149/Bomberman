using UnityEngine;
interface ITileMap
{
    Vector3 ConvertLocationToWorldPosition(Location tilemaplocation);
    Vector3 GetObjectTileMapPosition(Vector2 objectGlobalPosition);
}