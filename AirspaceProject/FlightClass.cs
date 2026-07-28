using System;

namespace AirSimulation
{
    public class Flight
    {
        public Aircraft Aircraft { get; }
        public FlightType Type { get; private set; }
        public FlightStatus Status { get; private set; }

        public Flight(Aircraft aircraft, FlightType type)
        {
            if (aircraft == null)
            {
                throw new ArgumentNullException("aircraft");
            }
            
            Aircraft = aircraft;
            Type = type;

            if (type == FlightType.Arrival)
            {
                Status = FlightStatus.ReadyForLanding;
            }
            else
            {
                Status = FlightStatus.Created
            }
        }

        public bool IsEmergency()
        {
            return Aircraft.IsEmergency;
        }

        public void UpdateStatus(FlightStatus status)
        {
            Status = status;
        }

        public void PrepareForDeparture()
        {
            if (Type != FlightType.Arrival)
            {
                throw new InvalidOperationException(
                    "Only an arrival flight can be prepared for departure.");
            }

            if (Status != FlightStatus.AssignedGate)
            {
                throw new InvalidOperationException(
                    "The flight must be assigned to a gate before departure.");
            }

            Type = FlightType.Departure;
            Status = FlightStatus.ReadyForDeparture;
        }

        public string GetDescription()
        {
            return $"{Aircraft.GetDescription()}, Type: {Type}, " +
                $"Status: {Status}";
        }
    }
}

