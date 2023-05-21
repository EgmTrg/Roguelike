using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UIElements;

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

		List<List<Vector2Int>> corridors = CreateCorridors(floorPositions, potentialRoomPositions);

		HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

		List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

		CreateRoomsAtDeadEnd(deadEnds, roomPositions);

		floorPositions.UnionWith(roomPositions);

		for (int i = 0; i < corridors.Count; i++) {
			//corridors[i] = IncreaseCorridorSizeByOne(corridors[i]);
			corridors[i] = IncreaseCorridorBrush3By3(corridors[i]);
			floorPositions.UnionWith(corridors[i]);
		}

		TilemapVisualizer.PaintFloorTiles(floorPositions);
		WallGenerator.CreateWalls(floorPositions, TilemapVisualizer);
	}

	private List<Vector2Int> IncreaseCorridorBrush3By3(List<Vector2Int> corridor) {
		List<Vector2Int> newCorridor = new List<Vector2Int>();
		for (int i = 1; i < corridor.Count; i++) {
			for (int k = -1; k < 2; k++) {
				for (int l = 0; l < 2; l++) {
					newCorridor.Add(corridor[i - 1] + new Vector2Int(k, l));
				}
			}
		}
		return newCorridor;
	}

	private List<Vector2Int> IncreaseCorridorSizeByOne(List<Vector2Int> corridor) {
		List<Vector2Int> newCorridor = new List<Vector2Int>();
		Vector2Int previousDirection = Vector2Int.zero;

		for (int i = 1; i < corridor.Count; i++) {
			Vector2Int directionFromCell = corridor[i] - corridor[i - 1];
			if (previousDirection != Vector2Int.zero && directionFromCell != previousDirection) {
				for (int k = -1; k < 2; k++) {
					for (int l = -1; l < 2; l++) {
						newCorridor.Add(corridor[i - 1] + new Vector2Int(k, l));
					}
				}
				previousDirection = directionFromCell;
			} else {
				Vector2Int newCorridorTileOffset = GetDirection90From(directionFromCell);
				newCorridor.Add(corridor[i - 1]);
				newCorridor.Add(corridor[i - 1] + newCorridorTileOffset);
			}
		}
		return newCorridor;
	}

	private Vector2Int GetDirection90From(Vector2Int directionFromCell) {
		if (directionFromCell == Vector2Int.up)
			return Vector2Int.right;
		if (directionFromCell == Vector2Int.right)
			return Vector2Int.down;
		if (directionFromCell == Vector2Int.down)
			return Vector2Int.left;
		if (directionFromCell == Vector2Int.left)
			return Vector2Int.up;
		return Vector2Int.zero;

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

	private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potantialRoomPositions) {
		var currentPosition = startPosition;
		potantialRoomPositions.Add(currentPosition);
		List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();


		for (int i = 0; i < corridorCount; i++) {
			var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);
			corridors.Add(corridor);
			currentPosition = corridor[corridor.Count - 1];
			potantialRoomPositions.Add(currentPosition);
			floorPositions.UnionWith(corridor);
		}
		return corridors;
	}
}
