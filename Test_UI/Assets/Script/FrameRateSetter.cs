using UnityEngine;

public class FrameRateSetter : MonoBehaviour
{
    void Awake()
    {
        // アプリ起動時にフレームレートを120に固定する
        Application.targetFrameRate = 120;
    }
}