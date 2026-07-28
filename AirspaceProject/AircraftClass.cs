using System;
using System.Collections.Generic;
using System.Text;

namespace AirSimulation
{
    public abstract class Aircraft
    {
        public string FlightNumber { get; }
        public bool IsEmergency { get; }

        protected Aircraft(string flightNumber, bool isEmergency)
        {
            if (string.IsNullOrWhiteSpace(flightNumber))
            {
                throw new ArgumentException("Flight number cannot be empty.");
            }

            FlightNumber = flightNumber.Trim().ToUpper();
            IsEmergency = isEmergency;
        }

        public abstract string AircraftType { get; }

        public virtual string GetDescription()
        {
            if (IsEmergency)
            {
                return $"{FlightNumber} - {AircraftType}, Emergency";
            }
            return $"{FlightNumber} - {AircraftType}";
        }
    }
}

