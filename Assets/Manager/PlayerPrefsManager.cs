using UnityEngine;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour {

	const string MICROPHONE_KEY 	= "microphone";
	const string SENSITIVITY_KEY 	= "sensitivity";
	const string SAMPLES_KEY 		= "samples";
	const string OPTIMIZESAMPLES_KEY= "optimizeSamples";
	const string THRESHOLD_KEY 		= "threshold";

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
			Debug.LogError("Threshold "+threshold.ToString()+" out of range");
		}
	}

	public static float GetThreshold (){
		return PlayerPrefs.GetFloat (THRESHOLD_KEY);
	}

	public static void SetSamples (int samples) {
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

	public static void SetOptimizeSamples (float optimizeSamples) {
		if (optimizeSamples >= 1.00f && optimizeSamples <= 10.0f) {
			PlayerPrefs.SetFloat (OPTIMIZESAMPLES_KEY, optimizeSamples);
		} else {
			Debug.LogError("optimizeSamples "+optimizeSamples.ToString()+" out of range");
		}
	}

	public static float GetOptimizeSamples (){
		if(PlayerPrefs.GetFloat (OPTIMIZESAMPLES_KEY) <=0.0f){
			return 1.0f;
		}
		return PlayerPrefs.GetFloat (OPTIMIZESAMPLES_KEY);
	}

	
}