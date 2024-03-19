// Warning: Some assembly references could not be resolved automatically. This might lead to incorrect decompilation of some parts,
// for ex. property getter/setter access. To get optimal decompilation results, please manually add the missing references to the list of loaded assemblies.
// WildernessOverhaul.WOVegetationList
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WOVegetationList
{
	public List<int> temperateWoodlandFlowers;

	public List<int> temperateWoodlandMushroom;

	public List<int> temperateWoodlandBushes;

	public List<int> temperateWoodlandRocks;

	public List<int> temperateWoodlandTrees;

	public List<int> temperateWoodlandDeadTrees;

	public List<int> temperateWoodlandBeach;

	public List<int> woodlandHillsFlowers;

	public List<int> woodlandHillsBushes;

	public List<int> woodlandHillsRocks;

	public List<int> woodlandHillsTrees;

	public List<int> woodlandHillsNeedleTrees;

	public List<int> woodlandHillsDeadTrees;

	public List<int> woodlandHillsDirtPlants;

	public List<int> woodlandHillsBeach;

	public List<int> mountainsFlowers;

	public List<int> mountainsGrass;

	public List<int> mountainsRocks;

	public List<int> mountainsTrees;

	public List<int> mountainsNeedleTrees;

	public List<int> mountainsDeadTrees;

	public List<int> mountainsBeach;

	public List<int> desertFlowers;

	public List<int> desertWaterFlowers;

	public List<int> desertPlants;

	public List<int> desertWaterPlants;

	public List<int> desertStones;

	public List<int> desertTrees;

	public List<int> desertCactus;

	public List<int> desertDeadTrees;

	public List<int> hauntedWoodlandFlowers;

	public List<int> hauntedWoodlandMushroom;

	public List<int> hauntedWoodlandBones;

	public List<int> hauntedWoodlandPlants;

	public List<int> hauntedWoodlandBushes;

	public List<int> hauntedWoodlandRocks;

	public List<int> hauntedWoodlandTrees;

	public List<int> hauntedWoodlandDirtTrees;

	public List<int> hauntedWoodlandDeadTrees;

	public List<int> hauntedWoodlandBeach;

	public List<int> rainforestFlowers;

	public List<int> rainforestEggs;

	public List<int> rainforestPlants;

	public List<int> rainforestBushes;

	public List<int> rainforestRocks;

	public List<int> rainforestTrees;

	public List<int> rainforestBeach;

	public List<int> subtropicalFlowers;

	public List<int> subtropicalMushroom;

	public List<int> subtropicalPlants;

	public List<int> subtropicalBulbs;

	public List<int> subtropicalRocks;

	public List<int> subtropicalTrees;

	public List<int> subtropicalDeadTrees;

	public List<int> subtropicalBeach;

	public List<int> swampFlowers;

	public List<int> swampEggs;

	public List<int> swampPlants;

	public List<int> swampBulbs;

	public List<int> swampRocks;

	public List<int> swampTrees;

	public List<int> swampDeadTrees;

	public List<int> swampBeach;

	public List<int> collectionTemperateWoodlandFlowers = new List<int>(new int[3] { 2, 21, 22 });

	public List<int> collectionTemperateWoodlandMushroom = new List<int>(new int[3] { 7, 9, 23 });

	public List<int> collectionTemperateWoodlandBushes = new List<int>(new int[3] { 1, 27, 28 });

	public List<int> collectionTemperateWoodlandRocks = new List<int>(new int[7] { 3, 4, 5, 6, 8, 10, 26 });

	public List<int> collectionTemperateWoodlandTrees = new List<int>(new int[8] { 11, 12, 13, 14, 15, 16, 17, 18 });

	public List<int> collectionTemperateWoodlandDeadTrees = new List<int>(new int[7] { 19, 20, 24, 25, 29, 30, 31 });

	public List<int> collectionTemperateWoodlandBeach = new List<int>(new int[9] { 31, 3, 3, 4, 4, 5, 5, 6, 29 });

	public List<int> collectionWoodlandHillsFlowers = new List<int>(new int[4] { 2, 7, 21, 22 });

	public List<int> collectionWoodlandHillsBushes = new List<int>(new int[3] { 9, 27, 31 });

	public List<int> collectionWoodlandHillsRocks = new List<int>(new int[9] { 1, 3, 4, 6, 8, 10, 17, 18, 28 });

	public List<int> collectionWoodlandHillsTrees = new List<int>(new int[19]
	{
		5, 5, 11, 11, 12, 13, 13, 13, 14, 14,
		14, 15, 15, 15, 16, 16, 16, 25, 30
	});

	public List<int> collectionWoodlandHillsNeedleTrees = new List<int>(new int[5] { 5, 11, 12, 25, 30 });

	public List<int> collectionWoodlandHillsDeadTrees = new List<int>(new int[3] { 19, 20, 24 });

	public List<int> collectionWoodlandHillsDirtPlants = new List<int>(new int[3] { 23, 26, 29 });

	public List<int> collectionWoodlandHillsBeach = new List<int>(new int[3] { 26, 29, 31 });

	public List<int> collectionMountainsFlowers = new List<int>(new int[1] { 22 });

	public List<int> collectionMountainsGrass = new List<int>(new int[4] { 2, 7, 9, 23 });

	public List<int> collectionMountainsRocks = new List<int>(new int[12]
	{
		1, 3, 4, 6, 8, 10, 14, 17, 18, 27,
		28, 31
	});

	public List<int> collectionMountainsTrees = new List<int>(new int[8] { 5, 11, 12, 13, 15, 21, 25, 30 });

	public List<int> collectionMountainsNeedleTrees = new List<int>(new int[8] { 5, 11, 12, 25, 30, 12, 25, 30 });

	public List<int> collectionMountainsDeadTrees = new List<int>(new int[6] { 16, 19, 20, 24, 26, 29 });

	public List<int> collectionMountainsBeach = new List<int>(new int[9] { 8, 26, 29, 2, 7, 7, 7, 23, 23 });

	public List<int> collectionDesertFlowers = new List<int>(new int[12]
	{
		9, 9, 9, 17, 24, 24, 24, 26, 26, 31,
		31, 31
	});

	public List<int> collectionDesertWaterFlowers = new List<int>(new int[6] { 7, 17, 24, 24, 29, 29 });

	public List<int> collectionDesertPlants = new List<int>(new int[3] { 7, 25, 27 });

	public List<int> collectionDesertWaterPlants = new List<int>(new int[11]
	{
		7, 7, 7, 7, 7, 25, 27, 27, 27, 27,
		27
	});

	public List<int> collectionDesertStones = new List<int>(new int[10] { 2, 3, 4, 6, 8, 18, 19, 20, 21, 22 });

	public List<int> collectionDesertTrees = new List<int>(new int[3] { 5, 13, 13 });

	public List<int> collectionDesertCactus = new List<int>(new int[4] { 1, 14, 15, 16 });

	public List<int> collectionDesertDeadTrees = new List<int>(new int[5] { 10, 11, 12, 23, 28 });

	public List<int> collectionHauntedWoodlandFlowers = new List<int>(new int[1] { 21 });

	public List<int> collectionHauntedWoodlandMushroom = new List<int>(new int[2] { 22, 23 });

	public List<int> collectionHauntedWoodlandBones = new List<int>(new int[1] { 11 });

	public List<int> collectionHauntedWoodlandPlants = new List<int>(new int[5] { 7, 14, 17, 29, 31 });

	public List<int> collectionHauntedWoodlandBushes = new List<int>(new int[4] { 2, 26, 27, 28 });

	public List<int> collectionHauntedWoodlandRocks = new List<int>(new int[8] { 1, 3, 4, 5, 6, 8, 10, 12 });

	public List<int> collectionHauntedWoodlandTrees = new List<int>(new int[6] { 13, 13, 13, 15, 15, 15 });

	public List<int> collectionHauntedWoodlandDirtTrees = new List<int>(new int[4] { 18, 19, 20, 31 });

	public List<int> collectionHauntedWoodlandDeadTrees = new List<int>(new int[6] { 16, 18, 24, 25, 30, 31 });

	public List<int> collectionHauntedWoodlandBeach = new List<int>(new int[6] { 31, 8, 9, 14, 29, 31 });

	public List<int> collectionRainforestFlowers = new List<int>(new int[6] { 6, 20, 21, 22, 26, 27 });

	public List<int> collectionRainforestEggs = new List<int>(new int[3] { 28, 29, 31 });

	public List<int> collectionRainforestPlants = new List<int>(new int[6] { 2, 5, 10, 11, 23, 24 });

	public List<int> collectionRainforestBushes = new List<int>(new int[4] { 3, 9, 16, 18 });

	public List<int> collectionRainforestRocks = new List<int>(new int[5] { 1, 4, 17, 19, 25 });

	public List<int> collectionRainforestTrees = new List<int>(new int[5] { 12, 13, 14, 15, 30 });

	public List<int> collectionRainforestBeach = new List<int>(new int[4] { 31, 11, 29, 17 });

	public List<int> collectionSubTropicalFlowers = new List<int>(new int[2] { 25, 26 });

	public List<int> collectionSubTropicalPlants = new List<int>(new int[6] { 7, 8, 9, 28, 29, 31 });

	public List<int> collectionSubTropicalMushroom = new List<int>(new int[1] { 22 });

	public List<int> collectionSubTropicalBulbs = new List<int>(new int[4] { 2, 14, 18, 21 });

	public List<int> collectionSubTropicalRocks = new List<int>(new int[6] { 3, 4, 5, 6, 10, 23 });

	public List<int> collectionSubTropicalTrees = new List<int>(new int[8] { 11, 12, 13, 15, 16, 17, 27, 30 });

	public List<int> collectionSubTropicalDeadTrees = new List<int>(new int[2] { 19, 24 });

	public List<int> collectionSubTropicalBeach = new List<int>(new int[2] { 29, 10 });

	public List<int> collectionSwampFlowers = new List<int>(new int[6] { 14, 21, 23, 26, 31, 1 });

	public List<int> collectionSwampPlants = new List<int>(new int[8] { 7, 8, 9, 20, 26, 27, 28, 29 });

	public List<int> collectionSwampEggs = new List<int>(new int[2] { 2, 22 });

	public List<int> collectionSwampRocks = new List<int>(new int[6] { 3, 4, 5, 6, 10, 11 });

	public List<int> collectionSwampTrees = new List<int>(new int[5] { 12, 13, 16, 17, 18 });

	public List<int> collectionSwampDeadTrees = new List<int>(new int[5] { 15, 19, 24, 25, 30 });

	public List<int> collectionSwampBeach = new List<int>(new int[5] { 22, 5, 5, 28, 28 });

	public List<float> elevationLevels = new List<float>(new float[14]
	{
		Random.Range(0.66f, 0.64f),
		Random.Range(0.61f, 0.59f),
		Random.Range(0.56f, 0.54f),
		Random.Range(0.51f, 0.49f),
		Random.Range(0.46f, 0.44f),
		Random.Range(0.41f, 0.39f),
		Random.Range(0.36f, 0.34f),
		Random.Range(0.31f, 0.29f),
		Random.Range(0.26f, 0.24f),
		Random.Range(0.21f, 0.19f),
		Random.Range(0.11f, 0.09f),
		Random.Range(0.096f, 0.84f),
		Random.Range(0.071f, 0.069f),
		Random.Range(0.041f, 0.039f)
	});

	public List<int> temperateWoodlandFlowersAtElevation0 = new List<int>(new int[2] { 21, 22 });

	public List<int> temperateWoodlandMushroomAtElevation0 = new List<int>(new int[2] { 7, 23 });

	public List<int> temperateWoodlandBushesAtElevation0 = new List<int>(new int[5] { 1, 1, 1, 27, 28 });

	public List<int> temperateWoodlandRocksAtElevation0 = new List<int>(new int[7] { 3, 4, 5, 6, 8, 10, 26 });

	public List<int> temperateWoodlandTreesAtElevation0 = new List<int>(new int[8] { 13, 14, 15, 17, 18, 25, 25, 25 });

	public List<int> temperateWoodlandBeachAtElevation0 = new List<int>(new int[9] { 31, 3, 3, 4, 4, 5, 5, 6, 29 });

	public List<int> temperateWoodlandDeadTreesAtElevation0 = new List<int>(new int[9] { 19, 20, 24, 25, 25, 25, 29, 30, 31 });

	public List<int> temperateWoodlandTreesAtElevation1 = new List<int>(new int[8] { 13, 14, 15, 17, 18, 25, 25, 25 });

	public List<int> temperateWoodlandDeadTreesAtElevation1 = new List<int>(new int[9] { 19, 20, 24, 25, 25, 25, 29, 30, 31 });

	public List<int> temperateWoodlandFlowersAtElevation1 = new List<int>(new int[2] { 21, 22 });

	public List<int> temperateWoodlandMushroomAtElevation1 = new List<int>(new int[2] { 7, 23 });

	public List<int> temperateWoodlandBushesAtElevation1 = new List<int>(new int[5] { 1, 1, 1, 27, 28 });

	public List<int> temperateWoodlandBeachAtElevation1 = new List<int>(new int[9] { 31, 3, 3, 4, 4, 5, 5, 6, 29 });

	public List<int> temperateWoodlandRocksAtElevation1 = new List<int>(new int[7] { 3, 4, 5, 6, 8, 10, 26 });

	public List<int> temperateWoodlandTreesAtElevation2 = new List<int>(new int[8] { 13, 14, 15, 17, 18, 25, 25, 25 });

	public List<int> temperateWoodlandDeadTreesAtElevation2 = new List<int>(new int[9] { 19, 20, 24, 25, 25, 25, 29, 30, 31 });

	public List<int> temperateWoodlandFlowersAtElevation2 = new List<int>(new int[2] { 21, 22 });

	public List<int> temperateWoodlandMushroomAtElevation2 = new List<int>(new int[2] { 7, 23 });

	public List<int> temperateWoodlandBushesAtElevation2 = new List<int>(new int[5] { 1, 1, 1, 27, 28 });

	public List<int> temperateWoodlandBeachAtElevation2 = new List<int>(new int[9] { 31, 3, 3, 4, 4, 5, 5, 6, 29 });

	public List<int> temperateWoodlandRocksAtElevation2 = new List<int>(new int[7] { 3, 4, 5, 6, 8, 10, 26 });

	public List<int> temperateWoodlandTreesAtElevation3 = new List<int>(new int[8] { 13, 14, 15, 17, 18, 25, 25, 25 });

	public List<int> temperateWoodlandDeadTreesAtElevation3 = new List<int>(new int[9] { 19, 20, 24, 25, 25, 25, 29, 30, 31 });

	public List<int> temperateWoodlandFlowersAtElevation3 = new List<int>(new int[2] { 21, 22 });

	public List<int> temperateWoodlandMushroomAtElevation3 = new List<int>(new int[2] { 7, 23 });

	public List<int> temperateWoodlandBushesAtElevation3 = new List<int>(new int[5] { 1, 1, 1, 27, 28 });

	public List<int> temperateWoodlandBeachAtElevation3 = new List<int>(new int[9] { 31, 3, 3, 4, 4, 5, 5, 6, 29 });

	public List<int> temperateWoodlandRocksAtElevation3 = new List<int>(new int[7] { 3, 4, 5, 6, 8, 10, 26 });

	public List<int> temperateWoodlandTreesAtElevation4 = new List<int>(new int[8] { 13, 14, 15, 17, 18, 25, 25, 25 });

	public List<int> temperateWoodlandDeadTreesAtElevation4 = new List<int>(new int[9] { 19, 20, 24, 25, 25, 25, 29, 30, 31 });

	public List<int> temperateWoodlandFlowersAtElevation4 = new List<int>(new int[2] { 21, 22 });

	public List<int> temperateWoodlandMushroomAtElevation4 = new List<int>(new int[2] { 7, 23 });

	public List<int> temperateWoodlandBushesAtElevation4 = new List<int>(new int[5] { 1, 1, 1, 27, 28 });

	public List<int> temperateWoodlandBeachAtElevation4 = new List<int>(new int[9] { 31, 3, 3, 4, 4, 5, 5, 6, 29 });

	public List<int> temperateWoodlandRocksAtElevation4 = new List<int>(new int[7] { 3, 4, 5, 6, 8, 10, 26 });

	public List<int> temperateWoodlandTreesAtElevation5 = new List<int>(new int[8] { 13, 14, 15, 17, 18, 25, 25, 25 });

	public List<int> temperateWoodlandDeadTreesAtElevation5 = new List<int>(new int[9] { 19, 20, 24, 25, 25, 25, 29, 30, 31 });

	public List<int> temperateWoodlandFlowersAtElevation5 = new List<int>(new int[2] { 21, 22 });

	public List<int> temperateWoodlandMushroomAtElevation5 = new List<int>(new int[2] { 7, 23 });

	public List<int> temperateWoodlandBushesAtElevation5 = new List<int>(new int[5] { 1, 1, 1, 27, 28 });

	public List<int> temperateWoodlandBeachAtElevation5 = new List<int>(new int[9] { 31, 3, 3, 4, 4, 5, 5, 6, 29 });

	public List<int> temperateWoodlandRocksAtElevation5 = new List<int>(new int[7] { 3, 4, 5, 6, 8, 10, 26 });

	public List<int> temperateWoodlandTreesAtElevation6 = new List<int>(new int[8] { 13, 14, 15, 17, 18, 25, 25, 25 });

	public List<int> temperateWoodlandDeadTreesAtElevation6 = new List<int>(new int[9] { 19, 20, 24, 25, 25, 25, 29, 30, 31 });

	public List<int> temperateWoodlandFlowersAtElevation6 = new List<int>(new int[2] { 21, 22 });

	public List<int> temperateWoodlandMushroomAtElevation6 = new List<int>(new int[2] { 7, 23 });

	public List<int> temperateWoodlandBushesAtElevation6 = new List<int>(new int[5] { 1, 1, 1, 27, 28 });

	public List<int> temperateWoodlandBeachAtElevation6 = new List<int>(new int[9] { 31, 3, 3, 4, 4, 5, 5, 6, 29 });

	public List<int> temperateWoodlandRocksAtElevation6 = new List<int>(new int[7] { 3, 4, 5, 6, 8, 10, 26 });

	public List<int> temperateWoodlandTreesAtElevation7 = new List<int>(new int[8] { 13, 14, 15, 17, 18, 25, 25, 25 });

	public List<int> temperateWoodlandDeadTreesAtElevation7 = new List<int>(new int[9] { 19, 20, 24, 25, 25, 25, 29, 30, 31 });

	public List<int> temperateWoodlandFlowersAtElevation7 = new List<int>(new int[2] { 21, 22 });

	public List<int> temperateWoodlandMushroomAtElevation7 = new List<int>(new int[2] { 7, 23 });

	public List<int> temperateWoodlandBushesAtElevation7 = new List<int>(new int[5] { 1, 1, 1, 27, 28 });

	public List<int> temperateWoodlandBeachAtElevation7 = new List<int>(new int[9] { 31, 3, 3, 4, 4, 5, 5, 6, 29 });

	public List<int> temperateWoodlandRocksAtElevation7 = new List<int>(new int[7] { 3, 4, 5, 6, 8, 10, 26 });

	public List<int> temperateWoodlandTreesAtElevation8 = new List<int>(new int[8] { 13, 14, 15, 17, 18, 25, 25, 25 });

	public List<int> temperateWoodlandDeadTreesAtElevation8 = new List<int>(new int[9] { 19, 20, 24, 25, 25, 25, 29, 30, 31 });

	public List<int> temperateWoodlandFlowersAtElevation8 = new List<int>(new int[2] { 21, 22 });

	public List<int> temperateWoodlandMushroomAtElevation8 = new List<int>(new int[2] { 7, 23 });

	public List<int> temperateWoodlandBushesAtElevation8 = new List<int>(new int[5] { 1, 1, 1, 27, 28 });

	public List<int> temperateWoodlandBeachAtElevation8 = new List<int>(new int[9] { 31, 3, 3, 4, 4, 5, 5, 6, 29 });

	public List<int> temperateWoodlandRocksAtElevation8 = new List<int>(new int[7] { 3, 4, 5, 6, 8, 10, 26 });

	public List<int> temperateWoodlandTreesAtElevation9A = new List<int>(new int[13]
	{
		25, 25, 25, 25, 25, 20, 19, 26, 26, 30,
		1, 1, 1
	});

	public List<int> temperateWoodlandTreesAtElevation9B = new List<int>(new int[8] { 11, 13, 13, 14, 15, 16, 17, 1 });

	public List<int> temperateWoodlandTreesAtElevation9C = new List<int>(new int[11]
	{
		11, 13, 14, 15, 16, 17, 17, 18, 18, 18,
		1
	});

	public List<int> temperateWoodlandTreesAtElevation9D = new List<int>(new int[8] { 13, 13, 25, 25, 25, 1, 1, 1 });

	public List<int> temperateWoodlandTreesAtElevation9E = new List<int>(new int[8] { 11, 14, 14, 14, 15, 15, 24, 1 });

	public List<int> temperateWoodlandTreesAtElevation9F = new List<int>(new int[9] { 11, 13, 13, 13, 14, 15, 25, 25, 1 });

	public List<int> temperateWoodlandDeadTreesAtElevation9 = new List<int>(new int[9] { 19, 20, 24, 25, 25, 25, 29, 30, 31 });

	public List<int> temperateWoodlandFlowersAtElevation9 = new List<int>(new int[2] { 21, 22 });

	public List<int> temperateWoodlandMushroomAtElevation9 = new List<int>(new int[2] { 7, 23 });

	public List<int> temperateWoodlandBushesAtElevation9 = new List<int>(new int[4] { 1, 1, 27, 28 });

	public List<int> temperateWoodlandBeachAtElevation9 = new List<int>(new int[9] { 31, 3, 3, 4, 4, 5, 5, 6, 29 });

	public List<int> temperateWoodlandRocksAtElevation9 = new List<int>(new int[7] { 3, 4, 5, 6, 8, 10, 26 });

	public List<int> temperateWoodlandTreesAtElevation10A = new List<int>(new int[13]
	{
		25, 25, 25, 25, 25, 20, 19, 26, 26, 30,
		1, 1, 1
	});

	public List<int> temperateWoodlandTreesAtElevation10B = new List<int>(new int[8] { 11, 13, 13, 14, 15, 16, 17, 1 });

	public List<int> temperateWoodlandTreesAtElevation10C = new List<int>(new int[11]
	{
		11, 13, 14, 15, 16, 17, 17, 18, 18, 18,
		1
	});

	public List<int> temperateWoodlandTreesAtElevation10D = new List<int>(new int[11]
	{
		11, 13, 14, 15, 16, 16, 17, 17, 18, 18,
		1
	});

	public List<int> temperateWoodlandTreesAtElevation10E = new List<int>(new int[8] { 11, 14, 14, 14, 15, 15, 24, 1 });

	public List<int> temperateWoodlandTreesAtElevation10F = new List<int>(new int[9] { 11, 13, 13, 13, 14, 15, 25, 25, 1 });

	public List<int> temperateWoodlandDeadTreesAtElevation10 = new List<int>(new int[10] { 19, 20, 24, 25, 25, 25, 25, 29, 30, 31 });

	public List<int> temperateWoodlandFlowersAtElevation10 = new List<int>(new int[2] { 21, 22 });

	public List<int> temperateWoodlandMushroomAtElevation10 = new List<int>(new int[2] { 7, 23 });

	public List<int> temperateWoodlandBushesAtElevation10 = new List<int>(new int[4] { 1, 1, 27, 28 });

	public List<int> temperateWoodlandBeachAtElevation10 = new List<int>(new int[9] { 31, 3, 3, 4, 4, 5, 5, 6, 29 });

	public List<int> temperateWoodlandRocksAtElevation10 = new List<int>(new int[7] { 3, 4, 5, 6, 8, 10, 26 });

	public List<int> temperateWoodlandTreesAtElevation11A = new List<int>(new int[14]
	{
		11, 13, 14, 15, 25, 25, 25, 25, 25, 20,
		19, 26, 26, 1
	});

	public List<int> temperateWoodlandTreesAtElevation11B = new List<int>(new int[12]
	{
		11, 12, 13, 13, 14, 15, 16, 17, 18, 26,
		26, 1
	});

	public List<int> temperateWoodlandTreesAtElevation11C = new List<int>(new int[13]
	{
		11, 13, 14, 15, 16, 16, 17, 17, 18, 18,
		26, 26, 1
	});

	public List<int> temperateWoodlandTreesAtElevation11D = new List<int>(new int[12]
	{
		11, 12, 13, 14, 15, 16, 17, 18, 18, 26,
		26, 1
	});

	public List<int> temperateWoodlandTreesAtElevation11E = new List<int>(new int[15]
	{
		11, 13, 14, 14, 14, 15, 15, 15, 16, 17,
		18, 24, 26, 26, 1
	});

	public List<int> temperateWoodlandTreesAtElevation11F = new List<int>(new int[15]
	{
		11, 13, 13, 14, 15, 16, 17, 17, 18, 18,
		18, 18, 26, 26, 1
	});

	public List<int> temperateWoodlandDeadTreesAtElevation11 = new List<int>(new int[8] { 19, 20, 24, 25, 25, 29, 30, 31 });

	public List<int> temperateWoodlandFlowersAtElevation11 = new List<int>(new int[2] { 21, 22 });

	public List<int> temperateWoodlandMushroomAtElevation11 = new List<int>(new int[2] { 7, 23 });

	public List<int> temperateWoodlandBushesAtElevation11 = new List<int>(new int[6] { 1, 26, 27, 27, 28, 28 });

	public List<int> temperateWoodlandBeachAtElevation11 = new List<int>(new int[9] { 31, 3, 3, 4, 4, 5, 5, 6, 29 });

	public List<int> temperateWoodlandRocksAtElevation11 = new List<int>(new int[7] { 3, 4, 5, 6, 8, 10, 26 });

	public List<int> temperateWoodlandTreesAtElevation12A = new List<int>(new int[8] { 11, 12, 13, 14, 15, 16, 17, 18 });

	public List<int> temperateWoodlandTreesAtElevation12B = new List<int>(new int[12]
	{
		11, 12, 12, 12, 13, 16, 16, 16, 17, 17,
		17, 18
	});

	public List<int> temperateWoodlandTreesAtElevation12C = new List<int>(new int[12]
	{
		11, 12, 12, 12, 13, 16, 16, 16, 17, 18,
		18, 18
	});

	public List<int> temperateWoodlandTreesAtElevation12D = new List<int>(new int[13]
	{
		11, 13, 14, 14, 14, 14, 15, 15, 15, 16,
		17, 18, 24
	});

	public List<int> temperateWoodlandTreesAtElevation12E = new List<int>(new int[13]
	{
		11, 13, 14, 14, 14, 15, 15, 15, 15, 16,
		17, 18, 24
	});

	public List<int> temperateWoodlandTreesAtElevation12F = new List<int>(new int[11]
	{
		11, 12, 13, 14, 15, 16, 16, 17, 17, 18,
		18
	});

	public List<int> temperateWoodlandDeadTreesAtElevation12 = new List<int>(new int[7] { 19, 20, 24, 25, 29, 30, 31 });

	public List<int> temperateWoodlandFlowersAtElevation12 = new List<int>(new int[6] { 2, 21, 21, 21, 22, 22 });

	public List<int> temperateWoodlandMushroomAtElevation12 = new List<int>(new int[6] { 7, 7, 7, 9, 23, 23 });

	public List<int> temperateWoodlandBushesAtElevation12 = new List<int>(new int[6] { 1, 26, 27, 27, 28, 28 });

	public List<int> temperateWoodlandBeachAtElevation12 = new List<int>(new int[9] { 31, 3, 3, 4, 4, 5, 5, 6, 29 });

	public List<int> temperateWoodlandRocksAtElevation12 = new List<int>(new int[7] { 3, 4, 5, 6, 8, 10, 26 });

	public List<int> temperateWoodlandTreesAtElevation13A = new List<int>(new int[8] { 11, 11, 14, 14, 15, 15, 15, 24 });

	public List<int> temperateWoodlandTreesAtElevation13B = new List<int>(new int[7] { 13, 13, 18, 18, 18, 27, 28 });

	public List<int> temperateWoodlandTreesAtElevation13C = new List<int>(new int[7] { 11, 12, 13, 14, 15, 16, 18 });

	public List<int> temperateWoodlandTreesAtElevation13D = new List<int>(new int[9] { 12, 12, 12, 16, 17, 27, 27, 28, 28 });

	public List<int> temperateWoodlandTreesAtElevation13E = new List<int>(new int[9] { 12, 12, 16, 16, 18, 27, 27, 28, 28 });

	public List<int> temperateWoodlandTreesAtElevation13F = new List<int>(new int[9] { 12, 12, 17, 17, 18, 27, 27, 28, 28 });

	public List<int> temperateWoodlandDeadTreesAtElevation13 = new List<int>(new int[6] { 19, 20, 24, 29, 30, 31 });

	public List<int> temperateWoodlandFlowersAtElevation13 = new List<int>(new int[3] { 2, 21, 22 });

	public List<int> temperateWoodlandMushroomAtElevation13 = new List<int>(new int[3] { 7, 9, 23 });

	public List<int> temperateWoodlandBushesAtElevation13 = new List<int>(new int[3] { 26, 27, 28 });

	public List<int> temperateWoodlandBeachAtElevation13 = new List<int>(new int[9] { 31, 3, 3, 4, 4, 5, 5, 6, 29 });

	public List<int> temperateWoodlandRocksAtElevation13 = new List<int>(new int[7] { 3, 4, 5, 6, 8, 10, 26 });

	public List<int> temperateWoodlandTreesAtElevation14A = new List<int>(new int[8] { 12, 16, 17, 18, 27, 27, 28, 28 });

	public List<int> temperateWoodlandTreesAtElevation14B = new List<int>(new int[11]
	{
		12, 12, 12, 16, 17, 27, 27, 27, 28, 28,
		28
	});

	public List<int> temperateWoodlandTreesAtElevation14C = new List<int>(new int[11]
	{
		12, 12, 16, 16, 18, 27, 27, 27, 28, 28,
		28
	});

	public List<int> temperateWoodlandTreesAtElevation14D = new List<int>(new int[11]
	{
		12, 12, 17, 17, 18, 27, 27, 27, 28, 28,
		28
	});

	public List<int> temperateWoodlandDeadTreesAtElevation14 = new List<int>(new int[11]
	{
		19, 19, 20, 20, 24, 24, 29, 29, 30, 31,
		31
	});

	public List<int> temperateWoodlandFlowersAtElevation14 = new List<int>(new int[6] { 2, 2, 2, 21, 22, 22 });

	public List<int> temperateWoodlandMushroomAtElevation14 = new List<int>(new int[6] { 7, 7, 9, 9, 9, 23 });

	public List<int> temperateWoodlandBushesAtElevation14 = new List<int>(new int[5] { 26, 26, 26, 27, 28 });

	public List<int> temperateWoodlandBeachAtElevation14 = new List<int>(new int[9] { 31, 3, 3, 4, 4, 5, 5, 6, 29 });

	public List<int> temperateWoodlandRocksAtElevation14 = new List<int>(new int[7] { 3, 4, 5, 6, 8, 10, 26 });

	public WOVegetationList(int rndSeed)
	{
		Random.seed = rndSeed;
	}

	public void SetNatureCollections()
	{
		mountainsTrees = new List<int>(new int[12]
		{
			5, 5, 11, 11, 12, 13, 13, 15, 15, 15,
			21, 30
		});
		mountainsNeedleTrees = new List<int>(new int[7] { 5, 5, 11, 11, 12, 12, 30 });
		mountainsDeadTrees = new List<int>(new int[7] { 16, 19, 20, 26, 26, 29, 29 });
		mountainsFlowers = new List<int>(new int[1] { 22 });
		mountainsRocks = new List<int>(new int[13]
		{
			1, 3, 4, 6, 7, 8, 10, 14, 17, 18,
			27, 28, 31
		});
		mountainsGrass = new List<int>(new int[8] { 2, 7, 7, 7, 9, 23, 23, 23 });
		swampTrees = collectionSwampTrees;
		swampEggs = collectionSwampEggs;
		swampBeach = collectionSwampBeach;
		swampFlowers = collectionSwampFlowers;
		swampRocks = collectionSwampRocks;
		swampDeadTrees = collectionSwampDeadTrees;
		swampPlants = collectionRainforestPlants;
		rainforestTrees = collectionRainforestTrees;
		rainforestEggs = collectionRainforestEggs;
		rainforestBeach = collectionRainforestBeach;
		rainforestFlowers = collectionRainforestFlowers;
		rainforestRocks = collectionRainforestRocks;
		rainforestBushes = collectionRainforestBushes;
		rainforestPlants = collectionRainforestPlants;
		subtropicalTrees = collectionSubTropicalTrees;
		subtropicalBulbs = collectionSubTropicalBulbs;
		subtropicalBeach = collectionSubTropicalBeach;
		subtropicalDeadTrees = collectionSubTropicalTrees.Concat(collectionSubTropicalDeadTrees).ToList();
		subtropicalFlowers = collectionSubTropicalFlowers;
		subtropicalRocks = collectionSubTropicalRocks;
		subtropicalMushroom = collectionSubTropicalMushroom;
		subtropicalPlants = collectionSubTropicalPlants;
		hauntedWoodlandTrees = new List<int> { 13, 13, 13, 13, 15, 16, 18, 30 };
		hauntedWoodlandBones = collectionHauntedWoodlandBones;
		hauntedWoodlandBeach = collectionHauntedWoodlandBeach;
		hauntedWoodlandDeadTrees = collectionHauntedWoodlandDeadTrees;
		hauntedWoodlandFlowers = collectionHauntedWoodlandFlowers;
		hauntedWoodlandRocks = collectionHauntedWoodlandRocks;
		hauntedWoodlandMushroom = collectionHauntedWoodlandMushroom;
		hauntedWoodlandPlants = collectionHauntedWoodlandPlants.Concat(collectionHauntedWoodlandBones).ToList();
		hauntedWoodlandBushes = collectionHauntedWoodlandBushes.Concat(collectionHauntedWoodlandBones).ToList();
		woodlandHillsTrees = collectionWoodlandHillsTrees;
		woodlandHillsBeach = collectionWoodlandHillsBeach;
		woodlandHillsDeadTrees = collectionWoodlandHillsDeadTrees;
		woodlandHillsFlowers = collectionWoodlandHillsFlowers;
		woodlandHillsRocks = collectionWoodlandHillsRocks;
		woodlandHillsBushes = collectionWoodlandHillsBushes;
		woodlandHillsNeedleTrees = collectionWoodlandHillsNeedleTrees;
		desertTrees = collectionDesertTrees;
		desertWaterPlants = collectionDesertPlants;
		desertDeadTrees = collectionDesertDeadTrees;
		desertFlowers = collectionDesertFlowers;
		desertStones = collectionDesertStones;
		desertPlants = collectionDesertPlants;
		desertWaterFlowers = collectionDesertWaterFlowers;
		desertCactus = collectionDesertCactus;
		temperateWoodlandTrees = collectionTemperateWoodlandTrees;
		temperateWoodlandMushroom = collectionTemperateWoodlandMushroom;
		temperateWoodlandBeach = collectionTemperateWoodlandBeach;
		temperateWoodlandDeadTrees = collectionTemperateWoodlandDeadTrees;
		temperateWoodlandFlowers = collectionTemperateWoodlandFlowers;
		temperateWoodlandRocks = collectionTemperateWoodlandRocks;
		temperateWoodlandBushes = collectionTemperateWoodlandBushes;
	}
}
