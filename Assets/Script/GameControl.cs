using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {

	public GameObject BodyView;
	public GameObject Lime;
	public GameObject Cheese;
	public GameObject Chilli;
	public GameObject Bomb;

	public GameObject Score1Text;
	public GameObject Score2Text;
	public GameObject TimeText;
	public GameObject ResultText;

	public GUISkin GameGUISkin;

	public AudioSource BackgroundSound;

	public static int Score1 = 0;
	public static int Score2 = 0;

	private GameObject _limeClone;
	private GameObject _cheeseClone;
	private GameObject _chilliClone;
	private float _laysTime =0f;
	private float _limeTime = 0f;
	private float _cheeseTime = 0f;
	private float _chilliTime = 0f;
	private float _bomb1Time = 0f;
	private float _bomb2Time = 0f;

	private float limeCreateTime;
	private float cheeseCreateTime;
	private float chilliCreateTime;
	private float bomb1CreateTime = 3f;
	private float bomb2CreateTime = 3f;

	private float _time = 31.0f;

	private bool isPlaying = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey ("escape")) {
			Application.Quit ();
		}

		if (Input.GetKey ("f10")) {
			BodyView.SetActive (!BodyView.activeSelf);
		}

		_limeTime += Time.deltaTime;
		_cheeseTime += Time.deltaTime;
		_chilliTime += Time.deltaTime;
		_bomb1Time += Time.deltaTime;
		_bomb2Time += Time.deltaTime;

		if (isPlaying) {
		
			if (_limeTime > limeCreateTime) {
				_limeClone = Instantiate<GameObject> (Lime, new Vector3 (Random.Range (-8.5f, 8.5f), 5f, 0), transform.rotation);
				Destroy (_limeClone, 4);
				limeCreateTime = Random.Range (0.5f, 1.5f);
				_limeTime = 0f;
			}

			if (_cheeseTime > cheeseCreateTime) {
				_cheeseClone = Instantiate<GameObject> (Cheese, new Vector3 (Random.Range (-8.5f, 8.5f), 5f, 0), transform.rotation);
				Destroy (_cheeseClone, 4);
				cheeseCreateTime = Random.Range (0.5f, 1.5f);
				_cheeseTime = 0f;
			}

			if (_chilliTime > chilliCreateTime) {
				_chilliClone = Instantiate<GameObject> (Chilli, new Vector3 (Random.Range (-8.5f, 8.5f), 5f, 0), transform.rotation);
				Destroy (_chilliClone, 4);
				chilliCreateTime = Random.Range (0.5f, 1.5f);
				_chilliTime = 0f;
			}

			if (_bomb1Time > bomb1CreateTime) {
				GameObject bombClone1 = Instantiate<GameObject> (Bomb, new Vector3 (Random.Range (-6.5f, 0f), 5f, 0), transform.rotation);
				Destroy (bombClone1, 4);
				bomb1CreateTime = Random.Range (3f, 5f);
				_bomb1Time = 0f;
			}

			if (_bomb2Time > bomb2CreateTime) {
				GameObject bombClone2 = Instantiate<GameObject> (Bomb, new Vector3 (Random.Range (0f, 6.5f), 5f, 0), transform.rotation);
				Destroy (bombClone2, 4);
				bomb2CreateTime = Random.Range (3f, 5f);
				_bomb2Time = 0f;
			}

			_time -= Time.deltaTime;
			int time = (int)_time;
			TimeText.GetComponent<TextMesh>().text = time.ToString();
			Score1Text.GetComponent<TextMesh> ().text = "Score: " + Score1.ToString ();
			Score2Text.GetComponent<TextMesh> ().text = "Score: " + Score2.ToString ();

			//showGameResult ();

			if (_time < 0) {
				isPlaying = false;
				ResultText.SetActive (true);
				string result;
				if (Score1 == Score2) {
					result = "雙方平手!!";
				} else {
					if (Score1 > Score2) {
						result = "恭喜玩家1 獲勝!";
					} else {
						result = "恭喜玩家2 獲勝!";
					}
				}

				ResultText.GetComponent<TextMesh> ().text =  result;
				//GameResultText.GetComponent<TextMesh> ().text = "玩家1得到的分數是: " + Score1.ToString () + " 分\n" + "玩家2得到的分數是: " + Score2.ToString () + " 分\n" + result;
				StartCoroutine (Gameover ());
			} else {
				ResultText.SetActive(false);
			}
		}
			

	}

	void OnGUI() {
		GUI.skin = GameGUISkin;
		/*
		if (isPlaying) {
			float scoreWidth = 400f;
			float scoreHeight = 80f;
			float score1X = Screen.width * 0.25f - scoreWidth / 2f;
			float score2X = Screen.width * 0.75f - scoreWidth / 2f;
			float scoreY = Screen.height * 0.05f;

			float timeWidth = 300f;
			float timeX = Screen.width * 0.5f - timeWidth / 2;
			float timeY = Screen.height * 0.15f;

			GUI.Label (new Rect (score1X, scoreY, scoreWidth, scoreHeight), "Score: " + Score1.ToString());
			GUI.Label (new Rect (score2X, scoreY, scoreWidth, scoreHeight), "Score: " + Score2.ToString());
			//GUI.Label (new Rect (timeX , timeY, timeWidth, scoreHeight), "" + (int)_time);
		}else {

			if (Score1 > 0 || Score2 > 0) {

				float labelPositionX = Screen.width * 0.25f;
				float labelPositionY = Screen.height * 0.25f;
				float labelWidth = Screen.width * 0.6f;
				float labelHeight = Screen.height * 0.15f;

				
				GUI.Label (new Rect (labelPositionX , labelPositionY , labelWidth, labelHeight), "玩家1得到的分數是: " + Score1.ToString() + " 分");
				GUI.Label (new Rect (labelPositionX , labelPositionY + labelHeight* 1.1f, labelWidth, labelHeight), "玩家2得到的分數是: " + Score2.ToString() + " 分");
				if (Score1 == Score2) {
					GUI.Label (new Rect (labelPositionX, labelPositionY + labelHeight * 1.1f * 2f, labelWidth, labelHeight), "雙方平手!!");
				} else {
					if (Score1 > Score2) {
						GUI.Label (new Rect (labelPositionX, labelPositionY + labelHeight * 1.1f * 2f, labelWidth, labelHeight), "恭喜玩家1 獲勝!");
					} else {
						GUI.Label (new Rect (labelPositionX, labelPositionY + labelHeight * 1.1f * 2f, labelWidth, labelHeight), "恭喜玩家2 獲勝!");
					}
				}
			}

			float buttonWidth = Screen.width / 5f;
			float buttonPositionY = Screen.height * 0.8f;
			if ( GUI.Button (new Rect (buttonWidth, buttonPositionY, buttonWidth, 60), "Play") ){
				BackgroundSound.Play ();
				isPlaying = true;
				_time = 31.0f;
				Score1 = 0;
				Score2 = 0;
				//SceneManager.LoadScene("LaysEvent");
			}

			if ( GUI.Button (new Rect (buttonWidth * 2f, buttonPositionY, buttonWidth, 60), "Reload") ){
				SceneManager.LoadScene("LaysEvent");
			}

			if ( GUI.Button (new Rect (buttonWidth * 3, buttonPositionY, buttonWidth, 60), "Exit ") ){
				Application.Quit();
			}

		}
		*/

		float buttonWidth = Screen.width / 5f;
		float buttonPositionY = Screen.height * 0.8f;
		if ( GUI.Button (new Rect (buttonWidth, buttonPositionY, buttonWidth, 60), "Play") ){
			BackgroundSound.Play ();
			isPlaying = true;
			_time = 31.0f;
			Score1 = 0;
			Score2 = 0;
			//SceneManager.LoadScene("LaysEvent");
		}

		if ( GUI.Button (new Rect (buttonWidth * 2f, buttonPositionY, buttonWidth, 60), "Reload") ){
			SceneManager.LoadScene("LaysEvent");
		}

		if ( GUI.Button (new Rect (buttonWidth * 3, buttonPositionY, buttonWidth, 60), "Exit ") ){
			Application.Quit();
		}
	}

	void showGameResult(){

		//GameResultText.SetActive (!isPlaying);
		//print("game reslut!");
		string result;
		if (Score1 == Score2) {
			result = "雙方平手!!";
		} else {
			if (Score1 > Score2) {
				result = "恭喜玩家1 獲勝!";
			} else {
				result = "恭喜玩家2 獲勝!";
			}
		}

		//GameResultText.GetComponent<TextMesh> ().text = "玩家1得到的分數是: " + Score1.ToString () + " 分\n" + "玩家2得到的分數是: " + Score2.ToString () + " 分\n" + result;
			
	}

	IEnumerator Gameover() {
		yield return new WaitForSeconds (3);
		BackgroundSound.Stop ();
	}
}
