# Airspace Project

## Airport Runway and Gate Simulator

Airspace Project is a C# console application that simulates basic airport runway and gate scheduling. The program allows users to add arrival and departure flights, process flights through landing and departure queues, assign available runways and gates, give emergency arrivals priority, and view a chronological event log.

The simulator is intended as an educational project for aviation students, instructors, trainees, and others interested in learning how airport resources can be coordinated. It focuses on runway and gate scheduling rather than attempting to model a complete air traffic control system.

## Main Features

- Add commercial, cargo, and private aircraft.
- Add arrival and departure flights.
- Store regular arrivals in first-in, first-out order.
- Give emergency arrivals priority over normal arrivals.
- Assign available runways to arriving and departing flights.
- Assign available gates to landed aircraft.
- Place landed aircraft in a waiting queue when all gates are occupied.
- Release gates when departure flights take off.
- Prepare a landed arrival flight for departure.
- Track flight status from creation through completion.
- Display landing, departure, and gate-waiting queues.
- Display runway and gate availability.
- Display all flights and their current status.
- Display a chronological event log.
- Load six sample flights for a complete demonstration.
- Reject invalid or duplicate flight information.

## Object-Oriented Programming Concepts

The project demonstrates the following object-oriented programming concepts:

### Abstraction

`Aircraft` is an abstract base class that contains information shared by all aircraft, including flight number and emergency status.

### Inheritance

The following classes inherit from `Aircraft`:

- `CommercialAircraft`
- `CargoAircraft`
- `PrivateAircraft`

Each derived class adds information that applies to its aircraft type.

### Polymorphism

The derived aircraft classes override `AircraftType` and `GetDescription()`. The program can use an `Aircraft` reference while calling the correct overridden method for the actual aircraft object.

### Encapsulation

Classes use private fields and public properties or methods to control access to their data. Validation is used to prevent invalid values from being stored.

### Composition

The `Airport` class contains and coordinates:

- Runways
- Gates
- Flights
- A scheduler
- A conflict detector

A `Flight` also contains an `Aircraft` object.

## Main Classes

| Class | Responsibility |
|---|---|
| `Aircraft` | Abstract base class for common aircraft information |
| `CommercialAircraft` | Represents an aircraft with a passenger count |
| `CargoAircraft` | Represents an aircraft with cargo weight |
| `PrivateAircraft` | Represents an aircraft with an owner name |
| `Flight` | Connects an aircraft to an arrival or departure and tracks its status |
| `Airport` | Stores and coordinates flights, runways, gates, the scheduler, and conflict detector |
| `Scheduler` | Manages landing, departure, and gate-waiting queues |
| `Runway` | Tracks runway availability and the assigned flight |
| `Gate` | Tracks gate availability and the assigned flight |
| `ConflictDetector` | Checks for occupied resources and invalid departure conditions |
| `TestProgram` | Provides the console menu and user interaction |

## Flight Statuses

The program uses the following flight statuses:

- `Created`
- `ReadyForLanding`
- `InLandingQueue`
- `AssignedRunway`
- `Landed`
- `WaitingForGate`
- `AssignedGate`
- `ReadyForDeparture`
- `InDepartureQueue`
- `Departed`
- `Complete`

## Requirements

- Windows computer
- Visual Studio with the .NET 10 SDK installed
- C# console application support

The project currently targets:

```text
net10.0
```

No third-party libraries are required.

## Project Structure

```text
AirspaceProject-master/
|
|-- AirspaceProject.slnx
|-- README.md
|-- .gitignore
|-- .gitattributes
|
`-- AirspaceProject/
    |-- AirspaceProject.csproj
    |-- AircraftClass.cs
    |-- AirportClass.cs
    |-- CargoAircraftClass.cs
    |-- CommercialAircraftClass.cs
    |-- ConflictDetectorClass.cs
    |-- FlightClass.cs
    |-- FlightStatus.cs
    |-- FlightType.cs
    |-- GateClass.cs
    |-- PrivateAircraftClass.cs
    |-- RunwayClass.cs
    |-- Scheduler.cs
    `-- TestProgram.cs
```

## How to Build and Run in Visual Studio

1. Clone or download the repository.
2. Extract the ZIP folder when using the download option.
3. Open `AirspaceProject.slnx` in Visual Studio.
4. In Solution Explorer, confirm that `AirspaceProject` is the startup project.
5. Select **Build > Rebuild Solution**.
6. Confirm that the build reports zero errors.
7. Press **Ctrl + F5** to run the program without debugging.

