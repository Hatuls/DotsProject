using UnityEngine;

[CreateAssetMenu (fileName = "Event System", menuName = "ScriptableObjects/Events/None")]
public class EventDispatcher : ScriptableObject
{

  public  UnityEngine.Events.UnityAction OnEventRaised;

    public void RaiseEvent()
    => OnEventRaised?.Invoke();
}
