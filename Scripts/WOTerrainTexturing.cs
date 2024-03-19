using System;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class WOTerrainTexturing : ITerrainTexturing
{
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

		public float maxTerrainHeight;

		public float oceanElevation;

		public float beachElevation;

		public int mapPixelX;

		public int mapPixelY;

		private float NoiseWeight(float worldX, float worldY)
		{
			return GetNoise(worldX, worldY, 0.05f, 0.9f, 0.4f, 3, 417028);
		}

		private byte GetWeightedRecord(float weight, float lowerGrassSpread = 0.5f, float upperGrassSpread = 0.95f)
		{
			if (weight < lowerGrassSpread)
			{
				return 1;
			}
			if (weight > upperGrassSpread)
			{
				return 3;
			}
			return 2;
		}

		private float GetNoise(float x, float y, float frequency, float amplitude, float persistance, int octaves, int seed = 0)
		{
			float num = 0f;
			for (int i = 0; i < octaves; i++)
			{
				num += Mathf.PerlinNoise((float)seed + x * frequency, (float)seed + y * frequency) * amplitude;
				frequency *= 2f;
				amplitude *= persistance;
			}
			return Mathf.Clamp(num, -1f, 1f);
		}

		public void Execute(int index)
		{
			int num = JobA.Row(index, tdDim);
			int num2 = JobA.Col(index, tdDim);
			int c = (int)Mathf.Clamp((float)hDim * ((float)num / (float)tdDim), 0f, hDim - 1);
			int r = (int)Mathf.Clamp((float)hDim * ((float)num2 / (float)tdDim), 0f, hDim - 1);
			float num3 = heightmapData[JobA.Idx(r, c, hDim)] * maxTerrainHeight;
			if (num3 <= oceanElevation)
			{
				tileData[index] = 0;
				return;
			}
			if (num3 <= beachElevation + (float)JobRand.Next(-15000000, 15000000) / 10000000f)
			{
				tileData[index] = 1;
				return;
			}
			int num4 = mapPixelX * 128 + num;
			int num5 = 64000 - mapPixelY * 128 + num2;
			float num6 = 0f;
			num6 += NoiseWeight(num4, num5);
			tileData[index] = GetWeightedRecord(num6);
		}
	}

	private const int seed = 417028;

	private const byte water = 0;

	private const byte dirt = 1;

	private const byte grass = 2;

	private const byte stone = 3;

	private readonly bool basicRoadsEnabled;

	protected static readonly int tileDataDim = 129;

	protected static readonly int assignTilesDim = 128;

	protected byte[] lookupTable;

	public WOTerrainTexturing(bool basicRoadsEnabled)
	{
		this.basicRoadsEnabled = basicRoadsEnabled;
		CreateLookupTable();
	}

	public virtual bool ConvertWaterTiles()
	{
		return true;
	}

	public virtual JobHandle ScheduleAssignTilesJob(ITerrainSampler terrainSampler, ref MapPixelData mapData, JobHandle dependencies, bool march = true)
	{
		NativeArray<byte> nativeArray = new NativeArray<byte>(tileDataDim * tileDataDim, Allocator.TempJob);
		GenerateTileDataJob jobData = default(GenerateTileDataJob);
		jobData.heightmapData = mapData.heightmapData;
		jobData.tileData = nativeArray;
		jobData.tdDim = tileDataDim;
		jobData.hDim = terrainSampler.HeightmapDimension;
		jobData.maxTerrainHeight = terrainSampler.MaxTerrainHeight;
		jobData.oceanElevation = terrainSampler.OceanElevation;
		jobData.beachElevation = terrainSampler.BeachElevation;
		jobData.mapPixelX = mapData.mapPixelX;
		jobData.mapPixelY = mapData.mapPixelY;
		JobHandle preAssignTilesHandle;
		JobHandle jobHandle = (preAssignTilesHandle = jobData.Schedule(tileDataDim * tileDataDim, 64, dependencies));
		if (basicRoadsEnabled)
		{
			ModManager.Instance.SendModMessage("BasicRoads", "scheduleRoadsJob", new object[3] { mapData, nativeArray, jobHandle }, delegate(string message, object data)
			{
				if (message == "error")
				{
					Debug.LogError(data as string);
				}
				else
				{
					preAssignTilesHandle = (JobHandle)data;
				}
			});
		}
		NativeArray<byte> nativeArray2 = new NativeArray<byte>(lookupTable, Allocator.TempJob);
		AssignTilesJob jobData2 = default(AssignTilesJob);
		jobData2.lookupTable = nativeArray2;
		jobData2.tileData = nativeArray;
		jobData2.tilemapData = mapData.tilemapData;
		jobData2.tdDim = tileDataDim;
		jobData2.tDim = assignTilesDim;
		jobData2.march = march;
		jobData2.locationRect = mapData.locationRect;
		JobHandle result = jobData2.Schedule(assignTilesDim * assignTilesDim, 64, preAssignTilesHandle);
		mapData.nativeArrayList.Add(nativeArray);
		mapData.nativeArrayList.Add(nativeArray2);
		return result;
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
