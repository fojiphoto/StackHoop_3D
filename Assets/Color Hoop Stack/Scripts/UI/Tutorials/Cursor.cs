using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public Camera cam;
    public RectTransform canvasRect;
    public RectTransform cursorRect;
    public RectTransform moreStackButton;
    public float ySpaceMove = 10f;
    public float ySpaceMoveTime;
    public float cursorMoveTime;
    Sequence seq;

    private void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ON_MOVE_TUTORIAL_CURSOR, param => MoveCursor());
    }

    private void OnEnable()
    {
        if (GameplayMgr.Instance.ringStackList.Count > 0)
        {
            if (GameplayMgr.Instance.currentLevel == 0)
            {
                Vector2 cursorScreenPos = GetScreenPosFromWorldPos(GameplayMgr.Instance.ringStackList[0].transform.position) - new Vector2(0, 400f);
                cursorRect.anchoredPosition = cursorScreenPos;
            }
            else if (GameplayMgr.Instance.currentLevel == 2)
            {
                Vector2 cursorScreenPos = moreStackButton.anchoredPosition + new Vector2(450f, canvasRect.sizeDelta.y * 0.5f - 300f);
                cursorRect.anchoredPosition = cursorScreenPos;
            }
            PlayIdleCursorAnimation(cursorRect.anchoredPosition);
        }
    }

    public void PlayIdleCursorAnimation(Vector2 anchoredPos)
    {
        seq = DOTween.Sequence();
        seq.Append(cursorRect.DOAnchorPos(anchoredPos + new Vector2(0f, ySpaceMove), ySpaceMoveTime).SetEase(Ease.Linear));
        seq.Append(cursorRect.DOAnchorPos(anchoredPos, ySpaceMoveTime).SetEase(Ease.Linear));
        seq.SetLoops(-1);
    }

    public Vector2 GetScreenPosFromWorldPos(Vector3 position)
    {
        Vector2 cursorViewportPos = cam.WorldToViewportPoint(position);
        Vector2 cursorScreenPos 
            = new Vector2(
                (cursorViewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
                (cursorViewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f));
        return cursorScreenPos;
    }

    public void MoveCursor()
    {
        seq.Kill();

        Vector2 newPos = GetScreenPosFromWorldPos(GameplayMgr.Instance.ringStackList[1].transform.position) - new Vector2(0f, 400f);
        cursorRect.DOLocalMove(newPos, cursorMoveTime).SetEase(Ease.Linear).OnComplete(()=> PlayIdleCursorAnimation(newPos));
    }

    public void OnDisable()
    {
        seq.Kill();
    }
}
