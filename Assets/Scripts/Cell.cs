using UnityEngine;

public enum CellStatus{
		Wall,
		Box,
		Free,
		OtherObstacle
}
	
public class Cell {
	private bool _isEmpty = true;
	private bool _isMarkedAsGoal = false;
	private CellStatus _cellState;
	public Transform AttachedTransform; 	
	
	public Cell() {
		State = CellStatus.Free;
	}
	
	public CellStatus State {
		
		get {
			return _cellState;
		}
		set {
			_cellState = value;
			
			if ( value == CellStatus.Free) {
				AttachedTransform = null;
			}
		}
	}
	
	public bool IsGoal {
		
		get{ return _isMarkedAsGoal;}
		set { _isMarkedAsGoal = value;}
	}
	
	public bool IsEmpty {
		get {
			return State == CellStatus.Free;
		}
	}
}