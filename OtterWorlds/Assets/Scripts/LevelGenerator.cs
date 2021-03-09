using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private enum ChunkType { NONE = 0, BOTTOM_TOP = 1, BOTTOM_TOP_LEFT = 2, BOTTOM_TOP_RIGHT = 3, BOTTOM_TOP_LEFT_RIGHT = 4};
    private const int CHUNK_TYPE_SIZE = 4;

    private const int ROOM_WIDTH = 6;
    private const int ROOM_HEIGHT = 4;
    private const int CHUNK_WIDTH = 10;

    public GameObject[] BT_chunks;
    public GameObject[] BTL_chunks;
    public GameObject[] BTR_chunks;
    public GameObject[] BTLR_chunks;

    public GameObject startingPoint;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateMap()
    {
        Vector2 position = new Vector2(0, 0); // Start generation from left-bottom corner

        ChunkType[,] map = new ChunkType[ROOM_WIDTH, ROOM_HEIGHT];
        ChunkType forcedChunk;
        ChunkType chunk;

        bool directedToTop;

        while (position.x < ROOM_WIDTH)
        {
            directedToTop = Random.Range(0, 2) == 1; // set generator direction randomly to top or to bottom

            do
            {
                chunk = (ChunkType) Random.Range(1, CHUNK_TYPE_SIZE + 1);
                map[(int)position.x, (int)position.y] = chunk;

                if ( (position.y == 0 && !directedToTop) || (position.y == ROOM_HEIGHT - 1 && directedToTop) )
                {
                    if (chunk != ChunkType.BOTTOM_TOP_LEFT_RIGHT && chunk != ChunkType.BOTTOM_TOP_RIGHT)
                    {
                        // possible dead end, force generation of chunk to the right
                        forcedChunk = (ChunkType) Random.Range( (int) ChunkType.BOTTOM_TOP_RIGHT, CHUNK_TYPE_SIZE + 1);
                        map[(int)position.x, (int)position.y] = forcedChunk;
                    }
                    break;
                }
                else if (directedToTop)
                {
                    position.y++;
                } 
                else
                {
                    position.y--;
                }

            } while (chunk != ChunkType.BOTTOM_TOP_LEFT_RIGHT && chunk != ChunkType.BOTTOM_TOP_RIGHT);

            position.x++;
        }

        ChunkType chunkTypeToInstantiate;
        GameObject chunkToInstantiate;

        for (int i = 0; i < ROOM_WIDTH; i++)
        {
            for (int j = 0; j < ROOM_HEIGHT; j++)
            {
                chunkTypeToInstantiate = map[i, j];

                if (chunkTypeToInstantiate == ChunkType.NONE)
                {
                    chunkTypeToInstantiate = (ChunkType) Random.Range(1, CHUNK_TYPE_SIZE + 1);
                }

                switch (chunkTypeToInstantiate)
                {
                    case ChunkType.BOTTOM_TOP:
                        chunkToInstantiate = BT_chunks[Random.Range(0, BT_chunks.Length)];
                        break;
                    case ChunkType.BOTTOM_TOP_LEFT:
                        chunkToInstantiate = BTL_chunks[Random.Range(0, BTL_chunks.Length)];
                        break;
                    case ChunkType.BOTTOM_TOP_RIGHT:
                        chunkToInstantiate = BTR_chunks[Random.Range(0, BTR_chunks.Length)];
                        break;
                    case ChunkType.BOTTOM_TOP_LEFT_RIGHT:
                    default:
                        chunkToInstantiate = BTLR_chunks[Random.Range(0, BTLR_chunks.Length)];
                        break;
                }

                Instantiate(chunkToInstantiate, new Vector2(CHUNK_WIDTH * i, CHUNK_WIDTH * j), Quaternion.identity);
            }
        }
    }
}
