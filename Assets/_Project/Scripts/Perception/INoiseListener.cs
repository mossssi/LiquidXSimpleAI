using UnityEngine;

// Every object which wants to hear noises should implement this
public interface INoiseListener
{
    public abstract void OnNoiseRecived(Vector3 position, float amplitude);
}
