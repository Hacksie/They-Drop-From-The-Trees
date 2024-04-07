
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

namespace HackedDesign
{
    public class Terrain
    {
        private TerrainType[,] terrainMap;
        private int seed;
        private float depth;
        private int width;
        private int height;
        private Vector3 worldOffset;
        private AnimationCurve depthCurve;
        //private MeshData meshData;

        public MeshData MeshData { get { return GenerateTerrainMesh(this.worldOffset); } }
        public TerrainType[,] TerrainMap { get => terrainMap; set => terrainMap = value; }
        public Texture2D TerrainTexture { get { return TextureFromTerrainMap(); } }
        private Dictionary<string, List<Vector2Int>> terrainTypeMap = new Dictionary<string, List<Vector2Int>>();

        public Terrain(int seed, int width, int height, float depth, AnimationCurve depthCurve, Vector3 worldOffset)
        {
            this.seed = seed < 0 ? Random.Range(0, int.MaxValue) : seed;
            this.width = width;
            this.height = height;
            this.depth = depth;
            this.depthCurve = depthCurve;
            this.worldOffset = worldOffset;
        }

        public List<Vector2Int> GetLocationsOfType(string name)
        {
            return terrainTypeMap[name];
        }

        public List<Vector2Int> GetLocationsOfTypes(List<string> names)
        {
            return terrainTypeMap.Where(t => names.Contains(t.Key)).SelectMany(v => v.Value).ToList();
        }

        public Vector2Int GetRandomLocation()
        {
            var list = terrainTypeMap.SelectMany(v => v.Value).ToList();
            return list[Random.Range(0, list.Count)];            
        }

        public Vector2Int GetRandomLocationOfType(string name)
        {
            var list = terrainTypeMap[name];
            // FIXME:
            // if(list.Count == 0)
            // {
            //     return null;
            // }
            return list[Random.Range(0, list.Count)];
        }

        public Vector2Int GetRandomLocationOfTypes(List<string> names)
        {
            var list = terrainTypeMap.Where(e => names.Contains(e.Key)).SelectMany(v => v.Value).ToList();
            return list[Random.Range(0, list.Count)];
        }

        public List<TerrainType> GetTerrainsOfType(string name)
        {
            List<TerrainType> result = new List<TerrainType>();
            var height = TerrainMap.GetLength(1);
            var width = TerrainMap.GetLength(0);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (terrainMap[x, y].name == name)
                    {
                        result.Add(terrainMap[x, y]);
                    }
                }
            }

