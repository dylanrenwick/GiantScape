using System.Text.RegularExpressions;

using UnityEngine;

namespace GiantScape.Client.Tilemaps
{
    public class TilemapImporter : MonoBehaviour
    {
        [SerializeField]
        private TextAsset jsonFile;

        [SerializeField]
        private TilemapJsonConverter tilemapJsonConverter;

        public void Import()
        {
            if (tilemapJsonConverter == null) return;

            Debug.Log("Importing from file...");
            tilemapJsonConverter.LoadJson(StripComments(jsonFile.text), Vector2Int.zero);
        }

        public void Clear()
        {

        }

#if UNITY_EDITOR
        public void Export()
        {
            throw new System.NotImplementedException();
        }
#endif

        private static Regex commentRegex = new Regex("\\/\\*.*?\\*\\/");

        private static string StripComments(string json)
        {
            return commentRegex.Replace(json, string.Empty);
        }
    }
}
