using System;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    PowerUpManager.PowerUpType type;
    float speed = 25f;
    public void Init(Vector3 pos, PowerUpManager.PowerUpType type)
    {
        GetComponent<MeshRenderer>().material.color = type == PowerUpManager.PowerUpType.Projectile ? Color.blue : Color.yellow;
        this.type = type;
        transform.position = pos;
    }

    private void Update()
    {
        transform.Translate(-Vector3.forward * Time.deltaTime * speed);


        if (transform.position.z < PlayerManager.BottomScreenPoint)
            Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PowerUpManager.GainedPowerUp(type);
            Destroy(this.gameObject);
        }
    }
}
