using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimBakedData
{
	public byte[] rawBytesData;
	public string name;
	public int animMapWidth;
	public int animMapHeight;
	public float animLen;
	public AnimBakedData(string name, Texture2D texture, float animLen)
	{
		this.name = name;
		this.rawBytesData = texture.GetRawTextureData ();
		animMapWidth = texture.width;
		animMapHeight = texture.height;
		this.animLen = animLen;
	}
}

public class AnimMapBaker
{
	#region 输入数据
	Animation animation = null;
	SkinnedMeshRenderer skinMeshRender = null;
	string modeName;
	Mesh bakedMesh = new Mesh();
	int meshVertexCount;
	bool hasSetData = false;
	#endregion

	public void SetAnimData(GameObject go)
	{
		hasSetData = false;
		if (null == go) {
			Debug.LogError ("输入的GameObject不能为空");
			return;
		}
		animation = go.GetComponent<Animation> ();
		skinMeshRender = go.GetComponentInChildren<SkinnedMeshRenderer> ();
		if (null == animation) {
			Debug.LogError ("在GameObject同层级未发现组件Animation");
			return;
		}
		if(null == skinMeshRender)
		{
			Debug.LogError ("在GameObject中未发现组件SkinnedMeshRenderer");
			return;
		}
		modeName = go.name;
		meshVertexCount = skinMeshRender.sharedMesh.vertexCount;
		hasSetData = true;
	}

	public List<AnimBakedData> Bake()
	{
		if (!hasSetData) {
			return null;
		}
		List<AnimBakedData> bakedDataList = new List<AnimBakedData> ();

		foreach (AnimationState state in animation) {
			AnimBakedData bakedData = BakeSingleAnimClip (state);
			bakedDataList.Add (bakedData);
		}
		return bakedDataList;
	}

	AnimBakedData BakeSingleAnimClip(AnimationState animationState )
	{
		int totalFrame = (int)(animationState.clip.length * animationState.clip.frameRate);
		int textureWidth = Mathf.NextPowerOfTwo (meshVertexCount);
		int textureHeight = Mathf.NextPowerOfTwo (totalFrame);

		Texture2D texture = new Texture2D (textureWidth, textureHeight, TextureFormat.RGBAHalf, false);
		texture.name = string.Format ("{0}_{1}.animMap", modeName, animationState.name);
		animation.Play (animationState.name);

		for (int i = 0; i < textureHeight; ++i) {
			float sampleTime = animationState.clip.length / textureHeight * i;
			animation [animationState.name].time = sampleTime;
			animation.Sample ();
			skinMeshRender.BakeMesh (bakedMesh);

			for (int j = 0; j < bakedMesh.vertexCount; ++j) {
				Vector3 vertice = bakedMesh.vertices [j];
				texture.SetPixel (j, i, new Color (vertice.x, vertice.y, vertice.z));
			}
		}
		texture.Apply ();
		return new AnimBakedData (texture.name, texture, animationState.clip.length);
	}
}

