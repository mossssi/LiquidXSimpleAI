using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LiquidX.Perception
{
    public class AIPerception : MonoBehaviour, INoiseListener
    {
        public Action<Player> OnPlayerFound;
        public Action OnPlayerLost;
        public Action<Vector3> OnSuspect;

		[Header("Vision")]
		[SerializeField] private Vector3 _eyeOffset;
        [SerializeField] private float _viewRadius;
        [SerializeField] private float _viewAngle;
        [SerializeField] private LayerMask _playerMask;
        [SerializeField] private LayerMask _obstacleMask;
        [SerializeField] private float _searchEverySeconds;
        [SerializeField] private float _lostDelay;
        [Header("Hearing")]
        [SerializeField] private float _hearingThreshold = 0.25f;

        private Player _detectedPlayer;
        private float _seenTime;
        private bool _noiseHeard;
        private Vector3 _noisePosition;

        public Vector3 EyeOffset => _eyeOffset;
        public float ViewRadius => _viewRadius;
        public float ViewAngle => _viewAngle;
        public Player Player => _detectedPlayer;
        public bool CanSeePlayer => _detectedPlayer != null;

		private void Start()
		{
            // For optimization we call checking with a little delay each time
            StartCoroutine(CheckForPlayer(_searchEverySeconds));
		}

        /// <summary>
        /// Get direction of an angle from transform.forward vector
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
		public Vector3 AngleToDirection(float angle)
        {
            angle += transform.eulerAngles.y;
            return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
        }

        private IEnumerator CheckForPlayer(float delay)
        {
            // Infinite loop
            while (true)
            {
                yield return new WaitForSeconds(delay);
                CheckForPlayer();
            }
        }

        private void CheckForPlayer()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position + _eyeOffset, _viewRadius, _playerMask);
            Vector3 origin = transform.position + _eyeOffset;
            
            // if player is not near guard try losing it
            if (colliders.Length == 0)
            {
				if (_detectedPlayer != null)
				{
					LostPlayer();
				}
                return;
			}

            var collider = colliders[0];
            var player = collider.GetComponent<Player>();
            if (player != null)
            {
                var direction = (player.transform.position - origin);
                direction.y = 0f;
                direction.Normalize();
                var angle = Vector3.Angle(transform.forward, direction);
                var distance = Vector3.Distance(origin, player.transform.position);
                // When player is in field of view and there is no obstacle in-between
                if (angle < _viewAngle / 2f && !Physics.Raycast(origin, direction, distance, _obstacleMask))
                {
                    PlayerVisible(player);
                }
                else
                {
					LostPlayer();
				}
            }
            else
			{
                Debug.LogErrorFormat($"There is no Player script attached to {collider.gameObject.name}");
			}
        }

        private void PlayerVisible(Player player)
        {
            _seenTime = Time.time;
            // Runs one time
			if (_detectedPlayer == null)
            {
                _detectedPlayer = player;
                OnPlayerFound?.Invoke(player);
			}
		}

        private void LostPlayer()
        {
            if (Time.time - _seenTime > _lostDelay)
            {
                // Runs one time
                if (_detectedPlayer != null)
                {
                    _detectedPlayer = null;
                    OnPlayerLost?.Invoke();
                }
            }
        }

        // interface
		public void OnNoiseRecived(Vector3 position, float amplitude)
		{
            if (_detectedPlayer != null) return;

			// Adjust guard hearing power with _hearingThreshold field 
			if (amplitude >= _hearingThreshold)
            {
                //Debug.LogFormat($"Hearing Noise {amplitude}");
                _noiseHeard = true;
                OnSuspect?.Invoke(position);
			}
		}

        // Drawing cone with Gizmos for fun :)
		private void OnDrawGizmos()
		{
            var color = _detectedPlayer == null ? Color.yellow : Color.red;
            Gizmos.color = color;
			Vector3 origin = transform.position + _eyeOffset;
			Vector3 leftLine = AngleToDirection(-_viewAngle / 2f);
			Vector3 rightLine = AngleToDirection(_viewAngle / 2f);
            Vector3 upLine = Quaternion.AngleAxis(90f, transform.forward) * rightLine;
            Vector3 downLine = Quaternion.AngleAxis(90f, transform.forward) * leftLine;
            float coneRadius = _viewRadius * Mathf.Sin(_viewAngle * Mathf.Deg2Rad / 2f);
            float coneHeight = Mathf.Sqrt(_viewRadius * _viewRadius - coneRadius * coneRadius);
            float angleSign = _viewAngle > 180f ? -1 : 1;

			Gizmos.DrawLine(origin, origin + leftLine * _viewRadius);
			Gizmos.DrawLine(origin, origin + rightLine * _viewRadius);
			Gizmos.DrawLine(origin, origin + upLine * _viewRadius);
			Gizmos.DrawLine(origin, origin + downLine * _viewRadius);
#if UNITY_EDITOR
            UnityEditor.Handles.color = color;
            UnityEditor.Handles.DrawWireDisc(origin + transform.forward * coneHeight * angleSign, transform.forward, coneRadius);
            UnityEditor.Handles.DrawWireArc(origin, transform.up, leftLine, _viewAngle, _viewRadius);
            UnityEditor.Handles.DrawWireArc(origin, transform.right, upLine, _viewAngle, _viewRadius);
#endif
		}
	}
}