using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Level_Loader : MonoBehaviour
{
    public TextAsset[] levels = new TextAsset[5];

    Keyboard_Input_Handler input;

    public int y_offset = 9;
    public int x_offset = 0;

    Tilemap player_tm;
    Tilemap room_tm;
    Tilemap boxes_tm;
    Tilemap void_tm;

    public TileBase floor;
    public TileBase wall;
    public TileBase void_tile;
    public TileBase red_box;
    public TileBase yellow_box;
    public TileBase blue_box;
    public TileBase red_finish;
    public TileBase yellow_finish;
    public TileBase blue_finish;

    public int level_counter = 1;

    // Start is called before the first frame update
    void Start()
    {
        input = GameObject.Find("GameRunner").GetComponent<Keyboard_Input_Handler>();
        player_tm = GameObject.Find("Player").GetComponent<Tilemap>();
        room_tm = GameObject.Find("Room").GetComponent<Tilemap>();
        boxes_tm = GameObject.Find("Boxes").GetComponent<Tilemap>();
        void_tm = GameObject.Find("VoidTiles").GetComponent<Tilemap>();
        StartCoroutine(IHaveToDelayLoadingTheFirstLevelForSomeReason());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Load(int level_number) {
            boxes_tm.ClearAllTiles();
            void_tm.ClearAllTiles();
            string[] level_data = levels[level_number].ToString().Split("\n");
            for(int i = 0; i < level_data.Length; i += 1) {
                level_data[i] = level_data[i].Replace(" ", "");
                for(int j = 0; j < level_data[i].Length; j += 1) {
                    room_tm.SetTile(new Vector3Int(x_offset + j, y_offset - i, 0), floor);
                    switch(level_data[i][j]) {
                        case 'W':
                            room_tm.SetTile(new Vector3Int(x_offset + j, y_offset - i, 0), wall);
                            break;
                        case 'V':
                            void_tm.SetTile(new Vector3Int(x_offset + j, y_offset - i, 0), void_tile);
                            break;
                        case 'P':
                            GameObject.Find("Player").GetComponent<TM_Player_Movement>().Reload(x_offset + j, y_offset - i);
                            break;
                        case 'R':
                            boxes_tm.SetTile(new Vector3Int(x_offset + j, y_offset - i, 0), red_box);
                            break;
                        case 'Y':
                            boxes_tm.SetTile(new Vector3Int(x_offset + j, y_offset - i, 0), yellow_box);
                            break;
                        case 'B':
                            boxes_tm.SetTile(new Vector3Int(x_offset + j, y_offset - i, 0), blue_box);
                            break;
                        case 'r':
                            room_tm.SetTile(new Vector3Int(x_offset + j, y_offset - i, 0), red_finish);
                            break;
                        case 'y':
                            room_tm.SetTile(new Vector3Int(x_offset + j, y_offset - i, 0), yellow_finish);
                            break;
                        case 'b':
                            room_tm.SetTile(new Vector3Int(x_offset + j, y_offset - i, 0), blue_finish);
                            break;
                    }
                }
            }
        }

    IEnumerator IHaveToDelayLoadingTheFirstLevelForSomeReason() {
        yield return null;
        Load(level_counter);
    }
}
