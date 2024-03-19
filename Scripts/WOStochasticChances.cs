using UnityEngine;

public class WOStochasticChances
{
	public float mapStyle;

	public float[] mapStyleChance = new float[6] { 30f, 35f, 40f, 45f, 50f, 55f };

	public float temperateMushroomRingChance;

	public float[] tempForestLimit = new float[2];

	public float tempForestFrequency;

	public float tempForestAmplitude;

	public float tempForestPersistence;

	public int tempForestOctaves;

	public float mountainStoneCircleChance;

	public float[] mountForestLimit = new float[2];

	public float mountForestFrequency;

	public float mountForestAmplitude;

	public float mountForestPersistence;

	public int mountForestOctaves;

	public float desert2DirtChance;

	public float desert1DirtChance;

	public float desert2GrassChance1;

	public float desert1GrassChance1;

	public float desert2GrassChance2;

	public float desert1GrassChance2;

	public float[] desertForestLimit = new float[2];

	public float desertFrequency;

	public float desertAmplitude;

	public float desertPersistence;

	public int desertOctaves;

	public float woodlandHillsDirtChance;

	public float woodlandHillsStoneCircleChance;

	public float[] woodlandHillsForestLimit = new float[2];

	public float woodlandHillsForestFrequency;

	public float woodlandHillsForestAmplitude;

	public float woodlandHillsForestPersistence;

	public int woodlandHillsForestOctaves;

	public float[] hauntedWoodlandForestLimit = new float[2];

	public float hauntedWoodlandForestFrequency;

	public float hauntedWoodlandForestAmplitude;

	public float hauntedWoodlandForestPersistence;

	public int hauntedWoodlandForestOctaves;

	public float[] rainforestForestLimit = new float[2];

	public float rainforestForestFrequency;

	public float rainforestForestAmplitude;

	public float rainforestForestPersistence;

	public int rainforestForestOctaves;

	public float[] subtropicalForestLimit = new float[2];

	public float subtropicalForestFrequency;

	public float subtropicalForestAmplitude;

	public float subtropicalForestPersistence;

	public int subtropicalForestOctaves;

	public float[] swampForestLimit = new float[2];

	public float swampForestFrequency;

	public float swampForestAmplitude;

	public float swampForestPersistence;

	public int swampForestOctaves;

	public WOStochasticChances(int rndSeed)
	{
		Random.seed = rndSeed;
		SetupMapStyle();
	}

	private void SetupMapStyle()
	{
		int num = Random.Range(0, 21);
		if (num < 6)
		{
			mapStyle = mapStyleChance[0];
		}
		else if (num < 11)
		{
			mapStyle = mapStyleChance[1];
		}
		else if (num < 15)
		{
			mapStyle = mapStyleChance[2];
		}
		else if (num < 18)
		{
			mapStyle = mapStyleChance[3];
		}
		else if (num < 20)
		{
			mapStyle = mapStyleChance[4];
		}
		else
		{
			mapStyle = mapStyleChance[5];
		}
		temperateMushroomRingChance = 0.025f;
		tempForestLimit[0] = 0.4f;
		tempForestLimit[1] = tempForestLimit[0] + 0.3f;
		tempForestFrequency = 0.01f;
		tempForestAmplitude = 0.9f;
		tempForestPersistence = 0.4f;
		tempForestOctaves = 3;
		mountainStoneCircleChance = 0.025f;
		mountForestLimit[0] = 0.2f;
		mountForestLimit[1] = mountForestLimit[0] + Random.Range(0.15f, 0.2f);
		mountForestFrequency = Random.Range(0.01f, 0.02f);
		mountForestAmplitude = Random.Range(0.6f, 0.7f);
		mountForestPersistence = 0.3f;
		mountForestOctaves = Random.Range(2, 3);
		rainforestForestLimit[0] = 0.2f;
		rainforestForestLimit[1] = rainforestForestLimit[0] + 0.3f;
		rainforestForestFrequency = 0.04f;
		rainforestForestAmplitude = 0.95f;
		rainforestForestPersistence = 0.35f;
		rainforestForestOctaves = Random.Range(3, 4);
		hauntedWoodlandForestLimit[0] = 0.4f;
		hauntedWoodlandForestLimit[1] = hauntedWoodlandForestLimit[0] + Random.Range(0.2f, 0.3f);
		hauntedWoodlandForestFrequency = Random.Range(0.05f, 0.1f);
		hauntedWoodlandForestAmplitude = 0.9f;
		hauntedWoodlandForestPersistence = 0.35f;
		hauntedWoodlandForestOctaves = Random.Range(3, 4);
		woodlandHillsDirtChance = Random.Range(20, 30);
		woodlandHillsStoneCircleChance = 0.075f;
		woodlandHillsForestLimit[0] = 0.4f;
		woodlandHillsForestLimit[1] = woodlandHillsForestLimit[0] + 0.3f;
		woodlandHillsForestFrequency = Random.Range(0.01f, 0.2f);
		woodlandHillsForestAmplitude = 0.95f;
		woodlandHillsForestPersistence = Random.Range(0.375f, 0.425f);
		woodlandHillsForestOctaves = Random.Range(3, 4);
		subtropicalForestLimit[0] = 0.4f;
		subtropicalForestLimit[1] = subtropicalForestLimit[0] + 0.3f;
		subtropicalForestFrequency = Random.Range(0.01f, 0.02f);
		subtropicalForestAmplitude = 0.9f;
		subtropicalForestPersistence = 0.35f;
		subtropicalForestOctaves = Random.Range(2, 3);
		desert2DirtChance = Random.Range(0, 1);
		desert1DirtChance = Random.Range(1, 6);
		desert2GrassChance1 = Random.Range(0, 10);
		desert1GrassChance1 = Random.Range(0, 30);
		desert2GrassChance2 = Random.Range(10, 15);
		desert1GrassChance2 = Random.Range(30, 50);
		desertForestLimit[0] = 0.3f;
		desertForestLimit[1] = desertForestLimit[0] + 0.1f;
		desertFrequency = Random.Range(0.1f, 0.2f);
		desertAmplitude = 0.5f;
		desertPersistence = Random.Range(0.1f, 0.2f);
		desertOctaves = Random.Range(2, 3);
		swampForestLimit[0] = 0.4f;
		swampForestLimit[1] = swampForestLimit[0] + 0.3f;
		swampForestFrequency = 0.01f;
		swampForestAmplitude = 0.9f;
		swampForestPersistence = 0.35f;
		swampForestOctaves = Random.Range(3, 4);
	}
}
