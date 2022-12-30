using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LevelBuilder2D
{
    public class PrefabTile : TileBase
    {
        public Sprite defaultSprite;
        public GameObject prefab;

        readonly public float prefabLocalOffset = 0.5f;
        readonly public float prefabZOffset = -1f;

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {

            //This prevents rogue prefab objects from appearing when the Tile palette is present
#if UNITY_EDITOR
            if (go != null)
            {
                if (go.scene.name == null)
                {
                    DestroyImmediate(go);
                }
            }
#endif
            
            if (go != null)
            {
                //Modify position of GO to match middle of Tile sprite
                go.transform.position = new Vector3(position.x + prefabLocalOffset
                    , position.y + prefabLocalOffset
                    , prefabZOffset);

            }

            return true;
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/2D/Tiles/Prefab Tile")]
        public static void CreatePrefabTiles()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeInstanceID) + "/NewPrefabTile.asset";

            AssetDatabase.CreateAsset(CreateInstance<PrefabTile>(), path);
        }
#endif


        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = defaultSprite;

            if (prefab && tileData.gameObject == null)
            {
                tileData.gameObject = prefab;
            }
        }
    }
}
