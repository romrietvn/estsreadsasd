////////////////////////////////////////////////////////////////////////////////
//  
// @module Google Ads Unity SDK 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;
using System.Collections.Generic;


#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2 
#else
using UnityEngine.SceneManagement;
#endif


//Attach the script to the empty gameobject on your sceneS
public class AdMobBannerInterstitial : MonoBehaviour {

	


	// --------------------------------------
	// Unity Events
	// --------------------------------------
	
	void Awake() {


		if(!GoogleMobileAd.IsInited) {
			GoogleMobileAd.Init();
		}



	}


	// --------------------------------------
	// PUBLIC METHODS
	// --------------------------------------

	public void ShowBanner() {
		GoogleMobileAd.ShowInterstitialAd();
	}



	// --------------------------------------
	// GET / SET
	// --------------------------------------



	public string sceneBannerId {
		get {
            return "FullScreen";
		}
	}

	
}
