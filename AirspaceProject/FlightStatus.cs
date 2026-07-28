using System;

namespace AirSimulation
{
    public enum FlightStatus
    {
        Created,
        ReadyForLanding,
        InLandingQueue,
        AssignedRunway,
        Landed,
        WaitingForGate,
        AssignedGate,
        ReadyForDeparture,
        InDepartureQueue,
        Departed,
        Complete
    }

}
