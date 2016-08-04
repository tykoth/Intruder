using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {
	public int gridRadius = 4;
	public float appendAndDisloveSpeed = 6f;
	public GameObject floorPiece;
	public float floorPieceSize = 1;

	private int center;
	private GameObject container;
	private static GridItem[,] grid;
	private bool gridIsUpdated = false;

	// Use this for initialization
	void Start () {
		container = new GameObject ("Magic Floor");
		container.transform.position = Vector3.zero;

		grid = new GridItem[gridRadius + 1, gridRadius + 1];
		Debug.Log ("Grid length: " + grid.Length);
		center = (gridRadius / 2);

		GridItem item = new GridItem (appendAndDisloveSpeed);
		item.position = Vector3.zero;

		item.instant = GameObject.Instantiate(floorPiece);
		item.instant.transform.position = Vector3.zero;
		item.instant.transform.parent = container.transform;
		item.instant.name = center + "x" + center; 
		grid[center,center] = item;

		for (int row = 0; row < (gridRadius+1); row++) {
			addHorizontalRow (row);
		}
		gridIsUpdated = true;
	}


	// Update is called once per frame
	void Update () {	
		int max = gridRadius + 1;

		if(gridIsUpdated)
		for (int row = 0; row < max; row++) {
			for (int col = 0; col < max; col++) {
				if (grid [col, row].IsMoveable ()) {
					grid [col, row].Move ();
				}
			}
		}

	}

	private void addHorizontalRow(int row) {
		for (int index = 0; index < (gridRadius+1); index++) {			
			int Y = center - row;
			if (grid [index, row] == null) {
				int X = center - index;
				if (index != center || row != center) {
					GridItem item = new GridItem (appendAndDisloveSpeed);
					item.instant = GameObject.Instantiate <GameObject> (floorPiece);
					item.instant.name = index + "x" + row; 
					item.instant.transform.position = new Vector3 (X*10, 20, Y*10);
					item.position = new Vector3 (X*floorPieceSize, 0, Y*floorPieceSize);
					item.instant.transform.parent = container.transform;
					grid [index, row] = item;
				}
			}
		}
			
	}

	private void addVeriticalRow(int col) {
		for (int index = 0; index < (gridRadius+1); index++) {			
			int X = center - col;
			if (grid[index, col] == null) {
				int  Y = center - index;
				if (index != center || col != center) {
					GridItem item = new GridItem (appendAndDisloveSpeed);
					item.instant = GameObject.Instantiate <GameObject> (floorPiece);
					item.instant.name = col + "x" + index; 
					item.instant.transform.position = new Vector3 (X*10, 20, Y*10);
					item.position = new Vector3 (X*floorPieceSize, 0, Y*floorPieceSize);
					item.instant.transform.parent = container.transform;
					grid [col, index] = item;

				}
			}
		}

	}


	public class GridItem {
		public Vector3 position;
		public GameObject instant;
		public float speed;

		public GridItem(float s) {
			speed = s;
		}

		public bool IsMoveable() {
			if (Vector3.Distance (instant.transform.position, position) > 0.2f) {
				return true;
			}
			instant.transform.position = position;
			return false;
		}

		public void Move() {
			instant.transform.position = Vector3.MoveTowards(instant.transform.position, position, (speed * Time.deltaTime));
		}
	}

}
