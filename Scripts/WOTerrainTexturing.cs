// Project:         Daggerfall Tools For Unity
// Copyright:       Copyright (C) 2009-2020 Daggerfall Workshop
// Web Site:        http://www.dfworkshop.net
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Source Code:     https://github.com/Interkarma/daggerfall-unity
// Original Author: Gavin Clayton (interkarma@dfworkshop.net)
// Contributors:    Hazelnut, Daniel87
//
// Notes:
//

using UnityEngine;
using System;
using DaggerfallConnect.Arena2;
using Unity.Jobs;
using Unity.Collections;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game.Utility.ModSupport;

namespace WildernessOverhaul
{
    public class WOTerrainTexturing : ITerrainTexturing
    {
        const int seed = 417028;
        const byte water = 0;
        const byte dirt = 1;
        const byte grass = 2;
        const byte stone = 3;

        static bool interestingErodedTerrainEnabled;
        readonly bool basicRoadsEnabled;

        // Order: Desert1, Desert2, Mountains, Rainforest, Swamp,
        // Subtropics, Mountain Woods, Woodland, Haunted Woods, Ocean
        static float[] frequency = ScaleArray(new float[] {0.02f, 0.02f, 0.025f, 0.035f, 0.02f, 0.02f, 0.035f, 0.035f, 0.035f, 0.1f}, 1.0f);
        static float[] amplitude = ScaleArray(new float[] {0.3f, 0.3f, 0.3f, 0.4f, 0.3f, 0.3f, 0.4f, 0.4f, 0.4f, 0.95f}, 1.0f);
        static float[] persistence = ScaleArray(new float[] {0.5f, 0.55f, 0.95f, 0.8f, 0.5f, 0.5f, 0.8f, 0.5f, 0.8f, 0.3f}, 1.0f);
        static int octaves = 5;
        static float[] upperWaterSpread = ScaleArray(new float[] {-1.0f, -1.0f, 0.0f, 0.0f, -1.0f, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f}, 1.0f);
        static float[] lowerGrassSpread = ScaleArray(new float[] {0.4f, 0.35f, 0.45f, 0.55f, 0.4f, 0.1f, 0.35f, 0.45f, 0.55f, 0.35f}, 0.5f);
        static float[] upperGrassSpread = ScaleArray(new float[] {0.5f, 0.5f, 0.95f, 0.95f, 0.5f, 0.95f, 0.95f, 0.95f, 0.95f, 0.95f}, 1.0f);

        // Method to scale arrays
        private static float[] ScaleArray(float[] array, float scaleFactor) {
            float[] scaledArray = new float[array.Length];
            for (int i = 0; i < array.Length; i++) {
                scaledArray[i] = array[i] * scaleFactor;
            }
            return scaledArray;
        }

        public static float treeLine = UnityEngine.Random.Range(0.675f, 0.69f);

        protected static readonly int tileDataDim = MapsFile.WorldMapTileDim + 1;
        protected static readonly int assignTilesDim = MapsFile.WorldMapTileDim;

        protected byte[] lookupTable;
        static int[][] lookupRegistry;

        static MapPixelData currentMapData;

        public WOTerrainTexturing(bool ITEnabled, bool basicRoadsEnabled)
        {
            interestingErodedTerrainEnabled = ITEnabled;
            this.basicRoadsEnabled = basicRoadsEnabled;
            CreateLookupTable();
        }

        // Turn off the normal water tile conversion in DFU, do it in this mod instead.
        public bool ConvertWaterTiles()
        {
            return true;
        }

