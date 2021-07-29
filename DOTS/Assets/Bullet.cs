
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed=40;
    Vector3 direction;
    Weapon weapon;
    bool isPlayer;
    public void Init(bool isPlayer, Weapon weapon,Vector3 dir)
    {
        direction = dir;
        this.isPlayer = isPlayer;
        this.weapon = weapon;
        GetComponentInChildren<MeshRenderer>().material.color = isPlayer ? Color.green : Color.red; 
    }
    private void Update()
    {


        if (transform.position.z < PlayerManager.BottomScreenPoint || transform.position.z > PlayerManager.TopScreenPoint)
            Destroy(this.gameObject);


        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
        {
        if (other.tag == "Player" && isPlayer== false)
        {
            GameManager.EndGame();
            Destroy(this.gameObject);
        }
        else if (other.tag=="Enemy" && isPlayer== true)
        {
            other.GetComponent<Enemy>().DestroyShip();
            Destroy(this.gameObject);
        }
    }
}