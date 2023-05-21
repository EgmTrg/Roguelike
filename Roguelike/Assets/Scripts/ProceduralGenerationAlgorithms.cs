using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class ProceduralGenerationAlgorithms {
	public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPos, int walkLength) {
		HashSet<Vector2Int> path = new HashSet<Vector2Int>();

		path.Add(startPos);
		var prevPos = startPos;

		for (int i = 0; i < walkLength; i++) {
			var newPos = prevPos + Direction2D.GetRandomCardinalDirection();
			path.Add(newPos);
			prevPos = newPos;
		}
		return path;
	}

	public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPos, int corridorLength) {
		List<Vector2Int> corridor = new List<Vector2Int>();
		var direction = Direction2D.GetRandomCardinalDirection();
		var currentPos = startPos;
		corridor.Add(currentPos);

		for (int i = 0; i < corridorLength; i++) {
			currentPos += direction;
			corridor.Add(currentPos);
		}

		return corridor;
	}

}

public static class Direction2D {
	public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>() {
		new Vector2Int(0, 1), // UP
		new Vector2Int(1, 0), // RIGHT
		new Vector2Int(0, -1), // DOWN
		new Vector2Int(-1, 0) // LEFT
	};

	public static Vector2Int GetRandomCardinalDirection() {
		return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
	}
}