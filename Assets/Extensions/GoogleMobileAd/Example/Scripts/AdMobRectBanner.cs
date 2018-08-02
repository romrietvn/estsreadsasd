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
public class AdMobRectBanner : MonoBehaviour
{


    public GADBannerSize size = GADBannerSize.SMART_BANNER;
    public TextAnchor anchor = TextAnchor.LowerCenter;


    private static Dictionary<string, GoogleMobileAdBanner> _registerdBanners = null;


    // --------------------------------------
    // Unity Events
    // --------------------------------------

    void Awake()
    {


        if (!GoogleMobileAd.IsInited)
        {
            GoogleMobileAd.Init();
        }



    }

    // --------------------------------------
    // PUBLIC METHODS
    // --------------------------------------

    public void ShowBanner()
    {
        GoogleMobileAdBanner banner;
        if (registerdBanners.ContainsKey(sceneBannerId))
        {
            banner = registerdBanners[sceneBannerId];
        }
        else
        {
            banner = GoogleMobileAd.CreateAdBanner(anchor, size);
            registerdBanners.Add(sceneBannerId, banner);
        }

        if (banner.IsLoaded && !banner.IsOnScreen)
        {
            banner.Show();
        }
    }

    public void HideBanner()
    {
        if (registerdBanners.ContainsKey(sceneBannerId))
        {
           
            GoogleMobileAdBanner banner = registerdBanners[sceneBannerId];
            if (banner.IsLoaded)
            {
                if (banner.IsOnScreen)
                {
                    banner.Hide();
                }
            }
            else
            {
                banner.ShowOnLoad = false;
            }
        }
    }

    // --------------------------------------
    // GET / SET
    // --------------------------------------


    public static Dictionary<string, GoogleMobileAdBanner> registerdBanners
    {
        get
        {
            if (_registerdBanners == null)
            {
                _registerdBanners = new Dictionary<string, GoogleMobileAdBanner>();
            }

            return _registerdBanners;
        }
    }

    public string sceneBannerId
    {
        get
        {
            return "RectBanner";
        }
    }


}
