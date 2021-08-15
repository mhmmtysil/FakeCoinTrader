using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EventManager;

public class Panel : MonoBehaviour
{
    [SerializeField]
    private PanelType _panelType;

    public PanelType PanelType { get => _panelType; }

    public void DeActivePanel() => gameObject.SetActive(false);

    public void ActivePanel() => gameObject.SetActive(true);
}
