public class LocalTime
{
	public static float TimeScale { get; private set; } = 1.0f;

	public static void SetTimeScale(float timeScale)
	{
		TimeScale = timeScale;
	}
}