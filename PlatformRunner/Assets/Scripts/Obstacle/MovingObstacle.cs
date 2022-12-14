using System.Collections;
using UnityEngine;

namespace Obstacle
{
    public class MovingObstacle : MonoBehaviour
    {
        [SerializeField] private GameObject bottomObject;
        [SerializeField] private Enums.MoveType moveType;
        [SerializeField] private float speed;
        [SerializeField] private float movingDistance;
        [SerializeField] private float rotateSpeed;
        private float _minLimit;
        private float _maxLimit;

        private void Awake()
        {
            var createdBottomObject = Instantiate(bottomObject, transform.parent);
            var pos = transform.position;
            createdBottomObject.transform.position = pos;
            if (moveType == Enums.MoveType.Horizontal)
            {
                createdBottomObject.transform.rotation = Quaternion.Euler(90, 90, 0);
                _minLimit = pos.x - movingDistance / 2f;
                _maxLimit = pos.x + movingDistance / 2f;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ |
                                                        RigidbodyConstraints.FreezePositionY |
                                                        RigidbodyConstraints.FreezeRotationX |
                                                        RigidbodyConstraints.FreezeRotationZ;
            }
            else if (moveType == Enums.MoveType.Vertical)
            {
                createdBottomObject.transform.rotation = Quaternion.Euler(90, 0, 0);
                _minLimit = pos.z - movingDistance / 2f;
                _maxLimit = pos.z + movingDistance / 2f;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX |
                                                        RigidbodyConstraints.FreezePositionY |
                                                        RigidbodyConstraints.FreezeRotationX |
                                                        RigidbodyConstraints.FreezeRotationZ;
            }

            createdBottomObject.transform.localScale = new Vector3(.5f, movingDistance / 2, .6f);


            StartCoroutine(DecreasePositionValue());
        }

        private void Update()
        {
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        }

        private IEnumerator DecreasePositionValue()
        {
            while (true)
            {
                yield return new WaitForSeconds(.01f);
                var pos = transform.position;
                if (moveType == Enums.MoveType.Horizontal)
                {
                    pos.x -= Time.deltaTime * speed;
                    if (pos.x < _minLimit)
                    {
                        break;
                    }
                }
                else if (moveType == Enums.MoveType.Vertical)
                {
                    pos.z -= Time.deltaTime * speed;
                    if (pos.z < _minLimit)
                    {
                        break;
                    }
                }

                transform.position = pos;
            }

            StartCoroutine(IncreasePositionValue());
        }

        private IEnumerator IncreasePositionValue()
        {
            while (true)
            {
                yield return new WaitForSeconds(.01f);
                var pos = transform.position;
                if (moveType == Enums.MoveType.Horizontal)
                {
                    pos.x += Time.deltaTime * speed;
                    if (pos.x > _maxLimit)
                    {
                        break;
                    }
                }
                else
                {
                    pos.z += Time.deltaTime * speed;
                    if (pos.z > _maxLimit)
                    {
                        break;
                    }
                }

                transform.position = pos;
            }

            StartCoroutine(DecreasePositionValue());
        }
    }
}