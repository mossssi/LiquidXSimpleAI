using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

	private void Awake()
	{
		// Making player singleton
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Debug.LogError("Multiple instances of Player is in the scene.", this);
			Destroy(this.gameObject);
			return;
		}
	}
}
