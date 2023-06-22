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

		public ChaseState(StateMachine stateMachine) : base(stateMachine)
		{
		}

		public override void EnterState()
		{
			_chasing = true;
		}

		public override void ExecuteState()
		{
			if (_stateMachine.Perception.Player != null)
			{
				_destination = Player.Instance.transform.position;
				_guard.Agent.SetDestination(_destination);
			}
			
			if (_guard.Agent.remainingDistance < 0.2f)
			{
				if (!_searching)
				{
					_searching = true;
					_reachSearchAreaTime = Time.time;
				}
				else if (Time.time - _reachSearchAreaTime > _searchDelay)
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

		private IEnumerator Wait(float delay)
		{
			yield return new WaitForSeconds(delay);
			_stateMachine.SetPatrolling();
		}

		public override void OnDrawGizmos()
		{
			if (!_chasing) return;
			
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(_destination, 0.5f);
		}
	}
}