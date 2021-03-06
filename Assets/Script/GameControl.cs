﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Windows.Kinect;

public class GameControl : MonoBehaviour {

	public GameObject BodyView;
	public GameObject Lime;
	public GameObject Cheese;
	public GameObject Chilli;
	public GameObject Bomb;
	public GameObject DetectPlayerManager;

	public GameObject Score1Text;
	public GameObject Score2Text;
	public GameObject TimeText;
	public GameObject ResultText;
	public Slider SpeedSlider;
	public Slider TimeSlider;
	public Text SetTimeText;

	public GUISkin GameGUISkin;

	public UnityEngine.AudioSource BackgroundSound;
	public UnityEngine.AudioSource CountDownSound;

	public static int Score1 = 0;
	public static int Score2 = 0;

	private DetectPlayers _playerManager;

	private GameObject[] _laysProducts; 
	private float _player1Time = 0f;
	private float _player2Time = 0f;
	private float _bomb1Time = 0f;
	private float _bomb2Time = 0f;
	private float _countDownTime = 0f;

	private float player1CreateTime;
	private float player2CreateTime;
	private float bomb1CreateTime;
	private float bomb2CreateTime;


	private float _time;
	private float _countedTime;
	private float _moveSpeed;
	private bool isPlaying = false;

	// Use this for initialization
	void Start () {
		
		_playerManager = DetectPlayerManager.GetComponent<DetectPlayers> ();

		_laysProducts = new GameObject[]{Lime, Chilli, Cheese};

		_moveSpeed = PlayerPrefs.GetFloat ("BodyMoveSpeed");
		if (_moveSpeed == 0) {
			_moveSpeed = 10f;
			PlayerPrefs.SetFloat ("BodyMoveSpeed", _moveSpeed);
		}

		SpeedSlider.value = _moveSpeed;

		float value = PlayerPrefs.GetFloat ("TimeSliderValue");
		if (value == 0) {
			PlayerPrefs.SetFloat ("TimeSliderValue", 2.0f);
			value = 2f;
		} else {
			value = PlayerPrefs.GetFloat ("TimeSliderValue");
		}
		TimeSlider.value = value;
		_countedTime = value * 15f;
		_time = _countedTime + 1f;
		SetTimeText.text = "時間: " + _countedTime + " 秒";
		TimeText.GetComponent<TextMesh>().text = _countedTime.ToString();
		Score1Text.GetComponent<TextMesh> ().text = "金呷讚: " + Score1.ToString ();
		Score2Text.GetComponent<TextMesh> ().text = "一定銀: " + Score2.ToString ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey ("escape")) {
			Application.Quit ();
		}

		if (Input.GetKey ("f10")) {
			BodyView.SetActive (!BodyView.activeSelf);
		}

		_player1Time += Time.deltaTime;
		_player2Time += Time.deltaTime;
		_bomb1Time += Time.deltaTime;
		_bomb2Time += Time.deltaTime;
		_countDownTime += Time.deltaTime;

