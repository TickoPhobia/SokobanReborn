using UnityEngine;
using System;


public class Root : MonoBehaviour {
	
	public GameObject GroundPlaceHolderPrefab;
	public GameObject WallPrefab;
	public GameObject BoxPrefab;

  	public int Height;
   	public int Width;
	public SokobanPlayer CurrentPlayer;
	
	public Cell[,] Grid;
	
	public void Start() {
		InitializeGrid();

		if(CurrentPlayer != null)
			CurrentPlayer.MovePlayerTo(new Coordinate(2,2));
	}
	
	public void InitializeGrid() {
		Grid = new Cell[Width, Height];
		
		for (int i = 0; i < Width; i++) {
			for ( int j = 0; j < Height; j++) {
				Grid[i,j] = new Cell();
			}
		} 
		
		
		LoadCells("nil");
	}

	public void SetCell(Coordinate index, Cell newCell) {
		Grid[index.x, index.y] = newCell;
	}
	
	public void LoadCells(String str) {
		//TODO: 
		var boxes = new Coordinate[] { new Coordinate(1,3), new Coordinate(2,0)};
		
		
		var walls =  new Coordinate[2*Width + 4]; //{ new Coordinate(0,0), new Coordinate(0,1), new Coordinate(0,2), new Coordinate(0,3)};
		for (int i = 0; i < Width; i++) {
			walls[i] = new Coordinate(i,0);
			walls[i+Width] = new Coordinate(i,Height-1);
		}
		walls[2*Width] = new Coordinate(2,3);
		walls[2*Width+1] = new Coordinate(3,5);
		walls[2*Width+2] = new Coordinate(5,1);
		walls[2*Width+3] = new Coordinate(4,3);
		
		CreateBox(boxes);
		CreateWall(walls);
	}
	
	public void CreateBox(Coordinate[] locations) {
		foreach (var box in locations) {
			Grid[box.x,box.y].State = CellStatus.Box;
			
			if(BoxPrefab != null) {
				var newBox = (GameObject) Instantiate(BoxPrefab, Coordinate.CoordinateToWorldPosition(box), Quaternion.identity);
				Grid[box.x,box.y].AttachedTransform = newBox.transform;
			}
		}
	}
	public void CreateWall(Coordinate[] locations) {
		foreach (var wall in locations) {
			Grid[wall.x,wall.y].State = CellStatus.Wall;
			
			if(WallPrefab != null) {
				var newWall = (GameObject) Instantiate(WallPrefab, Coordinate.CoordinateToWorldPosition(wall), Quaternion.identity);
				Grid[wall.x,wall.y].AttachedTransform = newWall.transform;
			}
		}
	}

	public bool ValidPositionForPlayer(Coordinate location) {
		if (  location.x < Width &&  location.y < Height   && location.x >= 0 && location.y >= 0) {
				if (Grid[location.x, location.y].State == CellStatus.Free)
				return true;
			}
			return false;	
	}
	
	public bool CanMoveAlongDirection(Coordinate dir) {
		Coordinate targetCoordinate = CurrentPlayer.PlayerPosition+dir;
		if (  targetCoordinate.x < Width &&  targetCoordinate.y < Height   && targetCoordinate.x >= 0 && targetCoordinate.y >= 0) {
				if (Grid[targetCoordinate.x, targetCoordinate.y].State == CellStatus.Free){
					return true;
				} else if (Grid[targetCoordinate.x, targetCoordinate.y].State == CellStatus.Box){
						if ( ValidPositionForPlayer(targetCoordinate+dir)){
							MoveBox(Grid[targetCoordinate.x, targetCoordinate.y], targetCoordinate, dir);
							return true;
						}
				}
		}
		return false;
	}
	
	public void MoveBox(Cell container, Coordinate cellCoordinate, Coordinate dir) {
		var targetCell = cellCoordinate + dir;
		
		Grid[cellCoordinate.x, cellCoordinate.y].AttachedTransform.position = Coordinate.CoordinateToWorldPosition(targetCell);
		
		Grid[targetCell.x, targetCell.y].State = CellStatus.Box;
		Grid[targetCell.x, targetCell.y].AttachedTransform = Grid[cellCoordinate.x, cellCoordinate.y].AttachedTransform;
		
		Grid[cellCoordinate.x, cellCoordinate.y].State = CellStatus.Free;
		Grid[cellCoordinate.x, cellCoordinate.y].AttachedTransform = null;
		
		
		
	}
}