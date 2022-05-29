using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
// using DG;

public class UIScore : MonoBehaviour
{
    public GameObject scorePrefab;
    [SerializeField] float duration;

    Text scoreText;
    EffectPool effectPool;

    private void Awake() {
        scoreText = GetComponent<Text>();
        effectPool = EffectPool.Instance;
    }

    public void SetScoreText (int score) 
    {
        scoreText.text = score.ToString();
    }

    public void AddScore(int score, Vector3 position, TweenCallback callBacks)
    {
        var scoreObject = effectPool.GetEffect(EffectType.Score, transform);
        // var scoreObject = Instantiate(scorePrefab, position, Quaternion.identity, transform);
        
        var scoreText = scoreObject.GetComponent<TMPro.TMP_Text>();
        scoreText.text  = score.ToString();
        
        scoreObject.transform.position = position;
        scoreObject.SetActive(true);
        scoreObject.transform.DOMove(transform.position, duration).SetUpdate(true).onComplete += () => {
            callBacks();
            effectPool.ReturnToPool(scoreObject, EffectType.Score);
            // Destroy(scoreObject);
        };
    }
}
