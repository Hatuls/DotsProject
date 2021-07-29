
using UnityEngine.Events;
using UnityEngine;

public static class SpawnWaveEvent 
{
    public static UnityAction<SpawnWave> Event;// = new UnityAction<SpawnWave>((sw) => { });

    public static void RaiseEvent(SpawnWave sw)
    {
        Event?.Invoke(sw);
    }
}
public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject powerup;
    public Weapon Weapon { get; set; }
    public SpawnWave SpawnWave { get; set; }
    private byte currentIndex;
    public bool IsAtFinalPosition { get; set; }
    public float Speed { get; set; }

    public int ID;

    Vector3 nextPos;

    public void DropPowerUp()
    {
        PowerUpManager.PowerUpType p = PowerUpManager.PowerUpType.None;
        switch ( Random.Range(0, 30)%3)
        {
            case 1:
                p = PowerUpManager.PowerUpType.Projectile;
                break;
            case 2:
                p = PowerUpManager.PowerUpType.Speed;
                break;
            case 0:
            default:
                return;
                
        }

        Instantiate(powerup).GetComponent<PowerUp>().Init(this.transform.position,p);
    }
    private void Start()
    {
        currentIndex = 0;
        GotToLocation();
    }
    private void GotToLocation()
    {
        if (currentIndex < SpawnWave.Road.Points.Length - 1)
        {
            nextPos = SpawnWave.Road.Points[currentIndex + 1];
            RotateToNextPos(nextPos,transform.position);

        }
        else if (currentIndex == SpawnWave.Road.Points.Length)
        {
            RotateToNextPos(Vector3.down,Vector3.zero );
            IsAtFinalPosition = true;
        }
        else
        {
            nextPos = SpawnWave.GetLastPosition(ref ID);
            RotateToNextPos(nextPos, transform.position);
        }
        currentIndex++;
    }
    private void RotateToNextPos(Vector3 pointA,Vector3 pointB)
    {
        var direction = pointB - pointA;
        direction.y = 0;
       transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
      
        if (IsAtFinalPosition == false)
        {

            if (Vector3.Distance(transform.position, nextPos) < 1)
            {
                GotToLocation();
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, nextPos, Time.deltaTime * Speed);
            }
        }
        else
        {
            if (currentTime >= shootDelay)
            {
                currentTime = 0;
                ShootProjectile();

            }
            else
            {
                currentTime += Time.deltaTime;
            }
        }

    }
    float shootDelay =3f;
    float currentTime;
    public void DestroyShip()
    {
        DropPowerUp();
      Destroy(this.gameObject);
    }


    private void ShootProjectile()
    {

        BulletManager.Instance.DeployBullet(false, null, -Vector3.forward, transform.position, 1);
    }
}
