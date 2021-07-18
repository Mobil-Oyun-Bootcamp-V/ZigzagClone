using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameManager()
    {
    }

    private static GameManager manager;

    public static GameManager Instance => manager;

    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
            DontDestroyOnLoad(this);
        }
        else if (manager != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Player = FindObjectOfType<PlayerController>()?.gameObject;
    }

    public enum GameState
    {
        Start,
        Play,
        End
    }

    public GameState CurrentState { get; private set; } = GameState.Start;
    public GameObject Player { get; private set; }

    public void StartGame()
    {
        CurrentState = GameState.Play;
    }


    public void EndGame()
    {
        CurrentState = GameState.End;
        FindObjectOfType<UIController>().EndGame();
    }
}
