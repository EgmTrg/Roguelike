using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator {
	[SerializeField] private int corridorLength = 14;
	[SerializeField] private int corridorCount = 5;
	[SerializeField][Range(0f, 1f)] private float roomPercent;

	protected override void RunProceduralGeneration() {
		CorridorFirstGenerator();
	}

	private void CorridorFirstGenerator() {
		HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
		HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

		CreateCorridors(floorPositions, potentialRoomPositions);

		HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

		List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

		CreateRoomsAtDeadEnd(deadEnds, roomPositions);

		floorPositions.UnionWith(roomPositions);

		TilemapVisualizer.PaintFloorTiles(floorPositions);
		WallGenerator.CreateWalls(floorPositions, TilemapVisualizer);
	}

	private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors) {
		foreach (var position in deadEnds) {
			if (roomFloors.Contains(position) == false) {
				var room = RunRandomWalk(randomWalkParameters, position);
				roomFloors.UnionWith(room);
			}
		}
	}

	private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions) {
		List<Vector2Int> deadEnds = new List<Vector2Int>();
		foreach (var position in floorPositions) {
			int neighboursCount = 0;
			foreach (var direction in Direction2D.cardinalDirectionsList) {
				if (floorPositions.Contains(position + direction)) {
					neighboursCount++;
				}
			}
			if (neighboursCount == 1) {
				deadEnds.Add(position);
			}
		}
		return deadEnds;
	}

	private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions) {
		HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
		int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

		List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

		foreach (var roomPosition in roomsToCreate) {
			var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);
			roomPositions.UnionWith(roomFloor);
		}

		return roomPositions;
	}

	private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potantialRoomPositions) {
		var currentPosition = startPosition;
		potantialRoomPositions.Add(currentPosition);

		for (int i = 0; i < corridorCount; i++) {
			var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);
			currentPosition = corridor[corridor.Count - 1];
			potantialRoomPositions.Add(currentPosition);
			floorPositions.UnionWith(corridor);
		}

	}
}
