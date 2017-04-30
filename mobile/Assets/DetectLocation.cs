using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Kudan.AR.Samples;
using System.Collections.Generic;

public class DetectLocation : MonoBehaviour {
	
	private Vector2 targetCoordinates;
	private Vector2 deviceCoordinates;
	private float distanceFromTarget = 0.00004f;
	private float proximity = 0.001f;
	private float sLatitude, sLongitude;
	public float dLatitude = 40.73547f, dLongitude = -73.90292f;
	private bool enableByRequest = true;
	public int maxWait = 10;
	public bool ready = false;
	public Text text;
	public SampleApp sa;


    public string url = "http://8e810043.ngrok.io";


	void Start(){
		targetCoordinates = new Vector2 (dLatitude, dLongitude);
		StartCoroutine (getLocation ());
	}

	IEnumerator getLocation(){
		LocationService service = Input.location;
		if (!enableByRequest && !service.isEnabledByUser) {
			Debug.Log("Location Services not enabled by user");
			yield break;
		}
		service.Start();
		while (service.status == LocationServiceStatus.Initializing && maxWait > 0) {
			yield return new WaitForSeconds(1);
			maxWait--;
		}
		if (maxWait < 1){
			Debug.Log("Timed out");
			yield break;
		}
		if (service.status == LocationServiceStatus.Failed) {
			Debug.Log("Unable to determine device location");
			yield break;
		} else {
			text.text = "Target Location : "+dLatitude + ", "+dLongitude+"\nMy Location: " + service.lastData.latitude + ", " + service.lastData.longitude;
			sLatitude = service.lastData.latitude;
			sLongitude = service.lastData.longitude;

            updatePoints(sLatitude, sLongitude);
		}
		//service.Stop();
		ready = true;
		startCalculate ();
	}


	void Update(){

	}


	public void startCalculate(){
		deviceCoordinates = new Vector2 (sLatitude, sLongitude);
		proximity = Vector2.Distance (targetCoordinates, deviceCoordinates);
		if (proximity <= distanceFromTarget) {
			text.text = text.text + "\nDistance : " + proximity.ToString ();
			text.text += "\nTarget Detected";
			sa.StartClicked ();
		} else {
			text.text = text.text + "\nDistance : " + proximity.ToString ();
			text.text += "\nTarget not detected, too far!";
		}
	}


    public void updatePoints(float lat, float lon)
    {

        WWWForm form = new WWWForm();
        form.AddField("lat", lat.ToString());
        form.AddField("long", lon.ToString());
        WWW www = new WWW(url, form);

        StartCoroutine(WaitForRequest(www));
       
    }



    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            Debug.Log(www.text);
        } else
        {
            Debug.Log(www.error);
        }

    }



}
