using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] InputManager _inputManager;

  static  bool isGameOver;
    public static GameManager Instance;



    public static bool IsDots;
    private void Awake()
    {
        Instance = this;
      
    }








    public static bool IsGameOver()
    => isGameOver;

    internal static void EndGame()
    {
      
        PlayerShootings.ResetWeaponLevel();
        UIManager.GameOverUI();
        EntityHandler.ResetEntities();
        Instance._inputManager.EnableInput = false;
        isGameOver = true;
    }

    internal static void SetEntityWorld()
    {
      //  entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    internal static void SetEntityWorld(Action p)
    {
        p?.Invoke();
    }
}
