using UnityEngine;
using System.Collections;

public class UI_SlideIn : MonoBehaviour {

	public enum SlideType{ FromBottom, FromTop, FromLeft, FromRight }
	public SlideType slideType;
	public float startDelay=0;
	public float transitionTime =0.3f;
	RectTransform rectObject;
	Vector2 anchoredPosition;
	Vector2 startingPosition;
	
	//Vector2 tempPosition;

	bool startAnimate = false;

	void Awake(){
		rectObject = GetComponent<RectTransform> ();
		startingPosition.x = rectObject.anchoredPosition.x;
		startingPosition.y = rectObject.anchoredPosition.y;
		anchoredPosition = rectObject.anchoredPosition;
	}

	// Use this for initialization
	void OnEnable () {
		//startAnimate = false;
		OpenWindow ();
		Invoke("StartAnimateCall",startDelay);
	}

	void StartAnimateCall()
	{
		startAnimate = true;
		CancelInvoke("StartAnimateCall");
	}
	public void OpenWindow(){		
		if (slideType == SlideType.FromBottom) {
			startingPosition.y = -Screen.height + (startingPosition.y);

		}
		else if (slideType == SlideType.FromTop) {
			startingPosition.y = Screen.height + startingPosition.y;

		}
		else if (slideType == SlideType.FromLeft) {
			startingPosition.x = -Screen.width + (startingPosition.x);

		}
		else if (slideType == SlideType.FromRight) {
			startingPosition.x = Screen.width + (startingPosition.x);
		
		}
		rectObject.anchoredPosition = startingPosition;
	}

	public void CloseWindow(){
		anchoredPosition.y = Screen.height * 1.25f;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (startAnimate)
		{
			if (slideType == SlideType.FromBottom || slideType == SlideType.FromTop)
			{
				startingPosition.y = Mathf.SmoothStep(startingPosition.y, anchoredPosition.y, transitionTime);
			}
			else if (slideType == SlideType.FromLeft || slideType == SlideType.FromRight)
			{
				startingPosition.x = Mathf.SmoothStep(startingPosition.x, anchoredPosition.x, transitionTime);
			}
			rectObject.anchoredPosition = startingPosition;
		}
	}

	private void OnDisable()
	{
		startAnimate = false;
	}
}
