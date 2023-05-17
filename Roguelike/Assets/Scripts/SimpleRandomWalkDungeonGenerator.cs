using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator {

	[SerializeField] private SimpleRandomWalkSO randomWalkParameters;

	protected override void RunProceduralGeneration() {
		HashSet<Vector2Int> floorPositions = RunRandomWalk();
		TilemapVisualizer.Clear();
		TilemapVisualizer.PaintFloorTiles(floorPositions);
	}

	protected HashSet<Vector2Int> RunRandomWalk() {
		var currentPos = startPosition;
		HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();

		for (int i = 0; i < randomWalkParameters.iterations; i++) {
			var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPos, randomWalkParameters.walkLength);
			floorPos.UnionWith(path);
			if (randomWalkParameters.startRandomlyEachIteration) {
				currentPos = floorPos.ElementAt(Random.Range(0, floorPos.Count));
			}
		}
		return floorPos;
	}
}
