﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

namespace Application {
public class ResultController : MonoBehaviour
  {
    private PlayerManager pm;

    private Text
        deviceConfigurationText,
        theoryApplicationText,
        timeMalusText,
        totalText,
        outcomeText,
        qualitativeDeviceText,
        qualitativeTheoryText,
        qualitativeTimeText;
    private SpriteRenderer memoji;

    public VideoPlayer videoPlayer;

    private Color green;
    private Color red;
    private Color grey;

    private AudioSource resultAudio;

    private SpriteRenderer retryButton;

    private Image videoBd;

        // Start is called before the first frame update

    IEnumerator disableDackdrop(float audioTimer)
    {
        yield return new WaitForSeconds(audioTimer);
        videoBd.enabled = false;
        Destroy(videoPlayer);
    }

    void Start()
    {
      pm = PlayerManager.getInstance();
      initUI();

      if (pm.outcome == Outcome.EXPLOSION) {
        videoPlayer
            = GameObject.Find("Video Player").GetComponent<VideoPlayer>();
        videoBd.enabled = true;
        videoPlayer.Play();
        StartCoroutine(disableDackdrop(3.6f));

      } else if (pm.outcome == Outcome.VERY_GOOD || pm.outcome == Outcome.GOOD) {
          resultAudio = GameObject.Find("pokHeal").GetComponent<AudioSource>();
          resultAudio.Play();
            GameObject
                .Find("retryButton")
                .GetComponent<Image>()
                .enabled = false;
      } else if (pm.outcome == Outcome.UNCHANGED) {
          resultAudio = GameObject.Find("cricket").GetComponent<AudioSource>();
          resultAudio.Play();
      } else {
          resultAudio = GameObject.Find("ouch").GetComponent<AudioSource>();
          resultAudio.Play();
      }

      green = new Color(0.5647059f, 1f, 0.48f, 1f);
      red = new Color(1f, 0.13f, 0f, 1f);
      grey = new Color(0.3396f, 0.3396f, 0.3396f, 1f);

      deviceConfigurationText.text = pm.computeMedicalEquipmentScore().ToString("f0");
      theoryApplicationText.text = pm.computeOutcomeScore().ToString("f0");
      timeMalusText.text = (pm.time).ToString("f0");
      totalText.text = pm.getTotalScore().ToString("f0");
      outcomeText.text = pm.outcome.ToString().Replace("_", " ");

      qualitativeDeviceText.text = pm.getQualitativeScore();
      qualitativeTheoryText.text = pm.getQualitativeScore();
      qualitativeTimeText.text = pm.getQualitativeTimeScore();

      qualitativeDeviceText.color = computeColor(qualitativeDeviceText.text);
      qualitativeTheoryText.color = computeColor(qualitativeTheoryText.text);
      qualitativeTimeText.color = computeColor(qualitativeTimeText.text);

      deviceConfigurationText.color = qualitativeDeviceText.color;
      theoryApplicationText.color = qualitativeTheoryText.color;
      timeMalusText.color = qualitativeTimeText.color;

      totalText.color = int.Parse(totalText.text) < 0 ? red : green;
      outcomeText.color = computeColor(outcomeText.text);

      memoji.sprite = Resources.Load(pm.medicalReport.getMemojiPathWithOutcome(pm.outcome.ToString()), typeof(Sprite)) as Sprite;
    }

    private Color computeColor(string outcome)
    {
      outcome = outcome.ToLower();
      if (outcome.Equals("very good") || outcome.Equals("good")) return green;
      if (outcome.Equals("neutral") || outcome.Equals("unchanged")) return grey;
      return red;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void initUI()
    {
      deviceConfigurationText = GameObject.Find("device-configuration").GetComponent<Text>();
      theoryApplicationText = GameObject.Find("theory-application").GetComponent<Text>();
      timeMalusText = GameObject.Find("time-malus").GetComponent<Text>();
      totalText = GameObject.Find("total").GetComponent<Text>();
      outcomeText = GameObject.Find("outcome").GetComponent<Text>();

      qualitativeDeviceText = GameObject.Find("qd").GetComponent<Text>();
      qualitativeTheoryText = GameObject.Find("qt").GetComponent<Text>();
      qualitativeTimeText = GameObject.Find("qb").GetComponent<Text>();

      memoji = GameObject.Find("memoji").GetComponent<SpriteRenderer>();
      videoBd = GameObject.Find("video-backdrop").GetComponent<Image>();
    }

    public void goHome() {
        PlayerManager.Instance.destroy();
        
        SceneManager.LoadScene(Constants.MENU, LoadSceneMode.Single);
    }

    public void retry() {
        int pathologyBackup = (int)pm.medicalReport.pathology.name;
        
        PlayerManager.Instance.destroy();

        new PathologyController().setPathology(pathologyBackup);

        SceneManager.LoadScene(Constants.GAME_1, LoadSceneMode.Single);

    }

    public void onDestroy(){
      Debug.Log("destroy");
      videoBd.enabled = false;
    }
  }

}