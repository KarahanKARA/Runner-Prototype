using System.Collections;
using UnityEngine;

namespace Obstacle
{
    public class HalfDonutObstacle : MonoBehaviour
    {
        [SerializeField] private GameObject movingObject;
        [SerializeField] private float speed;
        [SerializeField] private float movingDistance;
        [Space(20)]
        [Header("This value generates a random number between 1 and the entered number")]
        [Range(2,6)]
        [SerializeField] private int randomTimeInterval;
        private float _minLimit;
        private float _maxLimit;

        private void Awake()
        {
            var pos = movingObject.transform.position;
            _minLimit = pos.x - movingDistance / 2f;
            _maxLimit = pos.x + movingDistance / 2f;
            StartCoroutine(Waiter());
        }


        private IEnumerator Waiter()
        {
            while (true)
            {
                int randomStartTime = Random.Range(1,randomTimeInterval);
                yield return new WaitForSeconds(randomStartTime);
                yield return StartCoroutine(DecreasePositionValue());
            }
        }
        private IEnumerator DecreasePositionValue()
        {
            while (true)
            {
                yield return new WaitForSeconds(.01f);
                var pos = movingObject.transform.position;

                pos.x -= Time.deltaTime * speed;
                if (pos.x < _minLimit)
                {
                    break;
                }

                movingObject.transform.position = pos;
            }
            yield return StartCoroutine(IncreasePositionValue());
        }

        private IEnumerator IncreasePositionValue()
        {
            while (true)
            {
                yield return new WaitForSeconds(.01f);
                var pos = movingObject.transform.position;

                pos.x += Time.deltaTime * speed;
                if (pos.x > _maxLimit)
                {
                    break;
                }
                movingObject.transform.position = pos;
            }
        }
    }
}