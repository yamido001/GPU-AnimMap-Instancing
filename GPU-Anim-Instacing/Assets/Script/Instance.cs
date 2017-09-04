using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instance : MonoBehaviour {

	public GameObject[] prefabTemplate;
	public float xInterval;
	public float yInterval;
	public int xCount;
	public int yCount;

	// Use this for initialization
	void Start () {
		CreateInstance ();
	}

	bool CheckData()
	{
		for (int i = 0; i < prefabTemplate.Length; ++i) {
			if (prefabTemplate [i] == null) {
				Debug.LogError ("请确保输入的prefabTemplate中不为空");
				return false;
			}
		}
		return true;
	}

	GameObject GetRandomPrefab()
	{
		return GameObject.Instantiate(prefabTemplate[Random.Range(0, prefabTemplate.Length - 1)]);
	}

	void CreateInstance()
	{
		if (!CheckData ()) {
			return;
		}

		MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock ();
		for (int i = 0; i < xCount; ++i) {
			for (int j = 0; j < yCount; ++j) {
				GameObject childPrefab = GetRandomPrefab();
				childPrefab.transform.SetParent (this.transform);
				childPrefab.transform.localScale = Vector3.one;
				childPrefab.transform.eulerAngles = Vector3.zero;
				float xPos = (i - xCount / 2) * xInterval;
				float zPos = (j - yCount / 2) * yInterval;
				childPrefab.transform.localPosition = new Vector3 (xPos, 0f, zPos);
				//				MeshRenderer render = childPrefab.GetComponent<MeshRenderer> ();
				//
				//				propertyBlock.SetColor ("_ReplaceColor", new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f)));
				//				render.SetPropertyBlock (propertyBlock);
			}
		}
	}
}
