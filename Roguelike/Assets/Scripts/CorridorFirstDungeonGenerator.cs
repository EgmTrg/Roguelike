using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator {
	[SerializeField] private int corridorLength = 14;
	[SerializeField] private int corridorCount = 5;
	[SerializeField][Range(0f, 1f)] private float roomPercent;
	public SimpleRandomWalkSO roomGenerationParameters;

	protected override void RunProceduralGeneration() {
		CorridorFirstGenerator();
	}

	private void CorridorFirstGenerator() {
		HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

		CreateCorridors(floorPositions);

		TilemapVisualizer.PaintFloorTiles(floorPositions);
		WallGenerator.CreateWalls(floorPositions, TilemapVisualizer);
	}

	private void CreateCorridors(HashSet<Vector2Int> floorPositions) {
		var currnetPos = startPosition;

		for (int i = 0; i < corridorCount; i++) {
			var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currnetPos, corridorLength);
			currnetPos = corridor[corridor.Count - 1];
			floorPositions.UnionWith(corridor);
		}

	}
}
