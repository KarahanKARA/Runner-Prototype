using UnityEngine;
using UnityEngine.AI;

namespace OpponentCharacters
{
    public class OpponentCharacter : MonoBehaviour
    {
        [SerializeField] private Transform targetDestination;
        private NavMeshAgent _navMeshAgent;
        private Vector3 _onStartPosition;
        private bool _isFinished = false;
        private static readonly int Dance = Animator.StringToHash("Dance");

        private void Awake()
        {
            _onStartPosition = transform.position;
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (!_isFinished)
            {
                _navMeshAgent.SetDestination(targetDestination.position);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Obstacle"))
            {
                _navMeshAgent.SetDestination(_onStartPosition);
                transform.position = _onStartPosition;
                _navMeshAgent.SetDestination(_onStartPosition);
            }

            if (collision.collider.CompareTag("FinishTag"))
            {
                _isFinished = true;
                _navMeshAgent.SetDestination(transform.position);
                GetComponentInChildren<Animator>().SetBool(Dance, true);
            }
        }
    }
}
