using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake
{
    public class Square : MonoBehaviour
    {
        [SerializeField] SpriteRenderer SpriteRenderer;
        public bool isOccupied;
        public Vector2Int gridPos;

        public Color color
        {
            set { SpriteRenderer.color = value; }
        }

        void Start()
        {
            
        }
    }
}
