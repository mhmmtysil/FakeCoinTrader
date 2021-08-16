using System.Collections;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{
	static AdManager instance;
    public GameObject rewardButton;

	private RewardedAd rewardedGemVideo;
	string videoAdMobId = "ca-app-pub-7699077044967949/3892121663";

	public static AdManager Instance
	{
		get
		{
			if (instance == null)
				instance = GameObject.FindObjectOfType(typeof(AdManager)) as AdManager;

			return instance;
		}
	}
	void Awake()
    {
		MobileAds.Initialize(initStatus => { });
		RequestRewarded();
	}

	#region Rewarded Field

	public void RequestRewarded()
	{
		this.rewardedGemVideo = new RewardedAd(videoAdMobId);

		rewardedGemVideo.OnAdLoaded += HandleRewardedAdLoaded;
		rewardedGemVideo.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
		rewardedGemVideo.OnAdOpening += HandleRewardedAdOpening;
		rewardedGemVideo.OnAdFailedToShow += HandleRewardedAdFailedToShow;
		rewardedGemVideo.OnUserEarnedReward += HandleUserEarnedReward;
		rewardedGemVideo.OnAdClosed += HandleRewardedAdClosed;

		AdRequest request = new AdRequest.Builder().Build();
		this.rewardedGemVideo.LoadAd(request);
	}

	public void CheckVideoisReady()
	{
		if (rewardedGemVideo.IsLoaded())
		{
			rewardedGemVideo.Show();
		}
		else
		{
			rewardButton.SetActive(false);
		}
	}

	#endregion

	#region Rewarded CallBacks
	public void HandleRewardedAdLoaded(object sender, EventArgs args)
	{
		Debug.Log("HandleRewardedAdLoaded event received");
	}

	public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
	{
		Debug.Log("HandleRewardedAdFailedToLoad event received with message: " + args.Message);
		RequestRewarded();
	}

	public void HandleRewardedAdOpening(object sender, EventArgs args)
	{
		Debug.Log("HandleRewardedAdOpening event received");
	}

	public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
	{
		Debug.Log("HandleRewardedAdFailedToShow event received with message: " + args.Message);
		RequestRewarded();
	}

	public void HandleRewardedAdClosed(object sender, EventArgs args)
	{
		Debug.Log("HandleRewardedAdClosed event received");
		RequestRewarded();
	}

	public void HandleUserEarnedReward(object sender, Reward args)
	{
		GameManager.Instance.GiveEmerald(50);
		StartCoroutine(CloseAndReopenRewardButton());
	}
	#endregion

	IEnumerator CloseAndReopenRewardButton()
    {
		RequestRewarded();
		rewardButton.SetActive(false);
        yield return new WaitForSeconds(4);
        rewardButton.SetActive(true);
    }
}
