using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class BulletShooter : MonoBehaviour
    {
        public AudioManager.PlayerSFX sfxToPlay;

        void Start()
        {
        }

        void Update()
        {
            // Shoot
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AudioManager.instance.PlaySound(sfxToPlay);
                ObjectPooling.instance.InstanceCreate(ObjectsToPool.playerBullet , 
                transform.position);
            }
        }
    }
}
