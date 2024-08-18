using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

public class Level_Completion_Detector : MonoBehaviour
{
    Tilemap room_tm;
    Tilemap boxes_tm;

    public TileBase red_box;
    public TileBase red_finish;
    public TileBase yellow_box;
    public TileBase yellow_finish;
    public TileBase blue_box;
    public TileBase blue_finish;

    public bool running;
    public bool win = false;

    Level_Loader loader;

    // Start is called before the first frame update
    void Start()
    {
        room_tm = GameObject.Find("Room").GetComponent<Tilemap>();
        boxes_tm = GameObject.Find("Boxes").GetComponent<Tilemap>();
        loader = GameObject.Find("LevelLoad").GetComponent<Level_Loader>();
        StartCoroutine(LevelCheck());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator LevelCheck() {
        running = true;
        while(running) {
            List<(string, Vector3Int)> box_list = new List<(string, Vector3Int)>();
            List<(string, Vector3Int)> finish_list = new List<(string, Vector3Int)>();

            //get all boxes on the grid
            for(int x = boxes_tm.cellBounds.min.x; x < boxes_tm.cellBounds.max.x; x += 1) {
                for(int y = boxes_tm.cellBounds.min.y; y < boxes_tm.cellBounds.max.y; y += 1) {
                    TileBase box = boxes_tm.GetTile(new Vector3Int(x, y, 0));
                    if(box == red_box) {
                        box_list.Add(("red", new Vector3Int(x, y, 0)));
                    } else if(box == yellow_box) {
                        box_list.Add(("yellow", new Vector3Int(x, y, 0)));
                    } else if(box == blue_box) {
                        box_list.Add(("blue", new Vector3Int(x, y, 0)));
                    }
                }
            }

            //get all finishes on the grid
            for(int x = room_tm.cellBounds.min.x; x < room_tm.cellBounds.max.x; x += 1) {
                for(int y = room_tm.cellBounds.min.y; y < room_tm.cellBounds.max.y; y += 1) {
                    TileBase finish = room_tm.GetTile(new Vector3Int(x, y, 0));
                    if(finish == red_finish) {
                        finish_list.Add(("red", new Vector3Int(x, y, 0)));
                    } else if(finish == yellow_finish) {
                        finish_list.Add(("yellow", new Vector3Int(x, y, 0)));
                    } else if(finish == blue_finish) {
                        finish_list.Add(("blue", new Vector3Int(x, y, 0)));
                    }
                }
            }

            
            win = true;
            //check if finish list is empty first lmao
            if(finish_list.Count == 0) {
                win = false;
            }
            //check if every entry in finish list has corresponding entry in box list
            foreach((string finish_color, Vector3Int finish_pos) in finish_list) {
                if(!box_list.Contains((finish_color, finish_pos))) {
                    win = false;
                }
            }

            if(win) {
                loader.level_counter += 1;
                loader.Load(loader.level_counter);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
