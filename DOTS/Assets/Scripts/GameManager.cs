using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] InputManager _inputManager;
    [SerializeField]
    bool isGameOver;
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }








    public static bool IsGameOver()
    => Instance.isGameOver;

    internal static void EndGame()
    {
      
        PlayerShootings.ResetWeaponLevel();
        UIManager.GameOverUI();
        EntityHandler.ResetEntities();
        Instance._inputManager.EnableInput = false;
        Instance.isGameOver = true;
    }

    internal static void AddScore(byte bonus)
    {
        throw new NotImplementedException();
    }
}