        public virtual JobHandle ScheduleAssignTilesJob(ITerrainSampler terrainSampler, ref MapPixelData mapData, JobHandle dependencies, bool march = true)
        {
            // Cache tile data to minimise noise sampling during march.
            NativeArray<byte> tileData = new NativeArray<byte>(tileDataDim * tileDataDim, Allocator.TempJob);
            currentMapData = mapData;

            GenerateTileDataJob tileDataJob = new GenerateTileDataJob
            {
                heightmapData = mapData.heightmapData,
                tileData = tileData,
                tdDim = tileDataDim,
                hDim = terrainSampler.HeightmapDimension,
                maxTerrainHeight = terrainSampler.MaxTerrainHeight,
                oceanElevation = terrainSampler.OceanElevation,
                beachElevation = terrainSampler.BeachElevation,
                mapPixelX = mapData.mapPixelX,
                mapPixelY = mapData.mapPixelY,
                worldClimate = mapData.worldClimate,
            };
            JobHandle tileDataHandle = tileDataJob.Schedule(tileDataDim * tileDataDim, 64, dependencies);

            // Schedule the paint roads jobs if basic roads mod is enabled
            JobHandle preAssignTilesHandle = tileDataHandle;
            if (basicRoadsEnabled)
            {
                ModManager.Instance.SendModMessage("BasicRoads", "scheduleRoadsJob", new object[] { mapData, tileData, tileDataHandle },
                    (string message, object data) =>
                    {
                        if (message == "error")
                            Debug.LogError(data as string);
                        else
                            preAssignTilesHandle = (JobHandle)data;
                    });
            }

            // Assign tile data to terrain
            NativeArray<byte> lookupData = new NativeArray<byte>(lookupTable, Allocator.TempJob);
            AssignTilesJob assignTilesJob = new AssignTilesJob
            {
                lookupTable = lookupData,
                tileData = tileData,
                tilemapData = mapData.tilemapData,
                tdDim = tileDataDim,
                tDim = assignTilesDim,
                march = march,
                locationRect = mapData.locationRect,
            };
            JobHandle assignTilesHandle = assignTilesJob.Schedule(assignTilesDim * assignTilesDim, 64, preAssignTilesHandle);

            // Add both working native arrays to disposal list.
            mapData.nativeArrayList.Add(tileData);
            mapData.nativeArrayList.Add(lookupData);

            return assignTilesHandle;
        }

        protected struct AssignTilesJob : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<byte> tileData;
            [ReadOnly]
            public NativeArray<byte> lookupTable;

            public NativeArray<byte> tilemapData;

            public int tdDim;
            public int tDim;
            public bool march;
            public Rect locationRect;

            public void Execute(int index)
            {
			    int r = JobA.Row(index, tDim);
			    int c = JobA.Col(index, tDim);
			    if (tilemapData[index] == 0)
			    {
				    if (march)
				    {
					    int num = JobA.Idx(r, c, tdDim);
					    int num2 = tileData[num];
					    int num3 = tileData[num + 1];
					    int num4 = tileData[num + tdDim];
					    int num5 = tileData[num + tdDim + 1];
					    int num6 = (num2 & 1) | ((num3 & 1) << 1) | ((num4 & 1) << 2) | ((num5 & 1) << 3);
					    int num7 = num2 + num3 + num4 + num5 >> 2;
					    int index2 = num6 | (num7 << 4);
					    tilemapData[index] = lookupTable[index2];
				    }
				    else
				    {
					    tilemapData[index] = tileData[JobA.Idx(r, c, tdDim)];
				    }
                }
            }
        }

        protected struct GenerateTileDataJob : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<float> heightmapData;

            public NativeArray<byte> tileData;

            public int hDim;
            public int tdDim;
            public int tDim;
            public float maxTerrainHeight;
            public float oceanElevation;
            public float beachElevation;
            public int mapPixelX;
            public int mapPixelY;
            public int worldClimate;

