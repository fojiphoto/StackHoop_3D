using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCorrect : MonoBehaviour
{
    public float yOffset = 10f;
    public Camera cam;
    public RectTransform canvasRect;

    private List<Corrector> correctorList;

    private void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ON_ENABLED_CORRECTOR , param => EnableTutorial((RingStack) param));
        EventDispatcher.Instance.RegisterListener(EventID.ON_DISABLED_CORRECTOR, param => DisableTutorial());

        correctorList = new List<Corrector>();
    }

    public void EnableTutorial(RingStack ringStackStart)
    {
        DisableTutorial();
        foreach (RingStack ringStack in GameplayMgr.Instance.ringStackList)
        {
            if (ringStack.number != ringStackStart.number)
            {
                Vector3 screenPos = GetScreenPosFromWorldPos(ringStack.transform.position) + new Vector3(0f, yOffset, 0f);
                Corrector corrector = PoolerMgr.Instance.correctPooler.GetNextCorrector(CanMoveRingToStack(ringStackStart, ringStack), screenPos);
                correctorList.Add(corrector);
            }
        }
    }

    public void DisableTutorial()
    {
        if (correctorList.Count > 0)
        {
            foreach (Corrector corrector in correctorList)
            {
                PoolerMgr.Instance.correctPooler.ReturnPooledObject(corrector.gameObject);
            }
            correctorList.Clear();
        }
    }

    public bool CanMoveRingToStack(RingStack ringStackStart, RingStack ringStack)
    {
        if (ringStack.ringStack.Count == 4)
        {
            return false;
        }  
        else if (ringStack.ringStack.Count == 0)
        {
            return true;
        }
        else 
        {
            if (ringStack.ringStack.Peek().ringType == ringStackStart.ringStack.Peek().ringType)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public Vector3 GetScreenPosFromWorldPos(Vector3 position)
    {
        Vector3 outputViewportPos = cam.WorldToViewportPoint(position);
        Vector3 outputScreenPos = 
            new Vector3(
                (outputViewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
                (outputViewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f), 
                0f);
        return outputScreenPos;
    }
}