When the solution file does not open, open the following project file directly:

```text
AirspaceProject/AirspaceProject.csproj
```

## Command-Line Build and Run

From the repository's main folder, use:

```bash
dotnet build AirspaceProject/AirspaceProject.csproj
```

Then run:

```bash
dotnet run --project AirspaceProject/AirspaceProject.csproj
```

## Console Menu

```text
1. Add arrival flight
2. Add departure flight
3. Prepare landed flight for departure
4. Process next flight
5. View flight queues
6. View runways and gates
7. View all flights
8. View event log
9. Load sample flights
0. Exit
```

## Quick Start Demonstration

The fastest way to test the simulator is to use the built-in sample flights.

1. Run the program.
2. Enter `9` to load six sample flights.
3. Enter `5` to view the queues.
4. Confirm that emergency arrival `PV303` appears first in the landing queue.
5. Enter `4` to process the next flight.
6. Continue entering `4` to process the remaining arrivals and departures.
7. Enter `6` to view runway and gate assignments.
8. Enter `7` to view all flight statuses.
9. Enter `8` to view the chronological event log.

The sample simulation contains:

| Flight | Aircraft Type | Operation | Special Information |
|---|---|---|---|
| DP505 | Commercial | Departure | Starts at gate A1 |
| DP606 | Cargo | Departure | Starts at gate A2 |
| CM101 | Commercial | Arrival | Normal arrival |
| CG202 | Cargo | Arrival | Normal arrival |
| PV303 | Private | Arrival | Emergency arrival |
| CM404 | Commercial | Arrival | Normal arrival |

The expected initial landing order is:

```text
PV303
CM101
CG202
CM404
```

This demonstrates that emergency arrivals receive priority while normal arrivals remain in first-in, first-out order.

## Manual Test Scenarios

The program should be tested with the following scenarios:

- Add a normal commercial arrival.
- Add an emergency cargo or private arrival.
- Confirm emergency priority in the landing queue.
- Add departure flights to available gates.
- Attempt to assign two departures to the same gate.
- Process arrivals when all gates are occupied.
- Confirm that landed flights enter the gate-waiting queue.
- Process a departure and confirm that the released gate is assigned to the first waiting flight.
- Prepare a landed flight for departure.
- Confirm that completed flights are removed from active queues.
- Enter an empty flight number.
- Enter an invalid aircraft type.
- Enter an invalid emergency response.
- Enter a negative passenger count or cargo weight.
- Enter a duplicate flight number.
- Enter a gate number that does not exist.
- Attempt to prepare a flight for departure before it has landed and received a gate.

## Automated Testing Status

The current program includes a built-in sample simulation and can be tested manually through the console menu.

Before final project submission, an automated MSTest project should also be included to test:

- Normal first-in, first-out scheduling
- Emergency arrival priority
- Runway conflicts
- Gate conflicts
- Invalid input
- Flight status changes
- Gate-waiting behavior
- Completed flights
- Duplicate flight numbers

After the test project is added, the tests can be run in Visual Studio through:

```text
Test > Test Explorer > Run All Tests
```

## Team Members and Responsibilities

- **Rami** — User-facing features, aircraft input, arrival scheduling, and queue displays
- **Jaskaran** — Landing and departure scheduling, runway assignment, and emergency priority
- **Melissa** — UML diagrams, runway and gate relationships, and resource availability
- **Marcos** — Conflict detection, testing, input validation, and backlog organization

All team members are responsible for reviewing code, testing the completed program, and making meaningful Git commits.

## Known Limitations

- The program is a sequential console simulation and does not use real clock times.
- It prevents a runway or gate from being assigned while occupied, but it does not compare future scheduled time intervals.
- The program starts with two runways and three gates.
- Runways and gates cannot currently be added through the console menu.
- Simulation data is not saved after the program closes.
- The project does not include a graphical user interface.
- The current scheduling rules are implemented directly in `Scheduler` rather than through separate interchangeable Strategy classes.
- Automated unit tests must still be added before final submission.

## Future Improvements

Possible future improvements include:

- Add scheduled arrival and departure times.
- Detect overlapping future time intervals.
- Save and load simulation data.
- Allow users to add runways and gates.
- Add a graphical user interface.
- Add additional emergency and weather conditions.
- Add interchangeable scheduling strategies.
- Add more detailed automated unit-test coverage.

## Third-Party Code and Libraries

This project does not use third-party libraries. It uses the standard C# and .NET class libraries.

