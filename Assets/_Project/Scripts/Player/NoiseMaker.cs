using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LiquidX
{
	public class NoiseMaker : MonoBehaviour
	{
		[SerializeField] private LayerMask _noiseListenerMask;
		[SerializeField] private LayerMask _obstacleMask;
		[SerializeField] private AnimationCurve _noiseDecayCurve;
		[SerializeField, Min(0.1f)] private float _maxNoiseRadius = 50f;
		[SerializeField] private float _noiseMakingPeriod = 0.2f;

		private CharacterController _characterController;
		private ThirdPersonController _thirdPersonController;
		private float _runSpeedThreshold;

		private void Awake()
		{
			_characterController = GetComponent<CharacterController>();
			_thirdPersonController = GetComponent<ThirdPersonController>();
			_runSpeedThreshold = (_thirdPersonController.SprintSpeed * _thirdPersonController.SprintSpeed) - 4;
		}

		private void Start()
		{
			StartCoroutine(CheckMovementNoise(_noiseMakingPeriod));
		}

		private IEnumerator CheckMovementNoise(float delay)
		{
			while (true)
			{
				yield return new WaitForSeconds(delay);
				if (_characterController.velocity.sqrMagnitude > _runSpeedThreshold)
				{
					MakeNoise(_maxNoiseRadius, 1f);
				}
			}
		}

		public void MakeNoise(float radius, float amplitude)
		{
			Collider[] colliders = Physics.OverlapSphere(transform.position, radius, _noiseListenerMask);
			foreach (var collider in colliders)
			{
				if (collider.TryGetComponent<INoiseListener>(out INoiseListener noiseListener))
				{
					var direction = collider.transform.position - transform.position;
					var distance = direction.magnitude;
					if (!Physics.Raycast(transform.position, direction.normalized, distance, _obstacleMask))
					{
						float alpha = Mathf.Clamp01(distance / _maxNoiseRadius);
						amplitude *= _noiseDecayCurve.Evaluate(alpha);
						noiseListener.OnNoiseRecived(transform.position, amplitude);
					}
				}
			}
		}
	}
}