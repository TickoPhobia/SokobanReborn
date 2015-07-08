using UnityEngine;

public class Coordinate {
		public int x {get;set;}
		public int y {get;set;}
		
		public Coordinate (int X, int Y) {
			this.x=X;
			this.y=Y;
		}
		
		public static  Coordinate operator +(Coordinate c1, Coordinate c2)  {
			return new Coordinate(c1.x + c2.x, c1.y+c2.y);
		}
		
		public static Coordinate ScaleCoordinate(Coordinate c1, int scale) {
			return new Coordinate(scale*c1.x, scale*c1.y);
		}
		
		public static Vector3 CoordinateToWorldPosition(Coordinate origin) {
			return new Vector3(origin.x, 0f, origin.y);
		}
		
		public override string ToString() {
			return string.Format("X:{0}, Y{1}", x, y);
		}
	
}