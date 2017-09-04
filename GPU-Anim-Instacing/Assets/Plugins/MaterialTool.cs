using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


public class MaterialTool{

	static string materialPath = "Assets/Resources/AnimPrefab";
	static List<string> materialNameList = new List<string>(){
		"Footman_Attack01.animMap.mat",
		"Footman_Attack02.animMap.mat",
		"Footman_Death.animMap.mat",
		"Footman_GetHit.animMap.mat",
		"Footman_Idle.animMap.mat",
		"Footman_Run.animMap.mat",
		"Footman_Victory.animMap.mat",
		"Footman_Walk.animMap.mat",
	};

	[MenuItem("Tools/设置使用GPU Instancing")]
	public static void SetAllAnimMatUseInstacing()
	{
		List<Material> matList = LoadAllMaterial ();
		for (int i = 0; i < matList.Count; ++i) {
			matList [i].enableInstancing = true;
		}
	}

	[MenuItem("Tools/取消使用GPU Instancing")]
	public static void CancelAllAnimMatInstacing()
	{
		List<Material> matList = LoadAllMaterial ();
		for (int i = 0; i < matList.Count; ++i) {
			matList [i].enableInstancing = false;
		}
	}

	static List<Material> LoadAllMaterial()
	{
		List<Material> ret = new List<Material> ();
		for (int i = 0; i < materialNameList.Count; ++i) {
			Material mat = AssetDatabase.LoadAssetAtPath<Material>(Path.Combine(materialPath, materialNameList[i]));
			ret.Add (mat);
		}
		return ret;
	}
}
