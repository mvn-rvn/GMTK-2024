using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TM_Box_Move : MonoBehaviour
{
    public TileBase[] boxes = new TileBase[4];
    public TileBase wall;

    Tilemap player_tm;
    Tilemap room_tm;
    Tilemap boxes_tm;

    List<Vector3Int> neighbor_pos_list;

    Vector3Int direction_vec;

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

    public bool AttemptMoveBoxes(Vector3Int root_pos, string direction, TileBase target_box) {
        Vector3Int[] all_neighbors_pos = FindAllNeighbors(root_pos, target_box);
        direction_vec = new Vector3Int(0, 0, 0);
        foreach(Vector3Int pos in all_neighbors_pos) {
            switch(direction) {
                case "right":
                    direction_vec = new Vector3Int(1, 0, 0);
                    if(room_tm.GetTile(pos + direction_vec) == wall) {
                        return false;
                    } else if(boxes_tm.GetTile(pos + direction_vec) != target_box && boxes_tm.GetTile(pos + direction_vec) != null) {
                        if(!AttemptMoveBoxes(pos + direction_vec, direction, boxes_tm.GetTile(pos + direction_vec))) {
                            return false;
                        }
                    }
                    break;
                case "left":
                    direction_vec = new Vector3Int(-1, 0, 0);
                    if(room_tm.GetTile(pos + direction_vec) == wall) {
                        return false;
                    } else if(boxes_tm.GetTile(pos + direction_vec) != target_box && boxes_tm.GetTile(pos + direction_vec) != null) {
                        if(!AttemptMoveBoxes(pos + direction_vec, direction, boxes_tm.GetTile(pos + direction_vec))) {
                            return false;
                        }
                    }
                    break;
                case "up":
                    direction_vec = new Vector3Int(0, 1, 0);
                    if(room_tm.GetTile(pos + direction_vec) == wall) {
                        return false;
                    } else if(boxes_tm.GetTile(pos + direction_vec) != target_box && boxes_tm.GetTile(pos + direction_vec) != null) {
                        if(!AttemptMoveBoxes(pos + direction_vec, direction, boxes_tm.GetTile(pos + direction_vec))) {
                            return false;
                        }
                    }
                    break;
                case "down":
                    direction_vec = new Vector3Int(0, -1, 0);
                    if(room_tm.GetTile(pos + direction_vec) == wall) {
                        return false;
                    } else if(boxes_tm.GetTile(pos + direction_vec) != target_box && boxes_tm.GetTile(pos + direction_vec) != null) {
                        if(!AttemptMoveBoxes(pos + direction_vec, direction, boxes_tm.GetTile(pos + direction_vec))) {
                            return false;
                        }
                    }
                    break;
            }
        }
        for(int i = 0; i < all_neighbors_pos.Length; i += 1) {
            boxes_tm.SetTile(all_neighbors_pos[i], null);
            all_neighbors_pos[i] += direction_vec;
        }
        foreach(Vector3Int new_pos in all_neighbors_pos) {
            boxes_tm.SetTile(new_pos, target_box);
        }
        return true;
    }

    public bool AttemptBoxAlter(Vector3Int root_pos, string side, string into_away, TileBase target_box) {
        Vector3Int[] all_neighbors_pos = FindAllNeighbors(root_pos, target_box);
        //Fuck it I'm going full Yandere Dev
        if(into_away == "into") {
            if(side == "top" || side == "bottom") {
                foreach(Vector3Int pos in all_neighbors_pos) {
                    if(boxes_tm.GetTile(pos + new Vector3Int(0, 1, 0)) != target_box && boxes_tm.GetTile(pos + new Vector3Int(0, -1, 0)) != target_box) {
                        return false;
                    }
                }
            }
            if(side == "left" || side == "right") {
                foreach(Vector3Int pos in all_neighbors_pos) {
                    if(boxes_tm.GetTile(pos + new Vector3Int(1, 0, 0)) != target_box && boxes_tm.GetTile(pos + new Vector3Int(-1, 0, 0)) != target_box) {
                        return false;
                    }
                }
            }
            if(side == "top") {
                int y = -9999;
                foreach(Vector3Int pos in all_neighbors_pos) {
                    if(pos.y > y) {
                        y = pos.y;
                    }
                }
                foreach(Vector3Int pos in all_neighbors_pos) {
                    if(pos.y == y) {
                        boxes_tm.SetTile(pos, null);
                    }
                }
            }
            if(side == "bottom") {
                int y = 9999;
                foreach(Vector3Int pos in all_neighbors_pos) {
                    if(pos.y < y) {
                        y = pos.y;
                    }
                }
                foreach(Vector3Int pos in all_neighbors_pos) {
                    if(pos.y == y) {
                        boxes_tm.SetTile(pos, null);
                    }
                }
            }
            if(side == "left") {
                int x = 9999;
                foreach(Vector3Int pos in all_neighbors_pos) {
                    if(pos.x < x) {
                        x = pos.x;
                    }
                }
                foreach(Vector3Int pos in all_neighbors_pos) {
                    if(pos.x == x) {
                        boxes_tm.SetTile(pos, null);
                    }
                }
            }
            if(side == "right") {
                int x = -9999;
                foreach(Vector3Int pos in all_neighbors_pos) {
                    if(pos.x > x) {
                        x = pos.x;
                    }
                }
                foreach(Vector3Int pos in all_neighbors_pos) {
                    if(pos.x == x) {
                        boxes_tm.SetTile(pos, null);
                    }
                }
            }
        }

        if(into_away == "away") {
            if(side == "top") {
                foreach(Vector3Int pos in all_neighbors_pos) {
                    if(room_tm.GetTile(pos + Vector3Int.up) == wall || (boxes_tm.GetTile(pos + Vector3Int.up) != target_box && boxes_tm.GetTile(pos + Vector3Int.up) != null)) {
                        return false;
                    }
                }
                foreach(Vector3Int pos in all_neighbors_pos) {
                    boxes_tm.SetTile(pos + Vector3Int.up, target_box);
                }
            }
            if(side == "bottom") {
                foreach(Vector3Int pos in all_neighbors_pos) {
                    if(room_tm.GetTile(pos + Vector3Int.down) == wall || (boxes_tm.GetTile(pos + Vector3Int.down) != target_box && boxes_tm.GetTile(pos + Vector3Int.down) != null)) {
                        return false;
                    }
                }
                foreach(Vector3Int pos in all_neighbors_pos) {
                    boxes_tm.SetTile(pos + Vector3Int.down, target_box);
                }
            }
            if(side == "left") {
                foreach(Vector3Int pos in all_neighbors_pos) {
                    if(room_tm.GetTile(pos + Vector3Int.left) == wall || (boxes_tm.GetTile(pos + Vector3Int.left) != target_box && boxes_tm.GetTile(pos + Vector3Int.left) != null)) {
                        return false;
                    }
                }
                foreach(Vector3Int pos in all_neighbors_pos) {
                    boxes_tm.SetTile(pos + Vector3Int.left, target_box);
                }
            }
            if(side == "right") {
                foreach(Vector3Int pos in all_neighbors_pos) {
                    if(room_tm.GetTile(pos + Vector3Int.right) == wall || (boxes_tm.GetTile(pos + Vector3Int.right) != target_box && boxes_tm.GetTile(pos + Vector3Int.right) != null)) {
                        return false;
                    }
                }
                foreach(Vector3Int pos in all_neighbors_pos) {
                    boxes_tm.SetTile(pos + Vector3Int.right, target_box);
                }
            }
        }

        return true;
    }
}
