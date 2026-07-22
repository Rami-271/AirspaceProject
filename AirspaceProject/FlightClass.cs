using System;

namespace AirSimulation
{
    public class Flight
    {
        public Aircraft Aircraft { get; set; }
        public FlightType Type { get; set; }
        public FlightStatus Status { get; set; }

        public Flight(Aircraft aircraft, FlightType type)
        {
            Aircraft = aircraft;
            Type = type;

            if (type == FlightType.Arrival)
            {
                Status = FlightStatus.ReadyForLanding;
            }
            else
            {
                Status = FlightStatus.AssignedGate;
            }
        }

        public bool IsEmergency()
        {
            return Aircraft.IsEmergency;
        }
    }
}

