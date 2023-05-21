using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator {

	[SerializeField] protected SimpleRandomWalkSO randomWalkParameters;

	protected override void RunProceduralGeneration() {
		HashSet<Vector2Int> floorPositions = RunRandomWalk(randomWalkParameters, startPosition);
		TilemapVisualizer.Clear();
		TilemapVisualizer.PaintFloorTiles(floorPositions);
		WallGenerator.CreateWalls(floorPositions, TilemapVisualizer);
	}

	protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters, Vector2Int position) {
		var currentPos = position;
		HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();

		for (int i = 0; i < parameters.iterations; i++) {
			var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPos, parameters.walkLength);
			floorPos.UnionWith(path);
			if (parameters.startRandomlyEachIteration) {
				currentPos = floorPos.ElementAt(Random.Range(0, floorPos.Count));
			}
		}
		return floorPos;
	}
}
