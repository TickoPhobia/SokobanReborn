using UnityEngine;
public class SokobanPlayer : MonoBehaviour {
		private Coordinate _playerPos = new Coordinate(0,0);
		public Coordinate PlayerPosition {get { return _playerPos; }}
		
		public Root ReferenceToRoot;
		
		
		public void Start () {
			if (ReferenceToRoot == null) {
				Debug.Log("ERROR: Missing Reference to Root Element of Game");
			}
			
		}
		public void Update () {
			HandleInputs();
		}
		
		public void HandleInputs () {
			
			
			if(Input.GetKeyDown(KeyCode.A)) {
				// MoveLeft
				MovePlayer(new Coordinate(-1,0));
			}
			if(Input.GetKeyDown(KeyCode.D)) {
				// MoveRight
				MovePlayer(new Coordinate(+1,0));
			}
			
			if(Input.GetKeyDown(KeyCode.W)) {
				// MoveUp
				MovePlayer(new Coordinate(0,+1));
			}
			
			if(Input.GetKeyDown(KeyCode.S)) {
				// MoveDown
				MovePlayer(new Coordinate(0,-1));
			}
			
			
		}
		public void MovePlayer(Coordinate dir) {
			if(ReferenceToRoot.CanMoveAlongDirection(dir))	{
					transform.position += Coordinate.CoordinateToWorldPosition(dir);
					_playerPos = dir + PlayerPosition ;
				}
		}
		public void MovePlayerTo(Coordinate dir) {
			if(ReferenceToRoot.ValidPositionForPlayer(dir))  {
				transform.position = Coordinate.CoordinateToWorldPosition(dir)+ 0.5f*Vector3.up;
				_playerPos = dir;
			}
		}
		
}