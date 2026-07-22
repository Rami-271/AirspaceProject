using System;

namespace AirSimulation
{
    public enum FlightStatus
    {
        ReadyForLanding,
        InLandingQueue,
        AssignedRunway,
        Landed,
        AssignedGate,
        ReadyForDeparture,
        InDepartureQueue,
        Departed,
        Complete
    }

}