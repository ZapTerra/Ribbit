using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class InitializeAds : MonoBehaviour {
	public void Start() {
		// Initialize the Google Mobile Ads SDK.
		MobileAds.Initialize(initStatus => { });
	}
}
