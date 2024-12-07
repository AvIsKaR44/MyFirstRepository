using UnityEngine;
using UnityEngine.Events;

public class InitEvent : MonoBehaviour
{
    public UnityEvent OnClick;

    private void OnMouseDown()
    {
        OnClick?.Invoke();
    }
}
