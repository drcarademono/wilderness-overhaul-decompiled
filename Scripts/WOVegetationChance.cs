using UnityEngine;
using System.Collections.Generic; // Required for Dictionary
using WildernessOverhaul;

public class WOVegetationChance
{
    public float chanceOnDirt;
    public float chanceOnGrass;
    public float chanceOnStone;

    // Dictionary to hold scalers for each climate case
    private Dictionary<int, float> climateScalers;

    public WOVegetationChance(int rndSeed)
    {
        Random.seed = rndSeed;
        InitializeClimateScalers();
    }

    private void InitializeClimateScalers()
    {
        // Initialize the dictionary with default scalers for all climates you deal with
        climateScalers = new Dictionary<int, float>
        {
            {231, 1.0f},
            {226, 0.75f},
            {224, 1.0f},
            {225, 0.5f},
            {232, 1.0f},
            {230, 1.0f},
            {228, 1.0f},
            {227, 1.0f},
            {229, 1.0f},
        };
    }

    public void ChangeVegetationChances(float elevation, int climate)
    {
        float scaler = climateScalers.ContainsKey(climate) ? climateScalers[climate] : 1.0f;
        
        switch (climate)
        {
            case 231:
                if (elevation > 0.125f)
                {
                    chanceOnGrass = Mathf.Clamp(Random.Range(0.225f, 0.3f) * scaler, 0f, 1f);
                    chanceOnDirt = Mathf.Clamp(Random.Range(0.35f, 0.375f) * scaler, 0f, 1f);
                    chanceOnStone = Mathf.Clamp(Random.Range(0.5f, 0.525f) * scaler, 0f, 1f);
                }
                else if (elevation > 0.075f)
                {
                    chanceOnGrass = Mathf.Clamp(Random.Range(0.225f, 0.3f) * scaler, 0f, 1f);
                    chanceOnDirt = Mathf.Clamp(Random.Range(0.325f, 0.35f) * scaler, 0f, 1f);
                    chanceOnStone = Mathf.Clamp(Random.Range(0.675f, 0.7f) * scaler, 0f, 1f);
                }
                else if (elevation > 0.025f)
                {
                    chanceOnGrass = Mathf.Clamp(Random.Range(0.25f, 0.3f) * scaler, 0f, 1f);
                    chanceOnDirt = Mathf.Clamp(Random.Range(0.2f, 0.225f) * scaler, 0f, 1f);
                    chanceOnStone = Mathf.Clamp(Random.Range(0.65f, 0.675f) * scaler, 0f, 1f);
                }
                else
                {
                    chanceOnGrass = Mathf.Clamp(Random.Range(0.275f, 0.3f) * scaler, 0f, 1f);
                    chanceOnDirt = Mathf.Clamp(Random.Range(0.15f, 0.175f) * scaler, 0f, 1f);
                    chanceOnStone = Mathf.Clamp(Random.Range(0.625f, 0.65f) * scaler, 0f, 1f);
                }
                break;
            case 226:
			if (elevation > WildernessOverhaulMod.instance.treeLine)
			{
				chanceOnGrass = Mathf.Clamp(Random.Range(0.25f, 0.275f) * scaler, 0f, 1f);
				chanceOnDirt = Mathf.Clamp(Random.Range(0.2f, 0.225f) * scaler, 0f, 1f);
				chanceOnStone = Mathf.Clamp(Random.Range(0.325f, 0.35f) * scaler, 0f, 1f);
			}
			else if (elevation > 0.6f)
			{
				chanceOnGrass = Mathf.Clamp(Random.Range(0.275f, 0.3f) * scaler, 0f, 1f);
				chanceOnDirt = Mathf.Clamp(Random.Range(0.375f, 0.4f) * scaler, 0f, 1f);
				chanceOnStone = Mathf.Clamp(Random.Range(0.275f, 0.3f) * scaler, 0f, 1f);
			}
			else if (elevation > 0.4f)
			{
				chanceOnGrass = Mathf.Clamp(Random.Range(0.3f, 0.325f) * scaler, 0f, 1f);
				chanceOnDirt = Mathf.Clamp(Random.Range(0.35f, 0.4f) * scaler, 0f, 1f);
				chanceOnStone = Mathf.Clamp(Random.Range(0.225f, 0.25f) * scaler, 0f, 1f);
			}
			else if (elevation > 0.2f)
			{
				chanceOnGrass = Mathf.Clamp(Random.Range(0.325f, 0.375f) * scaler, 0f, 1f);
				chanceOnDirt = Mathf.Clamp(Random.Range(0.35f, 0.375f) * scaler, 0f, 1f);
				chanceOnStone = Mathf.Clamp(Random.Range(0.175f, 0.2f) * scaler, 0f, 1f);
			}
			else
			{
				chanceOnGrass = Mathf.Clamp(Random.Range(0.375f, 0.4f) * scaler, 0f, 1f);
				chanceOnDirt = Mathf.Clamp(Random.Range(0.3f, 0.35f) * scaler, 0f, 1f);
				chanceOnStone = Mathf.Clamp(Random.Range(0.25f, 0.275f) * scaler, 0f, 1f);
			}
			break;
		case 224:
			chanceOnGrass = Mathf.Clamp(Random.Range(0.1f, 0.15f) * scaler, 0f, 1f);
			chanceOnDirt = Mathf.Clamp(Random.Range(0.1f, 0.15f) * scaler, 0f, 1f);
			chanceOnStone = Mathf.Clamp(Random.Range(0.05f, 0.1f) * scaler, 0f, 1f);
			break;
		case 225:
			chanceOnGrass = Mathf.Clamp(Random.Range(0.05f, 0.25f) * scaler, 0f, 1f);
			chanceOnDirt = Mathf.Clamp(Random.Range(0.05f, 0.25f) * scaler, 0f, 1f);
			chanceOnStone = Mathf.Clamp(Random.Range(0.05f, 0.2f) * scaler, 0f, 1f);
			break;
		case 232:
			chanceOnGrass = Random.Range(0.05f, 0.09f);
			chanceOnDirt = Random.Range(0.045f, 0.065f);
			chanceOnStone = Random.Range(0.05f, 0.1f);
			break;
		case 230:
			if (elevation > 0.475f)
			{
				chanceOnGrass = Random.Range(0.035f, 0.08f);
				chanceOnDirt = Random.Range(0.12f, 0.15f);
				chanceOnStone = Random.Range(0.12f, 0.15f);
			}
			else if (elevation > 0.4f)
			{
				chanceOnGrass = Random.Range(0.04f, 0.1f);
				chanceOnDirt = Random.Range(0.1f, 0.13f);
				chanceOnStone = Random.Range(0.1f, 0.13f);
			}
			else if (elevation > 0.325f)
			{
				chanceOnGrass = Random.Range(0.05f, 0.1f);
				chanceOnDirt = Random.Range(0.09f, 0.11f);
				chanceOnStone = Random.Range(0.09f, 0.11f);
			}
			else if (elevation > 0.25f)
			{
				chanceOnGrass = Random.Range(0.09f, 0.12f);
				chanceOnDirt = Random.Range(0.07f, 0.09f);
				chanceOnStone = Random.Range(0.06f, 0.09f);
			}
			else
			{
				chanceOnGrass = Random.Range(0.1f, 0.13f);
				chanceOnDirt = Random.Range(0.06f, 0.09f);
				chanceOnStone = Random.Range(0.05f, 0.08f);
			}
			break;
		case 228:
			chanceOnStone = 0.3f;
			chanceOnDirt = 0.3f;
			chanceOnGrass = 0.3f;
			break;
		case 227:
			chanceOnGrass = 0.3f;
			chanceOnDirt = 0.3f;
			chanceOnStone = 0.3f;
			break;
		case 229:
			chanceOnStone = 0.3f;
			chanceOnDirt = 0.3f;
			chanceOnGrass = 0.3f;
			break;
		}
	}
}
