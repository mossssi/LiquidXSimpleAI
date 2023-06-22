using System.Collections;
using System.Collections.Generic;
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
				_destination = Player.Instance.transform.position;
				_guard.Agent.SetDestination(_destination);
			}
			
			if (_guard.Agent.remainingDistance < 0.5f)
			{
				// this section runs just one time
				if (!_searching)
				{
					_searching = true;
					_reachSearchAreaTime = Time.time;
				}
				else if (Time.time - _reachSearchAreaTime > _searchDelay) // After a delay return to patrolling
				{
					_stateMachine.SetPatrolling();
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