using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Weather;
using UnityEngine;

public class WORandomMover : MonoBehaviour
{
	[SerializeField]
	[Range(0f, 10f)]
	private float speed = 1f;

	[SerializeField]
	[Range(0.1f, 10f)]
	private float posChangeTime;

	[SerializeField]
	[Range(0f, 5f)]
	private float maxRoamingRangeVertical = 2f;

	[SerializeField]
	[Range(0f, 2.5f)]
	private float maxRoamingRangeHorizontal = 3f;

	private DaggerfallUnity dfUnity;

	private WeatherManager weatherManager;

	private GameObject player;

	private Camera mainCam;

	private Rigidbody rb;

	private Transform halo;

	private SpriteRenderer m_Renderer;

	private SpriteRenderer h_Renderer;

	private Material m_Material;

	private Material h_Material;

	private int my_StartTime;

	private int my_EndTime;

	private float light_offset;

	private float pulseFactor;

	private float m_Alpha;

	public Vector3 startPos;

	private Vector3 targetPos;

	private bool isOn;

	private bool init = true;

	public bool isPerforming;

	private float t;

	public void ToggleActivation(bool state)
	{
		if (state)
		{
			if (!isPerforming)
			{
				isPerforming = true;
			}
		}
		else if (isPerforming)
		{
			isPerforming = false;
		}
	}

	private void Awake()
	{
		dfUnity = GameObject.Find("DaggerfallUnity").GetComponent<DaggerfallUnity>();
		weatherManager = GameObject.Find("WeatherManager").GetComponent<WeatherManager>();
		mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		rb = GetComponent<Rigidbody>();
		m_Renderer = GetComponent<SpriteRenderer>();
		m_Material = GetComponent<SpriteRenderer>().material;
		h_Material = base.transform.GetChild(0).GetComponent<SpriteRenderer>().material;
		halo = base.transform.GetChild(0);
		h_Renderer = halo.GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		my_StartTime = Random.Range(1015, 1100);
		my_EndTime = Random.Range(320, 375);
		light_offset = Random.Range(0f, 1f);
		pulseFactor = Random.Range(0.1f, 0.75f);
		h_Material.SetColor("_Color", new Color(1f, 1f, 1f, 0.1f));
		targetPos = new Vector3(startPos.x + Random.Range(0f - maxRoamingRangeHorizontal, maxRoamingRangeHorizontal), startPos.y + Random.Range(0f - maxRoamingRangeVertical, maxRoamingRangeVertical), startPos.z + Random.Range(0f - maxRoamingRangeHorizontal, maxRoamingRangeHorizontal));
	}

	private void FixedUpdate()
	{
		if (!isPerforming)
		{
			return;
		}
		if ((dfUnity.WorldTime.Now.MinuteOfDay > my_StartTime || dfUnity.WorldTime.Now.MinuteOfDay < my_EndTime) && weatherManager.PlayerWeather.WeatherType != WeatherType.Rain && weatherManager.PlayerWeather.WeatherType != WeatherType.Snow && weatherManager.PlayerWeather.WeatherType != WeatherType.Rain && weatherManager.PlayerWeather.WeatherType != WeatherType.Snow && weatherManager.PlayerWeather.WeatherType != WeatherType.Thunder)
		{
			if (!isOn)
			{
				init = true;
				m_Alpha = -10f;
			}
			isOn = true;
		}
		else
		{
			if (isOn)
			{
				init = true;
			}
			isOn = false;
		}
		if (isOn && init)
		{
			m_Renderer.enabled = true;
			if (m_Alpha < 10f)
			{
				m_Alpha += Time.fixedDeltaTime * 5f;
				m_Material.SetColor("_Color", new Color(1f, 1f, 1f, m_Alpha));
				h_Material.SetColor("_Color", new Color(1f, 1f, 1f, Mathf.Clamp(m_Alpha, 0f, 0.1f)));
			}
			else
			{
				init = false;
			}
		}
		if (!isOn && init)
		{
			if (m_Alpha > -10f)
			{
				m_Alpha -= Time.fixedDeltaTime * 5f;
				m_Material.SetColor("_Color", new Color(1f, 1f, 1f, m_Alpha));
				h_Material.SetColor("_Color", new Color(1f, 1f, 1f, Mathf.Clamp(m_Alpha, 0f, 0.1f)));
			}
			else
			{
				init = false;
			}
			m_Renderer.enabled = false;
		}
		if (isOn && !init)
		{
			t += Time.fixedDeltaTime;
			m_Alpha = -10f + Mathf.PingPong(Time.time * pulseFactor, 1f) * 20f;
			m_Material.SetColor("_Color", new Color(1f, 1f, 1f, m_Alpha));
			halo.transform.localScale = new Vector3(m_Alpha, m_Alpha, m_Alpha);
			if (m_Alpha > 0f)
			{
				h_Renderer.enabled = true;
			}
			else
			{
				h_Renderer.enabled = false;
			}
			if (t > Random.Range(0.3f, 3f))
			{
				t = 0f;
				targetPos = new Vector3(startPos.x + Random.Range(0f - maxRoamingRangeHorizontal, maxRoamingRangeHorizontal), startPos.y + Random.Range(0f - maxRoamingRangeVertical, maxRoamingRangeVertical), startPos.z + Random.Range(0f - maxRoamingRangeHorizontal, maxRoamingRangeHorizontal));
			}
			rb.AddForce((targetPos - base.transform.localPosition) * speed);
		}
		base.transform.LookAt(base.transform.position + mainCam.transform.rotation * Vector3.forward, mainCam.transform.rotation * Vector3.up);
	}
}
