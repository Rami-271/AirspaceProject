using System;
using System.Collections.Generic;

namespace AirSimulation
{
    public class Airport
    {
        private string airportName;
        private List<Runway> runways;
        private List<Gate> gates;
        private List<Flight> flights;
        private Scheduler scheduler;
        private ConflictDetector conflictDetector;

        public string AirportName
        {
            get
            {
                return airportName;
            }
        }

        public Scheduler Scheduler
        {
            get
            {
                return scheduler;
            }
        }

        public ConflictDetector ConflictDetector
        {
            get
            {
                return conflictDetector;
            }
        }

        public Airport(string airportName)
        {
            if (string.IsNullOrWhiteSpace(airportName))
            {
                throw new ArgumentException("Airport name cannot be empty.");
            }

            this.airportName = airportName.Trim();
            runways = new List<Runway>();
            gates = new List<Gate>();
            flights = new List<Flight>();
            scheduler = new Scheduler();
            conflictDetector = new ConflictDetector();
        }

        public void AddRunway(Runway runway)
        {
            if (runway == null)
            {
                throw new ArgumentNullException("runway");
            }

            if (HasRunway(runway.RunwayNumber))
            {
                throw new ArgumentException(
                    "A runway with that number already exists.");
            }

            runways.Add(runway);
        }

        public void AddGate(Gate gate)
        {
            if (gate == null)
            {
                throw new ArgumentNullException("gate");
            }

            if (HasGate(gate.GateNumber))
            {
                throw new ArgumentException(
                    "A gate with that number already exists.");
            }

            gates.Add(gate);
        }

        public void AddFlight(Flight flight)
        {
            if (flight == null)
            {
                throw new ArgumentNullException("flight");
            }

            if (HasFlight(flight.Aircraft.FlightNumber))
            {
                throw new ArgumentException(
                    "A flight with that flight number already exists.");
            }

            if (flight.Type == FlightType.Departure &&
                conflictDetector.HasInvalidDeparture(flight, this))
            {
                throw new InvalidOperationException(
                    "A departure flight must have an assigned gate.");
            }

            flights.Add(flight);
            scheduler.AddFlight(flight);
        }

        public void AssignDepartureGate(Flight flight, string gateNumber)
        {
            if (flight == null)
            {
                throw new ArgumentNullException("flight");
            }

            if (flight.Type != FlightType.Departure)
            {
                throw new InvalidOperationException(
                    "Only departure flights can be assigned to a departure gate.");
            }

            if (HasFlight(flight.Aircraft.FlightNumber))
            {
                throw new InvalidOperationException(
                    "The flight has already been added to the airport.");
            }

            if (HasGateForFlight(flight))
            {
                throw new InvalidOperationException(
                    "The flight already has an assigned gate.");
            }

            if (!HasGate(gateNumber))
            {
                throw new ArgumentException("The gate was not found.");
            }

            Gate gate = GetGate(gateNumber);

            if (conflictDetector.HasGateConflict(gate))
            {
                throw new InvalidOperationException(
                    $"Gate {gate.GateNumber} is occupied.");
            }

            gate.AssignFlight(flight);
            flight.UpdateStatus(FlightStatus.ReadyForDeparture);

            scheduler.AddEvent(
                $"{flight.Aircraft.FlightNumber} was assigned to " +
                $"departure gate {gate.GateNumber}.");
        }

        public void PrepareFlightForDeparture(Flight flight)
        {
            if (flight == null)
            {
                throw new ArgumentNullException("flight");
            }

            if (!ContainsFlight(flight))
            {
                throw new InvalidOperationException(
                    "The flight is not part of this airport.");
            }

            if (!HasGateForFlight(flight))
            {
                throw new InvalidOperationException(
                    "The flight must have an assigned gate.");
            }

            flight.PrepareForDeparture();

            scheduler.AddEvent(
                $"{flight.Aircraft.FlightNumber} is ready for departure.");

            scheduler.AddDepartureFlight(flight);
        }

        public bool ProcessNextFlight()
        {
            return scheduler.ProcessNextFlight(this);
        }

        public bool HasAvailableRunway()
        {
            foreach (Runway runway in runways)
            {
                if (runway.IsAvailable)
                {
                    return true;
                }
            }

            return false;
        }

