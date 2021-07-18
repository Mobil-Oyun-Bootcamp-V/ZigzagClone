using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileSpawner : MonoBehaviour
{
    // Object Pooling list
    private List<GameObject> _tilePool;
    private int _poolSize = 3;

    // For checking the new tiles position if its too much left or right of the screen
    private Camera _cam;
    
    // Prefab for tiles
    [SerializeField] private GameObject tilePrefab;
    // Last created tile
    private GameObject _lastTile;
    // First visible tile at back
    private GameObject _firstTile;
    // The position for first ever created tile
    private Vector3 _startPoint = new Vector3(-1.5f, 0, 2.5f);
    // Distance between last tile and player for creation of new ones if smaller than this
    private float _tileCreateDist = 10f;
    // Distance between first tile and player for destroying old ones if higher than this
    private float _tileDestroyDist = 3f;

    

    // Gets player for position control initializes parameters
    void Start()
    {
        _cam = Camera.main;
        
        Random.InitState(DateTime.Now.Millisecond);
        InstantiatePool();
        CreateTile();
    }
    
    private void InstantiatePool()
    {
        _tilePool = new List<GameObject>();
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject obstacle = Instantiate(tilePrefab);
            obstacle.SetActive(false);
            _tilePool.Add(obstacle);
        }
    }

    // Checks the distance between player and last created tile also player and first visible tile at back
    // and creates new ones or destroys olders
    // using coroutines could improve the performance here
    private void Update()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Play)
        {
            Vector3 playerPos = GameManager.Instance.Player.transform.position;
            Vector3 lastTilePos = _lastTile.transform.position;
            Vector3 firstTilePos = _firstTile.transform.position;
            if (IsPointCloserThan(playerPos, lastTilePos, _tileCreateDist))
            {
                CreateTile();
            }
            if (IsPointFurtherThan(playerPos, firstTilePos, _tileDestroyDist))
            {
                DestroyTile();
            }
        }    
    }

    // Checks point closer than distance and front of the origin
    private bool IsPointCloserThan(Vector3 origin, Vector3 point, float distance)
    {
        origin.y = 0;
        point.y = 0;
        float dist = Vector3.Distance(point, origin);
        return dist <= distance && point.z >= origin.z && point.x <= origin.x;
    }

    // Checks point further than distance and back of the origin
    private bool IsPointFurtherThan(Vector3 origin,Vector3 point,float distance)
    {
        origin.y = 0;
        point.y = 0;
        var dist = Vector3.Distance(point, origin);
        return dist >= distance && origin.z >= point.z && origin.x <= point.x;
    }

    // Sets new tiles position
    // if the random number is 0 puts new tile to forward, if its 1 puts it to left of last created tile
    // if new tile 0.3f left or 0.7f right of the screen change direction to keep it around 45 degree to xz axis
    // so the camera shouldnt move too much left to keep the player in screen
    private GameObject SetTileInitials(GameObject tile)
    {
        if (ReferenceEquals(tile, null))
        {
            tile = Instantiate(tilePrefab);
        }
        tile.SetActive(true);
        if (ReferenceEquals(_lastTile,null))
        {
            tile.transform.position = _startPoint;
        }
        else
        {
            var rnd = Random.Range(0, 2);
            tile.transform.position = _lastTile.transform.position + (Vector3.left * rnd) + (Vector3.forward * (1- rnd));
            Vector3 pos = _cam.WorldToViewportPoint(tile.transform.position);
            if (pos.x >= 0.7f || pos.x <= 0.3f)
            {
                rnd = 1 - rnd;
                tile.transform.position = _lastTile.transform.position + (Vector3.left * rnd) + (Vector3.forward * (1- rnd));

            }
        }
        
        _lastTile = tile;
        if (ReferenceEquals(_firstTile, null))
        {
            _firstTile = tile;
        }
        return tile;
    }
    
    // If avaible sets tile from pool or creates new one and adds to pool
    private void CreateTile()
    {
        for (int i = 0; i < _tilePool.Count; i++)
        {
            GameObject tile = _tilePool[i];
            if (tile.activeInHierarchy == false)
            {
                _tilePool[i] = SetTileInitials(tile);
                return;
            }
        }
        GameObject newTile = SetTileInitials(null);
        _tilePool.Add(newTile);
    }

    // Starts destroy phase of passed tile, and sets next one ready
    private void DestroyTile()
    {
        _firstTile.gameObject.GetComponent<TileController>().Dispose();
        _firstTile = _tilePool.Find(obj =>
            obj.activeInHierarchy && !obj.Equals(_firstTile) &&
            IsPointCloserThan(_firstTile.transform.position, obj.transform.position, 1f));
    }
}
