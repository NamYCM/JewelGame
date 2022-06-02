using System;
using System.Collections;
using Michsky.UI.ModernUIPack;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UILoader : SingletonMono<UILoader>
{
    // [Serializable]
    // public class Condition : SerializableCallback<bool> {}
    // public Condition condition;
    public UnityEvent OnEndLoading = null;

    ProgressBar _progress;
    UIAnimateCompoment _uiAnimation;

    float _currentPercent = 0;
    bool _isFull = false, _isLoading = false;
    float _maxSpeed = 250f;
    string _targetScene;
    bool _canLoad = true;
    // bool _isCloseImediately = true;

    public bool CanLoad
    {
        get { return _canLoad; }
        set { _canLoad = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _uiAnimation = GetComponentInChildren<UIAnimateCompoment>(true);
        _progress = GetComponentInChildren<ProgressBar>(true);
        _progress.currentPercent = 0;
        _uiAnimation.OnCompleteActionEnable.AddListener(() => {
            StartCoroutine(LoadScene());
        });

        _uiAnimation.OnCompleteActionDisable.AddListener(() => {
            Reset();
            OnEndLoading?.Invoke();
        });

        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator LoadScene ()
    {
#if UNITY_WEBGL
        //because the webGl platform alway be 0 when use load scene async
        _currentPercent = 0.9f;

        while (!_isFull || !_canLoad)
        {
            //wait for full loader and sacrify all the condition to load
            yield return new WaitForSeconds(0.1f);
        }

        SceneManager.LoadScene(_targetScene);
#else
        // Debug.Log("into load");
        var loadAsync = SceneManager.LoadSceneAsync(_targetScene);
        loadAsync.allowSceneActivation = false;
        // Debug.Log("start load");
        do
        {
            // Debug.Log("loading");
            _currentPercent = loadAsync.progress;
        } while (loadAsync.progress < 0.9f);

        _currentPercent = loadAsync.progress;
        // _currentPercent = 0.9f;

        while (!_isFull || !_canLoad)
        {
            // Debug.Log("wait for condition");
            //wait for full loader and sacrify all the condition to load
            yield return new WaitForSeconds(0.1f);
        }
        // Debug.Log("loaded");
        // SceneManager.LoadScene(_targetScene);
        loadAsync.allowSceneActivation = true;

        // if (_isCloseImediately) Close();
#endif
    }

    void Reset ()
    {
        // _isCloseImediately = true;
        _canLoad = true;
        _isFull = false;
        _currentPercent = 0;
        _progress.currentPercent = 0;
    }

    public void LoadScene (string sceneName)
    {
        _isLoading = true;
        // _isCloseImediately = isCloseAfterLoad;
        _targetScene = sceneName;
        _uiAnimation.Show();
    }

    public void Close ()
    {
        if (!_isLoading) return;
        _isLoading = false;
        _uiAnimation.Disable();
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
