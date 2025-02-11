using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour {

    public EventHandler OnGenerateMap;

    [SerializeField] private float monsterPossibility = 0.7f;
    [SerializeField] private float obstaclePossibility;
    
    [SerializeField] private List<GameObject> bosses;
    [SerializeField] private List<GameObject> monsters;
    
    [SerializeField] private List<RoomSO> RoomSos;
    [SerializeField] private int bossCount = 3;
    [SerializeField] private int RoomWidth = 17;
    [SerializeField] private int RoomHeight = 10;
    
    private const int RowCount = 10;
    private const int ColumnCount = 10;
    private const int ManhattanDistance = 12;

    private Dictionary<int, RoomSO> RoomType2Room = new Dictionary<int, RoomSO>();
    private Dictionary<int, Room> Id2Room = new Dictionary<int, Room>();

    private int[,] mapMatrix = new int[RowCount, ColumnCount];
    private int startRoomIndex, endRoomIndex;
    private List<int> bossRooms = new List<int>();
    private List<int> path = new List<int>();
    private int[] preNodeMatrix = new int[RowCount * ColumnCount];
    private int[,] connectionMatrix = new int[RowCount, ColumnCount];
    
    public List<Room> rooms;
    public Room startRoom, endRoom;
    
    // Start is called before the first frame update
    void Start() {

        for (int i = 0; i < preNodeMatrix.Length; ++i) {
            preNodeMatrix[i] = -1;
        }

        foreach (var roomSO in RoomSos) {
            RoomType2Room.Add(roomSO.RoomType, roomSO);
        }

        GenerateMap();

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateMap() {
        // 生成起始点
        do {
            startRoomIndex = Random.Range(0, RowCount * ColumnCount);
            endRoomIndex = Random.Range(0, RowCount * ColumnCount);
        } while (GetManhattanDistanceBetweenPoints(startRoomIndex, endRoomIndex) < ManhattanDistance);
        // startRoomIndex = 0;
        // endRoomIndex = 99;
        // 获取起始点路径
        GeneratePath();
        // 生成分支
        GenerateBranch();
        // 生成房间
        GenerateRoomType();
        // 生成障碍物
        GenerateObstacle();

        OnGenerateMap?.Invoke(this, EventArgs.Empty);
    }

    private void GeneratePath() {
        Queue<int> q = new Queue<int>();
        int[] dx = new[] { 0, 1, 0, -1 }, dy = new[] { 1, 0, -1, 0 };
        HashSet<int> visited = new HashSet<int>();
        
        q.Enqueue(startRoomIndex);
        visited.Add(startRoomIndex);
        
        while (q.Count > 0) {
            int tempIndex = q.Dequeue();
            int tx = 0, ty = 0;
            GetXYByRoomIndex(tempIndex, ref tx, ref ty);
            
            if (tempIndex == endRoomIndex) {
                break;
            }
            
            int startRandomDirection = Random.Range(0, 4), directionCount = 4;
            for (int i = 0, currentDirection = startRandomDirection; i < directionCount; ++i, currentDirection = (currentDirection + i) % directionCount) {
                int x = dx[currentDirection] + tx, y = dy[currentDirection] + ty;
                int newRoomIndex = GetRoomIndexByXY(x, y);
                if ((x >= 0 && x < RowCount) && (y >= 0 && y < ColumnCount) && !visited.Contains(newRoomIndex)) {
                    preNodeMatrix[newRoomIndex] = tempIndex;
                    visited.Add(newRoomIndex);
                    q.Enqueue(newRoomIndex);
                }
            }
        }
        
        RecursiveCalculatePath(endRoomIndex, -1);
    }
    
    private void GenerateBranch() {
        int deltaBossRoom = path.Count / bossCount;
        
        var visitedRoom = new HashSet<int>();
        foreach (var node in path) {
            visitedRoom.Add(node);
        }
        
        for (int i = path.Count - 1; i >= 0; --i) {
            if ((i % deltaBossRoom == 0 || i == path.Count - 1) && i != 0) {
                bossRooms.Add(path[i]);
                int branchDeep = Random.Range(6, 12);
                RecursiveExtendBranch(path[i], -1, ref visitedRoom, branchDeep);
            }
        }

        bossRooms.Reverse();
    }

    private void GenerateRoomType() {
        for (int i = 0; i < RowCount; ++i) {
            for (int j = 0; j < ColumnCount; ++j) {
                Room room = Instantiate(RoomType2Room[connectionMatrix[i, j]].RoomPrefab, 
                    new Vector2(j * RoomWidth, -i * RoomHeight), Quaternion.identity).GetComponent<Room>();
                rooms.Add(room);
                int roomIndex = GetRoomIndexByXY(i, j);

                room.Id = roomIndex;
                Id2Room[roomIndex] = room;
                
                if (roomIndex == startRoomIndex) {
                    startRoom = room;
                } else if (roomIndex == endRoomIndex) {
                    endRoom = room;
                }
                
            }

        }
    }
    
    private void GenerateObstacle() {

        for (int i = 0, j = 0; i < bossRooms.Count && j < bosses.Count; ++i, ++j) {
            Room room = Id2Room[bossRooms[i]];
            BaseBoss boss = Instantiate(bosses[j], room.transform.position, quaternion.identity).GetComponent<BaseBoss>();
            room.boss = boss;
            if (j == bosses.Count - 1) {
                boss.OnBossDead += OnBossDead;
            }
        }
        
        foreach (var room in rooms) {
            if (room != startRoom && !bossRooms.Contains(room.Id)) {
                if (1f - Random.Range(0f, 1f) > monsterPossibility) {
                    int typeIndex = Random.Range(0, monsters.Count);
                    BaseMonster monster = Instantiate(monsters[typeIndex], room.transform.position, quaternion.identity).GetComponent<BaseMonster>();
                    room.monsters.Add(monster);
                }
            }
            // TODO Generate Obstacle
        }
        
    }

    private void OnBossDead(object sender, EventArgs e) {
        SceneManager.LoadScene("EndChooseScene");
    }

    // 工具方法
    int GetManhattanDistanceBetweenPoints(int startIndex, int endIndex) {
        int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
        GetXYByRoomIndex(startIndex, ref x1, ref y1);
        GetXYByRoomIndex(endIndex, ref x2, ref y2);
        return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
    }
    
    private int GetRoomIndexByXY(int x, int y) {
        return x * ColumnCount + y;
    }

    private void GetXYByRoomIndex(int index, ref int x, ref int y) {
        x = index / ColumnCount;
        y = index % ColumnCount;
    }
    
    // 8：B在A上， 4：B在A右，2：B在A下，1：B在A左
    private int GetRoomRelativePosition(int ARoom, int BRoom) {
        int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
        GetXYByRoomIndex(ARoom, ref x1, ref y1);
        GetXYByRoomIndex(BRoom, ref x2, ref y2);
        if (x2 < x1) {
            return 8;
        } else if (x2 > x1) {
            return 2;
        } else if (y2 < y1) {
            return 1;
        } else {
            return 4;
        }
    } 
    
    private void RecursiveCalculatePath(int currentRoom, int lastRoom) {
        if (currentRoom == -1) {
            return;
        }

        if (lastRoom != -1) {
            int ABRelativePosition = GetRoomRelativePosition(currentRoom, lastRoom);
            int BARelativePosition = GetRoomRelativePosition(lastRoom, currentRoom);
            int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
            GetXYByRoomIndex(currentRoom, ref x1, ref y1);
            GetXYByRoomIndex(lastRoom, ref x2, ref y2);
            connectionMatrix[x1, y1] |= ABRelativePosition;
            connectionMatrix[x2, y2] |= BARelativePosition;
        }
        RecursiveCalculatePath(preNodeMatrix[currentRoom], currentRoom);
        path.Add(currentRoom);
    }

    
    private void RecursiveExtendBranch(int currentRoom, int lastRoom, ref HashSet<int> visitedRoom, int deep) {

        if (deep == 0) {
            return;
        }
        
        visitedRoom.Add(currentRoom);
        int currentRoomX = 0, currentRoomY = 0, lastRoomX = 0, lastRoomY = 0;
        GetXYByRoomIndex(currentRoom, ref currentRoomX, ref currentRoomY);
        GetXYByRoomIndex(lastRoom, ref lastRoomX, ref lastRoomY);
        
        if (lastRoom != -1) {
            int ABRelativePosition = GetRoomRelativePosition(currentRoom, lastRoom);
            int BARelativePosition = GetRoomRelativePosition(lastRoom, currentRoom);
            connectionMatrix[currentRoomX, currentRoomY] |= ABRelativePosition;
            connectionMatrix[lastRoomX, lastRoomY] |= BARelativePosition;
        }
        
        int[] dx = new[] { 0, 1, 0, -1 }, dy = new[] { 1, 0, -1, 0 };
        int startRandomDirection = Random.Range(0, 4), directionCount = 4;
        for (int i = 0, currentDirection = startRandomDirection; i < directionCount; ++i, currentDirection = (currentDirection + i) % directionCount) {
            int x = dx[currentDirection] + currentRoomX, y = dy[currentDirection] + currentRoomY;
            int newRoomIndex = GetRoomIndexByXY(x, y);
            if ((x >= 0 && x < RowCount) && (y >= 0 && y < ColumnCount) && (!visitedRoom.Contains(newRoomIndex) || bossRooms.Contains(newRoomIndex))) {
                deep--;
                RecursiveExtendBranch(newRoomIndex, currentRoom, ref visitedRoom, deep);
                break;
            }
        }
    }
    

    
}