		if (isPlaying) {

			if (_player1Time > player1CreateTime) {
				int i = Random.Range (0, 3);
				GameObject _laysProductClone = Instantiate<GameObject> (_laysProducts[i], new Vector3 (Random.Range (-8.5f, -1f), 4f, 0), transform.rotation);
				Destroy (_laysProductClone, 4);
				player1CreateTime = Random.Range (0.5f, 1f);
				_player1Time = 0;
			}

			if (_player2Time > player2CreateTime) {
				int i = Random.Range (0, 3);
				GameObject _laysProductClone = Instantiate<GameObject> (_laysProducts[i], new Vector3 (Random.Range (1f, 8.5f), 4f, 0), transform.rotation);
				Destroy (_laysProductClone, 4);
				player2CreateTime = Random.Range (0.5f, 1f);
				_player2Time = 0;
			}
		
			_time -= Time.deltaTime;
			int time = (int)_time;
			TimeText.GetComponent<TextMesh>().text = time.ToString();
			Score1Text.GetComponent<TextMesh> ().text = "金呷讚: " + Score1.ToString ();
			Score2Text.GetComponent<TextMesh> ().text = "一定銀: " + Score2.ToString ();

			if (_time < 25) {
				if (_bomb1Time > bomb1CreateTime) {
					GameObject bombClone1 = Instantiate<GameObject> (Bomb, new Vector3 (Random.Range (-8f, -1f), 4f, 0), transform.rotation);
					Destroy (bombClone1, 3);
					bomb1CreateTime = Random.Range (0.5f, 2f);
					_bomb1Time = 0f;
				}

				if (_bomb2Time > bomb2CreateTime) {
					GameObject bombClone2 = Instantiate<GameObject> (Bomb, new Vector3 (Random.Range (1f, 8f), 4f, 0), transform.rotation);
					Destroy (bombClone2, 3);
					bomb2CreateTime = Random.Range (0.5f, 2f);
					_bomb2Time = 0f;
				}
			
			}

			if (_time < 6 && _countDownTime > 7) {
				CountDownSound.Play ();
				_countDownTime = 0f;
			}

			if (_time < 0) {
				isPlaying = false;
				StartCoroutine (Gameover ());
			} else {
			}

			if (isPlaying) {
				ResultText.SetActive(false);
			} else {
				if (Score1 > 0 || Score2 > 0) {
					ResultText.SetActive (true);
					string result;
					if (Score1 == Score2) {
						result = "雙方平手!!";
					} else {
						if (Score1 > Score2) {
							result = "恭喜 金呷讚 獲勝!";
						} else {
							result = "恭喜 一定銀 獲勝!";
						}
					}

					ResultText.GetComponent<TextMesh> ().text =  result;
				}

			}
		}
			

	}

	void OnGUI() {
		GUI.skin = GameGUISkin;

		SpeedSlider.onValueChanged.AddListener (delegate {setBodyMoveSpeed ();});
		TimeSlider.onValueChanged.AddListener (delegate {
			setCountedTime();	 
		});

		float buttonPosX = 100f;
		float buttonGap = 10f;
		float buttonWidth = Screen.width / 6f;
		float buttonPositionY = Screen.height * 0.85f;
		if ( GUI.Button (new Rect (buttonPosX, buttonPositionY, buttonWidth, 60), "Play") ){
			BackgroundSound.Play ();
			isPlaying = true;
			_time = _countedTime + 1f;
			Score1 = 0;
			Score2 = 0;
		}

		if ( GUI.Button (new Rect (buttonPosX + (buttonWidth + buttonGap), buttonPositionY, buttonWidth, 60), "Stop") ){
			isPlaying = false;
			_time = _countedTime + 1f;
			Score1 = 0;
			Score2 = 0;
			TimeText.GetComponent<TextMesh>().text = _countedTime.ToString();
			Score1Text.GetComponent<TextMesh> ().text = "金呷讚: " + Score1.ToString ();
			Score2Text.GetComponent<TextMesh> ().text = "一定銀: " + Score2.ToString ();
			BackgroundSound.Stop ();
		}

		if ( GUI.Button (new Rect (buttonPosX + (buttonWidth + buttonGap) * 2f, buttonPositionY, buttonWidth, 60), "Reload") ){
			_playerManager.InitPlayers();
		}

		if ( GUI.Button (new Rect (buttonPosX + (buttonWidth + buttonGap) * 3, buttonPositionY, buttonWidth, 60), "Exit ") ){
			Application.Quit();
		}
	}

	void setBodyMoveSpeed() {
		
		PlayerPrefs.SetFloat ("BodyMoveSpeed", SpeedSlider.value);

	
	}

	void setCountedTime() {
		PlayerPrefs.SetFloat ("TimeSliderValue", TimeSlider.value);
		_countedTime = TimeSlider.value * 15;
		SetTimeText.text = "時間: " + _countedTime.ToString () + " 秒";
		TimeText.GetComponent<TextMesh>().text = _countedTime.ToString();
	}

	IEnumerator Gameover() {
		yield return new WaitForSeconds (3);
		BackgroundSound.Stop ();
	}
}
