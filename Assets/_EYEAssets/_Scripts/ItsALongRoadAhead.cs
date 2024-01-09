using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItsALongRoadAhead : MonoBehaviour
{
    [SerializeField] private GameObject[] _roadTiles;
    [SerializeField] private GameObject _tileHolder;
    
    private Vector3 _origin = Vector3.zero;
    private Vector3 _newTilePosition= Vector3.zero;
    
    [SerializeField] private bool _isRandom;
    

    void Start()
    {
        SetupInitialRoadPieces();
    }

    private void SetupInitialRoadPieces()
    {
        if(_isRandom)
        {
            for (int i = 0; i < 27; i++)
            {
                var randomTile = Random.Range(0, _roadTiles.Length);

                var tile = Instantiate(_roadTiles[randomTile], _newTilePosition, Quaternion.identity);
                tile.transform.SetParent(_tileHolder.transform);
                _newTilePosition += new Vector3(0, 0, 5);
            }
        }
        else
        {
            for(int i = 0; i < 28; i++)
            { 
                var randomTile = Random.Range(0, _roadTiles.Length);

                var tile = Instantiate(_roadTiles[0], _newTilePosition, Quaternion.identity);
                tile.transform.SetParent(_tileHolder.transform);
                _newTilePosition += new Vector3(0, 0, 5);
            }
        }        
    }

    void Update()
    {
        
    }

    //set out a line of x number of tiles in a line.
    //move tiles at a constant rate in the -z direction
    //when tiles.z < the max limit then disable tile, reset at original spawn point and enable the tile
    //at some point it will be necessary to randomize the tiles based upon specified criteria
    //perhaps events can dictate when something might occur that you might need a bridge or war has devastated our transportation infrastructure.

    public void SpawnNewTile()
    {
        if(_isRandom)
        {
            var randomTile = Random.Range(0, _roadTiles.Length- 1);
            Instantiate(_roadTiles[randomTile], _origin, Quaternion.identity);
        }
        else
        {
            Instantiate(_roadTiles[0], transform.position, Quaternion.identity);
        }
    }




}
