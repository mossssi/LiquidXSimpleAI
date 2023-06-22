using UnityEngine;
using UnityEngine.AI;
using LiquidX.SM;
using LiquidX.Path;
using UnityEngine.InputSystem.XR;

namespace LiquidX.Enemy
{
	[RequireComponent(typeof(NavMeshAgent), typeof(StateMachine))]
	public class Guard : MonoBehaviour
	{
		[SerializeField] private GuardPath _path;

		private NavMeshAgent _agent;
		private StateMachine _stateMachine;
		private Animator _animator;

		// animation IDs
		private int _animIDSpeed;
		private int _animIDGrounded;
		private int _animIDJump;
		private int _animIDFreeFall;
		private int _animIDMotionSpeed;

		public NavMeshAgent Agent => _agent;
		public GuardPath Path => _path;

		private void Awake()
		{
			_agent = GetComponent<NavMeshAgent>();
			_stateMachine = GetComponent<StateMachine>();
			_animator = GetComponent<Animator>();
		}

		private void Start()
		{
			AssignAnimationIDs();
			_stateMachine.Initialize(this);
		}

		private void Update()
		{
			_animator.SetBool(_animIDGrounded, true);
			var speed = _agent.velocity.magnitude;
			_animator.SetFloat(_animIDSpeed, speed);
			_animator.SetFloat(_animIDMotionSpeed, 1f);
		}

		// Using int hashes to prevent type errors
		private void AssignAnimationIDs()
		{
			_animIDSpeed = Animator.StringToHash("Speed");
			_animIDGrounded = Animator.StringToHash("Grounded");
			_animIDJump = Animator.StringToHash("Jump");
			_animIDFreeFall = Animator.StringToHash("FreeFall");
			_animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
		}

		// Calls from animation events
		private void OnFootstep(AnimationEvent animationEvent)
		{
			if (animationEvent.animatorClipInfo.weight > 0.5f)
			{
				
			}
		}

		// Calls from animation events
		private void OnLand(AnimationEvent animationEvent)
		{
			if (animationEvent.animatorClipInfo.weight > 0.5f)
			{

			}
		}
	}
}