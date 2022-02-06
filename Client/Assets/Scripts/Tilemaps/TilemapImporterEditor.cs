using UnityEngine;
using UnityEditor;

namespace GiantScape.Client.Tilemaps
{
    [CustomEditor(typeof(TilemapImporter))]
    public class TilemapImporterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            TilemapImporter importer = (TilemapImporter)target;
            base.OnInspectorGUI();

            if (GUILayout.Button("Import from file..."))
            {
                importer.Import();
            }
#if UNITY_EDITOR
            if (GUILayout.Button("Export to file..."))
            {
                importer.Export();
            }
#endif
        }
    }
}
