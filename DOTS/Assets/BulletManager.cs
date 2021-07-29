
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] float forwardoffest;
    [SerializeField] float gapoffset;
    [SerializeField] GameObject bulletPrefab;
  public static BulletManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void OnDisable()
    {
  
        
    }
    public void DeployBullet(bool isPlayer,Weapon weapon ,Vector3 direction,Vector3 position, int level)
    {
        float middleOffset = 2;
        Vector3 pos;
        for (int i = 0; i < level; i++)
        {
            var bullet=   Instantiate(bulletPrefab).GetComponent<Bullet>();
                bullet.transform.rotation = Quaternion.LookRotation(direction);
            pos = position + bullet.transform.forward * forwardoffest ;

            if (level > 1)
            {
                pos = new Vector3()
                {
                    x = (pos.x + i * gapoffset) - (gapoffset * (level - 1)) / middleOffset,
                    y = pos.y,
                    z = pos.z
                };
            }

            bullet.transform.position = pos;
            bullet.Init(isPlayer, weapon, direction.normalized);
        }
    }
}
