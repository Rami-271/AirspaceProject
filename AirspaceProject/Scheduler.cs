using System;
using System.Collections.Generic;
using System.Text;

namespace AirSimulation
{
    public class Scheduler
    {
        private Queue<Flight> landingQueue;
        private Queue<Flight> departureQueue;

        public Scheduler()
        {
            landingQueue = new Queue<Flight>();
            departureQueue = new Queue<Flight>();
        }

        public void AddFlight(Flight flight)
        {
            if (flight.Type == FlightType.Arrival)
            {
                AddToLandingQueue(flight);
            }
            else
            {
                departureQueue.Enqueue(flight);
                flight.Status = FlightStatus.InDepartureQueue;
            }
        }

        private void AddToLandingQueue(Flight flight)
        {
            if (flight.IsEmergency())
            {
                Queue<Flight> newQueue = new Queue<Flight>();

                newQueue.Enqueue(flight);

                foreach (Flight existingFlight in landingQueue)
                {
                    newQueue.Enqueue(existingFlight);
                }

                landingQueue = newQueue;
            }
            else
            {
                landingQueue.Enqueue(flight);
            }

            flight.Status = FlightStatus.InLandingQueue;
        }
    }
}
