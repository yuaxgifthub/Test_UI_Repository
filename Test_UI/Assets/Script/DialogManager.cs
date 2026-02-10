using UnityEngine;

public class DialogManager : MonoBehaviour
{
    // �����Ƀq�G�����L�[�ɂ���_�C�A���O��R�t���܂�
    public GameObject dialog;
    public GameObject balloon;

    // Animator�𑀍삷�邽�߂̕ϐ�
    private Animator anim;

    void Start()
    {
        // �ŏ���Animator���擾���Ă����܂�
        if (dialog != null)
        {
            anim = dialog.GetComponent<Animator>();
        }
    }

    // �_�C�A���O���J������
    public void OpenDialog()
    {
        if (dialog != null)
        {
            dialog.SetActive(true);
        }
    }

    // �_�C�A���O�����A�j�����J�n���鏈��
    public void CloseDialog()
    {
        if (anim != null)
        {
            // �A�j���[�^�[�́uClose�v�g���K�[������
            anim.SetTrigger("Close");
            // ★追加：フキダシも非表示にする
        if (balloon != null)
        {
            balloon.SetActive(false);
        }
        }
    }

    // �A�j���[�V�����̍Ō�Ɂu�A�j���[�V�����C�x���g�v�ŌĂяo���֐�
    public void OnCloseAnimationComplete()
    {
        dialog.SetActive(false);
        
    }
}