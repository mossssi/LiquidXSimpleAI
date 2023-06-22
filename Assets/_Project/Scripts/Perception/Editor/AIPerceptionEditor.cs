using UnityEngine;
using UnityEditor;

namespace LiquidX.Perception
{
    [CustomEditor(typeof(AIPerception))]
    public class AIPerceptionEditor : Editor
    {
		private void OnSceneGUI()
		{
			AIPerception perception = (AIPerception)target;
			Handles.color = Color.yellow;
			Vector3 leftLine = perception.AngleToDirection(-perception.ViewAngle / 2f, false);
			Vector3 rightLine = perception.AngleToDirection(perception.ViewAngle / 2f, false);
			Vector3 origin = perception.transform.position + perception.EyeOffset;

			Handles.DrawWireArc(origin, Vector3.up, leftLine, perception.ViewAngle, perception.ViewRadius);
			Handles.DrawLine(origin, origin + leftLine * perception.ViewRadius);
			Handles.DrawLine(origin, origin + rightLine * perception.ViewRadius);

			if (perception.Player != null)
			{
				Handles.color = Color.red;
				Handles.DrawLine(origin, perception.Player.transform.position);
			}
		}
	}
}