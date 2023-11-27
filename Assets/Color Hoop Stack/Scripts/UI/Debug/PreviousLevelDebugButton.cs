using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviousLevelDebugButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ON_GO_PREVIOUS_LEVEL_DEBUG, param => OnClick());
    }

    public void OnClick()
    {
        GameplayMgr.Instance.GoPreviousLevel();
    }
}
