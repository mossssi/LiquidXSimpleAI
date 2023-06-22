using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LiquidX.SM.States
{
	public class SearchState : StateBase
	{
		private Vector3 _destination;
		private bool _chasing, _searching;
		private float _searchDelay = 2f;
		private float _reachSearchAreaTime;
		private float _searchSpeed = 3f;

		public SearchState(StateMachine stateMachine) : base(stateMachine)
		{
		}

		public override void EnterState()
		{
			_chasing = true;
			_guard.Agent.speed = _searchSpeed;
			_guard.Agent.SetDestination(_destination);
		}

		public override void ExecuteState()
		{
			// enters when guard reaches the suspected position
			if (_guard.Agent.remainingDistance < 0.5f)
			{
				// runs one time
				if (!_searching)
				{
					_searching = true;
					_reachSearchAreaTime = Time.time;
				}
				else if (Time.time - _reachSearchAreaTime > _searchDelay) // after a delay change state to patrolling
				{
					_stateMachine.SetPatrolling();
				}
			}
		}

		public override void ExitState()
		{
			_chasing = _searching = false;
		}

		/// <summary>
		/// Set latest noise hearing position
		/// </summary>
		/// <param name="position"></param>
		public void SetDestination(Vector3 position)
		{
			_destination = position;
		}

		public override void OnDrawGizmos()
		{
			if (!_chasing) return;

			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(_destination, 0.5f);
		}
	}
}