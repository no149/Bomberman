using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using QuikGraph;
using QuikGraph.Graphviz;
using QuikGraph.Algorithms.Search;
using QuikGraph.Algorithms;

class LocationMap
{
    private const string GroundTileMapName = "Ground";
    private const string TreesTileMapName = "Trees";
    private UndirectedGraph<Location, IEdge<Location>> _graph;
    public LocationMap()
    {
    }

    public IGraph<Location, IEdge<Location>> Graph { get { return _graph; } }
    public Vector3 ConvertMapLocationToWorldPosition(Location objectTilemaplocation)
    {
        return TileMapHelper.ConvertLocationToWorldPosition(objectTilemaplocation, GroundTileMapName, true);

    }
    public Location ConvertWorldPositionToMapPosition(Vector2 objectGlobalPosition)
    {
        var mapposition = TileMapHelper.GetObjectTileMapPosition(objectGlobalPosition, GroundTileMapName);
        return new Location((int)mapposition.x, (int)mapposition.y, false);
    }
    public Location[,] GenerateMapStateMatrix()
    {
        var boxes = GameManager.GameManagerInstance.Boxes.Select(b => b.gameObject);
        var groundTiles = TileMapHelper.GenerateTilesMatrix(GroundTileMapName);
        var treesTileMap = TileMapHelper.GetTileMap(TreesTileMapName);

        groundTiles = MarkBlockedLocations(groundTiles, boxes, treesTileMap);
        return groundTiles;
    }

    private Location[,] MarkBlockedLocations(Location[,] baseTiles, IEnumerable<UnityEngine.GameObject> blockingGameObjects, params Tilemap[] blockingTileMaps)
    {
        for (int i = 0; i < baseTiles.GetLength(0); i++)
        {
            for (int j = 0; j < baseTiles.GetLength(1); j++)
            {
                var groundTileLocation = baseTiles[i, j];
                if (groundTileLocation.blocked)
                    continue;

                var groundTileWorldPosition = TileMapHelper.ConvertLocationToWorldPosition(groundTileLocation, GroundTileMapName, false);
                foreach (var gameObject in blockingGameObjects)
                {
                    var boxCollidesTile = gameObject.GetComponent<UnityEngine.BoxCollider2D>().OverlapPoint(groundTileWorldPosition);
                    groundTileLocation.blocked = boxCollidesTile;
                    if (boxCollidesTile)
                        break;
                }

                if (groundTileLocation.blocked)
                {
                    baseTiles[i, j] = groundTileLocation;
                    continue;
                }
                foreach (var tilemap in blockingTileMaps)
                {
                    if (tilemap.HasTile(new Vector3Int((int)groundTileLocation.x, (int)groundTileLocation.y, 0)))
                    {
                        groundTileLocation.blocked = true;
                        break;
                    }
                }
                if (groundTileLocation.blocked)
                {
                    baseTiles[i, j] = groundTileLocation;
                }
            }
        }
        return baseTiles;
    }

    private Tilemap GetTreesTileMap()
    {
        return UnityEngine.GameObject.Find("Trees").GetComponent<Tilemap>();
    }

    public UndirectedGraph<Location, QuikGraph.IEdge<Location>> GenerateGraph(Location[,] tilesMatrix)
    {
        var edgelist = new List<QuikGraph.Edge<Location>>(tilesMatrix.Length);
        for (int i = 0; i < tilesMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < tilesMatrix.GetLength(1); j++)
            {
                var source = tilesMatrix[i, j];
                if (source.blocked)
                    continue;

                if (i > 0)
                {
                    var edge1 = CreateEdge(source, tilesMatrix[i - 1, j]);
                    edgelist.Add(edge1);
                }
                if (i < tilesMatrix.GetLength(0) - 1)
                {
                    var edge2 = CreateEdge(source, tilesMatrix[i + 1, j]);
                    edgelist.Add(edge2);

                }
                if (j > 0)
                {
                    var edge3 = CreateEdge(source, tilesMatrix[i, j - 1]);
                    edgelist.Add(edge3);

                }
                if (j < tilesMatrix.GetLength(1) - 1)
                {
                    var edge4 = CreateEdge(source, tilesMatrix[i, j + 1]);
                    edgelist.Add(edge4);
                }
            }
        }

        _graph = QuikGraph.GraphExtensions.ToUndirectedGraph<Location, QuikGraph.IEdge<Location>>(edgelist, false);
        return _graph;
    }
    Edge<Location> CreateEdge(Location source, Location dest)
    {
        return new QuikGraph.Edge<Location>(source, dest); ;

    }
    public UndirectedDepthFirstSearchAlgorithm<Location, IEdge<Location>> CreateTraversalAlgorithm()
    {
        return new UndirectedDepthFirstSearchAlgorithm<Location, IEdge<Location>>(_graph);
    }

    private void SceneSetupDone(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

}