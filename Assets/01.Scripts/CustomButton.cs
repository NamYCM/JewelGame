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

    public void OnPointerDown(PointerEventData eventData)
    {
        _onDown?.Invoke();
    }
}
