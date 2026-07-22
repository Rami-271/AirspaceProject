using System;
using System.Collections.Generic;
using System.Text;

namespace AirSimulation
{
    public class Scheduler
    {
        private Queue<Flight> landingQueue;
        private Queue<Flight> departureQueue;
        private Queue<Flight> gateWaitingQueue;
        private List<string> eventLog;

        public Scheduler()
        {
            landingQueue = new Queue<Flight>();
            departureQueue = new Queue<Flight>();
            gateWaitingQueue = new Queue<Flight>();
            eventLog = new List<string>();
        }

        public void AddFlight(Flight flight)
        {
            if (flight == null)
            {
                throw new ArgumentNullException("flight");
            }
            if (flight.Type == FlightType.Arrival)
            {
                AddToLandingQueue(flight);
            }
            else
            {
                departureQueue.Enqueue(flight);
                flight.Status = FlightStatus.InDepartureQueue;
                eventLog.Add($"{flight.Aircraft.Flightnumber} entered the departure queue.");
            }
        }

        private void AddToLandingQueue(Flight flight)
        {
            if (flight.IsEmergency())
            {
                Queue<Flight> newQueue = new Queue<Flight>();
                bool flightAdded = false;
            
                foreach (Flight existingFlight in landingQueue)
                {
                    if (!flightAdded && !existingFlight.IsEmergency())
                    {
                        newQueue.Enqueue(existingFlight);
                        flightAdded = true;
                    }

                    newQueue.Enqueue(existingFlight);                    
                }

                if (!flightAdded)
                {
                    newQueue.Enqueue(flight);
                }

                landingQueue = newQueue;

                eventLog.Add($"Emergency alert: {flight.Aircraft.FlightNumber} received landing priority.");
            }
            else
            {
                landingQueue.Enqueue(flight);
            }

            flight.Status = FlightStatus.InLandingQueue;

            eventLog.Add($"{flight.Aircraft.FlightNumber} entered the landing queue.");
        }

        public bool ProcessNextFlight(Airport airport)
        {
            if (airport == null)
            {
                throw new ArgumentNullException("airport");
            }

            AssignWaitingFlightsToGates(airport);

            if (landingQueue.Count > 0)
            {
                return ProcessArrival(airport);
            }

            if (departureQueue.Count > 0)
            {
                return ProcessDeparture(airport);
            }

            return false;
        }

        private bool ProcessArrival(Airport.airport)
        {
            if (!airport.HasAvailableRunway())
            {
                eventLog.Add("No runway is available for the next arrival.");
                retur false;
            }

            Runway runway = airport.GetAvailableRunway();
            Flight flight = landingQueue.Dequeue();

            runway.AssignFlight(flight);
            flight.Status = FlightStatus.AssignedRunway;

            eventLog.Add($"{flight.Aircraft.FlightNumber} was assigned to runway {runway.RunwayNumber}.");
            flight.Status = FlightStatus.Landed;

            eventLog.Add($"{flight.Aircraft.FlightNumber} landed on runway {runway.RunwayNumber}.");
            runway.ReleaseRunway();

            if (!airport.HasAvailableGate())
            {
                gateWaitingQueue.Enqueue(flight);

                eventLog.Add($"{flight.Aircraft.FlightNumber} is waiting for an available gate.");
            }
            else
            {
                Gate gate = airport.GetAvailableGate();

                gate.AssignFlight(flight);
                flight.Status = FlightStatus.AssignedGate;

                eventLog.Add($"{flight.Aircraft.FlightNumber} was assigned to gate {gate.GateNumber}.");
            }

            return true;
        }

        private bool ProcessDeparture(Airport airport)
        {
            Flight flight = departureQueue.Peek();

            if (!airport.HasGateForFlight(flight))
            {
                eventLog.Add($"{flight.Aircraft.FlightNumber} cannot depart because it has no assigned gate.");

                return false;
            }

            if (!airport.HasAvailableRunway())
            {
                eventLog.Add("No runway is available for the next departure.");

                return false;
            }

            Gate gate = airport.GetGateForFlight(flight);
            Runway runway = airport.GetAvailableRunway();

            departureQueue.Dequeue();

            runway.AssignFlight(flight);
            flight.Status = FlightStatus.AssignedRunway;

            eventLog.Add($"{flight.Aircraft.FlightNumber} was assigned to runway {runway.RunwayNumber} for departure.");

            gate.ReleaseGate();

            flight.Status = FlightStatus.Departed;

            eventLog.Add($"{flight.Aircraft.FlightNumber} departed from runway {runway.RunwayNumber}.");

            runway.ReleaseRunway();

            flight.Status = FlightStatus.Complete;

            eventLog.Add($"{flight.Aircraft.FlightNumber} is complete.");

            AssignWaitingFlightsToGates(airport);

            return true;
        }

        public int AssignWaitingFlightsToGates(Airport airport)
        {
            if (airport == null)
            {
                throw new ArgumentNullException("airport");
            }

            int assignedCount = 0;

            while (gateWaitingQueue.Count > 0 &&
                airport.HasAvailableGate())
            {
                Flight flight = gateWaitingQueue.Dequeue();
                Gate gate = airport.GetAvailableGate();

                gate.AssignFlight(flight);
                flight.Status = FlightStatus.AssignedGate;

                eventLog.Add($"{flight.Aircraft.FlightNumber} was assigned to gate {gate.GateNumber}.");

                assignedCount++;
            }

            return assignedCount;
        }

        public Flight[] GetLandingQueue()
        {
            return landingQueue.ToArray();
        }

        public Flight[] GetDepartureQueue()
        {
            return departureQueue.ToArray();
        }

        public Flight[] GetGateWaitingQueue()
        {
            return gateWaitingQueue.ToArray();
        }

        public string[] GetEventLog()
        {
            return eventLog.ToArray();
        }
        
    }
}
