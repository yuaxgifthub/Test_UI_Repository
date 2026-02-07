using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UltimateBannerManager : MonoBehaviour, IEndDragHandler, IDragHandler
{
    [Header("設定")]
    public ScrollRect scrollRect;
    public RectTransform content;
    public Transform pagerParent;
    public int realBannerCount = 5;
    public float snapSpeed = 0.05f;

    [Header("ドット画像")]
    public Sprite activeSprite;
    public Sprite inactiveSprite;

    private float[] pagePositions;
    private float itemWidth;
    private bool isLerping = false;

    void Start()
    {
        // 1枚あたりの正規化された幅を計算 (7枚構成なら 1/6)
        itemWidth = 1f / (realBannerCount + 2 - 1);

        // 各ページのスナップポイントを計算
        pagePositions = new float[realBannerCount + 2];
        for (int i = 0; i < pagePositions.Length; i++)
        {
            pagePositions[i] = (float)i * itemWidth;
        }

        // 初期位置を「本物の1枚目」にセット
        scrollRect.horizontalNormalizedPosition = pagePositions[1];
        UpdatePager(0);
    }

    public void OnDrag(PointerEventData eventData)
    {
        isLerping = false;
        StopAllCoroutines();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float currentPos = scrollRect.horizontalNormalizedPosition;
        int nearestPage = 0;
        float minDistance = float.MaxValue;

        for (int i = 0; i < pagePositions.Length; i++)
        {
            float distance = Mathf.Abs(currentPos - pagePositions[i]);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPage = i;
            }
        }
        StartCoroutine(SnapAndLoop(nearestPage));
    }

    IEnumerator SnapAndLoop(int targetPageIndex)
    {
        isLerping = true;
        float targetPos = pagePositions[targetPageIndex];

        while (Mathf.Abs(scrollRect.horizontalNormalizedPosition - targetPos) > 0.001f && isLerping)
        {
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, targetPos, snapSpeed);
            yield return null;
        }

        if (isLerping)
        {
            scrollRect.horizontalNormalizedPosition = targetPos;

            // 無限ループのワープ処理
            if (targetPageIndex == 0) // 左端ダミー(5)なら本物(5)へ
            {
                scrollRect.horizontalNormalizedPosition = pagePositions[realBannerCount];
            }
            else if (targetPageIndex == realBannerCount + 1) // 右端ダミー(1)なら本物(1)へ
            {
                scrollRect.horizontalNormalizedPosition = pagePositions[1];
            }
        }

        // ★追加：移動が終わったことを完全にシステムに分からせる
        scrollRect.StopMovement();
        isLerping = false;

        // ドットの更新 (ワープ後の位置から計算)
        int finalIndex = Mathf.RoundToInt(scrollRect.horizontalNormalizedPosition / itemWidth) - 1;
        UpdatePager(finalIndex);
    }

    void UpdatePager(int index)
    {
        for (int i = 0; i < pagerParent.childCount; i++)
        {
            Image dot = pagerParent.GetChild(i).GetComponent<Image>();
            if (dot != null)
                dot.sprite = (i == index) ? activeSprite : inactiveSprite;
        }
    }
    // 右ボタン用：次のバナーへ
    public void OnNextButton()
    {
        if (isLerping) return; // 動いている最中は無視

        // ボタンの選択状態を解除する
        EventSystem.current.SetSelectedGameObject(null);

        int currentIndex = Mathf.RoundToInt(scrollRect.horizontalNormalizedPosition / itemWidth);
        StartCoroutine(SnapAndLoop(currentIndex + 1));
    }

    // 左ボタン用：前のバナーへ
    public void OnPrevButton()
    {
        if (isLerping) return; // 動いている最中は無視

        // ボタンの選択状態を解除する
        EventSystem.current.SetSelectedGameObject(null);

        int currentIndex = Mathf.RoundToInt(scrollRect.horizontalNormalizedPosition / itemWidth);
        StartCoroutine(SnapAndLoop(currentIndex - 1));
    }
}