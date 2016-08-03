using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {
	//http://maps.googleapis.com/maps/api/geocode/json?latlng=55.4018502,10.3419367&zoom=18&size=512x512&scale=2&maptype=roadmap&format=xml

	public double latitude = 55.4018502;
	public double longtitude = 10.3419367;

	public Grid worldGrid;


	// Use this for initialization
	void Awake () {
		worldGrid = new Grid( 3, 40, latitude, longtitude);
	}




	// Update is called once per frame
	void Update () {
		if (worldGrid.needsUpdate) {
			List<Grid.GridItem> itemsToRender = worldGrid.GetRenderQueue ();	
			for(int index=0; index < itemsToRender.Count; index++) {
				Debug.Log ("Rendering " + itemsToRender [index].getImageUrl());
				//StartCoroutine_Auto (itemsToRender[index].Download ());
			}
			worldGrid.needsUpdate = false;
		}
	}
}
