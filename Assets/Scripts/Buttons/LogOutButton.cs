using UnityEngine;
using UnityEngine.EventSystems;
using static EventManager;

public class LogOutButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.SignOut();
        GameManager.Instance.PanelChange(PanelType.NewUser);
    }
}