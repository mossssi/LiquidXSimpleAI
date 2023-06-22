using System.Collections.Generic;
using UnityEngine;

namespace LiquidX.Path
{
	public class GuardPath : MonoBehaviour
	{
		[SerializeField] private List<Transform> _waypoints;
		[SerializeField] private bool _loop;

		public int Count => _waypoints.Count;

		private void Awake()
		{
			if (_waypoints.Count == 0)
			{
				GetComponentsInChildren<Transform>(_waypoints);
				_waypoints.Remove(transform);
			}
		}

		public int GetNextWayPointIndex(int currentIndex, bool loop = true)
		{
			if (loop)
			{
				return currentIndex < Count - 1 ? currentIndex + 1 : 0;
			}
			else
			{
				if (currentIndex < Count - 1)
				{
					return currentIndex + 1;
				}
				_waypoints.Reverse();
				return 1;
			}
		}

		public Vector3? GetWaypoint(int index)
		{
			if (index >= Count || index < 0) return null;

			return _waypoints[index].position;
		}

		private void OnDrawGizmosSelected()
		{
			if (_waypoints.Count < 2) return;

			Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
			Gizmos.color = Color.red;
			for (int i = 0; i < _waypoints.Count; i++)
			{
				if (i == _waypoints.Count - 1)
				{
					if (_loop)
					{
						Gizmos.DrawLine(_waypoints[i].localPosition, _waypoints[0].localPosition);
					}
				}
				else
				{
					Gizmos.DrawLine(_waypoints[i].localPosition, _waypoints[i + 1].localPosition);
				}
			}
		}

		public int GetClosestWaypointIndex(Vector3 position)
		{
			float minDistance = float.MaxValue;
			int closestIndex = 0;
			for (int i = 0; i < _waypoints.Count; i++)
			{
				float distance = Vector3.Distance(position, _waypoints[i].position);
				if (distance < minDistance)
				{
					closestIndex = i;
					minDistance = distance;
				}
			}
			return closestIndex;
		}
	}
}