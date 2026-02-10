using UnityEngine;

public class DialogManager : MonoBehaviour
{
    // ここにヒエラルキーにあるダイアログを紐付けます
    public GameObject dialog;

    // Animatorを操作するための変数
    private Animator anim;

    void Start()
    {
        // 最初にAnimatorを取得しておきます
        if (dialog != null)
        {
            anim = dialog.GetComponent<Animator>();
        }
    }

    // ダイアログを開く処理
    public void OpenDialog()
    {
        if (dialog != null)
        {
            dialog.SetActive(true);
        }
    }

    // ダイアログを閉じるアニメを開始する処理
    public void CloseDialog()
    {
        if (anim != null)
        {
            // アニメーターの「Close」トリガーを引く
            anim.SetTrigger("Close");
        }
        else
        {
            
            
        }
    }

    // アニメーションの最後に「アニメーションイベント」で呼び出す関数
    public void OnCloseAnimationComplete()
    {
        dialog.SetActive(false);
    }
}