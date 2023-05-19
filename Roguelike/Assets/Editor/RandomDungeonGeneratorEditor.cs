using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(AbstractDungeonGenerator), true)]
public class RandomDungeonGeneratorEditor : Editor {
	AbstractDungeonGenerator generator;

	private void Awake() {
		generator = (AbstractDungeonGenerator)target;
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		EditorGUILayout.Space();

		if (GUILayout.Button("Create Dungeon")) {
			generator.GenerateDungeon();
		}

		if (GUILayout.Button("Clear Dungeon")) {
			generator.ClearDungeon();
		}
	}
}
