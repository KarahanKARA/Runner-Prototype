using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject dustPoof;
        [SerializeField] private float swipeSpeed;
        [SerializeField] private float forwardSpeed;
        private Touch _touch;
        private bool _isDragging;
        private Vector3 _currentCursorPos;
        private Vector3 _onStartPos;
        private Vector3 _oldCursorPos;
        private Rigidbody _rigidbody;
        private bool _isCheckTheFinishLine = false;
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int Thinking = Animator.StringToHash("Thinking");

        private void Awake()
        {
            Application.targetFrameRate = 500;
            _rigidbody = GetComponent<Rigidbody>();
            _onStartPos = transform.position;
        }

        private void OnEnable()
        {
            GameManager.Instance.CanPlayerPaint = false;
            GameManager.Instance.CanPlayerSwipe = true;
            GameManager.Instance.CanPlayerMoveToForward = true;
            GameManager.Instance.IsPlayerFinishPaint = false;
        }

        private void Update()
        {
            if (GameManager.Instance.CanPlayerSwipe)
            {
#if UNITY_64
                SwipeMovementPC();
#endif
#if ANDRIOD
            SwipeMovementMobile();
#endif
            }

            if (GameManager.Instance.CanPlayerMoveToForward)
            {
                var pos = _rigidbody.position;
                if (_isCheckTheFinishLine)
                {
                    if (SceneManager.GetActiveScene().buildIndex == 2)
                    {
                        GetComponentInChildren<Animator>().SetBool("Dancing", true);
                        return;
                    }
                    transform.position = Vector3.MoveTowards(pos,
                        GameManager.Instance.GetPaintPointPosition(), Time.deltaTime*forwardSpeed);
                    return;
                }

                pos.z += Time.deltaTime * forwardSpeed;
                transform.position = pos;
            }

            if (GameManager.Instance.IsPlayerFinishPaint)
            {
                GetComponentInChildren<Animator>().SetBool("Dancing", true);
            }
        }

        private void SwipeMovementPC()
        {
            var pos = _rigidbody.position;
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

            pos.x = Mathf.Clamp(pos.x, -11f, 11f);
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
                    pos.x = Mathf.Clamp(pos.x, -11f, 11f);
                    _rigidbody.MovePosition(pos);
                }
            }
        }

        private IEnumerator AISceneOnDeath()
        {
            GameManager.Instance.CanPlayerSwipe = false;
            GameManager.Instance.CanPlayerMoveToForward = false;
            yield return new WaitForSeconds(.3f);
            GameManager.Instance.CanPlayerSwipe = true;
            GameManager.Instance.CanPlayerMoveToForward = true;
            transform.position = _onStartPos;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                if (SceneManager.GetActiveScene().buildIndex == 2)
                {
                    StartCoroutine(AISceneOnDeath());
                    _isDragging = false;
                    return;
                }
                dustPoof.SetActive(true);
                GameManager.Instance.CanPlayerSwipe = false;
                GameManager.Instance.CanPlayerMoveToForward = false;
                GetComponentInChildren<Animator>().SetBool(Death, true);
                GameManager.Instance.DeathScreenUIActivity();
            }

            else if (collision.gameObject.CompareTag("Sticks"))
            {
                Vector3 dir = collision.contacts[0].point - transform.position;
                dir = -dir.normalized;
                dir.y = 0;
                GetComponent<Rigidbody>().AddForce(dir * 50f, ForceMode.Impulse);
            }
            else if (collision.gameObject.CompareTag("FinishTag"))
            {
                _isCheckTheFinishLine = true;
                collision.collider.isTrigger = true;
                GameManager.Instance.CanPlayerSwipe = false;
                GameManager.Instance.OpenConfettiVFX();
            }
            else if (collision.gameObject.CompareTag("PaintPoint"))
            {
                Destroy(collision.gameObject);
                GameManager.Instance.CanPlayerMoveToForward = false;
                GameManager.Instance.CanPlayerPaint = true;
                GetComponentInChildren<Animator>().SetBool(Thinking, true);
            }
        }
    }
}