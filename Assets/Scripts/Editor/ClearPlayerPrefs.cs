using UnityEngine;
using UnityEditor;
using System.Collections;

public class ClearPlayerPrefs : MonoBehaviour 
{
	[MenuItem("Util/ClearPlayerPrefs")] 
	static void DeleteMyPlayerPrefs() 
	{ 
		PlayerPrefs.DeleteAll();
	} 
}
