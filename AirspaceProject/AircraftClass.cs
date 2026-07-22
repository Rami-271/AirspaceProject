using System;
using System.Collections.Generic;
using System.Text;

namespace AirSimulation
{
    public abstract class Aircraft
    {
        public string FlightNumber { get; set; }
        public bool IsEmergency { get; set; }

        protected Aircraft(string flightNumber, bool isEmergency)
        {
            if (string.IsNullOrWhiteSpace(flightNumber))
            {
                throw new ArgumentException("Flight number cannot be empty.");
            }

            FlightNumber = flightNumber;
            IsEmergency = isEmergency;
        }

        public abstract string AircraftType { get; }

        public virtual string GetDescription()
        {
            return $"{FlightNumber} - {AircraftType}";
        }
    }
}

