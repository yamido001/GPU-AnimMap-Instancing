using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AnimMapBakerWindow : EditorWindow {

	#region 属性
	AnimMapBaker baker = null;
	GameObject targetGo = null;
	Shader shader = null;
	string subPath = null;
	#endregion

	[MenuItem("Window/AnimMapBaker")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(AnimMapBakerWindow));
	}

	void OnGUI()
	{
		EditorGUILayout.LabelField ("输入需要导出的Prefab");
		targetGo = (GameObject)EditorGUILayout.ObjectField (targetGo, typeof(GameObject), true);

		EditorGUILayout.LabelField ("选择导出时使用的shader");
		shader = (Shader)EditorGUILayout.ObjectField (shader, typeof(Shader), true);

		EditorGUILayout.LabelField ("输入保存路径");
		subPath = EditorGUILayout.TextField (subPath);


		if (GUILayout.Button ("Bake")) {
			if (null == baker) {
				baker = new AnimMapBaker ();
			}
			baker.SetAnimData (targetGo);
			List<AnimBakedData> bakedDataList = baker.Bake ();
			for (int i = 0; i < bakedDataList.Count; ++i) {
				AnimBakedData bakedData = bakedDataList [i];
				SavePrefab (bakedData);
			}
		}
	}

	void SavePrefab(AnimBakedData data)
	{
		Material mat = SaveMaterial (data);
		if (null == mat) {
			return;
		}
		GameObject go = new GameObject ();
		go.AddComponent<MeshRenderer> ().sharedMaterial = mat;
		go.AddComponent<MeshFilter> ().sharedMesh = targetGo.GetComponentInChildren<SkinnedMeshRenderer> ().sharedMesh;
		string folderPath = GetFolderPath();
		PrefabUtility.CreatePrefab(Path.Combine(folderPath, data.name + ".prefab").Replace("\\", "/"), go);
	}

	Material SaveMaterial(AnimBakedData data)
	{
		if (shader == null) {
			EditorUtility.DisplayDialog ("error", "请设定使用的shader", "OK");
			return null;
		}
		SkinnedMeshRenderer skinMeshRender = targetGo.GetComponentInChildren<SkinnedMeshRenderer> ();
		if (skinMeshRender == null) {
			EditorUtility.DisplayDialog ("error", "targetGo中没有找到SkinnedMeshRenderer", "OK");
			return null;
		}
		Texture2D animMap = SaveTexture (data);
		Material mat = new Material (shader);
		mat.SetTexture ("_MainTex", skinMeshRender.sharedMaterial.mainTexture);
		mat.SetTexture ("_AnimMap", animMap);
		mat.SetFloat ("_AnimLen", data.animLen);
		SaveObject (mat, data.name, ".mat");
		return mat;
	}

	Texture2D SaveTexture(AnimBakedData data)
	{
		Texture2D animMap = new Texture2D(data.animMapWidth, data.animMapHeight, TextureFormat.RGBAHalf, false);
		animMap.LoadRawTextureData(data.rawBytesData);
		SaveObject(animMap, data.name, ".asset");
		return animMap;
	}

	void SaveObject(Object obj, string name, string suffix)
	{
		string folderPath = GetFolderPath ();
		string savedPath = System.IO.Path.Combine (folderPath, name + suffix);
		AssetDatabase.CreateAsset (obj, savedPath);
		AssetDatabase.Refresh ();
	}

	string GetFolderPath()
	{
		string folderPath = System.IO.Path.Combine ("Assets", subPath);
		if (!AssetDatabase.IsValidFolder (folderPath)) {
			AssetDatabase.CreateFolder ("Assets", subPath);
		}
		return folderPath;
	}
}
