using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Weather;
using UnityEngine;

public class WOShootingStarController : MonoBehaviour
{
	private int my_StartTime;

	private int my_EndTime;

	public ParticleSystem ps;

	private DaggerfallUnity dfUnity;

	private WeatherManager weatherManager;

	private void Start()
	{
		dfUnity = GameObject.Find("DaggerfallUnity").GetComponent<DaggerfallUnity>();
		weatherManager = GameObject.Find("WeatherManager").GetComponent<WeatherManager>();
		my_StartTime = Random.Range(1045, 1100);
		my_EndTime = Random.Range(330, 360);
	}

	private void Update()
	{
		if ((dfUnity.WorldTime.Now.MinuteOfDay > my_StartTime || dfUnity.WorldTime.Now.MinuteOfDay < my_EndTime) && weatherManager.PlayerWeather.WeatherType != WeatherType.Rain && weatherManager.PlayerWeather.WeatherType != WeatherType.Rain && weatherManager.PlayerWeather.WeatherType != WeatherType.Snow && weatherManager.PlayerWeather.WeatherType != WeatherType.Snow && weatherManager.PlayerWeather.WeatherType != WeatherType.Thunder && weatherManager.PlayerWeather.WeatherType != WeatherType.Overcast && weatherManager.PlayerWeather.WeatherType != WeatherType.Cloudy && weatherManager.PlayerWeather.WeatherType != WeatherType.Fog)
		{
			if (!ps.isPlaying)
			{
				ps.Play();
			}
		}
		else if ((dfUnity.WorldTime.Now.MinuteOfDay <= my_StartTime || dfUnity.WorldTime.Now.MinuteOfDay >= my_EndTime) && ps.isPlaying)
		{
			ps.Stop();
		}
	}
}
