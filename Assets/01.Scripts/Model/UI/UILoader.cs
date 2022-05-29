using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Michsky.UI.ModernUIPack;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UILoader : SingletonMono<UILoader>
{
    [Serializable]
    public class Condition : SerializableCallback<bool> {}
    public Condition condition;
    public UnityEvent OnEndLoading = null;

    ProgressBar _progress;
    UIAnimateCompoment _uiAnimation;

    float _currentPercent = 0;
    bool _isFull = false;
    float _maxSpeed = 250f;
    string _targetScene;

    // Start is called before the first frame update
    void Start()
    {
        // condition.SetMethod(this, "Test2", true, null);
        // condition.SetMethod(this, "Test", true, null);
        Debug.Log(condition.Invoke());

        _uiAnimation = GetComponentInChildren<UIAnimateCompoment>(true);
        _progress = GetComponentInChildren<ProgressBar>(true);
        _progress.currentPercent = 0;
        _uiAnimation.OnCompleteActionEnable.AddListener(() => {
            LoadScene();
        });

        _uiAnimation.OnCompleteActionDisable.AddListener(() => {
            Reset();
            OnEndLoading?.Invoke();
        });
        // CanLoad();
        DontDestroyOnLoad(gameObject);

    }

    public bool Test ()
    {
        Debug.Log("1");
        return true;
    }
    public bool Test2 ()
    {
        Debug.Log("2");
        return false;
    }

    private async void LoadScene ()
    {
        var loadAsync = SceneManager.LoadSceneAsync(_targetScene);
        loadAsync.allowSceneActivation = false;

        do
        {
            _currentPercent = loadAsync.progress;
        } while (loadAsync.progress < 0.9f);

        _currentPercent = loadAsync.progress;

        while (!_isFull)
        {
            //wait for full loader
            await Task.Delay(100);
        }

        loadAsync.allowSceneActivation = true;

        _uiAnimation.Disable();
    }

    void Reset ()
    {
        _isFull = false;
        _currentPercent = 0;
        _progress.currentPercent = 0;
    }

    public void LoadScene (string sceneName)
    {
        _targetScene = sceneName;
        _uiAnimation.Show();
    }

    private void Update() {
        if (_progress.currentPercent < 90f)
        {
            _progress.currentPercent = Mathf.MoveTowards(_progress.currentPercent, _currentPercent * 100, _maxSpeed * Time.deltaTime);
        }
        else if (_progress.currentPercent >= 90f && _progress.currentPercent < 100f)
        {
            _progress.currentPercent = Mathf.MoveTowards(_progress.currentPercent, 100, _maxSpeed * Time.deltaTime);
        }
        else
        {
            _isFull = true;
        }
    }
}
