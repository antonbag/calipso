using UnityEngine;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour {

	const string MICROPHONE_KEY = "microphone";
	const string SENSITIVITY_KEY = "sensitivity";

	const string SAMPLES_KEY = "samples";

	const string THRESHOLD_KEY = "threshold";

	public static void SetMicrophone (int mic) {
		PlayerPrefs.SetInt (MICROPHONE_KEY, mic);
	}

	public static int GetMicrophone (){
		return PlayerPrefs.GetInt (MICROPHONE_KEY);
	}

	public static void SetSensitivity (float sensitivity) {
		if (sensitivity >= 1f && sensitivity <= 1000f) {
			PlayerPrefs.SetFloat (SENSITIVITY_KEY, sensitivity);
		} else {
			Debug.LogError("Sensitivity out of range");
		}
	}

	public static float GetSensitivity (){
		return PlayerPrefs.GetFloat (SENSITIVITY_KEY);
	}

	public static void SetThreshold (float threshold) {
		if (threshold >= 0f && threshold <= 1f) {
			PlayerPrefs.SetFloat (THRESHOLD_KEY, threshold);
		} else {
			Debug.LogError("Threshold out of range");
		}
	}

	public static float GetThreshold (){
		return PlayerPrefs.GetFloat (THRESHOLD_KEY);
	}

	public static void SetSamples (int samples) {
		Debug.Log(samples);
		if (samples >= 64 && samples <= 1024) {
			PlayerPrefs.SetInt (SAMPLES_KEY, samples);
		} else {
			//no debería pasar nunca... pero porsiaca
			Debug.LogError("samples out of range"+samples);
		}
	}
 
	public static int getSamples () {
		return PlayerPrefs.GetInt(SAMPLES_KEY);
	}

	
}