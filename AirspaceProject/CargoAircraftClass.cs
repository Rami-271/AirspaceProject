using System;
using System.Collections.Generic;
using System.Text;

namespace AirSimulation
{
    public class CargoAircraft : Aircraft
    {
        public double CargoWeight { get; set; }

        public CargoAircraft(string flightNumber, bool isEmergency, double cargoWeight)
            : base(flightNumber, isEmergency)
        {
            if (cargoWeight < 0)
            {
                throw new ArgumentException("Cargo weight cannot be negative.");
            }

            CargoWeight = cargoWeight;
        }

        public override string AircraftType => "Cargo";

        public override string GetDescription()
        {
            return $"{FlightNumber} - Cargo Aircraft, Cargo Weight: {CargoWeight} lbs";
        }
    }
}
