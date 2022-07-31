using UnityEngine;

namespace Obstacle
{
    public class RotatorObstacle : MonoBehaviour
    {
        [SerializeField] private Enums.RotateDirection direction;
        [SerializeField] private float speed;

        private void Awake()
        {
            if (direction == Enums.RotateDirection.Left)
            {
                speed = -speed;
            }
        }

        private void Update()
        {
            transform.Rotate(0,speed*Time.deltaTime,0);
        }
    }
}