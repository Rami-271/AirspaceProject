using System;
using System.Collections.Generic;
using System.Text;

namespace AirSimulation
{
    public class CommercialAircraft : Aircraft
    {
        public int PassengerCount { get; set; }

        public CommercialAircraft(string flightNumber, bool isEmergency, int passengerCount) : base(flightNumber, isEmergency)
        {
            if (passengerCount < 0)
            {
                throw new ArgumentException("Passenger count cannot be negative.");
            }

            PassengerCount = passengerCount;
        }

        public override string AircraftType => "Commercial";

        public override string GetDescription()
        {
            return $"{FlightNumber} - Commercial Aircraft, Passengers: {PassengerCount}";
        }
    }
}
