using System;

namespace AirSimulation
{
    public class Gate
    {
        private string gateNumber;
        private Flight currentFlight;
        private bool isAvailable;

        public string GateNumber
        {
            get
            {
                return gateNumber;
            }
        }

        public Flight CurrentFlight
        {
            get
            {
                return currentFlight;
            }
        }

        public bool IsAvailable
        {
            get
            {
                return isAvailable;
            }
        }

        public Gate(string gateNumber)
        {
            if (string.IsNullOrWhiteSpace(gateNumber))
            {
                throw new ArgumentException("Gate number cannot be empty.");
            }

            this.gateNumber = gateNumber;
            currentFlight = null;
            isAvailable = true;
        }

        public void AssignFlight(Flight flight)
        {
            if (flight == null)
            {
                throw new ArgumentNullException("flight");
            }

            if (!isAvailable)
            {
                throw new InvalidOperationException(
                    $"Gate {gateNumber} is occupied.");
            }

            currentFlight = flight;
            isAvailable = false;
        }

        public void ReleaseGate()
        {
            currentFlight = null;
            isAvailable = true;
        }

        public string GetDescription()
        {
            if (isAvailable)
            {
                return $"Gate {gateNumber}: Available";
            }

            return $"Gate {gateNumber}: Occupied by " +
                $"{currentFlight.Aircraft.FlightNumber}";
        }
    }
}
