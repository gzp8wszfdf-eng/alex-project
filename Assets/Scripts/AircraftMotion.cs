using UnityEngine;

public sealed class AircraftMotion : MonoBehaviour
{
    public enum FlightPhase { Parked, TaxiToRunway, TakeoffRoll, ClimbOut, Approach, LandingRoll, TaxiToGate }

    [SerializeField] private FlightPhase phase = FlightPhase.Parked;
    [SerializeField] private float taxiSpeed = 5f;
    [SerializeField] private float runwaySpeed = 24f;
    [SerializeField] private float turnSpeed = 45f;

    public FlightPhase Phase => phase;

    public void SetPhase(FlightPhase nextPhase) => phase = nextPhase;

    private void Update()
    {
        float speed = phase == FlightPhase.TakeoffRoll || phase == FlightPhase.LandingRoll
            ? runwaySpeed : taxiSpeed;

        if (phase == FlightPhase.TaxiToRunway || phase == FlightPhase.TaxiToGate ||
            phase == FlightPhase.TakeoffRoll || phase == FlightPhase.LandingRoll)
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }

        if (phase == FlightPhase.ClimbOut)
        {
            transform.position += transform.forward * (runwaySpeed * Time.deltaTime);
            transform.position += Vector3.up * (2.2f * Time.deltaTime);
        }

        if (phase == FlightPhase.Approach)
        {
            transform.position += transform.forward * (taxiSpeed * Time.deltaTime);
            transform.position += Vector3.down * (0.6f * Time.deltaTime);
        }
    }
}