            // Gets noise value
            private float NoiseWeight(float worldX, float worldY, float height)
            {
                float persistenceRnd = 0.95f;
                int climateNum = 9;
                switch (worldClimate) {
                    case (int)MapsFile.Climates.Desert:
                        climateNum = 0;
                        persistenceRnd = persistence[climateNum] + ((height / maxTerrainHeight) * 2) - 0.25f;
                        break;
                    case (int)MapsFile.Climates.Desert2:
                        climateNum = 1;
                        persistenceRnd = persistence[climateNum] + ((height / maxTerrainHeight) * 2) - 0.25f;
                        break;
                    case (int)MapsFile.Climates.Mountain:
                        climateNum = 2;
                        if ((height / maxTerrainHeight) + JobRand.Next(-5, 5) / 1000f > treeLine)
                            persistenceRnd = persistence[climateNum] - treeLine + ((height / maxTerrainHeight) * 1.2f);
                        else
                            persistenceRnd = persistence[climateNum] - (1 - (height / maxTerrainHeight)) * 0.4f;
                        break;
                    case (int)MapsFile.Climates.Rainforest:
                        climateNum = 3;
                        persistenceRnd = persistence[climateNum] + ((height / maxTerrainHeight) * 1.5f) - 0.35f;
                        break;
                    case (int)MapsFile.Climates.Swamp:
                        climateNum = 4;
                        persistenceRnd = persistence[climateNum] + ((height / maxTerrainHeight) * 3) - 0.25f;
                        break;
                    case (int)MapsFile.Climates.Subtropical:
                        climateNum = 5;
                        persistenceRnd = persistence[climateNum] + ((height / maxTerrainHeight) * 2f) - 0.25f;
                        break;
                    case (int)MapsFile.Climates.MountainWoods:
                        climateNum = 6;
                        persistenceRnd = persistence[climateNum] + ((height / maxTerrainHeight) * 1.5f) - 0.35f;
                        break;
                    case (int)MapsFile.Climates.Woodlands:
                        climateNum = 7;
                        if (interestingErodedTerrainEnabled)
                            persistenceRnd = persistence[climateNum] + ((height / maxTerrainHeight) * 2.5f);
                        else
                            persistenceRnd = persistence[climateNum] + ((height / maxTerrainHeight) * 1.4f) - 0.30f;
                        break;
                    case (int)MapsFile.Climates.HauntedWoodlands:
                        climateNum = 8;
                        persistenceRnd = persistence[climateNum] + ((height / maxTerrainHeight) * 1.5f) - 0.35f;
                        break;
                    case (int)MapsFile.Climates.Ocean:
                        climateNum = 9;
                        persistenceRnd = persistence[climateNum] + ((height / maxTerrainHeight) * 1.5f) - 0.35f;
                        break;
                }
                return GetNoise(worldX, worldY, frequency[climateNum], amplitude[climateNum], persistenceRnd, octaves, seed);
            }

            // Sets texture by range
            private byte GetWeightedRecord(float weight, float upperWaterSpread = 0.0f, float lowerGrassSpread = 0.35f, float upperGrassSpread = 0.95f)
            {
                if (weight < upperWaterSpread)
                    return water;
                else if (weight >= upperWaterSpread && weight < lowerGrassSpread)
                    return dirt;
                else if (weight >= lowerGrassSpread && weight < upperGrassSpread)
                    return grass;
                else
                    return stone;
            }

            // Noise function
            private float GetNoise(
              float x,
              float y,
              float frequency,
              float amplitude,
              float persistence,
              int octaves,
              int seed = 0)
            {
                float finalValue = 0f;
                for (int i = 0; i < octaves; ++i)
                {
                    finalValue += Mathf.PerlinNoise(seed + (x * frequency), seed + (y * frequency)) * amplitude;
                    frequency *= 2.0f;
                    amplitude *= persistence;
                }
                return Mathf.Clamp(finalValue, -1, 1);
            }

