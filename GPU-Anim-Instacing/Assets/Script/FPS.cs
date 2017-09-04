using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour {

	int mFrames = 0;
	int mFPS = 0;
	// Use this for initialization
	void Start () {
		InvokeRepeating ("UpdateFPS", 0f, 1f);		
	}
	
	// Update is called once per frame
	void Update () {
		++mFrames;
	}

	void UpdateFPS()
	{
		mFPS = mFrames;
		mFrames = 0;
	}

	void OnGUI(){
		GUIStyle style=new GUIStyle();
		style.normal.background = null;    //这是设置背景填充的
		style.normal.textColor=new Color(1,0,0);   //设置字体颜色的
		style.fontSize = 80;       //当然，这是字体颜色

		GUI.Label (new Rect (0, 0, 200, 200), "fps:" + mFPS, style);
	}
}
