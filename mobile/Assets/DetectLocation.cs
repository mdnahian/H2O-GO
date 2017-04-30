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
    //public Kudan.AR.PlaceWaterMarker waterMarker;
    public SampleApp sa;

    public bool isShowing = false;

    public RawImage image;

    public Button snapButton;



    public LocationService service = Input.location;


    public string url = "http://6385ca68.ngrok.io";


	void Start(){
        Button btn = snapButton.GetComponent<Button>();
        snapButton.onClick.AddListener(screenshot);
		targetCoordinates = new Vector2 (dLatitude, dLongitude);
        updatePoints(dLatitude, dLongitude);
        StartCoroutine (getLocation ());
	}

	IEnumerator getLocation(){
		if (!enableByRequest && !service.isEnabledByUser) {
			Debug.Log("Location Services not enabled by user");
			yield break;
		}
		service.Start(1,1);
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

            //updatePoints(sLatitude, sLongitude);
        }
        //service.Stop();
        ready = true;
		startCalculate ();
	}


	void Update(){
        if (service.status != LocationServiceStatus.Failed)
        {
            text.text = "Target Location : " + dLatitude + ", " + dLongitude + "\nMy Location: " + service.lastData.latitude + ", " + service.lastData.longitude;
            sLatitude = service.lastData.latitude;
            sLongitude = service.lastData.longitude;

            if(!isShowing)
            {
                startCalculate();
            }
            
        }


    }


	public void startCalculate(){
		deviceCoordinates = new Vector2 (sLatitude, sLongitude);
		proximity = Vector2.Distance (targetCoordinates, deviceCoordinates);
        if (proximity <= distanceFromTarget) {
			text.text = text.text + "\nDistance : " + proximity.ToString ();
			text.text += "\nTarget Detected";
            //waterMarker.placeMarker();
            //sa.StartClicked();
            isShowing = true;
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

            string[] coors = www.text.Split(',');
            float lat;
            float lon;
            float.TryParse(coors[0], out lat);
            float.TryParse(coors[1], out lon);

            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector2(lat, lon);
        } else
        {
            Debug.Log(www.error);
        }

    }







    IEnumerator TakeScreenshot(RawImage image, Rect rect, string fileName, float ratio)
    {
        // Wait for the end of the frame to avoid any rendering artifacts
        yield return new WaitForEndOfFrame();

        // Get the camera from which the screenshot will be grabbed
        Camera camera = Camera.main;

        // Apply the ratio
        rect.height *= ratio;
        rect.width *= ratio;

        //Set the target texture render  
        camera.Render();

        // Create a a new Texture2D that is the same size as the camera view
        Texture2D texture = new Texture2D((int)(camera.pixelWidth), (int)(camera.pixelHeight));

        // Read the pixels of the screen and apply them to the texture
        texture.ReadPixels(rect, 0, 0);
        texture.Apply();

        // Encode To PNG
        byte[] bytes = texture.EncodeToPNG();

        //Save to 
        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/" + fileName, bytes);

        Debug.Log(Application.persistentDataPath + "/" + fileName);

        // Apply texture to a UI element for testing purposes
        //image.texture = texture;

        Debug.Log("Starting...");

        WWWForm form = new WWWForm();
        form.AddField("lat", sLatitude.ToString());
        form.AddField("long", sLongitude.ToString());
        form.AddBinaryData("image", bytes, fileName, "image/png");

        Debug.Log("Uploading...");

        WWW w = new WWW("http://6385ca68.ngrok.io/upload", form);

        StartCoroutine(WaitForClarifai(w));


    }


    private IEnumerator WaitForClarifai(WWW w)
    {

        while(w.isDone == false)
        {
            Debug.Log(w.progress.ToString());
            yield return null;
        } 

        if (w.error == null)
        {
            Debug.Log("RESPONSE:" + w.text);
        }
        else
        {
            Debug.Log("ERROR: " + w.error);
        }
        

        
    }



    public void screenshot()
    {
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        string fileName = System.DateTime.Now.Ticks + ".jpg";
        StartCoroutine(TakeScreenshot(image, rect, fileName, 1));
    }


}
