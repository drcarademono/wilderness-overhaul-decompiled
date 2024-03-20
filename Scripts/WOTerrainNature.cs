using System;
using System.Collections.Generic;
using DaggerfallConnect;
using DaggerfallConnect.Arena2;
using DaggerfallConnect.Utility;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Utility;
using DaggerfallWorkshop.Utility.AssetInjection;
using UnityEngine;
using WildernessOverhaul;

public class WOTerrainNature : ITerrainNature
{
	private static int randomSeed;

	private static bool dynamicNatureClearance;

	private static bool vegetationInLocations;

	private static bool firefliesExist;

	private static bool shootingStarsExist;

	private static bool InterestingErodedTerrainEnabled;

	private static float fireflyDistance;

	private static float shootingStarsMinimum;

	private static float shootingStarsMaximum;

	private static float generalNatureClearance;

	private static float natureClearance1;

	private static float natureClearance2;

	private static float natureClearance3;

	private static float natureClearance4;

	private static float natureClearance5;

	private static Mod mod;

	public float height;

	private static Vector3? LastTreePosition = null;

	private const float maxSteepness = 50f;

	private const float slopeSinkRatio = 70f;

    private int currentBillboardCount = 0;

	public WOVegetationList vegetationList;

	public WOVegetationChance vegetationChance;

	public WOStochasticChances stochastics;

	private static int TerrainDistance;

	private static readonly HashSet<Vector2Int> triedBillboards = new HashSet<Vector2Int>();

	private static Func<float> getTreeScaleCallback = () => UnityEngine.Random.Range(0.6f, 1.4f);

	private static Func<Color32> getTreeColorCallback = () => Color.Lerp(Color.white, Color.grey, UnityEngine.Random.value);

	public bool NatureMeshUsed { get; protected set; }

	public WOTerrainNature(Mod woMod, bool DMEnabled, bool ITEnabled, int rngSeed, bool dNClearance, bool vegInLoc, bool fireflies, bool shootingStars, float fireflyActivationDistance, float shootingStarsMin, float shootingStarsMax, float gNClearance, float nClearance1, float nClearance2, float nClearance3, float nClearance4, float nClearance5)
	{
		mod = woMod;
		if (DMEnabled)
		{
			new List<int>(new int[3] { 5, 13, 30 });
		}
		else
		{
			new List<int>(new int[3] { 5, 13, 13 });
		}
		Debug.Log("Wilderness Overhaul: DREAM Sprites enabled: " + DMEnabled);
		InterestingErodedTerrainEnabled = ITEnabled;
		Debug.Log("Wilderness Overhaul: Interesting Eroded Terrain enabled: " + InterestingErodedTerrainEnabled);
		randomSeed = rngSeed;
		UnityEngine.Random.seed = randomSeed;
		Debug.Log("Wilderness Overhaul: Random Seed: " + rngSeed);
		dynamicNatureClearance = dNClearance;
		Debug.Log("Wilderness Overhaul: Setting Dynamic Nature Clearance: " + dynamicNatureClearance);
		vegetationInLocations = vegInLoc;
		Debug.Log("Wilderness Overhaul: Setting Vegetation in Jungle Location: " + vegetationInLocations);
		generalNatureClearance = gNClearance;
		Debug.Log("Wilderness Overhaul: Setting General Nature Clearance: " + generalNatureClearance);
		firefliesExist = fireflies;
		Debug.Log("Wilderness Overhaul: Generate Fireflies at Night: " + firefliesExist);
		fireflyDistance = fireflyActivationDistance;
		Debug.Log("Wilderness Overhaul: Activation Distance of Fireflies: " + fireflyDistance);
		shootingStarsExist = shootingStars;
		Debug.Log("Wilderness Overhaul: Generate Shooting Stars at Night: " + shootingStarsExist);
		shootingStarsMinimum = shootingStarsMin;
		shootingStarsMaximum = shootingStarsMax;
		Debug.Log("Wilderness Overhaul: Shooting Stars Chance Min: " + shootingStarsMinimum + " and Max: " + shootingStarsMaximum);
		natureClearance1 = nClearance1;
		Debug.Log("Wilderness Overhaul: Setting Nature Clearance 1: " + natureClearance1);
		natureClearance2 = nClearance2;
		Debug.Log("Wilderness Overhaul: Setting Nature Clearance 2: " + natureClearance2);
		natureClearance3 = nClearance3;
		Debug.Log("Wilderness Overhaul: Setting Nature Clearance 3: " + natureClearance3);
		natureClearance4 = nClearance4;
		Debug.Log("Wilderness Overhaul: Setting Nature Clearance 4: " + natureClearance4);
		natureClearance5 = nClearance5;
		Debug.Log("Wilderness Overhaul: Setting Nature Clearance 5: " + natureClearance5);
		stochastics = new WOStochasticChances(randomSeed);
		vegetationChance = new WOVegetationChance(randomSeed);
		vegetationList = new WOVegetationList(randomSeed);
		vegetationList.SetNatureCollections();
	}

