using System;
using System.Collections.Generic;
using System.Text;

namespace AirSimulation
{
    public class CommercialAircraft : Aircraft
    {
        public int PassengerCount { get; }

        public CommercialAircraft(string flightNumber, bool isEmergency, int passengerCount) : base(flightNumber, isEmergency)
        {
            if (passengerCount < 0)
            {
                throw new ArgumentException("Passenger count cannot be negative.");
            }

            PassengerCount = passengerCount;
        }

        public override string AircraftType => "Commercial Aircraft";

        public override string GetDescription()
        {
            string description = $"{FlightNumber} - Commercial Aircraft, " + $"Passengers: {PassengerCount}";

            if (IsEmergency)
            {
                description += ", Emergency";
            }

            return description;
        }
    }
}
