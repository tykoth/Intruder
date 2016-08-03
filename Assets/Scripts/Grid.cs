using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Grid {

	public int radius = 1;
	private int gridBlockSize;
	public int GridLength;
	private static double _defaultgridBlockSize = 50.0;
	private double _defautlTitleOffSetTop = -0.0027567f;
	private double _defaultTitleOffSetLeft = 0.001498f;

	private double _defaultTexureUnit = 0.04f / _defaultgridBlockSize;

	private double titleOffSetTop;
	private double titleOffSetLeft;

	private float textureOffSet;
	private float textureScale;

	private double latitude = 55.4018502;
	private double longtitude = 10.3419367;

	private Dictionary<Vector2, GridItem> localGrid = new Dictionary<Vector2, GridItem> ();
	public GameObject collection;
	public bool needsUpdate = true;

	public static string localStorageDirectory = Application.persistentDataPath + Path.DirectorySeparatorChar + "maps";

	public Grid (int radius, int blockSize, double latitude, double longtitude) {
		this.radius = radius;
		this.gridBlockSize = blockSize;
		this.latitude = latitude;
		this.longtitude = longtitude;

		initializeGrid ();
	}

	public void Set( Dictionary<Vector2, GridItem> map) {
		localGrid = map;
	}

	public GridItem Get(Vector2 position) {
		if (localGrid.ContainsKey (position))
			return localGrid [position];
		else
			return null;
	}

	void initializeGrid () {
		int gridRadius = 1;
		for (int index = 0; index < radius; index++) {
			gridRadius = gridRadius + 2;	
		}

		titleOffSetLeft = _defaultTitleOffSetLeft;
		titleOffSetTop  = _defautlTitleOffSetTop;

		textureOffSet = (float)(0f + (_defaultTexureUnit * gridBlockSize));
		textureScale = (float)(1f - (_defaultTexureUnit * gridBlockSize));

		int gridCountX = gridRadius + 1;
		int gridCountY = gridRadius + 1;
		int gridCenterRow = (gridCountY) / 2;

		GridLength = gridCountX -1;

		Vector2 localGridPosition;
		GridItem item;

		collection = new GameObject ("World Pieces");
		collection.transform.position = new Vector3 (0, 0, 0);

		for (int indexX = 1; indexX < gridCountX; indexX++) {
			for (int indexY = 1; indexY < gridCountY; indexY++) {
				localGridPosition = new Vector2 (indexX, indexY);
				string info = " pos: " + localGridPosition.ToString();

				double localLatitude = latitude - ((indexY - gridCenterRow) * titleOffSetLeft);
				double localLongtitude = longtitude + ((indexX - gridCenterRow) * titleOffSetTop);
	
				item = new GridItem ("Item[" + indexX + "x" + indexY + "] - " + info , localLatitude, localLongtitude, gridBlockSize, localGridPosition, textureOffSet, textureScale, gridCountY, collection.transform);
				localGrid.Add (localGridPosition, item);

			}	
		}

		if(!Directory.Exists(Grid.localStorageDirectory)) {
			Debug.Log("Creating storage at " + localStorageDirectory);
			Directory.CreateDirectory(localStorageDirectory);
		}

	}

	public List<GridItem> GetRenderQueue() {
		List<Vector2> itemsProcessed = new List<Vector2> ();
		List<Grid.GridItem> itemsToRender = new List<Grid.GridItem> ();

		int maxRows = GridLength;
		int ring = 0;
		GridItem item;
		while (ring < maxRows) {
			ring++;
			for (int index = ring; index < maxRows+1; index++) {				
				item = Get (new Vector2 (ring, index)); 
				if (item != null && !itemsProcessed.Contains(new Vector2 (ring, index))) { 
					//Debug.Log ("Getting " + new Vector2 (ring, index));
					itemsProcessed.Add (new Vector2 (ring, index));
					itemsToRender.Add (item);
				}

				item = Get (new Vector2 (index, ring)); 
				if (item != null && !itemsProcessed.Contains(new Vector2 (index, ring))) { 
					//Debug.Log ("Getting " + new Vector2 (index, ring));
					itemsProcessed.Add (new Vector2 (index, ring));
					itemsToRender.Add (item);
				}
			}
			for (int index = maxRows; index > ring; index--) {				
				item = Get (new Vector2 (maxRows, index)); 
				if (item != null && !itemsProcessed.Contains(new Vector2 (maxRows, index))) { 
					//Debug.Log ("Getting " + new Vector2 (maxRows, index));
					itemsProcessed.Add (new Vector2 (maxRows, index));					
					itemsToRender.Add (item);
				}

				item = Get (new Vector2 (index, maxRows)); 
				if (item != null && !itemsProcessed.Contains(new Vector2 (index, maxRows))) { 
					//Debug.Log ("Getting " + new Vector2 (maxRows, index));
					itemsProcessed.Add (new Vector2 (index, maxRows));					
					itemsToRender.Add (item);
				}
			}
			maxRows--;
		}

		itemsToRender.Reverse ();
		return itemsToRender;
	}

	public class GridItem {
		
		public double latitude;
		public double longtitude;
		public string name;
		public Texture2D texture2D = new Texture2D (1024, 1024, TextureFormat.ARGB32, false);
		public float textureOffSet;
		public float textureScale;
		public Vector3 position;
		public Vector2 localPosition;
		public Vector2 localScale;
		public Vector2 gridPosition;
		public GameObject obj;

		private int blockSize;
		private int maxY;
		private int centerRow;
		private bool mapLoaded = false;

		private Transform parent;

		public GridItem (string name, double latitude, double longtitude, int blockSize, Vector2 gridPosition, float textureOffSet, float textureScale, int maxY, Transform parent) {
			this.name = name;
			this.latitude = latitude;
			this.longtitude = longtitude;
			this.blockSize = blockSize;
			this.gridPosition = gridPosition;
			this.textureOffSet = textureOffSet;
			this.textureScale = textureScale;
			this.maxY = maxY;
			this.parent = parent;

			initializeGridItem ();
		}

		public GameObject Draw () {
			obj = GameObject.CreatePrimitive (PrimitiveType.Cube);
			obj.transform.localScale = new Vector3 (blockSize, 1, blockSize);
			obj.transform.position = position;
			obj.name = name;
			obj.transform.parent = this.parent;
			TryImageFromCache();
			return obj;
		}


		public void TryImageFromCache() {
			if (File.Exists(Grid.localStorageDirectory + Path.DirectorySeparatorChar + "block." + latitude.ToString ("F9") + "-" + longtitude.ToString ("F9") + ".png")) {
				Debug.Log(latitude.ToString ("F9") + "-" + longtitude.ToString ("F9") + " loaded from cache");
				texture2D.LoadImage(File.ReadAllBytes(Grid.localStorageDirectory + Path.DirectorySeparatorChar + "block." + latitude.ToString ("F9") + "-" + longtitude.ToString ("F9") + ".png"));	
				Renderer rend;
				rend = obj.GetComponent<Renderer> ();

				rend.material.mainTexture = texture2D;
				rend.material.mainTextureOffset = localPosition;
				rend.material.SetTextureScale ("_MainTex", localScale);
				mapLoaded = true;
			}	
		}

		void initializeGridItem () {
			localPosition = new Vector2 (0, this.textureOffSet);
			localScale = new Vector2 (1, textureScale);	
			centerRow = maxY / 2;

			int x = (int)((gridPosition.x - centerRow) * blockSize);
			int y = (int)((gridPosition.y - centerRow) * blockSize);

			position = new Vector3 (x, 0.5f, y);
			Draw ();
		}

		public string getImageUrl () {
			return "http://maps.googleapis.com/maps/api/staticmap?center=" + latitude.ToString ("F9") + "," + longtitude.ToString ("F9") + "&zoom=18&size=512x512&scale=2&maptype=roadmap";
		}

		public IEnumerator Download () {
			if (!mapLoaded) {
				string url = getImageUrl ();
				//Debug.Log (url);
				WWW www = new WWW (url);
				yield return www;
				www.LoadImageIntoTexture (texture2D);
				File.WriteAllBytes(Grid.localStorageDirectory + Path.DirectorySeparatorChar + "block." + latitude.ToString ("F9") + "-" + longtitude.ToString ("F9") + ".png"  ,www.bytes);
				Debug.Log("Writing file \n" + Grid.localStorageDirectory + Path.DirectorySeparatorChar + "block." + latitude.ToString ("F9") + "-" + longtitude.ToString ("F9") + ".png"); 
				Renderer rend;
				rend = obj.GetComponent<Renderer> ();

				rend.material.mainTexture = texture2D;
				rend.material.mainTextureOffset = localPosition;
				rend.material.SetTextureScale ("_MainTex", localScale);
				mapLoaded = true;
			}
		}

	}


}
