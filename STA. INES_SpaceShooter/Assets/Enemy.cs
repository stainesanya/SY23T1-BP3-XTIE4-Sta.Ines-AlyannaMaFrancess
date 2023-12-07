using System.Collections;
using System.Collections.Generic;
using SpaceShooter;
using UnityEngine;
namespace SpaceShooter
{
 public interface IEnemy
    {
        int CurrentHP { get; set; } //total hits from player bullet before it "dies"
        float GetHorizontalSpeed { get; } //how fast does it move going left?

        void OnEnable(); //Upon creation (from object pool), you start the "ShootInterval" IEnumerator here.
        void Update(); //With constant functions and checkers (like HP)

        void Move(); //Move and check if off screen
        IEnumerator ShootInterval(); //Follow how the enemy shoots with a specific interval.
    }

public class Enemy : MonoBehaviour
{
  
  public float moveSpeed = 4f;
    private void Update()
    {
        transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);

        if (transform.position.x <GameManager.instance.CameraBounds.x)
        ObjectPooling.instance.InstanceReturn(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("playerBullet"))
        {
         if (other.gameObject.GetComponent<Enemy>() != null)
              ObjectPooling.instance.InstanceReturn(other.gameObject); 
             ObjectPooling.instance.InstanceReturn(gameObject); 
        }
       
    }
}

}
