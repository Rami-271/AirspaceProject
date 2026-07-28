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
                AddDepartureFlight(flight);
            }
        }

        public void AddDepartureFlight(Flight flight)
        {
            if (flight == null)
            {
                throw new ArgumentNullException("flight");
            }

            if (flight.Type != FlightType.Departure)
            {
                throw new InvalidOperationException(
                    "Only departure flights can enter the departure queue.");
            }

            if (flight.Status != FlightStatus.ReadyForDeparture)
            {
                throw new InvalidOperationException(
                    "The flight is not ready for departure.");
            }

            departureQueue.Enqueue(flight);
            flight.UpdateStatus(FlightStatus.InDepartureQueue);

            eventLog.Add(
                $"{flight.Aircraft.FlightNumber} entered the departure queue.");
        }

        private void AddToLandingQueue(Flight flight)
        {
            if (flight.Status != FlightStatus.ReadyForLanding)
            {
                throw new InvalidOperationException(
                    "The flight is not ready for the landing queue.");
            }

            if (flight.IsEmergency())
            {
                Queue<Flight> newQueue = new Queue<Flight>();
                bool flightAdded = false;

                foreach (Flight existingFlight in landingQueue)
                {
                    if (!flightAdded && !existingFlight.IsEmergency())
                    {
                        newQueue.Enqueue(flight);
                        flightAdded = true;
                    }

                    newQueue.Enqueue(existingFlight);
                }

                if (!flightAdded)
                {
                    newQueue.Enqueue(flight);
                }

                landingQueue = newQueue;

                eventLog.Add(
                    $"Emergency alert: {flight.Aircraft.FlightNumber} " +
                    "received landing priority.");
            }
            else
            {
                landingQueue.Enqueue(flight);
            }

            flight.UpdateStatus(FlightStatus.InLandingQueue);

            eventLog.Add(
                $"{flight.Aircraft.FlightNumber} entered the landing queue.");
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

        private bool ProcessArrival(Airport airport)
        {
            if (!airport.HasAvailableRunway())
            {
                eventLog.Add(
                    "No runway is available for the next arrival.");
                return false;
            }

            Runway runway = airport.GetAvailableRunway();

            if (airport.ConflictDetector.HasRunwayConflict(runway))
            {
                eventLog.Add(
                    $"Runway {runway.RunwayNumber} has a conflict.");
                return false;
            }

            Flight flight = landingQueue.Dequeue();

            runway.AssignFlight(flight);
            flight.UpdateStatus(FlightStatus.AssignedRunway);

            eventLog.Add(
                $"{flight.Aircraft.FlightNumber} was assigned to " +
                $"runway {runway.RunwayNumber}.");

            flight.UpdateStatus(FlightStatus.Landed);

            eventLog.Add(
                $"{flight.Aircraft.FlightNumber} landed on " +
                $"runway {runway.RunwayNumber}.");

            runway.ReleaseRunway();

            if (!airport.HasAvailableGate())
            {
                gateWaitingQueue.Enqueue(flight);
                flight.UpdateStatus(FlightStatus.WaitingForGate);

                eventLog.Add(
                    $"{flight.Aircraft.FlightNumber} is waiting for " +
                    "an available gate.");
            }
            else
            {
                Gate gate = airport.GetAvailableGate();

                if (airport.ConflictDetector.HasGateConflict(gate))
                {
                    gateWaitingQueue.Enqueue(flight);
                    flight.UpdateStatus(FlightStatus.WaitingForGate);

                    eventLog.Add(
                        $"{flight.Aircraft.FlightNumber} is waiting for " +
                        "an available gate.");
                }
                else
                {
                    gate.AssignFlight(flight);
                    flight.UpdateStatus(FlightStatus.AssignedGate);

                    eventLog.Add(
                        $"{flight.Aircraft.FlightNumber} was assigned to " +
                        $"gate {gate.GateNumber}.");
                }
            }

            return true;
        }

        private bool ProcessDeparture(Airport airport)
        {
            Flight flight = departureQueue.Peek();

            if (!airport.HasGateForFlight(flight))
            {
                eventLog.Add(
                    $"{flight.Aircraft.FlightNumber} cannot depart " +
                    "because it has no assigned gate.");

                return false;
            }

            if (!airport.HasAvailableRunway())
            {
                eventLog.Add(
                    "No runway is available for the next departure.");

                return false;
            }

            Gate gate = airport.GetGateForFlight(flight);
            Runway runway = airport.GetAvailableRunway();

            if (airport.ConflictDetector.HasRunwayConflict(runway))
            {
                eventLog.Add(
                    $"Runway {runway.RunwayNumber} has a conflict.");
                return false;
            }

            departureQueue.Dequeue();

            runway.AssignFlight(flight);
            flight.UpdateStatus(FlightStatus.AssignedRunway);

            eventLog.Add(
                $"{flight.Aircraft.FlightNumber} was assigned to " +
                $"runway {runway.RunwayNumber} for departure.");

            gate.ReleaseGate();

            eventLog.Add(
                $"Gate {gate.GateNumber} was released by " +
                $"{flight.Aircraft.FlightNumber}.");

            flight.UpdateStatus(FlightStatus.Departed);

            eventLog.Add(
                $"{flight.Aircraft.FlightNumber} departed from " +
                $"runway {runway.RunwayNumber}.");

            runway.ReleaseRunway();
            flight.UpdateStatus(FlightStatus.Complete);

            eventLog.Add(
                $"{flight.Aircraft.FlightNumber} is complete.");

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
                Flight flight = gateWaitingQueue.Peek();
                Gate gate = airport.GetAvailableGate();

                if (airport.ConflictDetector.HasGateConflict(gate))
                {
                    return assignedCount;
                }

                gateWaitingQueue.Dequeue();
                gate.AssignFlight(flight);
                flight.UpdateStatus(FlightStatus.AssignedGate);

                eventLog.Add(
                    $"{flight.Aircraft.FlightNumber} was assigned to " +
                    $"gate {gate.GateNumber}.");

                assignedCount++;
            }

            return assignedCount;
        }

        public void AddEvent(string eventMessage)
        {
            if (string.IsNullOrWhiteSpace(eventMessage))
            {
                throw new ArgumentException(
                    "Event message cannot be empty.");
            }

            eventLog.Add(eventMessage);
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
