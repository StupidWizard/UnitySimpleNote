using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ReadWriteLocalExample : MonoBehaviour {

	public Texture2D testTexture;
	public string fileName = "test_texture.png";

	public RawImage rawImage;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	string localDataPath {
		get {
			return System.IO.Path.Combine(Application.persistentDataPath, fileName);
		}
	}


	void SaveTextureToLocal() {
		byte[] bytes = GetTestData();
		string path = localDataPath;
		System.IO.File.WriteAllBytes(path, bytes);

		Debug.Log("Saved texture to path: " + path);
	}

	void LoadLocalTextureByWWW() {
		string path = "file://" + localDataPath;
		StartCoroutine(DoLoadLocalTextureWWW(path));
	}

	IEnumerator DoLoadLocalTextureWWW(string path) {
		WWW www = new WWW(path);
		yield return www;

		if (!string.IsNullOrEmpty(www.error)) {
			Debug.LogError("Load error: " + www.error);
		}

		rawImage.texture = www.texture;
		Debug.Log("Load www Success!");
	}


	void LoadLocalTextureByFileIO() {
		string path = localDataPath;

		if (File.Exists(path))     {
			var fileData = File.ReadAllBytes(path);
			var tex = new Texture2D(2, 2);
			tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
			rawImage.texture = tex;

			Debug.Log("Load IO Success!");
		} else {
			Debug.LogError("File not found");
		}
	}

	void OnGUI() {
		if (GUI.Button(new Rect(10, 10, Screen.width/6, Screen.height/8), "Save")) {
			SaveTextureToLocal();
		}

		if (GUI.Button(new Rect(10, 20 + Screen.height/8, Screen.width/6, Screen.height/8), "Load www")) {
			LoadLocalTextureByWWW();
		}

		if (GUI.Button(new Rect(10, 2 * (20 + Screen.height/8), Screen.width/6, Screen.height/8), "Load force")) {
			LoadLocalTextureByFileIO();
		}
	}







	// dont care this
	byte[] GetTestData() {
		Texture2D texture = new Texture2D(testTexture.width, testTexture.height);
		for (int i = 0; i < texture.width; i++) {
			for (int j = 0; j < texture.height; j++) {
				texture.SetPixel(i, j, testTexture.GetPixel(i, j));
			}
		}
		texture.Apply();

		return texture.EncodeToPNG();
	}
}
