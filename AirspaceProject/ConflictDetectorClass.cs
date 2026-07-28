using System;

namespace AirSimulation
{
    public class ConflictDetector
    {
        public bool HasRunwayConflict(Runway runway)
        {
            if (runway == null)
            {
                throw new ArgumentNullException("runway");
            }

            return !runway.IsAvailable;
        }

        public bool HasGateConflict(Gate gate)
        {
            if (gate == null)
            {
                throw new ArgumentNullException("gate");
            }

            return !gate.IsAvailable;
        }

        public bool HasInvalidDeparture(Flight flight, Airport airport)
        {
            if (flight == null)
            {
                throw new ArgumentNullException("flight");
            }

            if (airport == null)
            {
                throw new ArgumentNullException("airport");
            }

            if (flight.Type != FlightType.Departure)
            {
                return false;
            }

            if (!airport.HasGateForFlight(flight))
            {
                return true;
            }
            if (flight.Status != FlightStatus.ReadyForDeparture)
            {
                return true;
            }

            return false;
        }
    }
}