        public Runway GetAvailableRunway()
        {
            foreach (Runway runway in runways)
            {
                if (runway.IsAvailable)
                {
                    return runway;
                }
            }

            throw new InvalidOperationException(
                "No runway is available.");
        }

        public bool HasAvailableGate()
        {
            foreach (Gate gate in gates)
            {
                if (gate.IsAvailable)
                {
                    return true;
                }
            }

            return false;
        }

        public Gate GetAvailableGate()
        {
            foreach (Gate gate in gates)
            {
                if (gate.IsAvailable)
                {
                    return gate;
                }
            }

            throw new InvalidOperationException(
                "No gate is available.");
        }

        public bool HasRunway(string runwayNumber)
        {
            if (string.IsNullOrWhiteSpace(runwayNumber))
            {
                return false;
            }

            string selectedRunway = runwayNumber.Trim().ToUpper();

            foreach (Runway runway in runways)
            {
                if (runway.RunwayNumber == selectedRunway)
                {
                    return true;
                }
            }

            return false;
        }

        public Runway GetRunway(string runwayNumber)
        {
            if (string.IsNullOrWhiteSpace(runwayNumber))
            {
                throw new ArgumentException(
                    "Runway number cannot be empty.");
            }

            string selectedRunway = runwayNumber.Trim().ToUpper();

            foreach (Runway runway in runways)
            {
                if (runway.RunwayNumber == selectedRunway)
                {
                    return runway;
                }
            }

            throw new InvalidOperationException(
                "The runway was not found.");
        }

        public bool HasGate(string gateNumber)
        {
            if (string.IsNullOrWhiteSpace(gateNumber))
            {
                return false;
            }

            string selectedGate = gateNumber.Trim().ToUpper();

            foreach (Gate gate in gates)
            {
                if (gate.GateNumber == selectedGate)
                {
                    return true;
                }
            }

            return false;
        }

        public Gate GetGate(string gateNumber)
        {
            if (string.IsNullOrWhiteSpace(gateNumber))
            {
                throw new ArgumentException("Gate number cannot be empty.");
            }

            string selectedGate = gateNumber.Trim().ToUpper();

            foreach (Gate gate in gates)
            {
                if (gate.GateNumber == selectedGate)
                {
                    return gate;
                }
            }

            throw new InvalidOperationException(
                "The gate was not found.");
        }

        public bool HasFlight(string flightNumber)
        {
            if (string.IsNullOrWhiteSpace(flightNumber))
            {
                return false;
            }

            string selectedFlight = flightNumber.Trim().ToUpper();

            foreach (Flight flight in flights)
            {
                if (flight.Aircraft.FlightNumber == selectedFlight)
                {
                    return true;
                }
            }

            return false;
        }

        public Flight GetFlight(string flightNumber)
        {
            if (string.IsNullOrWhiteSpace(flightNumber))
            {
                throw new ArgumentException(
                    "Flight number cannot be empty.");
            }

            string selectedFlight = flightNumber.Trim().ToUpper();

            foreach (Flight flight in flights)
            {
                if (flight.Aircraft.FlightNumber == selectedFlight)
                {
                    return flight;
                }
            }

            throw new InvalidOperationException(
                "The flight was not found.");
        }

        public bool HasGateForFlight(Flight flight)
        {
            if (flight == null)
            {
                throw new ArgumentNullException("flight");
            }

            foreach (Gate gate in gates)
            {
                if (gate.CurrentFlight == flight)
                {
                    return true;
                }
            }

            return false;
        }

        public Gate GetGateForFlight(Flight flight)
        {
            if (flight == null)
            {
                throw new ArgumentNullException("flight");
            }

            foreach (Gate gate in gates)
            {
                if (gate.CurrentFlight == flight)
                {
                    return gate;
                }
            }

            throw new InvalidOperationException(
                "The flight does not have an assigned gate.");
        }

        private bool ContainsFlight(Flight selectedFlight)
        {
            foreach (Flight flight in flights)
            {
                if (flight == selectedFlight)
                {
                    return true;
                }
            }

            return false;
        }

        public Runway[] GetRunways()
        {
            return runways.ToArray();
        }

        public Gate[] GetGates()
        {
            return gates.ToArray();
        }

        public Flight[] GetFlights()
        {
            return flights.ToArray();
        }
    }
}
