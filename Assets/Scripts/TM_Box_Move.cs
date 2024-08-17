using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TM_Box_Move : MonoBehaviour
{
    public TileBase[] boxes = new TileBase[4];

    Tilemap player_tm;
    Tilemap room_tm;
    Tilemap boxes_tm;

    List<Vector3Int> neighbor_pos_list;

    // Start is called before the first frame update
    void Start()
    {
        player_tm = GameObject.Find("Player").GetComponent<Tilemap>();
        room_tm = GameObject.Find("Room").GetComponent<Tilemap>();
        boxes_tm = gameObject.GetComponent<Tilemap>();

        neighbor_pos_list = new List<Vector3Int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3Int[] FindAllNeighbors(Vector3Int root_pos, TileBase target_box) {
        FloodFillCheck(root_pos, target_box);
        Vector3Int[] final_result = neighbor_pos_list.ToArray();
        neighbor_pos_list.Clear();
        return final_result;
    }

    void FloodFillCheck(Vector3Int pos, TileBase target_box) {
        if(boxes_tm.GetTile(pos) == target_box && !neighbor_pos_list.Contains(pos)) {
            neighbor_pos_list.Add(pos);
            FloodFillCheck(pos + new Vector3Int(1, 0, 0), target_box);
            FloodFillCheck(pos + new Vector3Int(-1, 0, 0), target_box);
            FloodFillCheck(pos + new Vector3Int(0, 1, 0), target_box);
            FloodFillCheck(pos + new Vector3Int(0, -1, 0), target_box);
        }
    }

    public bool IsMovable(Vector3Int root_pos, string direction, TileBase same_color_box) {
        Vector3Int[] all_neighbors_pos = FindAllNeighbors(root_pos, same_color_box);
        return true;
    }
}
