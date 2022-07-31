using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float swipeSpeed;
        [SerializeField] private float forwardSpeed;
        private Touch _touch;
        private bool _isDragging;
        private Vector3 _currentCursorPos;
        private Vector3 _oldCursorPos;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            Application.targetFrameRate = 500;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
#if UNITY_64
            SwipeMovementPC();
#endif
#if ANDRIOD
            SwipeMovementMobile();
#endif
        }

        private void SwipeMovementPC()
        {
            var pos = _rigidbody.position;
            pos.z += Time.deltaTime * forwardSpeed;

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
                var movementMagnitude = ((_currentCursorPos - _oldCursorPos).magnitude * swipeSpeed) / 400f;
                if (_currentCursorPos.x < _oldCursorPos.x)
                {
                    movementMagnitude = -movementMagnitude;
                }

                pos.x += movementMagnitude;
                _oldCursorPos = _currentCursorPos;
            }

            pos.x = Mathf.Clamp(pos.x, -7.5f, 7.5f);
            _rigidbody.MovePosition(pos);
        }
        private void SwipeMovementMobile()
        {
            if (Input.touchCount > 0)
            {
                _touch = Input.GetTouch(0);

                if (_touch.phase == TouchPhase.Moved)
                {
                    var pos = _rigidbody.position;
                    pos.z += Time.deltaTime * forwardSpeed;
                    pos.x += _touch.deltaPosition.x * swipeSpeed;
                    pos.x = Mathf.Clamp(pos.x, -7.5f, 7.5f);
                    _rigidbody.MovePosition(pos);
                }
            }
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                Debug.Log("DEATH");
            }

            else if (collision.gameObject.CompareTag("Sticks"))
            {
                Vector3 dir = collision.contacts[0].point - transform.position;
                dir = -dir.normalized;
                dir.y = 0;
                GetComponent<Rigidbody>().AddForce(dir * 50f, ForceMode.Impulse);
            }
        }
    }
}