using UnityEngine;

[CreateAssetMenu(fileName = "Event System", menuName = "ScriptableObjects/Events/One Argument Int")]
public class EventOfInt : ScriptableObject
{
    public UnityEngine.Events.UnityAction<int> OnEventRaised;

    public void RaiseEvent(int value)
    => OnEventRaised?.Invoke(value);
}
