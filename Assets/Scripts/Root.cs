using UnityEngine;

public class Root : MonoBehaviour {
	
	public GUISkin DefaultSkin;
	
	public GameObject GroundPlaceHolderPrefab;
	public GameObject WallPrefab;
	public GameObject BoxPrefab;
	
	public GameObject GoalPrefab;
	
	public GameObject SokobanPlayerPrefab;

  	public int Height;
   	public int Width;
	public SokobanPlayer CurrentPlayer;
	
	public Cell[,] Grid;
	
	public GameState CurrentGameState = GameState.NotStarted;
	private int _totalGridGoals;
	public int TotalGridGoals {
		get {return _totalGridGoals;}
		set { 
			_totalGridGoals = value;
			CheckGameStatus();
			}
	}
	
	public void Start() {
		InitializeGrid();
	}
	
	public void InitializeGrid() {

		TextAsset textFile = (TextAsset)Resources.Load("Levels/3", typeof(TextAsset));
	
		var levelDef = GameDefinition.ParseString(textFile.text);
				
		Width = levelDef.GridSize.x;
		Height = levelDef.GridSize.y;		
				
		Grid = new Cell[Width, Height];
		
		for (int i = 0; i < Width; i++) {
			for ( int j = 0; j < Height; j++) {
				Grid[i,j] = new Cell();
			}
		} 
				
				
		CreatePlayer(levelDef.PlayerDefaultLocation);
		CreateBox(levelDef.Boxes);
		CreateWall(levelDef.Walls);
		MarkCellAsGoal(levelDef.Goals);
		
		TotalGridGoals = levelDef.Goals.Length;
		
		CurrentGameState = GameState.Playing;
	}

	public void SetCell(Coordinate index, Cell newCell) {
		Grid[index.x, index.y] = newCell;
	}
	
	public void CreatePlayer(Coordinate location) {
		var SokobanPlayerGaObj = (GameObject) Instantiate(SokobanPlayerPrefab, Vector3.zero, Quaternion.identity );
		CurrentPlayer = SokobanPlayerGaObj.GetComponent<SokobanPlayer>();	
		
		if(CurrentPlayer != null) {
			CurrentPlayer.ReferenceToRoot = this;
			CurrentPlayer.MovePlayerTo(location);	
		}
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
	
	public void MarkCellAsGoal(Coordinate[] locations) {
		foreach (var goal in locations) {
			Grid[goal.x,goal.y].IsGoal = true;
			
			if(GoalPrefab != null) {
				var newGoal = (GameObject) Instantiate(GoalPrefab, Coordinate.CoordinateToWorldPosition(goal), Quaternion.identity);
				Grid[goal.x,goal.y].AttachedTransform = newGoal.transform;
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
		
		if (Grid[cellCoordinate.x, cellCoordinate.y].IsGoal) {
			TotalGridGoals++;
		} 
		
		if (Grid[targetCell.x, targetCell.y].IsGoal) {
			TotalGridGoals--;
		} 
		
	}
	
	public void CheckGameStatus() {
		if(TotalGridGoals>0) {
			
		} else {
			CurrentGameState = GameState.GameOver;
		}
	}
	
	
	public void OnGUI() {
		
		switch(CurrentGameState){
			case GameState.Playing:
				GUI.Label(new Rect(10,10,100,45), TotalGridGoals.ToString(), DefaultSkin.label);
				break;
			case GameState.GameOver:
				GUI.Label(new Rect(10,10,300,45), "YOU WON!", DefaultSkin.label);
				break;
		}
	}
}