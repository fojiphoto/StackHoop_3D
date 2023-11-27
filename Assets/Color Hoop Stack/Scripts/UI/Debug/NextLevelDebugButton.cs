using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelDebugButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ON_GO_NEXT_LEVEL_DEBUG, param => OnClick());
    }

    public void OnClick()
    {
        GameplayMgr.Instance.GoNextLevel();
    }
}
