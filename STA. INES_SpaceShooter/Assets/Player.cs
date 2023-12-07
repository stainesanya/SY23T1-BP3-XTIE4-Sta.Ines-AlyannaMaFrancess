using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed = 4;

        public float boostedSpeed = 8;

        private Vector2 screenBounds;
        private float objectWidth;
        private float objectHeight;

        void Start()
        {
            screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
            objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
            objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        }

    
        void Update()
        {
            bool isShiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            float currentSpeed = isShiftHeld ? boostedSpeed : moveSpeed;

            transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * currentSpeed,
            Input.GetAxis("Vertical") * Time.deltaTime * currentSpeed, 0);

            Vector3 viewPos = transform.position;
            viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
            viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
            transform.position = viewPos;
        }
    }
}