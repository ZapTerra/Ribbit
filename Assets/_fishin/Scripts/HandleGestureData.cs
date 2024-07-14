using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class HandleGestureData : MonoBehaviour {

	//put all the gesture files in persistentDataPath into one filele
	// public static void SaveFilesToHunkaloChunk() {
	// 	using (BinaryWriter writer = new BinaryWriter(File.Open("YourData.ree", FileMode.Create))) {
	// 		string[] paths = Directory.GetFiles(Application.persistentDataPath, "*.xml");
	// 		writer.Write(paths.Length);
	// 		foreach (string path in paths) {
	// 			string fileName = Path.GetFileName(path);
	// 			string contents = File.ReadAllText(path);
	// 			writer.Write(fileName);
	// 			writer.Write(contents);
	// 		}
	// 	}
	//

	//download all the gesture files in persistentDataPathth
	// public static void WriteHunkaloChunkToDownloadedRAM() {
	// 	SaveFilesToHunkaloChunk();
	// 	byte[] bytes = File.ReadAllBytes("YourData.ree");
	// 	string file = Convert.ToBase64String(bytes);
	// 	Debug.Log("cry about it");
	// 	BrowserTextDownload("FileForC.jnd", file);
	//

	//unpack a chunky hunk
	//(get all the gesture files out of a file created by WriteHunkaloChunkToDownloadedRAM)M)
	// public static void UnpackThatHunkyChunk() {
	// 	using (BinaryReader reader = new BinaryReader(File.Open("FileForC.jnd", FileMode.Open))) {
	// 		int length = reader.ReadInt32();
	// 		for (int i = 0; i < length; i++) {
	// 			File.WriteAllText("D:/hoppin' minigame/Assets/GesturesFromFriends/" + reader.ReadString(), reader.ReadString());
	// 		}
	// 	}
	// }

	// [DllImport("__Internal")]
	// public static extern void BrowserTextDownload(string filename, string base64Data);

}
