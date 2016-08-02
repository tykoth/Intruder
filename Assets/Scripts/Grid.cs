using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid {

	private int radius = 1;
	private int gridBlockSize;

	private static double _defaultgridBlockSize = 50;
	private double _defautlTitleOffSetTop = -0.0027567f / _defaultgridBlockSize;
	private double _defaultTitleOffSetLeft = 0.001498f / _defaultgridBlockSize;

	private double _defaultTexureUnit = 0.04f / _defaultgridBlockSize;

	private float titleOffSetTop;
	private float titleOffSetLeft;

	private float textureOffSet;
	private float textureScale;

	private double latitude = 55.4018502;
	private double longtitude = 10.3419367;

	private Dictionary<Vector2, GridItem> localGrid = new Dictionary<Vector2, GridItem> ();
	public GameObject collection;

	public Grid (int radius, int blockSize, double latitude, double longtitude) {
		this.radius = radius;
		this.gridBlockSize = blockSize;
		this.latitude = latitude;
		this.longtitude = longtitude;

		initializeGrid ();
	}


	void initializeGrid () {
		int gridRadius = 1;
		for (int index = 0; index < radius; index++) {
			gridRadius = gridRadius + 2;	
		}

		titleOffSetLeft = (float)(_defaultTitleOffSetLeft * gridBlockSize);
		titleOffSetTop = (float)(_defautlTitleOffSetTop * gridBlockSize);

		textureOffSet = (float)(0f + (_defaultTexureUnit * gridBlockSize));
		textureScale = (float)(1f - (_defaultTexureUnit * gridBlockSize));

		int gridCountX = gridRadius + 1;
		int gridCountY = gridRadius + 1;
		int gridCenterRow = (gridCountY) / 2;

		Debug.Log ("gridCenterRow : " + gridCenterRow);
		Vector2 localGridPosition;
		GridItem item;

		collection = new GameObject ("World Pieces");
		collection.transform.position = new Vector3 (0, 0, 0);

		for (int indexX = 1; indexX < gridCountX; indexX++) {
			for (int indexY = 1; indexY < gridCountY; indexY++) {
				localGridPosition = new Vector2 (indexX, indexY);
				Debug.Log ("((indexY - gridCenterRow): " + ((indexY - gridCenterRow)));

				double localLatitude = latitude - ((indexY - gridCenterRow) * titleOffSetLeft);
				double localLongtitude = longtitude + ((indexX - gridCenterRow) * titleOffSetTop);
	
				Debug.Log ("Item[" + indexX + "x" + indexY + "]" + ",localLatitude : " + localLatitude.ToString ("F9") + ", localLongtitude: " + localLongtitude.ToString ("F9"));
				item = new GridItem ("Item[" + indexX + "x" + indexY + "]", localLatitude, localLongtitude, gridBlockSize, localGridPosition, textureOffSet, textureScale, gridCountY, collection.transform);
				item.Draw ();
				localGrid.Add (localGridPosition, item);

			}	
		}


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

		private int blockSize;
		private int maxY;
		private int centerRow;

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
			GameObject obj = GameObject.CreatePrimitive (PrimitiveType.Cube);

			Renderer rend;
			rend = obj.GetComponent<Renderer> ();

			obj.transform.localScale = new Vector3 (blockSize, 1, blockSize);
			obj.transform.position = position;
			obj.name = name;
			obj.transform.parent = this.parent;

			rend.material.mainTexture = texture2D;
			rend.material.mainTextureOffset = localPosition;
			rend.material.SetTextureScale ("_MainTex", localScale);
			return obj;
		}

		void initializeGridItem () {
			localPosition = new Vector2 (0, this.textureOffSet);
			localScale = new Vector2 (1, textureScale);	
			centerRow = maxY / 2;

			int x = (int)((gridPosition.x - centerRow) * blockSize);
			int y = (int)((gridPosition.y - centerRow) * blockSize);

			Debug.Log (x + "," + y + " CenterRow: " + centerRow + " , maxY: " + maxY);

			position = new Vector3 (x, 0.5f, y);
			Download ();
		}

		public string getImageUrl () {
			return "http://maps.googleapis.com/maps/api/staticmap?center=" + latitude.ToString ("F9") + "," + longtitude.ToString ("F9") + "&zoom=18&size=512x512&scale=2&maptype=roadmap";
		}

		public void Download () {
			string url = getImageUrl ();
			Debug.Log (url);
			WWW www = new WWW (url);
			while (!www.isDone || www.error != null) {
			}
			www.LoadImageIntoTexture (texture2D);
		}

	}


}
