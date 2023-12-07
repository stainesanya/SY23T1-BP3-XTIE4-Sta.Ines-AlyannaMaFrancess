using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class PlayerBullet : MonoBehaviour
    {
        [SerializeField]
        private float Speed = 18;

        void Update()
        {
            transform.Translate(Time.deltaTime * Speed, 0, 0);

            if (transform.position.x > -GameManager.instance.CameraBounds.x)
                ObjectPooling.instance.InstanceReturn(gameObject); 
        }
    }
}
