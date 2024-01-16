#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RingStack : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public Stack<Ring> ringStack;
    [HideInInspector] public bool canControl = true;
    public BoxCollider boxCol;
    [HideInInspector] public int number;
    public static RingStack ringStacks;

   

    private void Awake()
    {
       
        ringStack = new Stack<Ring>();
        if (ringStacks==null)
        {
            ringStacks=this;
        }
    }

    private void OnEnable()
    {
        ringStack.Clear();
        canControl = true;
        GameplayMgr.Instance.DeactivateCapForAllRingStacks();
    }

    // Start is called before the first frame update
    private void Start()
    {
       
    }

    // Update is called once per frame
    private void Update()
    {
        if(!IsStackFullSameColor())
        transform.GetChild(2).gameObject.SetActive(false);
        
    }

    public void AddNewRing(Ring newRing)
    {
        ringStack.Push(newRing);
        newRing.transform.SetParent(transform);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (canControl)
        {
            InputMgr.Instance.HandleTap(this);
        }

    }

    public bool IsStackFullSameColor()
    {
        if(ringStack!=null & ringStack.Peek().ringType != RingType.NONE){
        RingType firstRingType = ringStack.Peek().ringType;
        foreach (Ring ring in ringStack)
        {
            if (!(ring.ringType == firstRingType))
                return false;
        }

        if (ringStack.Count < GameplayMgr.Instance.stackNumberMax)
            return false;
        }
        return true;
    }
}
