using System;

namespace AirSimulation
{
    public class PrivateAircraft : Aircraft
    {
        public string OwnerName { get; }

        public PrivateAircraft(string flightNumber, bool isEmergency, string ownerName)
            : base(flightNumber, isEmergency)
        {
            if (string.IsnullOrWhiteSpace(ownerName))
            {
                throw new ArgumentException("Owner name cannot be empty.");
            }
            
            OwnerName = ownerName.Trim();
        }

        public override string AircraftType => "Private Aircraft";

        public override string GetDescription()
        {
            string description = $"{FlightNumber} - Private Aircraft, " +
                $"Owner: {OwnerName}";

            if (IsEmergency)
            {
                description += ", Emergency";
            }

            return description;
        }
    }
}
