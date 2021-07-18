using UnityEngine;

// Class for tile effects
public class TileController : MonoBehaviour
{
    // Enum for creation (ascending) and destruction (descending) effects
    private enum TileMode
    {
        Ascending,Descending,Set
    }

    // current mode for tile
    private TileMode _mode;
    // ascending speed on creation
    private float _ascendingSpeed = 3f;
    // descending speed on destroy
    private float _descendingSpeed = 3f;
    // tiles transform
    private Transform _transform;

    // function for ascending the tile from -3 to 0 on creation, calls when object is visible in game after setActive(true)
    private void OnBecameVisible()
    {
        _mode = TileMode.Ascending;
        Vector3 pos = _transform.position;
        pos.y = -3;
        _transform.position = pos;
    }

    private void Awake()
    {
        _transform = transform;
    }

    // Ascends the tile 
    void Update()
    {
        switch (_mode)
        {
            case TileMode.Ascending:
                _transform.position += Vector3.up * (_ascendingSpeed * Time.deltaTime);
                if (_transform.position.y >= 0)
                {
                    Vector3 pos = _transform.position;
                    pos.y = 0;
                    _transform.position = pos;
                    _mode = TileMode.Set;
                }
                break;
            case TileMode.Descending:
                _transform.position += Vector3.down * (_descendingSpeed * Time.deltaTime);
                if (transform.position.y <= -3)
                {
                    gameObject.SetActive(false);
                }
                break;
        }
    }

    // for creating descending effect after player passed this tile
    public void Dispose()
    {
        _mode = TileMode.Descending;
    }
}
