using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class enemyShooter : MonoBehaviour
    {
        [SerializeField]
        private GameObject bullets;
        [SerializeField]
        private float shootForce = 10f;
        [SerializeField]
        private float shootInterval = 1.5f;

        private float lastShootTime;

        private void Update()
        {
            
            if (Time.time - lastShootTime >= shootInterval)
            {
                
                ShootBullet();
                lastShootTime = Time.time;
            }
        }

        private void ShootBullet()
        {
            
            GameObject bullet = Instantiate(bullets, transform.position, Quaternion.identity);
            Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();

            
            bulletRigidbody.velocity = new Vector2(-shootForce, 0);
        }
    }
}
