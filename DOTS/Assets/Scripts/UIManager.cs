using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static UIManager Instance;

    [SerializeField]
    TextMeshProUGUI _scoreText;

    [SerializeField]
    TextMeshProUGUI _bulletText;

    [SerializeField]
    GameObject _gameOverText;

    [SerializeField]
    TextMeshProUGUI _enemyCounter;

    [SerializeField]
    TextMeshProUGUI _bulletPerShoot;
    
    [SerializeField]
    TextMeshProUGUI _fpsCounter;

   public static int totalAmountOfBullets;
   public static int totalAmountOfEnemys;
    private void Awake()
    {
        Instance = this;
    }

    public static void GameOverUI() => Instance._gameOverText.SetActive(true);
    // Update is called once per frame
    public static void SetMisslesPerShoot(int amount)
           => Instance._bulletPerShoot.text = string.Concat("Missiles Per Shoot: ",amount);
    public static void RegisterEnemy()
    {
        totalAmountOfEnemys++;

        Instance._enemyCounter.text =string.Concat("Enemy Created: ", totalAmountOfEnemys);
    }
    public static void RegisterBullt()
    {
    
        totalAmountOfBullets++;
        Instance._bulletText.text = string.Concat("Bullet Count: ", totalAmountOfBullets);

    }
    internal static void AddScore(ref int amount)
    {
        Instance._scoreText.text = string.Concat("Score Count: ", amount);
    }
    float deltaTime = 0.0f;
    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        _fpsCounter.text = string.Concat("FPS: ", 1f/ deltaTime);
    }
}
