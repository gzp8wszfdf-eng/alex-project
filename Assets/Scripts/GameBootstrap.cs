using UnityEngine;

public sealed class GameBootstrap : MonoBehaviour
{
    [SerializeField] private bool buildDemoAirportOnStart = true;

    private void Start()
    {
        Application.targetFrameRate = 60;
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        if (buildDemoAirportOnStart)
        {
            AirportWorldBuilder builder = FindFirstObjectByType<AirportWorldBuilder>();
            if (builder == null)
                builder = gameObject.AddComponent<AirportWorldBuilder>();

            builder.Build();
        }
    }
}
