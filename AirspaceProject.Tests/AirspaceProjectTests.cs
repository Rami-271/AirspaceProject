using System;
using AirSimulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirspaceProject.Tests
{
    [TestClass]
    public class AirspaceProjectTests
    {
        private static Airport CreateAirport()
        {
            Airport airport = new Airport("Test Airport");

            airport.AddRunway(new Runway("1"));
            airport.AddRunway(new Runway("2"));

            airport.AddGate(new Gate("A1"));
            airport.AddGate(new Gate("A2"));

            return airport;
        }

        [TestMethod]
        public void LandingQueueUsesFifoAndEmergencyPriority()
        {
            Airport airport = CreateAirport();

            airport.AddFlight(new Flight(
                new CommercialAircraft("CM101", false, 100),
                FlightType.Arrival));

            airport.AddFlight(new Flight(
                new CargoAircraft("CG202", false, 2000),
                FlightType.Arrival));

            airport.AddFlight(new Flight(
                new PrivateAircraft("PV303", true, "Taylor"),
                FlightType.Arrival));

            Flight[] queue = airport.Scheduler.GetLandingQueue();

            Assert.AreEqual(3, queue.Length);
            Assert.AreEqual("PV303", queue[0].Aircraft.FlightNumber);
            Assert.AreEqual("CM101", queue[1].Aircraft.FlightNumber);
            Assert.AreEqual("CG202", queue[2].Aircraft.FlightNumber);
        }

        [TestMethod]
        public void OccupiedRunwayAndGateReportConflicts()
        {
            Flight firstFlight = new Flight(
                new CommercialAircraft("CM101", false, 100),
                FlightType.Arrival);

            Flight secondFlight = new Flight(
                new CargoAircraft("CG202", false, 2000),
                FlightType.Arrival);

            ConflictDetector detector = new ConflictDetector();

            Runway runway = new Runway("1");
            runway.AssignFlight(firstFlight);

            Assert.IsTrue(detector.HasRunwayConflict(runway));

            Assert.ThrowsExactly<InvalidOperationException>(() =>
                runway.AssignFlight(secondFlight));

            Gate gate = new Gate("A1");
            gate.AssignFlight(firstFlight);

            Assert.IsTrue(detector.HasGateConflict(gate));

            Assert.ThrowsExactly<InvalidOperationException>(() =>
                gate.AssignFlight(secondFlight));
        }

        [TestMethod]
        public void InvalidAircraftInformationIsRejected()
        {
            Assert.ThrowsExactly<ArgumentException>(() =>
                new CommercialAircraft("", false, 100));

            Assert.ThrowsExactly<ArgumentException>(() =>
                new CommercialAircraft("CM101", false, -1));

            Assert.ThrowsExactly<ArgumentException>(() =>
                new CargoAircraft("CG202", false, -1));
        }

        [TestMethod]
        public void ArrivalProcessingUpdatesStatusAndAssignsGate()
        {
            Airport airport = CreateAirport();

            Flight arrival = new Flight(
                new CommercialAircraft("CM101", false, 100),
                FlightType.Arrival);

            airport.AddFlight(arrival);

            Assert.AreEqual(
                FlightStatus.InLandingQueue,
                arrival.Status);

            bool processed = airport.ProcessNextFlight();

            Assert.IsTrue(processed);

            Assert.AreEqual(
                FlightStatus.AssignedGate,
                arrival.Status);

            Assert.AreSame(
                arrival,
                airport.GetGate("A1").CurrentFlight);

            Assert.IsTrue(
                airport.GetRunway("1").IsAvailable);
        }

        [TestMethod]
        public void DepartureCompletesAndReleasesGate()
        {
            Airport airport = CreateAirport();

            Flight departure = new Flight(
                new CommercialAircraft("DP505", false, 120),
                FlightType.Departure);

            airport.AssignDepartureGate(departure, "A1");
            airport.AddFlight(departure);

            Assert.IsFalse(
                airport.GetGate("A1").IsAvailable);

            bool processed = airport.ProcessNextFlight();

            Assert.IsTrue(processed);

            Assert.AreEqual(
                FlightStatus.Complete,
                departure.Status);

            Assert.IsTrue(
                airport.GetGate("A1").IsAvailable);

            Assert.AreEqual(
                0,
                airport.Scheduler.GetDepartureQueue().Length);
        }
    }
}