	public virtual void LayoutNature(DaggerfallTerrain dfTerrain, DaggerfallBillboardBatch dfBillboardBatch, float terrainScale, int terrainDist)
	{
		TerrainDistance = terrainDist;
		LastTreePosition = null;
		if (dynamicNatureClearance)
		{
			if (dfTerrain.MapData.LocationType == DFRegion.LocationTypes.TownCity)
			{
				generalNatureClearance = natureClearance1;
			}
			else if (dfTerrain.MapData.LocationType == DFRegion.LocationTypes.TownHamlet)
			{
				generalNatureClearance = natureClearance2;
			}
			else if (dfTerrain.MapData.LocationType == DFRegion.LocationTypes.TownVillage || dfTerrain.MapData.LocationType == DFRegion.LocationTypes.HomeWealthy || dfTerrain.MapData.LocationType == DFRegion.LocationTypes.ReligionCult)
			{
				generalNatureClearance = natureClearance3;
			}
			else if (dfTerrain.MapData.LocationType == DFRegion.LocationTypes.HomeFarms || dfTerrain.MapData.LocationType == DFRegion.LocationTypes.ReligionTemple || dfTerrain.MapData.LocationType == DFRegion.LocationTypes.Tavern || dfTerrain.MapData.LocationType == DFRegion.LocationTypes.HomePoor)
			{
				generalNatureClearance = natureClearance4;
			}
			else if (dfTerrain.MapData.LocationType == DFRegion.LocationTypes.DungeonLabyrinth || dfTerrain.MapData.LocationType == DFRegion.LocationTypes.DungeonKeep || dfTerrain.MapData.LocationType == DFRegion.LocationTypes.DungeonRuin || dfTerrain.MapData.LocationType == DFRegion.LocationTypes.Graveyard || dfTerrain.MapData.LocationType == DFRegion.LocationTypes.Coven)
			{
				generalNatureClearance = natureClearance5;
			}
			if (dfTerrain.MapData.worldClimate == 227)
			{
				generalNatureClearance = 0.5f;
			}
		}
		Rect locationRect = dfTerrain.MapData.locationRect;
		if (locationRect.x > 0f && locationRect.y > 0f)
		{
			locationRect.xMin -= generalNatureClearance;
			locationRect.xMax += generalNatureClearance;
			locationRect.yMin -= generalNatureClearance;
			locationRect.yMax += generalNatureClearance;
		}
		Terrain component = dfTerrain.gameObject.GetComponent<Terrain>();
		if (!component)
		{
			return;
		}
		TerrainData terrainData = component.terrainData;
		if (!terrainData)
		{
			return;
		}
		dfBillboardBatch.Clear();
		MeshReplacement.ClearNatureGameObjects(component);
		foreach (Transform item in dfBillboardBatch.transform)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		UnityEngine.Random.InitState(TerrainHelper.MakeTerrainKey(dfTerrain.MapPixelX, dfTerrain.MapPixelY));
		Vector2 zero = Vector2.zero;
		int num = 128;
		int heightmapDimension = DaggerfallUnity.Instance.TerrainSampler.HeightmapDimension;
		float scale = terrainData.heightmapScale.x * (float)heightmapDimension / (float)num;
		float maxTerrainHeight = DaggerfallUnity.Instance.TerrainSampler.MaxTerrainHeight;
		_ = DaggerfallUnity.Instance.TerrainSampler.BeachElevation;
		if (InterestingErodedTerrainEnabled)
		{
			maxTerrainHeight = 4890f;
		}
		DFLocation.ClimateSettings worldClimateSettings = MapsFile.GetWorldClimateSettings(dfTerrain.MapData.worldClimate);
		if (shootingStarsExist)
		{
			AddShootingStar(dfTerrain, dfBillboardBatch, 90f, 1200f, shootingStarsMinimum, shootingStarsMaximum);
		}
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num; j++)
			{
				int num2 = dfTerrain.MapPixelX * 128 + j;
				int num3 = 64000 - dfTerrain.MapPixelY * 128 + i;
				float steepness = terrainData.GetSteepness((float)j / (float)num, (float)i / (float)num);
				if (steepness > 50f)
				{
					continue;
				}
				zero.x = j;
				zero.y = i;
				if (locationRect.x > 0f && locationRect.y > 0f && locationRect.Contains(zero))
				{
					continue;
				}
				int num4 = dfTerrain.MapData.tilemapSamples[j, i] & 0x3F;
				if (num4 == 2)
				{
					if (UnityEngine.Random.Range(0f, 1f) > vegetationChance.chanceOnGrass)
					{
						continue;
					}
				}
				else if (num4 == 1)
				{
					if (UnityEngine.Random.Range(0f, 1f) > vegetationChance.chanceOnDirt)
					{
						continue;
					}
				}
				else if (num4 == 3)
				{
					if (UnityEngine.Random.Range(0f, 1f) > vegetationChance.chanceOnStone)
					{
						continue;
					}
				}
				else if (num4 == 0)
				{
					continue;
				}
				int num5 = (int)Mathf.Clamp((float)heightmapDimension * ((float)j / (float)num), 0f, heightmapDimension - 1);
				int num6 = (int)Mathf.Clamp((float)heightmapDimension * ((float)i / (float)num), 0f, heightmapDimension - 1);
				height = dfTerrain.MapData.heightmapSamples[num6, num5];
				vegetationChance.ChangeVegetationChances(height, dfTerrain.MapData.worldClimate);
				if (num4 != 0 && !(height <= 0f))
				{
					ContainerObject baseData = new ContainerObject(dfTerrain, dfBillboardBatch, component, scale, steepness, j, i, maxTerrainHeight);
					PlaceNatureBillboards(num4, baseData, worldClimateSettings.WorldClimate, num2, num3);
				}
			}
		}
		dfBillboardBatch.Apply();
	}

	private void PlaceNatureBillboards(int tile, ContainerObject baseData, int climate, float latitude, float longitude)
	{
		float num = 0f;
		List<int> billboardCollection = new List<int>();
		List<int> billboardCollection2 = new List<int>();
		List<int> billboardCollection3 = new List<int>();
		List<int> billboardCollection4 = new List<int>();
		List<int> billboardCollection5 = new List<int>();
		List<int> billboardCollection6 = new List<int>();
		List<int> billboardCollection7 = new List<int>();
		float limit = 0f;
		float num2 = 0f;
		switch (climate)
		{
		case 223:
		case 231:
			if (tile == 2 && UnityEngine.Random.Range(0f, 100f) <= stochastics.temperateMushroomRingChance)
			{
				AddMushroomRingToBatch(baseData, 23);
				return;
			}
			num = GetNoise(latitude, longitude, stochastics.tempForestFrequency, stochastics.tempForestAmplitude, stochastics.tempForestPersistence, stochastics.tempForestOctaves, 100);
			num = Mathf.Clamp(num, 0f, 1f);
			billboardCollection = vegetationList.temperateWoodlandDeadTrees;
			billboardCollection2 = vegetationList.temperateWoodlandBeach;
			billboardCollection3 = vegetationList.temperateWoodlandTrees;
			billboardCollection4 = vegetationList.temperateWoodlandMushroom;
			billboardCollection5 = vegetationList.temperateWoodlandFlowers;
			billboardCollection6 = vegetationList.temperateWoodlandBushes;
			billboardCollection7 = vegetationList.temperateWoodlandRocks;
			limit = stochastics.tempForestLimit[0];
			num2 = stochastics.tempForestLimit[1];
			break;
		case 226:
			if (tile == 2 && UnityEngine.Random.Range(0f, 100f) <= stochastics.mountainStoneCircleChance)
			{
				AddStoneCircleToBatch(baseData, vegetationList.mountainsRocks, 1, 0);
				return;
			}
			num = GetNoise(latitude, longitude, stochastics.mountForestFrequency, stochastics.mountForestAmplitude, stochastics.mountForestPersistence, stochastics.mountForestOctaves, 100);
			num = Mathf.Clamp(num, 0f, 1f);
			billboardCollection = vegetationList.mountainsDeadTrees;
			billboardCollection2 = vegetationList.mountainsTrees;
			billboardCollection3 = vegetationList.mountainsTrees;
			billboardCollection4 = vegetationList.mountainsNeedleTrees;
			billboardCollection5 = vegetationList.mountainsFlowers;
			billboardCollection6 = vegetationList.mountainsGrass;
			billboardCollection7 = vegetationList.mountainsRocks;
			limit = stochastics.mountForestLimit[0];
			num2 = stochastics.mountForestLimit[1];
			break;
		case 232:
			if (tile == 2 && UnityEngine.Random.Range(0f, 100f) <= stochastics.mountainStoneCircleChance)
			{
				AddStoneCircleToBatch(baseData, vegetationList.hauntedWoodlandBones, 0, 0);
				return;
			}
			num = GetNoise(latitude, longitude, stochastics.hauntedWoodlandForestFrequency, stochastics.hauntedWoodlandForestAmplitude, stochastics.hauntedWoodlandForestPersistence, stochastics.hauntedWoodlandForestOctaves, 100);
			num = Mathf.Clamp(num, 0f, 1f);
			billboardCollection = vegetationList.hauntedWoodlandDeadTrees;
			billboardCollection2 = vegetationList.hauntedWoodlandBeach;
			billboardCollection3 = vegetationList.hauntedWoodlandTrees;
			billboardCollection4 = vegetationList.hauntedWoodlandMushroom;
			billboardCollection5 = vegetationList.hauntedWoodlandFlowers;
			billboardCollection6 = vegetationList.hauntedWoodlandBushes;
			billboardCollection7 = vegetationList.hauntedWoodlandRocks;
			limit = stochastics.hauntedWoodlandForestLimit[0];
			num2 = stochastics.hauntedWoodlandForestLimit[1];
			break;
		case 230:
			num = GetNoise(latitude, longitude, stochastics.woodlandHillsForestFrequency, stochastics.woodlandHillsForestAmplitude, stochastics.woodlandHillsForestPersistence, stochastics.woodlandHillsForestOctaves, 100);
			num = Mathf.Clamp(num, 0f, 1f);
			billboardCollection = vegetationList.woodlandHillsDeadTrees;
			billboardCollection2 = vegetationList.woodlandHillsBeach;
			billboardCollection3 = vegetationList.woodlandHillsTrees;
			billboardCollection4 = vegetationList.woodlandHillsNeedleTrees;
			billboardCollection5 = vegetationList.woodlandHillsFlowers;
			billboardCollection6 = vegetationList.woodlandHillsBushes;
			billboardCollection7 = vegetationList.woodlandHillsRocks;
			limit = stochastics.woodlandHillsForestLimit[0];
			num2 = stochastics.woodlandHillsForestLimit[1];
			break;
		case 227:
			num = GetNoise(latitude, longitude, stochastics.rainforestForestFrequency, stochastics.rainforestForestAmplitude, stochastics.rainforestForestPersistence, stochastics.rainforestForestOctaves, 100);
			num = Mathf.Clamp(num, 0f, 1f);
			billboardCollection = vegetationList.rainforestTrees;
			billboardCollection2 = vegetationList.rainforestBeach;
			billboardCollection3 = vegetationList.rainforestTrees;
			billboardCollection4 = vegetationList.rainforestEggs;
			billboardCollection5 = vegetationList.rainforestFlowers;
			billboardCollection6 = vegetationList.rainforestPlants;
			billboardCollection7 = vegetationList.rainforestRocks;
			limit = stochastics.rainforestForestLimit[0];
			num2 = stochastics.rainforestForestLimit[1];
			break;
		case 224:
			num = GetNoise(latitude, longitude, stochastics.desertFrequency, stochastics.desertAmplitude, stochastics.desertPersistence, stochastics.desertOctaves, 100);
			num = Mathf.Clamp(num, 0f, 1f);
			billboardCollection = vegetationList.desertDeadTrees;
			billboardCollection2 = vegetationList.desertWaterPlants;
			billboardCollection3 = vegetationList.desertTrees;
			billboardCollection4 = vegetationList.desertWaterFlowers;
			billboardCollection5 = vegetationList.desertFlowers;
			billboardCollection6 = vegetationList.desertCactus;
			billboardCollection7 = vegetationList.desertStones;
			limit = stochastics.rainforestForestLimit[0];
			num2 = stochastics.rainforestForestLimit[1];
			break;
		case 225:
			num = GetNoise(latitude, longitude, stochastics.desertFrequency, stochastics.desertAmplitude, stochastics.desertPersistence, stochastics.desertOctaves, 100);
			num = Mathf.Clamp(num, 0f, 1f);
			billboardCollection = vegetationList.desertDeadTrees;
			billboardCollection2 = vegetationList.desertCactus;
			billboardCollection3 = vegetationList.desertDeadTrees;
			billboardCollection4 = vegetationList.desertCactus;
			billboardCollection5 = vegetationList.desertStones;
			billboardCollection6 = vegetationList.desertCactus;
			billboardCollection7 = vegetationList.desertStones;
			limit = stochastics.rainforestForestLimit[0];
			num2 = stochastics.rainforestForestLimit[1];
			break;
		case 229:
			num = GetNoise(latitude, longitude, stochastics.subtropicalForestFrequency, stochastics.subtropicalForestAmplitude, stochastics.subtropicalForestPersistence, stochastics.subtropicalForestOctaves, 100);
			num = Mathf.Clamp(num, 0f, 1f);
			billboardCollection = vegetationList.subtropicalDeadTrees;
			billboardCollection2 = vegetationList.subtropicalBeach;
			billboardCollection3 = vegetationList.subtropicalTrees;
			billboardCollection4 = vegetationList.subtropicalMushroom;
			billboardCollection5 = vegetationList.subtropicalFlowers;
			billboardCollection6 = vegetationList.subtropicalBulbs;
			billboardCollection7 = vegetationList.subtropicalRocks;
			limit = stochastics.subtropicalForestLimit[0];
			num2 = stochastics.subtropicalForestLimit[1];
			break;
		case 228:
			num = GetNoise(latitude, longitude, stochastics.swampForestFrequency, stochastics.swampForestAmplitude, stochastics.swampForestPersistence, stochastics.swampForestOctaves, 100);
			num = Mathf.Clamp(num, 0f, 1f);
			billboardCollection = vegetationList.swampDeadTrees;
			billboardCollection2 = vegetationList.swampBeach;
			billboardCollection3 = vegetationList.swampTrees;
			billboardCollection4 = vegetationList.swampEggs;
			billboardCollection5 = vegetationList.swampFlowers;
			billboardCollection6 = vegetationList.swampPlants;
			billboardCollection7 = vegetationList.swampRocks;
			limit = stochastics.swampForestLimit[0];
			num2 = stochastics.swampForestLimit[1];
			break;
		}
		switch (tile)
		{
		case 1:
		case 10:
		case 20:
			if (!(height > UnityEngine.Random.Range(0.025f, 0.027f)) || !(GetWeightedRecord(num) == "forest"))
			{
				break;
			}
			if (UnityEngine.Random.Range(0, 100) < UnityEngine.Random.Range(70, 80))
			{
				AddBillboardToBatch(baseData, billboardCollection, UnityEngine.Random.Range(0.15f, 0.3f), checkOnLand: true);
				for (int l = 0; l < UnityEngine.Random.Range(0, 3); l++)
				{
					AddBillboardToBatch(baseData, billboardCollection2, UnityEngine.Random.Range(1f, 2f), checkOnLand: true);
				}
			}
			else if ((float)UnityEngine.Random.Range(0, 100) < stochastics.mapStyle)
			{
				for (int m = 0; m < UnityEngine.Random.Range(1, 5); m++)
				{
					if (UnityEngine.Random.Range(0, 100) < 50 && height > UnityEngine.Random.Range(0.1f, 0.15f) && climate == 231)
					{
						AddBillboardToBatch(baseData, billboardCollection, UnityEngine.Random.Range(0.75f, 1.5f), checkOnLand: true, 3);
					}
					else
					{
						AddBillboardToBatch(baseData, billboardCollection3, UnityEngine.Random.Range(0.75f, 1.5f), checkOnLand: true);
					}
				}
			}
			else
			{
				AddBillboardToBatch(baseData, billboardCollection, UnityEngine.Random.Range(0.15f, 0.3f), checkOnLand: true);
				for (int n = 0; n < UnityEngine.Random.Range(0, 3); n++)
				{
					AddBillboardToBatch(baseData, billboardCollection2, UnityEngine.Random.Range(1f, 2f), checkOnLand: true);
				}
			}
			break;
		case 2:
		case 11:
		case 21:
			if (GetWeightedRecord(num, limit, num2) == "flower")
			{
				if (UnityEngine.Random.Range(0, 100) < 5)
				{
					AddBillboardToBatch(baseData, billboardCollection4, 0f, checkOnLand: true);
				}
				for (int num3 = 0; num3 < UnityEngine.Random.Range(4, 7); num3++)
				{
					AddBillboardToBatch(baseData, billboardCollection5, UnityEngine.Random.Range(0.45f, 0.75f), checkOnLand: true);
				}
				float num4 = UnityEngine.Random.Range(0, 100);
				if (num4 < stochastics.mapStyleChance[1])
				{
					for (int num5 = 0; num5 < UnityEngine.Random.Range(2, 4); num5++)
					{
						AddBillboardToBatch(baseData, billboardCollection5, UnityEngine.Random.Range(0.6f, 1f), checkOnLand: true);
					}
				}
				if (num4 < 5f)
				{
					for (int num6 = 0; num6 < UnityEngine.Random.Range(0, 3); num6++)
					{
						AddBillboardToBatch(baseData, billboardCollection6, UnityEngine.Random.Range(0.85f, 1.15f), checkOnLand: true);
					}
				}
			}
			else if (GetWeightedRecord(num, limit, num2) == "grass")
			{
				float num7 = UnityEngine.Random.Range(0, 100);
				if (num7 < 4f)
				{
					for (int num8 = 0; num8 < UnityEngine.Random.Range(9, 14); num8++)
					{
						AddBillboardToBatch(baseData, billboardCollection6, UnityEngine.Random.Range(2.25f, 2.75f), checkOnLand: true);
					}
					for (int num9 = 0; num9 < UnityEngine.Random.Range(3, 5); num9++)
					{
						AddBillboardToBatch(baseData, billboardCollection6, UnityEngine.Random.Range(0.5f, 0.75f), checkOnLand: true);
					}
				}
				if (num7 < 3f)
				{
					for (int num10 = 0; num10 < UnityEngine.Random.Range(3, 8); num10++)
					{
						AddBillboardToBatch(baseData, billboardCollection3, UnityEngine.Random.Range(1f, 2.25f), checkOnLand: true);
					}
				}
			}
			else if (num >= num2 - 0.01f && num < num2)
			{
				for (int num11 = 0; num11 < UnityEngine.Random.Range(5, 15); num11++)
				{
					AddBillboardToBatch(baseData, billboardCollection6, UnityEngine.Random.Range(0.25f, 1.5f), checkOnLand: true);
				}
			}
			else if (num <= num2 + 0.01f && num >= num2)
			{
				for (int num12 = 0; num12 < UnityEngine.Random.Range(10, 20); num12++)
				{
					AddBillboardToBatch(baseData, billboardCollection6, UnityEngine.Random.Range(0.5f, 2f), checkOnLand: true);
				}
			}
			else if (num <= num2 + 0.02f && num > num2 + 0.01f)
			{
				for (int num13 = 0; num13 < UnityEngine.Random.Range(5, 15); num13++)
				{
					AddBillboardToBatch(baseData, billboardCollection6, UnityEngine.Random.Range(0.25f, 1.5f), checkOnLand: true);
				}
			}
			else
			{
				if (!(GetWeightedRecord(num, limit, num2) == "forest"))
				{
					break;
				}
				float num14 = UnityEngine.Random.Range(0, 100);
				for (int num15 = 0; num15 < UnityEngine.Random.Range(3, 4); num15++)
				{
					AddBillboardToBatch(baseData, billboardCollection3, UnityEngine.Random.Range(1f, 2.25f), checkOnLand: true);
				}
				AddFirefly(baseData, 0.1f, 5f, 15, 35);
				if (num14 < stochastics.mapStyleChance[3])
				{
					AddBillboardToBatch(baseData, billboardCollection2, UnityEngine.Random.Range(2f, 3f), checkOnLand: true);
					for (int num16 = 0; num16 < UnityEngine.Random.Range(0, 2); num16++)
					{
						AddBillboardToBatch(baseData, billboardCollection3, UnityEngine.Random.Range(2f, 3f), checkOnLand: true);
					}
					for (int num17 = 0; num17 < UnityEngine.Random.Range(0, 2); num17++)
					{
						AddBillboardToBatch(baseData, billboardCollection6, UnityEngine.Random.Range(2f, 3f), checkOnLand: true);
					}
					AddFirefly(baseData, 0.05f, 10f, 50, 100);
				}
				if (num14 < stochastics.mapStyleChance[1])
				{
					for (int num18 = 0; num18 < UnityEngine.Random.Range(0, 1); num18++)
					{
						AddBillboardToBatch(baseData, billboardCollection6, UnityEngine.Random.Range(2f, 3f), checkOnLand: true);
					}
					AddFirefly(baseData, 0.025f, 15f, 100, 250);
				}
			}
			break;
		case 3:
		case 12:
		case 22:
			if (GetWeightedRecord(num) == "forest")
			{
				for (int i = 0; i < UnityEngine.Random.Range(0, 3); i++)
				{
					AddBillboardToBatch(baseData, billboardCollection7, UnityEngine.Random.Range(0.25f, 1f), checkOnLand: true);
				}
				if (height > 0.15f && UnityEngine.Random.Range(0f, 100f) < 5f)
				{
					if (UnityEngine.Random.Range(stochastics.mapStyleChance[3], stochastics.mapStyleChance[stochastics.mapStyleChance.Length - 1]) < stochastics.mapStyle)
					{
						AddBillboardToBatch(baseData, billboardCollection, UnityEngine.Random.Range(0.75f, 1.15f), checkOnLand: true);
					}
					else if (climate == 231)
					{
						for (int j = 0; j < UnityEngine.Random.Range(0, 3); j++)
						{
							AddBillboardToBatch(baseData, billboardCollection, UnityEngine.Random.Range(0.75f, 1.5f), checkOnLand: true, 3);
						}
					}
				}
			}
			if (GetWeightedRecord(num) == "flower")
			{
				for (int k = 0; k < UnityEngine.Random.Range(0, 2); k++)
				{
					AddBillboardToBatch(baseData, billboardCollection7, UnityEngine.Random.Range(0.25f, 0.5f), checkOnLand: true);
				}
			}
			break;
		}
	}

	private static bool TryImportGameObject(int archive, int record, bool clone, out GameObject go)
	{
		if (DaggerfallUnity.Settings.AssetInjection && ModManager.Instance != null)
		{
			Vector2Int item = new Vector2Int(archive, record);
			if (!triedBillboards.Contains(item))
			{
				if (ModManager.Instance.TryGetAsset<GameObject>(GetName(archive, record), clone, out go))
				{
					return true;
				}
				triedBillboards.Add(item);
			}
		}
		go = null;
		return false;
	}

	private static string GetName(int archive, int record)
	{
		return $"{archive:000}_{record}";
	}

	private static int GetTreePrototypeIndex(TerrainData terrainData, GameObject prefab)
	{
		TreePrototype[] treePrototypes = terrainData.treePrototypes;
		for (int i = 0; i < treePrototypes.Length; i++)
		{
			if (treePrototypes[i].prefab == prefab)
			{
				return i;
			}
		}
		List<TreePrototype> list = new List<TreePrototype>(treePrototypes);
		TreePrototype item = new TreePrototype
		{
			prefab = prefab,
			bendFactor = 1f
		};
		list.Add(item);
		terrainData.treePrototypes = list.ToArray();
		terrainData.RefreshPrototypes();
		return list.Count - 1;
	}

	public static bool ImportWONatureGameObject(int archive, int record, Terrain terrain, int x, int y)
	{
		if (!TryImportGameObject(archive, record, clone: false, out var go))
		{
			return false;
		}
		Vector3 vector = new Vector3((float)x / 127f, 0f, (float)y / 127f);
		if (LastTreePosition.HasValue && Mathf.Approximately(LastTreePosition.Value.x, vector.x) && Mathf.Approximately(LastTreePosition.Value.z, vector.z))
		{
			return true;
		}
		LastTreePosition = vector;
		UnityEngine.Random.State state = UnityEngine.Random.state;
		float num = getTreeScaleCallback();
		Color32 color = getTreeColorCallback();
		float rotation = UnityEngine.Random.Range(0f, 360f);
		int treePrototypeIndex = GetTreePrototypeIndex(terrain.terrainData, go);
		TreeInstance instance = new TreeInstance
		{
			heightScale = num,
			widthScale = num,
			color = color,
			lightmapColor = Color.white,
			position = vector,
			rotation = rotation,
			prototypeIndex = treePrototypeIndex
		};
		terrain.AddTreeInstance(instance);
		UnityEngine.Random.state = state;
		return true;
	}

	public void AddBillboardToBatch(ContainerObject baseData, List<int> billboardCollection, float posVariance, bool checkOnLand)
	{
        // Check if adding another billboard would exceed the maximum batch size
        //if (currentBillboardCount >= 16250)
        //{
            //Debug.Log("Wilderness Overhaul: Maximum batch size reached.");
            //return;
        //}

		int index = (int)Mathf.Round(UnityEngine.Random.Range(0, billboardCollection.Count));
		Vector3 vector = new Vector3(((float)baseData.x + UnityEngine.Random.Range(0f - posVariance, posVariance)) * baseData.scale, 0f, ((float)baseData.y + UnityEngine.Random.Range(0f - posVariance, posVariance)) * baseData.scale);
		float num = baseData.terrain.SampleHeight(vector + baseData.terrain.transform.position);
		vector.y = num - baseData.steepness / 70f;
		if (checkOnLand && !TileTypeCheck(vector, baseData, isOnAnyWaterTile: true, isOnPureGroundTile: false, isOnOrCloseToShallowWaterTile: false, isOnOrCloseToStreetTile: true) && TileTypeCheck(vector, baseData, isOnAnyWaterTile: false, isOnPureGroundTile: true, isOnOrCloseToShallowWaterTile: false, isOnOrCloseToStreetTile: false) && baseData.steepness < Mathf.Clamp(90f - num / baseData.maxTerrainHeight / 0.85f * 100f, 40f, 90f))
		{
			if (TerrainDistance > 1 || !ImportWONatureGameObject(baseData.dfBillboardBatch.TextureArchive, billboardCollection[index], baseData.terrain, baseData.x, baseData.y))
			{
				baseData.dfBillboardBatch.AddItem(billboardCollection[index], vector);
                //currentBillboardCount++;
			}
			else if (!NatureMeshUsed)
			{
				NatureMeshUsed = true;
			}
		}
		if (!checkOnLand && TileTypeCheck(vector, baseData, isOnAnyWaterTile: true, isOnPureGroundTile: false, isOnOrCloseToShallowWaterTile: false, isOnOrCloseToStreetTile: false) && !TileTypeCheck(vector, baseData, isOnAnyWaterTile: false, isOnPureGroundTile: false, isOnOrCloseToShallowWaterTile: false, isOnOrCloseToStreetTile: true) && baseData.steepness < Mathf.Clamp(90f - num / baseData.maxTerrainHeight / 0.85f * 100f, 40f, 90f))
		{
			if (TerrainDistance > 1 || !ImportWONatureGameObject(baseData.dfBillboardBatch.TextureArchive, billboardCollection[index], baseData.terrain, baseData.x, baseData.y))
			{
				baseData.dfBillboardBatch.AddItem(billboardCollection[index], vector);
                //currentBillboardCount++;
			}
			else if (!NatureMeshUsed)
			{
				NatureMeshUsed = true;
			}
		}
	}

	private static int GetTerrainDist(DFPosition mapPosition, int terrainMapPixelX, int terrainMapPixelY)
	{
		int a = Mathf.Abs(terrainMapPixelX - mapPosition.X);
		int b = Mathf.Abs(terrainMapPixelY - mapPosition.Y);
		return Mathf.Max(a, b);
	}

	public void AddBillboardToBatch(ContainerObject baseData, List<int> billboardCollection, float posVariance, bool checkOnLand, int record)
	{
        // Check if adding another billboard would exceed the maximum batch size
        //if (currentBillboardCount >= 16250)
        //{
            //Debug.Log("Wilderness Overhaul: Maximum batch size reached.");
        //    return;
        //}

		Vector3 vector = new Vector3(((float)baseData.x + UnityEngine.Random.Range(0f - posVariance, posVariance)) * baseData.scale, 0f, ((float)baseData.y + UnityEngine.Random.Range(0f - posVariance, posVariance)) * baseData.scale);
		float num = baseData.terrain.SampleHeight(vector + baseData.terrain.transform.position);
		vector.y = num - baseData.steepness / 70f;
		if (checkOnLand && !TileTypeCheck(vector, baseData, isOnAnyWaterTile: true, isOnPureGroundTile: false, isOnOrCloseToShallowWaterTile: false, isOnOrCloseToStreetTile: true) && TileTypeCheck(vector, baseData, isOnAnyWaterTile: false, isOnPureGroundTile: true, isOnOrCloseToShallowWaterTile: false, isOnOrCloseToStreetTile: false) && baseData.steepness < Mathf.Clamp(100f - num / baseData.maxTerrainHeight * 100f, 40f, 100f))
		{
			if (TerrainDistance > 1 || !ImportWONatureGameObject(baseData.dfBillboardBatch.TextureArchive, billboardCollection[record], baseData.terrain, baseData.x, baseData.y))
			{
				baseData.dfBillboardBatch.AddItem(billboardCollection[record], vector);
                //currentBillboardCount++;
			}
			else if (!NatureMeshUsed)
			{
				NatureMeshUsed = true;
			}
		}
		if (!checkOnLand && TileTypeCheck(vector, baseData, isOnAnyWaterTile: true, isOnPureGroundTile: false, isOnOrCloseToShallowWaterTile: false, isOnOrCloseToStreetTile: false) && !TileTypeCheck(vector, baseData, isOnAnyWaterTile: false, isOnPureGroundTile: false, isOnOrCloseToShallowWaterTile: false, isOnOrCloseToStreetTile: true) && baseData.steepness < Mathf.Clamp(100f - num / baseData.maxTerrainHeight * 100f, 40f, 100f))
		{
			if (TerrainDistance > 1 || !ImportWONatureGameObject(baseData.dfBillboardBatch.TextureArchive, billboardCollection[record], baseData.terrain, baseData.x, baseData.y))
			{
				baseData.dfBillboardBatch.AddItem(billboardCollection[record], vector);
                //currentBillboardCount++;
			}
			else if (!NatureMeshUsed)
			{
				NatureMeshUsed = true;
			}
		}
	}

	public static void AddMushroomRingToBatch(ContainerObject baseData, int record)
	{
		Vector3 vector = new Vector3((float)baseData.x * baseData.scale, 0f, ((float)baseData.y + 0.5f) * baseData.scale);
		float num = baseData.terrain.SampleHeight(vector + baseData.terrain.transform.position);
		vector.y = num - baseData.steepness / 70f;
		baseData.dfBillboardBatch.AddItem(record, vector);
		vector = new Vector3(((float)baseData.x + 0.272f) * baseData.scale, 0f, ((float)baseData.y - 0.404f) * baseData.scale);
		num = baseData.terrain.SampleHeight(vector + baseData.terrain.transform.position);
		vector.y = num - baseData.steepness / 70f;
		baseData.dfBillboardBatch.AddItem(record, vector);
		vector = new Vector3(((float)baseData.x - 0.272f) * baseData.scale, 0f, ((float)baseData.y - 0.404f) * baseData.scale);
		num = baseData.terrain.SampleHeight(vector + baseData.terrain.transform.position);
		vector.y = num - baseData.steepness / 70f;
		baseData.dfBillboardBatch.AddItem(record, vector);
		vector = new Vector3(((float)baseData.x - 0.475f) * baseData.scale, 0f, ((float)baseData.y + 0.154f) * baseData.scale);
		num = baseData.terrain.SampleHeight(vector + baseData.terrain.transform.position);
		vector.y = num - baseData.steepness / 70f;
		baseData.dfBillboardBatch.AddItem(record, vector);
		vector = new Vector3(((float)baseData.x + 0.475f) * baseData.scale, 0f, ((float)baseData.y + 0.154f) * baseData.scale);
		num = baseData.terrain.SampleHeight(vector + baseData.terrain.transform.position);
		vector.y = num - baseData.steepness / 70f;
		baseData.dfBillboardBatch.AddItem(record, vector);
	}

	public static void AddStoneCircleToBatch(ContainerObject baseData, List<int> billboardCollection, int record1, int record2)
	{
		Vector3 vector = new Vector3((float)baseData.x * baseData.scale, 0f, (float)baseData.y * baseData.scale);
		float num = baseData.terrain.SampleHeight(vector + baseData.terrain.transform.position);
		vector.y = num - baseData.steepness / 70f;
		baseData.dfBillboardBatch.AddItem(billboardCollection[record1], vector);
		vector = new Vector3(((float)baseData.x + 0.4f) * baseData.scale, 0f, (float)baseData.y * baseData.scale);
		num = baseData.terrain.SampleHeight(vector + baseData.terrain.transform.position);
		vector.y = num - baseData.steepness / 70f;
		baseData.dfBillboardBatch.AddItem(billboardCollection[record2], vector);
		vector = new Vector3(((float)baseData.x - 0.4f) * baseData.scale, 0f, (float)baseData.y * baseData.scale);
		num = baseData.terrain.SampleHeight(vector + baseData.terrain.transform.position);
		vector.y = num - baseData.steepness / 70f;
		baseData.dfBillboardBatch.AddItem(billboardCollection[record2], vector);
		vector = new Vector3((float)baseData.x * baseData.scale, 0f, ((float)baseData.y + 0.4f) * baseData.scale);
		num = baseData.terrain.SampleHeight(vector + baseData.terrain.transform.position);
		vector.y = num - baseData.steepness / 70f;
		baseData.dfBillboardBatch.AddItem(billboardCollection[record2], vector);
		vector = new Vector3((float)baseData.x * baseData.scale, 0f, ((float)baseData.y - 0.4f) * baseData.scale);
		num = baseData.terrain.SampleHeight(vector + baseData.terrain.transform.position);
		vector.y = num - baseData.steepness / 70f;
		baseData.dfBillboardBatch.AddItem(billboardCollection[record2], vector);
		vector = new Vector3(((float)baseData.x + 0.3f) * baseData.scale, 0f, ((float)baseData.y + 0.3f) * baseData.scale);
		num = baseData.terrain.SampleHeight(vector + baseData.terrain.transform.position);
		vector.y = num - baseData.steepness / 70f;
		baseData.dfBillboardBatch.AddItem(billboardCollection[record2], vector);
		vector = new Vector3(((float)baseData.x - 0.3f) * baseData.scale, 0f, ((float)baseData.y + 0.3f) * baseData.scale);
		num = baseData.terrain.SampleHeight(vector + baseData.terrain.transform.position);
		vector.y = num - baseData.steepness / 70f;
		baseData.dfBillboardBatch.AddItem(billboardCollection[record2], vector);
		vector = new Vector3(((float)baseData.x + 0.3f) * baseData.scale, 0f, ((float)baseData.y - 0.3f) * baseData.scale);
		num = baseData.terrain.SampleHeight(vector + baseData.terrain.transform.position);
		vector.y = num - baseData.steepness / 70f;
		baseData.dfBillboardBatch.AddItem(billboardCollection[record2], vector);
		vector = new Vector3(((float)baseData.x - 0.3f) * baseData.scale, 0f, ((float)baseData.y - 0.3f) * baseData.scale);
		num = baseData.terrain.SampleHeight(vector + baseData.terrain.transform.position);
		vector.y = num - baseData.steepness / 70f;
		baseData.dfBillboardBatch.AddItem(billboardCollection[record2], vector);
	}

	public static void AddFirefly(ContainerObject baseData, float rndFirefly, float distanceVariation, int minNumber, int maxNumber)
	{
		if (firefliesExist && rndFirefly >= UnityEngine.Random.Range(0f, 100f) && DaggerfallUnity.Instance.WorldTime.Now.SeasonValue != DaggerfallDateTime.Seasons.Winter)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "fireflyContainer";
			gameObject.transform.parent = baseData.dfBillboardBatch.transform;
			gameObject.transform.position = new Vector3(baseData.dfTerrain.transform.position.x + (float)baseData.x * baseData.scale, baseData.terrain.SampleHeight(new Vector3((float)baseData.x * baseData.scale, 0f, (float)baseData.y * baseData.scale) + baseData.dfTerrain.transform.position) + baseData.dfTerrain.transform.position.y, baseData.dfTerrain.transform.position.z + (float)baseData.y * baseData.scale);
			gameObject.AddComponent<WODistanceChecker>();
			gameObject.GetComponent<WODistanceChecker>().distance = fireflyDistance;
			for (int i = 0; i < UnityEngine.Random.Range(minNumber, maxNumber); i++)
			{
				gameObject.GetComponent<WODistanceChecker>().CreateFirefly(mod.GetAsset<GameObject>("Firefly", clone: true), baseData.dfTerrain, baseData.x, baseData.y, baseData.scale, baseData.terrain, distanceVariation);
			}
			gameObject.GetComponent<WODistanceChecker>().AddChildrenToArray();
			gameObject.GetComponent<WODistanceChecker>().DeactivateAllChildren();
		}
	}

	public static void AddShootingStar(DaggerfallTerrain dfTerrain, DaggerfallBillboardBatch dfBillboardBatch, float rotationAngleX, float heightInTheSky, float sSMin, float sSMax)
	{
		Vector3 vector = new Vector3(dfTerrain.transform.position.x, dfTerrain.transform.position.y, dfTerrain.transform.position.z);
		GameObject asset = mod.GetAsset<GameObject>("ShootingStars", clone: true);
		asset.transform.position = new Vector3(vector.x, vector.y + heightInTheSky, vector.z);
		asset.transform.parent = dfBillboardBatch.transform;
		asset.transform.rotation = Quaternion.Euler(rotationAngleX, 0f, 0f);
		asset.AddComponent<WOShootingStarController>();
		asset.GetComponent<WOShootingStarController>().ps = asset.GetComponent<ParticleSystem>();
		ParticleSystem.EmissionModule emission = asset.GetComponent<WOShootingStarController>().ps.emission;
		emission.rateOverTime = new ParticleSystem.MinMaxCurve(sSMin / 1000f, sSMax / 1000f);
	}

	public static bool TileTypeCheck(Vector3 pos, ContainerObject baseData, bool isOnAnyWaterTile, bool isOnPureGroundTile, bool isOnOrCloseToShallowWaterTile, bool isOnOrCloseToStreetTile)
	{
		bool result = true;
		bool flag = false;
		if (isOnAnyWaterTile)
		{
			int num = (int)Mathf.Round(pos.x / baseData.scale);
			int num2 = (int)Mathf.Round(pos.z / baseData.scale);
			if (baseData.dfTerrain.MapData.tilemapSamples.In2DArrayBounds(num, num2))
			{
				int num3 = baseData.dfTerrain.MapData.tilemapSamples[num, num2] & 0x3F;
				if (num3 != 0 && num3 != 4 && num3 != 5 && num3 != 6 && num3 != 7 && num3 != 8 && num3 != 19 && num3 != 20 && num3 != 21 && num3 != 22 && num3 != 23 && num3 != 29 && num3 != 30 && num3 != 31 && num3 != 32 && num3 != 33 && num3 != 34 && num3 != 35 && num3 != 36 && num3 != 37 && num3 != 38 && num3 != 40 && num3 != 41 && num3 != 43 && num3 != 44 && num3 != 48 && num3 != 49 && num3 != 50 && num3 != 60 && num3 != 61)
				{
					return false;
				}
			}
		}
		if (isOnPureGroundTile)
		{
			int num = (int)Mathf.Round(pos.x / baseData.scale);
			int num2 = (int)Mathf.Round(pos.z / baseData.scale);
			if (baseData.dfTerrain.MapData.tilemapSamples.In2DArrayBounds(num, num2))
			{
				int num3 = baseData.dfTerrain.MapData.tilemapSamples[num, num2] & 0x3F;
				if (num3 != 1 && num3 != 2 && num3 != 3)
				{
					return false;
				}
			}
		}
		if (isOnOrCloseToShallowWaterTile)
		{
			for (int i = 0; i < 2; i++)
			{
				if (flag)
				{
					break;
				}
				for (int j = 0; j < 2; j++)
				{
					if (flag)
					{
						break;
					}
					float num4 = 1f;
					float num5 = 1f;
					int num = (int)Mathf.Round(pos.x / baseData.scale + ((i == 1) ? num4 : num5));
					int num2 = (int)Mathf.Round(pos.z / baseData.scale + ((j == 1) ? num4 : num5));
					if (baseData.dfTerrain.MapData.tilemapSamples.In2DArrayBounds(num - i, num2 - j))
					{
						int num3 = baseData.dfTerrain.MapData.tilemapSamples[num - i, num2 - j] & 0x3F;
						if (num3 != 4 && num3 != 5 && num3 != 6 && num3 != 7 && num3 != 8 && num3 != 19 && num3 != 20 && num3 != 21 && num3 != 22 && num3 != 23 && num3 != 29 && num3 != 30 && num3 != 31 && num3 != 32 && num3 != 33 && num3 != 34 && num3 != 35 && num3 != 36 && num3 != 37 && num3 != 38 && num3 != 40 && num3 != 41 && num3 != 43 && num3 != 44 && num3 != 48 && num3 != 49 && num3 != 50 && num3 != 60 && num3 != 61)
						{
							result = false;
							return result;
						}
						flag = true;
					}
				}
			}
		}
		if (isOnOrCloseToStreetTile)
		{
			for (int k = 0; k < 2; k++)
			{
				if (flag)
				{
					break;
				}
				for (int l = 0; l < 2; l++)
				{
					if (flag)
					{
						break;
					}
					float num4 = 0.7f;
					float num5 = -0.3f;
					int num = (int)Mathf.Round(pos.x / baseData.scale + ((k == 1) ? num4 : num5));
					int num2 = (int)Mathf.Round(pos.z / baseData.scale + ((l == 1) ? num4 : num5));
					if (baseData.dfTerrain.MapData.tilemapSamples.In2DArrayBounds(num - k, num2 - l))
					{
						int num3 = baseData.dfTerrain.MapData.tilemapSamples[num - k, num2 - l] & 0x3F;
						if (num3 != 46 && num3 != 47 && num3 != 55)
						{
							result = false;
							return result;
						}
						flag = true;
					}
				}
			}
		}
		return result;
	}

	private float GetNoise(float x, float y, float frequency, float amplitude, float persistance, int octaves, int seed = 120)
	{
		float num = 0f;
		for (int i = 0; i < octaves; i++)
		{
			num += Mathf.PerlinNoise((float)seed + x * frequency, (float)seed + y * frequency) * amplitude;
			frequency *= 7f;
			amplitude *= persistance;
		}
		return Mathf.Clamp(num, -1f, 1f);
	}

	private string GetWeightedRecord(float weight, float limit1 = 0.3f, float limit2 = 0.6f)
	{
		if (weight < limit1)
		{
			return "flower";
		}
		if (weight >= limit1 && weight < limit2)
		{
			return "grass";
		}
		return "forest";
	}
}
