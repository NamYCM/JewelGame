using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChain : MonoBehaviour
{
    Animator animator;
    int level = 0;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void Init (int level)
    {
        this.level = level;
    }

    public void StartDestroyChain ()
    {
        // LevelSelectManager.Instance.ModelWindow.StartBuild.SetLoadingWindow(true).SetTitle("Opening level").Show();

        animator.SetTrigger("Destroy");
        //ensure do not stop coroutine when updating
        LevelSelectManager.Instance.StartCoroutine(Data.OpenLevel(level, null, (message) => {
            //if it's still on this scene
            if (LevelSelectManager.Instance)
            {
                LevelSelectManager.Instance.ModelWindow.StartBuild.SetLoadingWindow(false).SetTitle($"Open level {level} failed").SetMessage(message).Show();
            }
        }));
    }

    public void DestroyChain ()
    {
        gameObject.SetActive(false);
    }
}
