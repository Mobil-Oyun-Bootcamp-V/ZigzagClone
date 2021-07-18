using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float _speed = 2f;
 
    void Update()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Play)
        {
            Vector3 pos = transform.position;
            Vector3 playerPos = GameManager.Instance.Player.transform.position;
            pos += new Vector3(-.1f, 0, .1f * ((playerPos.z - pos.z)/4f)) * (_speed * Time.deltaTime);
            transform.position = pos;
        }
    }
    
}
