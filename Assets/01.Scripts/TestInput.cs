using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Networking;

public class TestInput : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    PlayerInput inputActions;

    private void Awake() {
        inputActions = new PlayerInput();
    }

    private void Start() {
        inputActions.GameEditor.MButtonClick.started += MButton;
        inputActions.GamePlay.MouseLeftClick.started += OnStarted;
        inputActions.GamePlay.MouseLeftClick.canceled += OnCanceled;
    }

    private void MButton(InputAction.CallbackContext obj)
    {
        Debug.Log("M");
        // throw new NotImplementedException();
    }

    private void OnCanceled(InputAction.CallbackContext obj)
    {
        Debug.Log("2");
        // throw new NotImplementedException();
    }

    private void OnStarted(InputAction.CallbackContext obj)
    {
        Debug.Log("1");
        // throw new NotImplementedException();
    }

    private void OnEnable() {
        inputActions.Enable();
        TouchSimulation.Enable();

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable() {
        inputActions.Disable();
        TouchSimulation.Disable();

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= FingerDown;

    }

    private void Update() {
        Debug.Log(UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count);
    }

    void StartTouch (InputAction.CallbackContext context)
    {
        _text.text = (int.Parse(_text.text) + 1).ToString();
        Debug.Log("start at: " + inputActions.GamePlay.TouchPosition.ReadValue<Vector2>() +  " " + Input.mousePosition);
    }

    void EndTouch (InputAction.CallbackContext context)
    {
        Debug.Log("end at: " + inputActions.GamePlay.TouchPosition.ReadValue<Vector2>() +  " " + Input.mousePosition);
    }

    void FingerDown (Finger finger)
    {
        Debug.Log(finger.screenPosition);
    }

    public void Test2 ()
    {
        StartCoroutine(Test());
    }

    private IEnumerator Test ()
    {
        // UnityWebRequest testrequest = new UnityWebRequest("https://us-central1-testapi-d3e3a.cloudfunctions.net/getLevelMap/get-all-map", "GET");
        // testrequest.SetRequestHeader("Cookie", string.Format("session={0}", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImY0ZTc2NDk3ZGE3Y2ZhOWNjMDkwZDcwZTIyNDQ2YTc0YjVjNTBhYTkiLCJ0eXAiOiJKV1QifQ.eyJ1c2VyUm9sZSI6dHJ1ZSwiaXNzIjoiaHR0cHM6Ly9zZWN1cmV0b2tlbi5nb29nbGUuY29tL3Rlc3RhcGktZDNlM2EiLCJhdWQiOiJ0ZXN0YXBpLWQzZTNhIiwiYXV0aF90aW1lIjoxNjU0MTQzMjQ4LCJ1c2VyX2lkIjoibmFtIiwic3ViIjoibmFtIiwiaWF0IjoxNjU0MTQzMjQ4LCJleHAiOjE2NTQxNDY4NDgsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnt9LCJzaWduX2luX3Byb3ZpZGVyIjoiY3VzdG9tIn19.VjDuGwrUfsrTFahqJsyHrWA1XIXmEB3MFzorRKG6jIepscoOjZbWll_0d6d4L6SPz8DbIPaa_PVi3F2rvAKnQTzgsa1872PNRiIuLweux6rtyqUsUl2cR_exuRas9-eDb6d9wkYiqqHJFtMBWTaRNeIj0vze1jOzpFfX_2Cdat06O8U1lFGAkrllClAhOuw8yWRElRc_o9sI9TTxmkD4JhoxPtK-F2Gav0ueOCGCN_rYofdsWg6otJrxglN_635yA9TDYh0VPhGvpQelYi_B8u4Ql1mktc_t89PL8Qcge-SVRvdpn3xMLjXetFsMBXNc3m75td9hK21VEOwC1PLNaw"));

        // using (UnityWebRequest www = UnityWebRequest.Get("https://us-central1-testapi-d3e3a.cloudfunctions.net/getLevelMap/get-all-map"))
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:5001/testapi-d3e3a/us-central1/login/test2"))
        {
            www.SetRequestHeader("Cookie", string.Format("__session={0}", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImY0ZTc2NDk3ZGE3Y2ZhOWNjMDkwZDcwZTIyNDQ2YTc0YjVjNTBhYTkiLCJ0eXAiOiJKV1QifQ.eyJ1c2VyUm9sZSI6dHJ1ZSwiaXNzIjoiaHR0cHM6Ly9zZWN1cmV0b2tlbi5nb29nbGUuY29tL3Rlc3RhcGktZDNlM2EiLCJhdWQiOiJ0ZXN0YXBpLWQzZTNhIiwiYXV0aF90aW1lIjoxNjU0MTQzMjQ4LCJ1c2VyX2lkIjoibmFtIiwic3ViIjoibmFtIiwiaWF0IjoxNjU0MTQzMjQ4LCJleHAiOjE2NTQxNDY4NDgsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnt9LCJzaWduX2luX3Byb3ZpZGVyIjoiY3VzdG9tIn19.VjDuGwrUfsrTFahqJsyHrWA1XIXmEB3MFzorRKG6jIepscoOjZbWll_0d6d4L6SPz8DbIPaa_PVi3F2rvAKnQTzgsa1872PNRiIuLweux6rtyqUsUl2cR_exuRas9-eDb6d9wkYiqqHJFtMBWTaRNeIj0vze1jOzpFfX_2Cdat06O8U1lFGAkrllClAhOuw8yWRElRc_o9sI9TTxmkD4JhoxPtK-F2Gav0ueOCGCN_rYofdsWg6otJrxglN_635yA9TDYh0VPhGvpQelYi_B8u4Ql1mktc_t89PL8Qcge-SVRvdpn3xMLjXetFsMBXNc3m75td9hK21VEOwC1PLNaw"));
            // www.SetRequestHeader("access-control-request-headers", "ETag");
            www.SetRequestHeader("Authorization", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImY0ZTc2NDk3ZGE3Y2ZhOWNjMDkwZDcwZTIyNDQ2YTc0YjVjNTBhYTkiLCJ0eXAiOiJKV1QifQ.eyJ1c2VyUm9sZSI6dHJ1ZSwiaXNzIjoiaHR0cHM6Ly9zZWN1cmV0b2tlbi5nb29nbGUuY29tL3Rlc3RhcGktZDNlM2EiLCJhdWQiOiJ0ZXN0YXBpLWQzZTNhIiwiYXV0aF90aW1lIjoxNjU0MTQzMjQ4LCJ1c2VyX2lkIjoibmFtIiwic3ViIjoibmFtIiwiaWF0IjoxNjU0MTQzMjQ4LCJleHAiOjE2NTQxNDY4NDgsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnt9LCJzaWduX2luX3Byb3ZpZGVyIjoiY3VzdG9tIn19.VjDuGwrUfsrTFahqJsyHrWA1XIXmEB3MFzorRKG6jIepscoOjZbWll_0d6d4L6SPz8DbIPaa_PVi3F2rvAKnQTzgsa1872PNRiIuLweux6rtyqUsUl2cR_exuRas9-eDb6d9wkYiqqHJFtMBWTaRNeIj0vze1jOzpFfX_2Cdat06O8U1lFGAkrllClAhOuw8yWRElRc_o9sI9TTxmkD4JhoxPtK-F2Gav0ueOCGCN_rYofdsWg6otJrxglN_635yA9TDYh0VPhGvpQelYi_B8u4Ql1mktc_t89PL8Qcge-SVRvdpn3xMLjXetFsMBXNc3m75td9hK21VEOwC1PLNaw");
            yield return www.SendWebRequest();

            Debug.Log("send");
            if (www.isNetworkError)
            {
                throw new Exception("something wrong in send www request \n" + www.error);
            }
            else
            {
                string body = www.downloadHandler.text;

                if (www.responseCode == 200)
                {
                    // LevelData levelData = JsonConvert.DeserializeObject<LevelData>(body);
                    Debug.Log("update succesful");
                    // onSucessfulGet?.Invoke(levelData);
                }
                else
                {
                    Debug.LogWarning("Update level data of user failed!!\n" + body);
                    // onFailedGet?.Invoke(body);
                }
            }
        }
    }
}
