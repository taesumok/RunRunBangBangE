using UnityEngine;
using UnityEngine.Advertisements;
 
public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] RewardedAdsButton rewardedAdsButton;
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode;
    private string _gameId;
 
    void Awake()
    {
        Debug.Log("AdsManager!");
        InitializeAds();
    }
   
    public void InitializeAds()
    {
    #if UNITY_IOS
            _gameId = _iOSGameId;
    #elif UNITY_ANDROID
            _gameId = _androidGameId;
    #elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
    #endif
        Debug.Log("_gameId : " + _gameId);
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Debug.Log("InitializeAds!");
            Advertisement.Initialize(_gameId, _testMode, this);
        }
        else{
            rewardedAdsButton.LoadAd();
        }

    }

 
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");

        rewardedAdsButton.LoadAd();
        
    }
 
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}