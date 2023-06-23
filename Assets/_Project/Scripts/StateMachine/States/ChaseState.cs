using UnityEngine;

namespace LiquidX.SM.States
{
	public class ChaseState : StateBase
	{
		private Vector3 _destination;
		private bool _chasing, _searching;
		private float _searchDelay = 2f;
		private float _reachSearchAreaTime;
		private float _chaseSpeed = 4f;
		private float _stoppingDistance = 1f;

		public ChaseState(StateMachine stateMachine) : base(stateMachine)
		{
		}

		public override void EnterState()
		{
			_chasing = true;
			_guard.Agent.speed = _chaseSpeed;
		}

		public override void ExecuteState()
		{
			if (_stateMachine.Perception.Player != null)
			{
				var playerPosition = Player.Instance.transform.position;
				var direction = (playerPosition - _guard.transform.position).normalized * _stoppingDistance;
				_destination = playerPosition - direction;
				_guard.Agent.SetDestination(_destination);
				if (_guard.Agent.velocity.sqrMagnitude < 1)
				{
					Vector3 delta = new Vector3(playerPosition.x - _guard.transform.position.x, 0.0f, playerPosition.z - _guard.transform.position.z);
					if (delta != Vector3.zero)
					{
						Quaternion rotation = Quaternion.LookRotation(delta);
						_guard.transform.rotation = rotation;
					}
				}
			}
			
			if (_guard.Agent.remainingDistance < 0.25f)
			{
				if (!_stateMachine.Perception.CanSeePlayer)
				{
					_stateMachine.SetSuspect(_destination);
				}
			}
		}

		public override void ExitState()
		{
			_chasing = false;
			_searching = false;
		}

		public override void OnDrawGizmos()
		{
			if (!_chasing) return;
			
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(_destination, 0.5f);
		}
	}
}