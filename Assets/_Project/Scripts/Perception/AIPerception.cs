using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LiquidX.Perception
{
    public class AIPerception : MonoBehaviour
    {
        public Action<Player> OnPlayerFound;
        public Action OnPlayerLost;

        [SerializeField] private Vector3 _eyeOffset;
        [SerializeField] private float _viewRadius;
        [SerializeField] private float _viewAngle;
        [SerializeField] private LayerMask _playerMask;
        [SerializeField] private LayerMask _obstacleMask;
        [SerializeField] private float _searchEverySeconds;
        [SerializeField] private float _lostDelay;

        private Player _detectedPlayer;
        private float _seenTime;

        public Vector3 EyeOffset => _eyeOffset;
        public float ViewRadius => _viewRadius;
        public float ViewAngle => _viewAngle;
        public Player Player => _detectedPlayer;
        public bool CanSeePlayer => _detectedPlayer != null;

		private void Start()
		{
            StartCoroutine(CheckForPlayer(_searchEverySeconds));
		}

		public Vector3 AngleToDirection(float angle, bool global)
        {
            if (!global)
            {
                angle += transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
        }

        private IEnumerator CheckForPlayer(float delay)
        {
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
                if (_detectedPlayer != null)
                {
                    _detectedPlayer = null;
                    OnPlayerLost?.Invoke();
                }
            }
        }
    }
}