using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Windows.Kinect;

public class DetectPlayers : MonoBehaviour {

	public BodySourceManager BodySrcManager;
	public GameObject Player1;
	public GameObject Player2;
	public JointType TrackedJoint;
	public Text TrackingBodiesText;

	private float multiplier;
	private BodySourceManager bodyManager;
	private KinectSensor kinectSensor;
	private CoordinateMapper coordinateMapper;
	private Body[] bodies;

	private GameObject _player1Object;
	private GameObject _player2Object;

	private Body _player1;
	private Body _player2;
	private Body _tempPlayer;

	private int _trackBodyCount;
	private float _limitZ = 4f;
	private float _limitX = -1.7f;

	// Use this for initialization
	void Start () {

		if (BodySrcManager == null)
		{
			Debug.Log("Assign Game Object with Body Source Manager");
		}
		else
		{
			Debug.Log("Started!!!!!!!!");
			bodyManager = BodySrcManager.GetComponent<BodySourceManager>();
			this.kinectSensor = KinectSensor.GetDefault ();
			coordinateMapper = this.kinectSensor.CoordinateMapper;
			if (bodyManager == null)
			{
				Debug.Log("bodyManager is null when start");
				return;
			}
				
			InitPlayers ();
		}
	}

	public void InitPlayers() {

		multiplier = PlayerPrefs.GetFloat ("BodyMoveSpeed");
		//print ("multiplier: " + multiplier);

		if (_player1Object != null) {
			Destroy (_player1Object);
		}

		_player1Object = Instantiate (Player1, new Vector3 (-3f, -3f, 0f), transform.rotation);
		_player1Object.SetActive(false);

		if (_player2Object != null) {
			Destroy (_player2Object);
		}
		_player2Object = Instantiate (Player2, new Vector3 (3f, -3f, 0f), transform.rotation);
		_player2Object.SetActive(false);
		_player1 = null;
		_player2 = null;
	}

		
	
	// Update is called once per frame
	void Update () {
		if (bodyManager == null)
		{
			Debug.Log("bodyManager is null");
			return;
		}

		bodies = bodyManager.GetData();

		if (bodies == null)
		{
			Debug.Log("bodies is null");
			return;
		}

		bodies = Array.FindAll(bodies, body => (body.Joints[TrackedJoint].Position.Z < _limitZ && body.Joints[TrackedJoint].Position.X > _limitX));

		print ("bodies: " + bodies.Length);

		_trackBodyCount = 0;
		int _nonTrackBodyCount = 0;

		foreach (var body in bodies)
		{
			if (body == null)
			{
				
				//Debug.Log("body is null");
				continue;
			}
				
			if (body.IsTracked) {
				_trackBodyCount += 1;

				Debug.Log ("body is tracked" + body.TrackingId);


				if (body.Joints [TrackedJoint].Position.Z < _limitZ) {
					
					if (_player1 == null) {
						_player1 = body;
						print ("Player1: " + _player1.TrackingId);
					} else if (_player2 == null && body.TrackingId != _player1.TrackingId) {
						_player2 = body;
						print ("Player2: " + _player2.TrackingId);
						if (_player1.Joints [TrackedJoint].Position.X > _player2.Joints [TrackedJoint].Position.X) {
							_tempPlayer = _player1;
							_player1 = _player2;
							_player2 = _tempPlayer;
						}
					}
				}

				if (_player1 != null) {
					_player1Object.SetActive (true);
					var pos1 = _player1.Joints [TrackedJoint].Position;
					_player1Object.transform.position = new Vector3 (pos1.X * multiplier, -3);
					if (pos1.Z > _limitZ) {
						_player1 = null;
						_player1Object.SetActive (false);
					}
					//print ("p1 pos: " + pos1.X);
				}

				if (_player2 != null) {
					_player2Object.SetActive (true);
					var pos2 = _player2.Joints [TrackedJoint].Position;
					_player2Object.transform.position = new Vector3 (pos2.X * multiplier, -3);
					if (pos2.Z > _limitZ) {
						_player2 = null;
						_player2Object.SetActive (false);
					}
				}

			} else {
				_nonTrackBodyCount += 1;
			}
		}

		TrackingBodiesText.text = "Tracking Bodies: " + _trackBodyCount;

		print ("Now Kinect is tracking " + _trackBodyCount + "bodies!");
		print ("Non Track Bodies: " + _nonTrackBodyCount);

	}
		
}
