using ElevatorApp;
using FluentAssertions;
using NUnit;

namespace ElevatorTests;

public class Tests
{
    Elevator elevator;


    [SetUp]
    public void Setup()
    {
        elevator = new Elevator();
    }

    [Test]
    public void GivenNoOneOnElevatorOrRequestedStops_WhenMove_ThenDoNotMoveAndPassTime()
    {
        elevator.CurrentFloor = 1;
        elevator.Time = 5;

        elevator.Move();
        elevator.CurrentFloor.Should().Be(1);
        elevator.Time.Should().Be(6);
    }

    [Test]
    public void GivenDirectionIsUp_WhenMove_ThenIncrementFloorByOneAndPassTimeByOne()
    {
        elevator.CurrentFloor = 1;
        elevator.Time = 5;

        elevator.Passengers.Add(new Passenger(3));

        elevator.Move();
        elevator.CurrentFloor.Should().Be(2);
        elevator.Time.Should().Be(6);
    }

    [Test]
    public void GivenDirectionIsDown_WhenMove_ThenDecrementFloorByOneAndPassTimeByOne()
    {
        elevator.CurrentFloor = 5;
        elevator.Time = 5;

        elevator.Passengers.Add(new Passenger(3));

        elevator.Move();
        elevator.CurrentFloor.Should().Be(4);
        elevator.Time.Should().Be(6);
    }

    [Test]
    public void GivenElevatorAtFloorWithPassengers_WhenDisembarkPassengers_ThenRemovePassengersWithDesiredFloor()
    {
        var passengerOne = new Passenger(1);
        var passengerTwo = new Passenger(2);
        var passengerTwoTwo = new Passenger(2);
        var passengerThree = new Passenger(3);

        elevator.Passengers.AddRange(new List<Passenger> { passengerOne, passengerTwo, passengerTwoTwo, passengerThree });

        elevator.CurrentFloor = 2;

        elevator.DisembarkPassengers();

        elevator.Passengers.Count.Should().Be(2);
        elevator.Passengers.Count(p => p.DesiredFloor == 2).Should().Be(0);
    }

    [Test]
    public void GivenDestinationFloor_WhenEmbarkPassenger_ThenPassengerAddedToEndWithDestinationFloor()
    {
        var currentPassenger = new Passenger(5);
        elevator.PendingRequests.Add(new PendingRequest { RequestedDirection = Direction.Up, Floor = 1 });
        elevator.Passengers.Add(currentPassenger);

        var destinationFloor = 1234;

        elevator.EmbarkPassenger(destinationFloor);

        elevator.Passengers.Count.Should().Be(2);
        elevator.Passengers.Last().DesiredFloor.Should().Be(1234);

    }

    [Test]
    public void GivenOnBoardPassengerThatWantsToGoUp_WhenGetDirection_ThenDirectionIsUp()
    {
        var passenger = new Passenger(5);
        elevator.Passengers.Add(passenger);

        var result = elevator.Direction;
        result.Should().Be(Direction.Up);
    }

    [Test]
    public void GivenOnBoardPassengerThatWantsToGoUp_ButWaitingPassengerWantsToGoDown_WhenGetDirection_ThenDirectionIsUp()
    {
        var passenger = new Passenger(5);

        var request = new PendingRequest { Floor = 3, RequestedDirection = Direction.Down };

        elevator.Passengers.Add(passenger);
        elevator.PendingRequests.Add(request);

        var result = elevator.Direction;
        result.Should().Be(Direction.Up);
    }

    [Test]
    public void GivenOnBoardPassengerThatWantsToGoDown_AndWaitingPassengerWantsToGoDown_WhenGetDirection_ThenDirectionIsDown()
    {
        elevator.CurrentFloor = 7;
        var passenger = new Passenger(5);

        var request = new PendingRequest { Floor = 3, RequestedDirection = Direction.Down };

        elevator.Passengers.Add(passenger);
        elevator.PendingRequests.Add(request);

        var result = elevator.Direction;
        result.Should().Be(Direction.Down);
    }

    [Test]
    public void GivenNoOnboardPassengerAndRequestIsAbove_WhenGetDirection_ThenDirectionIsUp()
    {
        var request = new PendingRequest { Floor = 3, RequestedDirection = Direction.Down };

        elevator.PendingRequests.Add(request);

        var result = elevator.Direction;
        result.Should().Be(Direction.Up);
    }

    [Test]
    public void GivenNoOnboardPassengerAndRequestIsBelow_WhenGetDirection_ThenDirectionIsDown()
    {
        elevator.CurrentFloor = 5;
        var request = new PendingRequest { Floor = 3, RequestedDirection = Direction.Down };

        elevator.PendingRequests.Add(request);

        var result = elevator.Direction;
        result.Should().Be(Direction.Down);
    }
}