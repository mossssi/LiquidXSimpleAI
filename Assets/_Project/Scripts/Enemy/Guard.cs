using UnityEngine;
using UnityEngine.AI;
using LiquidX.SM;
using LiquidX.Path;

namespace LiquidX.Enemy
{
	[RequireComponent(typeof(NavMeshAgent), typeof(StateMachine))]
	public class Guard : MonoBehaviour
	{
		[SerializeField] private GuardPath _path;

		private NavMeshAgent _agent;
		private StateMachine _stateMachine;

		public NavMeshAgent Agent => _agent;
		public GuardPath Path => _path;

		private void Awake()
		{
			_agent = GetComponent<NavMeshAgent>();
			_stateMachine = GetComponent<StateMachine>();
		}

		private void Start()
		{
			_stateMachine.Initialize(this);
		}
	}
}