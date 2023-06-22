using UnityEngine;

namespace LiquidX.SM.States
{
	public class PatrolState : StateBase
	{
		private float _reachDestinationThreshold = 0.25f;
		private float _waitAtWaypoint = 2f;
		private int _targetWaypointIndex;
		private float _waitTimer = 0;
		private bool _loopWaypoint = true;
		private float _patrolSpeed = 2f;

		public PatrolState(StateMachine stateMachine) : base(stateMachine)
		{
		}

		public override void EnterState()
		{
			_guard.Agent.speed = _patrolSpeed;
			_targetWaypointIndex = _guard.Path.GetClosestWaypointIndex(_guard.transform.position);
			SetWayPoint(_targetWaypointIndex);
		}

		public override void ExecuteState()
		{
			Patrolling();
		}

		public override void ExitState()
		{
		}

		private void Patrolling()
		{
			if (_guard.Agent.remainingDistance < _reachDestinationThreshold)
			{
				_waitTimer += Time.deltaTime;
				if (_waitTimer > _waitAtWaypoint)
				{
					SetNextWaypoint();
					_waitTimer = 0;
				}
			}
		}

		private void SetNextWaypoint()
		{
			_targetWaypointIndex = _guard.Path.GetNextWayPointIndex(_targetWaypointIndex, _loopWaypoint);
			SetWayPoint(_targetWaypointIndex);
		}

		private void SetWayPoint(int waypointIndex)
		{
			var position = _guard.Path.GetWaypoint(waypointIndex);
			if (position != null)
			{
				_guard.Agent.SetDestination(position.Value);
			}
		}
	}
}