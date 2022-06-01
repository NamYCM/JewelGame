using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CustomButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private UnityEvent _onDown = new UnityEvent();

    public UnityEvent OnDown => _onDown;

    Button _button;

    private void Awake() {
        _button = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_button.interactable) return;
        _onDown?.Invoke();
    }

    public void Disable ()
    {
        _button.interactable = false;
    }
}
