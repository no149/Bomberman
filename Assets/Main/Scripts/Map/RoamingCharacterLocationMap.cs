using System.Collections.Generic;
using QuikGraph;
using QuikGraph.Algorithms.Search;
using System;
using UnityEngine;
using System.Linq;
class RoamingCharacterLocationMap
{
    string _tilemapName;
    Location _initLocation;
    IGraph<Location, QuikGraph.IEdge<Location>> _mapGraph;
    private UndirectedDepthFirstSearchAlgorithm<Location, IEdge<Location>> _searchAlgorithm;
    private List<Location> _locationsToVisit;
    int _currentLocationIndex;
    private int _direction = 1;
    private LocationMap _locationMap;
    private Location _currentLocation;

    public bool Ready { get; private set; }

    public RoamingCharacterLocationMap(Character character)
    {
        _locationMap = new LocationMap();
        _initLocation = GetCharacterLocalLocation(character);
    }

    private Location GetCharacterLocalLocation(Character character)
    {
        var localpos = _locationMap.ConvertWorldPositionToMapPosition(character.transform.position);
        return localpos;
    }

    public Vector2 ConvertToWorldPosition(Location characterLocation)
    {
        return _locationMap.ConvertMapLocationToWorldPosition(characterLocation);
    }

    public void Init()
    {
        var locationMatrix = _locationMap.GenerateMapStateMatrix();
        _mapGraph = _locationMap.GenerateGraph(locationMatrix);
        _locationsToVisit = new List<Location>(locationMatrix.Length);
        _searchAlgorithm = _locationMap.CreateTraversalAlgorithm();
        _searchAlgorithm.DiscoverVertex += GraphLocation_Discovered;
        _searchAlgorithm.FinishVertex += GraphLocation_Finished;
        _searchAlgorithm.Finished += Search_Finished;
        _searchAlgorithm.Compute(_initLocation);


    }

    private void GraphLocation_Finished(Location vertex)
    {
        _locationsToVisit.Add(vertex);
    }



    private void GraphLocation_Discovered(Location vertex)
    {
        _locationsToVisit.Add(vertex);

        //   _locationsToVisit.Add(vertex);
    }

    public Location GetNextLocation()
    {

        if (_mapGraph == null)
            throw new System.InvalidOperationException("Not initialized");


        if (_locationsToVisit.Count == 0)
        {
            return _currentLocation;
        }
        // else if (_currentLocationIndex == 0)
        // {
        //     _direction = 1;
        // }
        // var edges = _searchAlgorithm.VisitedGraph.AdjacentEdges(_currentLocation).ToArray();
        // ShuffleEdges(edges);
        _currentLocation = _locationsToVisit[_currentLocationIndex++];

        //       _currentLocationIndex += _direction;
        return _currentLocation;
    }

    private IEdge<Location>[] ShuffleEdges(IEdge<Location>[] edges)
    {
        if (edges.Count() == 1)
            return edges.ToArray();
        var rand = new System.Random();
        for (int i = 0; i < edges.Length; i++)
        {
            var rndIndex = rand.Next(edges.Length - 1);
            edges[i] = edges[rndIndex];
        }
        throw new NotImplementedException();
    }
    private int[] CreateRandomIndexes(int[] arr)
    {
        //loop thru arr
        // swap indexes
        // if last item, return
        // else recurse
        throw new NotImplementedException();

    }
    //could be used to randonly select a path.
    // void Permuate(int[] arr, int ix)
    // {
    //     if (ix == arr.length)
    //     {

    //         for (var k = 0; k < arr.length; k++)
    //         {
    //             console.log(arr[k]);
    //         }
    //         console.log("done")
    //  }
    //     else
    //         for (var i = ix; i < arr.length; i++)
    //         {

    //             var temp = arr[i];
    //             arr[i] = arr[ix];
    //             arr[ix] = temp;
    //             perm(arr, i + 1);
    //             temp = arr[i];
    //             arr[i] = arr[ix];
    //             arr[ix] = temp;
    //         }
    // }
    private void Search_Finished(object sender, EventArgs e)
    {
        ConnectDisjointLocations();
        Ready = true;
    }

    private void ConnectDisjointLocations()
    {
        for (int i = 0; i < _locationsToVisit.Count; i++)
        {
            if (i - 1 >= 0)
            {
                var prevLocation = _locationsToVisit[i - 1];
                var currentLocation = _locationsToVisit[i];
                IEdge<Location> connectingEdge;
                var hasConnectingEdge = _searchAlgorithm.VisitedGraph.TryGetEdge(prevLocation, currentLocation, out connectingEdge);
                if (!hasConnectingEdge)
                {
                    var fromPrevLocationEdges = _searchAlgorithm.VisitedGraph.AdjacentEdges(prevLocation);
                    foreach (var edge in fromPrevLocationEdges)
                    {

                        var hasToCurrentLocationEdge = _searchAlgorithm.VisitedGraph.TryGetEdge(edge.Target, currentLocation, out connectingEdge);
                        if (hasToCurrentLocationEdge)
                        {
                            _locationsToVisit.Insert(i, edge.Target);
                            i++;
                            break;
                        }
                    }
                }
            }
        }
    }
}