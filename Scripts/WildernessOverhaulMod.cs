using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Game.Utility.ModSupport.ModSettings;
using UnityEngine;
using WildernessOverhaul;

public class WildernessOverhaulMod : MonoBehaviour
{
	public static WildernessOverhaulMod instance;

	public float Amplitude = 0.9f;

	public float Frequency = 0.01f;

	public float Persistence = 0.35f;

	public int Octaves = 3;

	public float ForestLimitMin = 0.4f;

	public float ForestLimitMax = 0.7f;

	public int seed = 100;

	public static Mod mod;

	private static ModSettings settings;

	private static WOTerrainTexturing woTexturing;

	private static WOTerrainNature woNature;

	private static Mod DREAMMod;

	private static bool DREAMModEnabled;

	private static Mod WODTerrainMod;

	private static bool WODTerrainModEnabled;

	private static Mod BasicRoadsMod;

	private static bool BasicRoadsModEnabled;

	private static Material terrainMaterial;

	public float treeLine = 0.69f;

	private int rngSeed;

	private bool dynamicVegetationClearance;

	private bool vegetationInLocations;

	private bool fireflies;

	private bool shootingStars;

	private float fireflyActivationDistance;

	private float shootingStarsMin;

	private float shootingStarsMax;

	private float generalNatureClearance;

	private float natureClearance1;

	private float natureClearance2;

	private float natureClearance3;

	private float natureClearance4;

	private float natureClearance5;

	[Invoke(StateManager.StateTypes.Start, 0)]
	public static void Init(InitParams initParams)
	{
		mod = initParams.Mod;
		instance = new GameObject(mod.Title).AddComponent<WildernessOverhaulMod>();
		DREAMMod = ModManager.Instance.GetModFromGUID("5e1af2fc-2c12-4d05-829c-12b37f396e19");
		if (DREAMMod != null && DREAMMod.Enabled)
		{
			DREAMModEnabled = true;
			Debug.Log("Wilderness Overhaul: DREAM Mod is active");
		}
		WODTerrainMod = ModManager.Instance.GetModFromGUID("a9091dd7-e07a-4171-b16d-d13d67a5f221");
		if (WODTerrainMod != null && WODTerrainMod.Enabled)
		{
			WODTerrainModEnabled = true;
			Debug.Log("Wilderness Overhaul: World of Daggerfall - Terrain Mod is active");
		}
		BasicRoadsMod = ModManager.Instance.GetModFromGUID("566ab21a-22d8-4eea-8ccd-6cb8f7a7ed25");
		if (BasicRoadsMod != null && BasicRoadsMod.Enabled)
		{
			BasicRoadsModEnabled = true;
			Debug.Log("Wilderness Overhaul: Basic Roads Mod is active");
		}
	}

	private void Start()
	{
		Debug.Log("Wilderness Overhaul: Initiating Mod");
		settings = mod.GetSettings();
		rngSeed = settings.GetValue<int>("General", "RandomSeed");
		dynamicVegetationClearance = settings.GetValue<bool>("TerrainNature", "DynamicVegetationClearance");
		fireflies = settings.GetValue<bool>("Nature", "Fireflies");
		shootingStars = settings.GetValue<bool>("Nature", "ShootingStars");
		fireflyActivationDistance = settings.GetValue<float>("Nature", "FireflyActivationDistance");
		shootingStarsMin = settings.GetValue<float>("Nature", "ShootingStarsMinChance");
		shootingStarsMax = settings.GetValue<float>("Nature", "ShootingStarsMaxChance");
		generalNatureClearance = settings.GetValue<float>("TerrainNature", "GeneralNatureClearance");
		natureClearance1 = settings.GetValue<float>("DynamicNatureClearance", "Cities");
		natureClearance2 = settings.GetValue<float>("DynamicNatureClearance", "Hamlets");
		natureClearance3 = settings.GetValue<float>("DynamicNatureClearance", "Villages,Homes(Wealthy),ReligiousCults");
		natureClearance4 = settings.GetValue<float>("DynamicNatureClearance", "Farms,Taverns,Temples,Homes(Poor)");
		natureClearance5 = settings.GetValue<float>("DynamicNatureClearance", "Dungeons(Laybinths,Keeps,Ruins,Graveyards,Covens)");
		woNature = new WOTerrainNature(mod, DREAMModEnabled, WODTerrainModEnabled, rngSeed, dynamicVegetationClearance, vegInLoc: false, fireflies, shootingStars, fireflyActivationDistance, shootingStarsMin, shootingStarsMax, generalNatureClearance, natureClearance1, natureClearance2, natureClearance3, natureClearance4, natureClearance5);
		woTexturing = new WOTerrainTexturing(WODTerrainModEnabled, BasicRoadsModEnabled);
		DaggerfallUnity.Instance.TerrainNature = woNature;
		DaggerfallUnity.Instance.TerrainTexturing = woTexturing;
		mod.IsReady = true;
		Debug.Log("Wilderness Overhaul: Mod Initiated");
	}
}
