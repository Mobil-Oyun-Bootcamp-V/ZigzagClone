using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    
    private Rigidbody _rb;
    // players movement direction 0 : left (-x) 1 : forward (+z)
    private int _direction = 0;
    // players movement speed
    [SerializeField] private float speed = 200f;
    private float _gravityValue = -9.81f;

    public UnityAction onScore;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // offset is 0.5 left if moving left, 0.5 forward if moving forward
    // on autoplay mode checks the offset position is above the ground, changes direction otherwise
    // changes direction on touch, 0 for left 1 for forward
    private void Update()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Play)
        {
            if (GameSettings.autoPlayOn)
            {
                Vector3 offset = (new Vector3(-0.5f,0,0) * (1 - _direction)) + (new Vector3(0,0,0.5f) * _direction);
                if (!Physics.Raycast(transform.position+offset,Vector3.down))
                {
                    ChangeDirection();
                }
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    if (!EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                    {
                        if (Input.touches[0].phase == TouchPhase.Began)
                        {
                            ChangeDirection();
                        }
                    } 
                }
            }
        } 
    }

    private void ChangeDirection()
    {
        _direction = 1 - _direction;
        onScore?.Invoke();
    }

    private Vector3 _playerVelocity;
    // moves the player according to direction and adds gravitational force
    private void FixedUpdate()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Play)
        {
            if (IsGrounded())
            {
                _playerVelocity = (_direction == 0 ? Vector3.left : Vector3.forward) * (speed * Time.fixedDeltaTime);
                _playerVelocity.y += Time.deltaTime * _gravityValue;
                _rb.velocity = _playerVelocity;
            }
            else
            {
                GameManager.Instance.EndGame();
            }
            
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down);
    }
}
