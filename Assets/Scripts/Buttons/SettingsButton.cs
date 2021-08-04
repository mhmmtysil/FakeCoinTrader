using UnityEngine;
using UnityEngine.EventSystems;
using static EventManager;

public class SettingsButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.PanelChange(PanelType.Settings);
    }
}
