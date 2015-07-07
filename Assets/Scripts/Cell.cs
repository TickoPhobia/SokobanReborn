using UnityEngine;

public enum CellStatus{
		Wall,
		Box,
		Free,
		OtherObstacle
}
	
public class Cell {
	private bool _isEmpty = true;
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
	
	public bool IsEmpty {
		get {
			return State == CellStatus.Free;
		}
	}
}