using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class GameDefinition{
	public Coordinate GridSize;
	public Coordinate PlayerDefaultLocation;
	public Coordinate[] Walls;
	public Coordinate[] Boxes;
	
	public Coordinate[] Goals;
	
	
	public GameDefinition () {
		GridSize = new Coordinate(2,2);
		PlayerDefaultLocation = new Coordinate(0,0);
		
		Walls = new []{new Coordinate(0,1)};
		Boxes = new []{new Coordinate(0,1)};
		Goals = new [] {new Coordinate(1,1)};
	}
	
	public GameDefinition (Coordinate gridSize, Coordinate playerLocation, Coordinate [] walls, Coordinate[] boxes, Coordinate[] goals) {
		this.GridSize= gridSize;
		this.PlayerDefaultLocation = playerLocation;
		
		this.Walls = walls;
		this.Boxes = boxes;
		this.Goals = goals;
	} 
	
	
	public static GameDefinition ParseString(string inputStr) {
		
		List<string> lines = new List<string>();
		
		using (StringReader sr = new StringReader(inputStr)) {
    		string line;
    		while ((line = sr.ReadLine()) != null) {
        		lines.Add(line);
    		}
		}
		
		var line0Split =lines[0].Split(' ');
		Coordinate gridSize = new Coordinate(  Convert.ToInt32(line0Split[0]) ,   Convert.ToInt32(line0Split[1]) );
		
		var line1Split =lines[1].Split(' '); 
		Coordinate playerDefaultLocation = new Coordinate(  Convert.ToInt32(line1Split[0]) ,   Convert.ToInt32(line1Split[1]) );
	
		int numberOfWalls = Convert.ToInt32(lines[2]);
		int numberOfBoxes = Convert.ToInt32(lines[2+numberOfWalls + 1]);
		int numberOfGoals = Convert.ToInt32(lines[2 + numberOfWalls + 1 + numberOfBoxes + 1 ]);
		
		Coordinate[] walls = new Coordinate[numberOfWalls];
		Coordinate[] boxes = new Coordinate[numberOfBoxes];
		Coordinate[] goals = new Coordinate[numberOfGoals];
		
		
		for(int i = 0; i < numberOfWalls; i++) {
			var currentLine = lines[3+i].Split(' ');
			walls[i] = new Coordinate(  Convert.ToInt32(currentLine[0]) ,   Convert.ToInt32(currentLine[1]) );
		}
		
		for(int i = 0; i < numberOfBoxes; i++) {
			var currentLine = lines[4+numberOfWalls+i].Split(' ');
			boxes[i] = new Coordinate(  Convert.ToInt32(currentLine[0]) ,   Convert.ToInt32(currentLine[1]) );
		}
		
		for(int i = 0; i < numberOfGoals; i++) {
			var currentLine = lines[5 + numberOfWalls + numberOfBoxes +i].Split(' ');
			goals[i] = new Coordinate(  Convert.ToInt32(currentLine[0]) ,   Convert.ToInt32(currentLine[1]) );
		}
	
		if(numberOfBoxes != numberOfGoals) {
			Debug.LogWarning("Number of Boxes does not match the number of goals");
		}
	
		return new GameDefinition(gridSize,playerDefaultLocation,walls,boxes,goals);
	}
}