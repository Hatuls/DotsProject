using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Scenes;
using UnityEngine.SceneManagement;
public class SceneHandler : MonoBehaviour
{ 






    public void MoveToScene(int num)
    {
        GameManager.IsDots =num == 2;
        SceneManager.LoadScene(num);
    }
}
