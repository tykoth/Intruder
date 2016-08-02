using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {
	//http://maps.googleapis.com/maps/api/geocode/json?latlng=55.4018502,10.3419367&zoom=18&size=512x512&scale=2&maptype=roadmap&format=xml

	public float titleOffSetTop = -0.0027567f;
	public float titleOffSetLeft = 0.001498f;

	public double latitude = 55.4018502;
	public double longtitude = 10.3419367;

	public Grid worldGrid;


	// Use this for initialization
	void Start () {
		worldGrid = new Grid( 2, 150, latitude, longtitude);
	}



	// Update is called once per frame
	void Update () {
	
	}
}