            public void Execute(int index)
            {
                int x = JobA.Row(index, tdDim);
                int y = JobA.Col(index, tdDim);
                int uB = heightmapData.Length;

                // Height sample for ocean and beach tiles
                int hx = (int)Mathf.Clamp(hDim * ((float)x / (float)tdDim), 0, hDim - 1);
                int hy = (int)Mathf.Clamp(hDim * ((float)y / (float)tdDim), 0, hDim - 1);
                float height = heightmapData[JobA.Idx(hy, hx, hDim)] * maxTerrainHeight;

                // Ocean and Beach texture
                if (height < oceanElevation - 0.001f) {
                    tileData[index] = water;
                    return;
                }

                // Adds a little +/- randomness to threshold so beach line isn't too regular
                // Thinner beach where a rock face is diving down steep into water
                if (height < beachElevation - 9.0f) { // Constant added for World of Daggerfall ports compatibility
                    tileData[index] = dirt;
                    return;
                }

                // Rock Mountain Face
                int rnd = JobRand.Next(75,90);
                if (SteepnessWithinLimits(true, Mathf.Clamp(rnd - ((height / maxTerrainHeight) / rnd),30f,60f), heightmapData, maxTerrainHeight, hx, hy, hDim, uB, index, tdDim, tileData)) {
                    tileData[index] = stone;
                    return;
                }

                // Dirt slope Face
                //rnd = JobRand.Next(50,75);
                //if (SteepnessWithinLimits(true, Mathf.Clamp(rnd - ((height / maxTerrainHeight) / rnd),40f,90f), heightmapData, maxTerrainHeight, hx, hy, hDim, uB, index, tdDim, tileData)) {
                //    tileData[index] = dirt;
                //    return;
                //}

                // Adds a little +/- randomness to threshold so beach line isn't too regular
                if (height <= beachElevation - 9.0f + (JobRand.Next(-100, 100) / 100f)) {
                    tileData[index] = dirt;
                    return;
                }

                // Get latitude and longitude of this tile
                int latitude = (int)(mapPixelX * MapsFile.WorldMapTileDim + x);
                int longitude = (int)(MapsFile.MaxWorldTileCoordZ - mapPixelY * MapsFile.WorldMapTileDim + y);

                // Set texture tile using weighted noise
                float weight = 0;
                weight += NoiseWeight(latitude, longitude, height);


                int climateNum = 9;
                switch (worldClimate)
                {
                    case (int)MapsFile.Climates.Desert:
                        climateNum = 0;
                        break;
                    case (int)MapsFile.Climates.Desert2:
                        climateNum = 1;
                        break;
                    case (int)MapsFile.Climates.Mountain:
                        climateNum = 2;
                        break;
                    case (int)MapsFile.Climates.Rainforest:
                        climateNum = 3;
                        break;
                    case (int)MapsFile.Climates.Swamp:
                        climateNum = 4;
                        break;
                    case (int)MapsFile.Climates.Subtropical:
                        climateNum = 5;
                        break;
                    case (int)MapsFile.Climates.MountainWoods:
                        climateNum = 6;
                        break;
                    case (int)MapsFile.Climates.Woodlands:
                        climateNum = 7;
                        break;
                    case (int)MapsFile.Climates.HauntedWoodlands:
                        climateNum = 8;
                        break;
                    case (int)MapsFile.Climates.Ocean:
                        climateNum = 9;
                        break;
                }
                tileData[index] = GetWeightedRecord(weight, upperWaterSpread[climateNum], lowerGrassSpread[climateNum], upperGrassSpread[climateNum]);

                // Check for lowest local point in desert to place oasis
                //if (worldClimate == (int)MapsFile.Climates.Desert2 && LowestPointFound(30, heightmapData, maxTerrainHeight, hx, hy, hDim, uB, index, tdDim, tileData)) {
                //    tileData[index] = water;
                //    return;
                //}

                //if (worldClimate == (int)MapsFile.Climates.Desert && LowestPointFound(80, heightmapData, maxTerrainHeight, hx, hy, hDim, uB, index, tdDim, tileData)) {
                //    tileData[index] = water;
                //    return;
                //}



                /* rnd = JobRand.Next(25,35);
                if (tileData[index] == stone && SteepnessWithinLimits(false, Mathf.Clamp(rnd - ((height / maxTerrainHeight) / rnd),40f,90f), heightmapData, maxTerrainHeight, hx, hy, hDim, uB, index, tdDim, tileData)) {
                    tileData[index] = grass;
                    return;
                } */

                // Max angle for dirt patches
                rnd = JobRand.Next(20,25);
                if (tileData[index] == dirt && SteepnessWithinLimits(false, rnd, heightmapData, maxTerrainHeight, hx, hy, hDim, uB, index, tdDim, tileData)) {
                    tileData[index] = dirt;
                    return;
                }

                if (tileData[index] == dirt && !SteepnessWithinLimits(false, rnd, heightmapData, maxTerrainHeight, hx, hy, hDim, uB, index, tdDim, tileData)) {
                    tileData[index] = grass;
                    return;
                }
            }
        }

        static bool LowestPointFound(int chance, NativeArray<float> heightmapData, float maxTerrainHeight, int hx, int hy, int hDim, int upperBound, int index, int tdDim, NativeArray<byte> tileData)
        {
            int newChance = (int)(chance - (chance * heightmapData[JobA.Idx(hy, hx, hDim)] * 2));

            if (tileData[index] != dirt ||
              JobA.Row(index, tdDim) - 5 <= 0 ||
              JobA.Col(index, tdDim) + 5 >= tdDim ||
              JobA.Row(index, tdDim) + 5 >= tdDim ||
              JobA.Col(index, tdDim) - 5 <= 0)
            {
                return false;
            }
            else
            {
                float thisHeight = heightmapData[JobA.Idx(hy, hx, hDim)] * maxTerrainHeight;
                if (JobA.Idx(hy - 5, hx - 5, hDim) < 0 ||
                    JobA.Idx(hy - 5, hx - 5, hDim) > upperBound ||
                    JobA.Idx(hy - 5, hx + 5, hDim) < 0 ||
                    JobA.Idx(hy - 5, hx + 5, hDim) > upperBound ||
                    JobA.Idx(hy + 5, hx - 5, hDim) < 0 ||
                    JobA.Idx(hy + 5, hx - 5, hDim) > upperBound ||
                    JobA.Idx(hy + 5, hx + 5, hDim) < 0 ||
                    JobA.Idx(hy + 5, hx + 5, hDim) > upperBound)
                {
                    return false;
                }
                else
                {
                    for (int a = -4; a < 5; a++)
                    {
                        for (int b = -4; b < 5; b++)
                        {
                            if ((a != 0 && b != 0) && heightmapData[JobA.Idx(hy + a, hx + b, hDim)] * maxTerrainHeight < thisHeight + (0.0000035f * Mathf.Abs(a)) + (0.0000035f * Mathf.Abs(b)))
                            {
                                return false;
                            }
                        }
                    }
                    return (JobRand.Next(0, 100) <= newChance);
                }
            }
        }

        static bool SteepnessWithinLimits(bool bigger, float steepness, NativeArray<float> heightmapData, float maxTerrainHeight, int hx, int hy, int hDim, int upperBound, int index, int tdDim, NativeArray<byte> tileData)
        {
            int offsetX = 0;
            int offsetY = 0;
            if (JobA.Col(index, tdDim) + 1 >= tdDim)
            {
                offsetY = -1;
            }
            if (JobA.Row(index, tdDim) + 1 >= tdDim)
            {
                offsetX = -1;
            }

            float minSmpl = 0;
            float maxSmpl = 0;
            float smpl = minSmpl = maxSmpl = heightmapData[JobA.Idx(hy, hx, hDim)] * maxTerrainHeight;
            for (int a = 0; a <= 1; a++)
            {
                for (int b = 0; b <= 1; b++)
                {
                    smpl = heightmapData[JobA.Idx(hy + a + offsetY, hx + b + offsetX, hDim)] * maxTerrainHeight;

                    if (smpl < minSmpl)
                    {
                        minSmpl = smpl;
                    }
                    if (smpl > maxSmpl)
                    {
                        maxSmpl = smpl;
                    }
                }
            }

            float diff = (maxSmpl - minSmpl) * 10f;

            if (bigger) {
                return (diff >= steepness);
            } else {
                return (diff <= steepness);
            }
        }

        // Encodes a byte with Daggerfall tile neighbours
        static int FindTileIndex(int[][] array, int bl, int br, int tr, int tl)
        {
            int[] testArray = new int[4]{bl,br,tr,tl};
            for (int i = 0; i < array.Length; ++i) {
                if(array[i][0] == testArray[0] && array[i][1] == testArray[1] && array[i][2] == testArray[2] && array[i][3] == testArray[3]) {
                    return i;
                }
            }
            Debug.LogErrorFormat("Couldnt find index. Setting tile to water.");
            return 0;
        }

	    private void CreateLookupTable()
	    {
		    lookupTable = new byte[64];
		    AddLookupRange(0, 1, 5, 48, reverse: false, 0);
		    AddLookupRange(2, 1, 10, 51, reverse: true, 16);
		    AddLookupRange(2, 3, 15, 53, reverse: false, 32);
		    AddLookupRange(3, 3, 15, 53, reverse: true, 48);
	    }

	    private void AddLookupRange(int baseStart, int baseEnd, int shapeStart, int saddleIndex, bool reverse, int offset)
	    {
		    if (reverse)
		    {
			    lookupTable[offset] = MakeLookup(baseStart, rotate: false, flip: false);
			    lookupTable[offset + 1] = MakeLookup(shapeStart + 2, rotate: true, flip: true);
			    lookupTable[offset + 2] = MakeLookup(shapeStart + 2, rotate: false, flip: false);
			    lookupTable[offset + 3] = MakeLookup(shapeStart + 1, rotate: true, flip: true);
			    lookupTable[offset + 4] = MakeLookup(shapeStart + 2, rotate: false, flip: true);
			    lookupTable[offset + 5] = MakeLookup(shapeStart + 1, rotate: false, flip: true);
			    lookupTable[offset + 6] = MakeLookup(saddleIndex, rotate: true, flip: false);
			    lookupTable[offset + 7] = MakeLookup(shapeStart, rotate: true, flip: true);
			    lookupTable[offset + 8] = MakeLookup(shapeStart + 2, rotate: true, flip: false);
			    lookupTable[offset + 9] = MakeLookup(saddleIndex, rotate: false, flip: false);
			    lookupTable[offset + 10] = MakeLookup(shapeStart + 1, rotate: false, flip: false);
			    lookupTable[offset + 11] = MakeLookup(shapeStart, rotate: false, flip: false);
			    lookupTable[offset + 12] = MakeLookup(shapeStart + 1, rotate: true, flip: false);
			    lookupTable[offset + 13] = MakeLookup(shapeStart, rotate: false, flip: true);
			    lookupTable[offset + 14] = MakeLookup(shapeStart, rotate: true, flip: false);
			    lookupTable[offset + 15] = MakeLookup(baseEnd, rotate: false, flip: false);
		    }
		    else
		    {
			    lookupTable[offset] = MakeLookup(baseStart, rotate: false, flip: false);
			    lookupTable[offset + 1] = MakeLookup(shapeStart, rotate: true, flip: false);
			    lookupTable[offset + 2] = MakeLookup(shapeStart, rotate: false, flip: true);
			    lookupTable[offset + 3] = MakeLookup(shapeStart + 1, rotate: true, flip: false);
			    lookupTable[offset + 4] = MakeLookup(shapeStart, rotate: false, flip: false);
			    lookupTable[offset + 5] = MakeLookup(shapeStart + 1, rotate: false, flip: false);
			    lookupTable[offset + 6] = MakeLookup(saddleIndex, rotate: false, flip: false);
			    lookupTable[offset + 7] = MakeLookup(shapeStart + 2, rotate: true, flip: false);
			    lookupTable[offset + 8] = MakeLookup(shapeStart, rotate: true, flip: true);
			    lookupTable[offset + 9] = MakeLookup(saddleIndex, rotate: true, flip: false);
			    lookupTable[offset + 10] = MakeLookup(shapeStart + 1, rotate: false, flip: true);
			    lookupTable[offset + 11] = MakeLookup(shapeStart + 2, rotate: false, flip: true);
			    lookupTable[offset + 12] = MakeLookup(shapeStart + 1, rotate: true, flip: true);
			    lookupTable[offset + 13] = MakeLookup(shapeStart + 2, rotate: false, flip: false);
			    lookupTable[offset + 14] = MakeLookup(shapeStart + 2, rotate: true, flip: true);
			    lookupTable[offset + 15] = MakeLookup(baseEnd, rotate: false, flip: false);
		    }
	    }

	    private byte MakeLookup(int index, bool rotate, bool flip)
	    {
		    if (index > 55)
		    {
			    throw new IndexOutOfRangeException("Index out of range. Valid range 0-55");
		    }
		    if (rotate)
		    {
			    index += 64;
		    }
		    if (flip)
		    {
			    index += 128;
		    }
		    return (byte)index;
	    }
    }
}
