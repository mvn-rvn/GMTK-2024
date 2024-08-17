using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TM_Player_Movement : MonoBehaviour
{
    Keyboard_Input_Handler input;

    Tilemap player_tm;
    Tilemap room_tm;
    Tilemap boxes_tm;

    public TileBase pl_up;
    public TileBase pl_down;
    public TileBase pl_left;
    public TileBase pl_right;
    TileBase pl_current;
    public TileBase wall;
    public TileBase[] boxes = new TileBase[4];

    int x = 0;
    int y = 0;
    public int start_x;
    public int start_y;

    // Start is called before the first frame update
    void Start()
    {
        input = GameObject.Find("GameRunner").GetComponent<Keyboard_Input_Handler>();
        player_tm = gameObject.GetComponent<Tilemap>();
        room_tm = GameObject.Find("Room").GetComponent<Tilemap>();
        boxes_tm = GameObject.Find("Boxes").GetComponent<Tilemap>();
        player_tm.ClearAllTiles();
        pl_current = pl_up;
        x = start_x;
        y = start_y;
        player_tm.SetTile(new Vector3Int(x, y, 0), pl_current);
    }

    // Update is called once per frame
    void Update()
    {
        if(input.HandleInput() == "up" || input.HandleInput() == "down" || input.HandleInput() == "left" || input.HandleInput() == "right") {
            int prev_x = x;
            int prev_y = y;
            string direction = input.HandleInput();
            player_tm.ClearAllTiles();
            switch(input.HandleInput()) {
                case "up":
                    y += 1;
                    pl_current = pl_up;
                    break;
                case "down":
                    y -=1;
                    pl_current = pl_down;
                    break;
                case "left":
                    x -= 1;
                    pl_current = pl_left;
                    break;
                case "right":
                    x += 1;
                    pl_current = pl_right;
                    break;
            }
            if(room_tm.GetTile(new Vector3Int(x, y, 0)) == wall) {
                x = prev_x;
                y = prev_y;
            } else if(boxes_tm.GetTile(new Vector3Int(x, y, 0)) != null
            && GameObject.Find("Boxes").GetComponent<TM_Box_Move>().IsMovable(new Vector3Int(x, y, 0), direction, boxes_tm.GetTile(new Vector3Int(x, y, 0))) == false) {
                x = prev_x;
                y = prev_y;
            }
            player_tm.SetTile(new Vector3Int(x, y, 0), pl_current);
        }
    }
}
