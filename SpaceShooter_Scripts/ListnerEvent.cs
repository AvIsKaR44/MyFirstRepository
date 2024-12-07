using UnityEngine;

public class ListnerEvent : MonoBehaviour
{
  public InitEvent initEvent;

    private void Start()
    {
        initEvent.OnClick.AddListener(OnClickEvent);         
    }

    private void OnClickEvent()
    {
        Debug.Log("Нажали");
    }
}
