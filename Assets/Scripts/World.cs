using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {
	//http://maps.googleapis.com/maps/api/geocode/json?latlng=55.4018502,10.3419367&zoom=18&size=512x512&scale=2&maptype=roadmap&format=xml

	public float titleOffSetTop = -0.0027567f;
	public float titleOffSetLeft = 0.001498f;

	public double Latitude = 55.4018502;
	public double Longtitude = 10.3419367;



	// Use this for initialization
	void Start () {
		var collection = new GameObject ("World Pieces");
		collection.transform.position = new Vector3 (0, 0, 0);

		var ground = GameObject.CreatePrimitive (PrimitiveType.Cube);
		ground.transform.localScale = new Vector3 (50, 1, 50);
		ground.transform.position = new Vector3 (0, -0.5f, 0);
		ground.name = "MiddlePiece";
		ground.transform.parent = collection.transform;
		StartCoroutine(addTextureToCube (ground, Latitude, Longtitude));

		var leftPiece = GameObject.CreatePrimitive (PrimitiveType.Cube);
		leftPiece.transform.localScale = new Vector3 (50, 1, 50);
		leftPiece.transform.position = new Vector3 (-50, -0.5f, 0);
		leftPiece.name = "LeftPiece";
		leftPiece.transform.parent = collection.transform;
		StartCoroutine(addTextureToCube (leftPiece, Latitude, Longtitude - titleOffSetLeft));

		var rightPiece = GameObject.CreatePrimitive (PrimitiveType.Cube);
		rightPiece.transform.localScale = new Vector3 (50, 1, 50);
		rightPiece.transform.position = new Vector3 (50, -0.5f, 0);
		rightPiece.name = "RightPiece";
		rightPiece.transform.parent = collection.transform;
		StartCoroutine(addTextureToCube (rightPiece, Latitude, Longtitude + titleOffSetLeft));

		var topPiece = GameObject.CreatePrimitive (PrimitiveType.Cube);
		topPiece.transform.localScale = new Vector3 (50, 1, 50);
		topPiece.transform.position = new Vector3 (0, -0.5f, 50);
		topPiece.name = "TopPiece";
		topPiece.transform.parent = collection.transform;
		StartCoroutine(addTextureToCube (topPiece, Latitude + titleOffSetTop, Longtitude));


		var topLeftPiece = GameObject.CreatePrimitive (PrimitiveType.Cube);
		topLeftPiece.transform.localScale = new Vector3 (50, 1, 50);
		topLeftPiece.transform.position = new Vector3 (-50, -0.5f, 50);
		topLeftPiece.name = "topLeftPiece";
		topLeftPiece.transform.parent = collection.transform;
		StartCoroutine(addTextureToCube (topLeftPiece, Latitude+titleOffSetTop, Longtitude - titleOffSetLeft));



		var topRightPiece = GameObject.CreatePrimitive (PrimitiveType.Cube);
		topRightPiece.transform.localScale = new Vector3 (50, 1, 50);
		topRightPiece.transform.position = new Vector3 (50, -0.5f, 50);
		topRightPiece.name = "topRightPiece";
		topRightPiece.transform.parent = collection.transform;
		StartCoroutine(addTextureToCube (topRightPiece, Latitude+titleOffSetTop, Longtitude + titleOffSetLeft));


		var bottomPiece = GameObject.CreatePrimitive (PrimitiveType.Cube);
		bottomPiece.transform.localScale = new Vector3 (50, 1, 50);
		bottomPiece.transform.position = new Vector3 (0, -0.5f, -50);
		bottomPiece.name = "BottomPiece";
		bottomPiece.transform.parent = collection.transform;
		bottomPiece.transform.parent = collection.transform;
		StartCoroutine(addTextureToCube (bottomPiece, Latitude - titleOffSetTop, Longtitude));


		var bottomLeftPiece = GameObject.CreatePrimitive (PrimitiveType.Cube);
		bottomLeftPiece.transform.localScale = new Vector3 (50, 1, 50);
		bottomLeftPiece.transform.position = new Vector3 (-50, -0.5f, -50);
		bottomLeftPiece.name = "bottomLeftPiece";
		bottomLeftPiece.transform.parent = collection.transform;
		StartCoroutine(addTextureToCube (bottomLeftPiece, Latitude-titleOffSetTop, Longtitude - titleOffSetLeft));

		var bottomRightPiece = GameObject.CreatePrimitive (PrimitiveType.Cube);
		bottomRightPiece.transform.localScale = new Vector3 (50, 1, 50);
		bottomRightPiece.transform.position = new Vector3 (50, -0.5f, -50);
		bottomRightPiece.name = "bottomRightPiece";
		bottomRightPiece.transform.parent = collection.transform;
		StartCoroutine(addTextureToCube (bottomRightPiece, Latitude-titleOffSetTop, Longtitude + titleOffSetLeft));
	
	}


	private IEnumerator addTextureToCube(GameObject obj, double lng, double lnt) {
		Renderer rend;
		rend = obj.GetComponent<Renderer> ();
		Texture2D texture2D = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);
		string url = "http://maps.googleapis.com/maps/api/staticmap?center=" + lng.ToString("F9") + "," + lnt.ToString("F9") + "&zoom=18&size=512x512&scale=2&maptype=roadmap";
		Debug.Log (url);
		WWW www = new WWW(url);
		yield return www;
		www.LoadImageIntoTexture(texture2D);
		rend.material.mainTexture = texture2D;
		rend.material.mainTextureOffset = new Vector2 (0,0.04f);
		rend.material.SetTextureScale("_MainTex", new Vector2(1, 0.96f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