            return result;
        }

        public List<TerrainType> GetTerrainsOfType(string[] names)
        {
            List<TerrainType> result = new List<TerrainType>();
            var height = TerrainMap.GetLength(1);
            var width = TerrainMap.GetLength(0);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (names.Contains(terrainMap[x, y].name))
                    {
                        result.Add(terrainMap[x, y]);
                    }
                }
            }

            return result;
        }

        public Vector3 TerrainPositionToWorld(Vector2Int position) => new Vector3(position.x, worldOffset.y, height - position.y - 1);

        public Vector2Int WorldPositionToTerrain(Vector3 worldPosition) => new Vector2Int(Mathf.RoundToInt(worldPosition.x), height - Mathf.RoundToInt(worldPosition.z) - 1); // FIXME: Do we need worldOffset here?

        public TerrainType SampleTerrain(Vector3 worldPosition)
        {
            var offset = WorldPositionToTerrain(worldPosition);
            return TerrainMap[offset.x, offset.y];
        }

        public float SampleTerrainHeight(Vector3 worldPosition)
        {
            //FIXME: Check bounds
            var offset = WorldPositionToTerrain(worldPosition);
            Debug.Log(offset);
            return TerrainMap[offset.x, offset.y].height;
            //return 0;
        }

        public float SampleTerrainMeshHeight(Vector3 worldPosition)
        {
            //FIXME: Check bounds
            var offset = WorldPositionToTerrain(worldPosition);
            Debug.Log(offset);
            return TerrainMap[offset.x, offset.y].meshHeight;
            //return 0;
        }


        public void Generate(float scale, int octaves, float persistance, float lacunarity, Vector2 noiseOffset, TerrainType[] regions)
        {
            float[,] noiseMap = GenerateNoiseMap(seed, this.width, this.height, scale, octaves, persistance, lacunarity, noiseOffset);
            TerrainMap = GenerateTerrainMap(noiseMap, this.width, this.height, regions, this.depthCurve, this.depth);
        }

        private TerrainType[,] GenerateTerrainMap(float[,] noiseMap, int width, int height, TerrainType[] regions, AnimationCurve depthCurve, float depth)
        {
            TerrainType[,] terrainMap = new TerrainType[width, height];
            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    var terrainSquare = GenerateTerrainMapSquare(noiseMap[x, z], regions, depthCurve, depth);
                    terrainMap[x, z] = terrainSquare;
                    if(!terrainTypeMap.ContainsKey(terrainSquare.name))
                    {
                        terrainTypeMap.Add(terrainSquare.name, new List<Vector2Int>());
                    }

                    terrainTypeMap[terrainSquare.name].Add(new Vector2Int(x, z));
                }
            }
            return terrainMap;
        }

        private Texture2D TextureFromTerrainMap()
        {
            Texture2D texture = new Texture2D(this.width, this.height)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };

            Color[] colorMap = new Color[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    colorMap[y * width + x] = terrainMap[x, y].color;
                }
            }

            texture.SetPixels(colorMap);
            texture.Apply();
            return texture;
        }

        private TerrainType GenerateTerrainMapSquare(float height, TerrainType[] regions, AnimationCurve depthCurve, float depth)
        {
            for (int i = 0; i < regions.Length; i++)
            {
                if (height <= regions[i].height)
                {
                    var terrain = regions[i];
                    terrain.height = height;
                    terrain.meshHeight = depthCurve.Evaluate(height) * depth;
                    return terrain;
                }
            }

            return regions[0];
        }


        private MeshData GenerateTerrainMesh(Vector3 worldOffset)
        {
            int width = terrainMap.GetLength(0);
            int height = terrainMap.GetLength(1);
            float topLeftX = (width - 1) / -2f;
            float topLeftZ = (height - 1) / 2f;

            MeshData meshData = new MeshData(width, height);
            int vertexIndex = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    meshData.vertices[vertexIndex] = new Vector3(topLeftX + x + worldOffset.x, terrainMap[x, y].meshHeight + worldOffset.y, topLeftZ - y + worldOffset.z);
                    meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                    if (x < width - 1 && y < height - 1)
                    {
                        meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                        meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                    }

                    vertexIndex++;
                }
            }

            return meshData;
        }

        private float[,] GenerateNoiseMap(int seed, int mapWidth, int mapHeight, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
        {
            float[,] noiseMap = new float[mapWidth, mapHeight];

            var prng = new System.Random(seed);
            Vector2[] octaveOffsets = new Vector2[octaves];

            for (int i = 0; i < octaves; i++)
            {
                float offsetX = prng.Next(-100000, 100000) + offset.x;
                float offsetY = prng.Next(-100000, 100000) + offset.y;
                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }

            if (scale <= 0)
            {
                scale = 0.0001f;
            }

            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;
            float halfWidth = mapWidth / 2f;
            float halfHeight = mapHeight / 2f;

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    float amplitude = 1;
                    float frequency = 1;

                    float noiseHeight = 0;

                    for (int i = 0; i < octaves; i++)
                    {
                        float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                        float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                        float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= persistance;
                        frequency *= lacunarity;
                    }

                    if (noiseHeight > maxNoiseHeight)
                    {
                        maxNoiseHeight = noiseHeight;
                    }
                    else if (noiseHeight < minNoiseHeight)
                    {
                        minNoiseHeight = noiseHeight;
                    }

                    noiseMap[x, y] = noiseHeight;
                }
            }

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                }
            }

            return noiseMap;
        }
    }

    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public float meshHeight;
        public Color color;
        public float treeChance;
    }
}