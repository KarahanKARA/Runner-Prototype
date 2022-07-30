using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float swipeSpeed;
        [SerializeField] private float forwardSpeed;
        public LayerMask Ground;
        private Touch _touch;
        private bool _isDragging;
        private Vector3 _currentCursorPos;
        private Vector3 _oldCursorPos;
        private Rigidbody r;

        private void Awake()
        {
            Application.targetFrameRate = 500;
        }

        private void Update()
        {
            SwipeMovement();
            ForwardMovement();
        }

        private void SwipeMovement()
        {
            // if (!GameManager.instance.isGameOver || !GameManager.instance.isWinThisGame)
            // {
            if (Input.touchCount > 0)
            {
                _touch = Input.GetTouch(0);

                if (_touch.phase == TouchPhase.Moved)
                {
                    var pos = transform.position;
                    pos.x += _touch.deltaPosition.x * swipeSpeed;
                    if (pos.x >= 3f)
                    {
                        pos.x = 3f;
                    }

                    if (pos.x <= -3f)
                    {
                        pos.x = -3f;
                    }

                    transform.position = pos;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                _isDragging = true;
                _currentCursorPos = Input.mousePosition;
                _oldCursorPos = _currentCursorPos;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
            }

            if (_isDragging)
            {
                _currentCursorPos = Input.mousePosition;
                var pos = transform.position;
                var movementMagnitude = ((_currentCursorPos - _oldCursorPos).magnitude * swipeSpeed) / 400f;
                if (_currentCursorPos.x < _oldCursorPos.x)
                {
                    movementMagnitude = -movementMagnitude;
                }

                pos.x += movementMagnitude;
                if (pos.x >= 7.5f)
                {
                    pos.x = 7.5f;
                }

                if (pos.x <= -7.5f)
                {
                    pos.x = -7.5f;
                }

                _oldCursorPos = _currentCursorPos;
                transform.position = pos;
            }
            // }
        }

        private void ForwardMovement()
        {
            var pos = transform.position;
            pos.z += Time.deltaTime * forwardSpeed;
            transform.position = pos;
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("RotatingPlatform"))
            {
                transform.parent = null;
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("RotatingPlatform"))
            {
                transform.parent = collision.transform;
            }
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                Debug.Log("geldi");
            }
        }
    }
}