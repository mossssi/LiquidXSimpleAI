using UnityEngine;

public interface INoiseListener
{
    public abstract void OnNoiseRecived(Vector3 position, float amplitude);
}
