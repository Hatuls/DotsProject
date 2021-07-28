using UnityEngine;

[CreateAssetMenu(fileName = "Event System", menuName = "ScriptableObjects/Events/One Argument Float")]
public class EventOfFloat : ScriptableObject
{

    public UnityEngine.Events.UnityAction<float> OnEventRaised;

    public void RaiseEvent(float value)
    => OnEventRaised?.Invoke(value);
}
