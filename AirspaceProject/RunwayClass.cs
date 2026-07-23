using System;

namespace AirSimulation
{
    public class Runway
    {
        private string runwayNumber;
        private Flight currentFlight;
        private bool isAvailable;

        public string RunwayNumber
        {
            get
            {
                return runwayNumber;
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

        public Runway(string runwayNumber)
        {
            if (string.IsNullOrWhiteSpace(runwayNumber))
            {
                throw new ArgumentException("Runway number cannot be empty.");
            }

            this.runwayNumber = runwayNumber;
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
                    $"Runway {runwayNumber} is occupied.");
            }

            currentFlight = flight;
            isAvailable = false;
        }

        public void ReleaseRunway()
        {
            currentFlight = null;
            isAvailable = true;
        }

        public string GetDescription()
        {
            if (isAvailable)
            {
                return $"Runway {runwayNumber}: Available";
            }

            return $"Runway {runwayNumber}: Occupied by " +
                $"{currentFlight.Aircraft.FlightNumber}";
        }
    }
}
