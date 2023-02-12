using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;


public class FullscreenAd : MonoBehaviour {

	// Start is called before the first frame update
	void Start() {
		fisheEatBobe.adCounter = 0;

	}

	// Update is called once per frame
	void Update() {

	}

	private InterstitialAd interstitial;

	private void RequestInterstitial() {
#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-3940256099942544/1033173712";
		//#elif UNITY_IPHONE
		//string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
		string adUnitId = "unexpected_platform";
#endif

		// Initialize an InterstitialAd.
		this.interstitial = new InterstitialAd(adUnitId);

		// Called when an ad request has successfully loaded.
		this.interstitial.OnAdLoaded += HandleOnAdLoaded;
		// Called when an ad request failed to load.
		this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
		// Called when an ad is shown.
		this.interstitial.OnAdOpening += HandleOnAdOpening;
		// Called when the ad is closed.
		this.interstitial.OnAdClosed += HandleOnAdClosed;

		AdRequest request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		this.interstitial.LoadAd(request);

	}

	private void MakeMoney() {
		if (this.interstitial.IsLoaded()) {
			this.interstitial.Show();
		}
	}

	public void MakeMoneyEverywhere() {
		RequestInterstitial();
		MakeMoney();
	}
	public void HandleOnAdLoaded(object sender, EventArgs args) {
		Time.timeScale = 0;
		AudioListener audioListener = FindObjectOfType<AudioListener>();
		audioListener.enabled = false;

	}

	public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
		MonoBehaviour.print("HandleFailedToReceiveAd event received");
	}

	public void HandleOnAdOpening(object sender, EventArgs args) {
		MonoBehaviour.print("HandleAdOpening event received");
	}

	public void HandleOnAdClosed(object sender, EventArgs args) {
		MonoBehaviour.print("HandleAdClosed event received");
		Time.timeScale = 1;
		AudioListener audioListener = FindObjectOfType<AudioListener>();
		audioListener.enabled = true;
		interstitial.Destroy();
	}
}

