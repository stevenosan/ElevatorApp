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
    public void GivenDirectionIsUp_WhenMove_ThenIncrementFloorByOneAndPassTimeByOne()
    {
        elevator.Direction = Direction.Up;
        elevator.CurrentFloor = 1;
        elevator.Time = 5;

        elevator.Move();
        elevator.CurrentFloor.Should().Be(2);
        elevator.Time.Should().Be(6);
    }


    [Test]
    public void GivenDirectionIsDown_WhenMove_ThenDecrementFloorByOneAndPassTimeByOne()
    {
        elevator.Direction = Direction.Down;
        elevator.CurrentFloor = 5;
        elevator.Time = 5;

        elevator.Move();
        elevator.CurrentFloor.Should().Be(4);
        elevator.Time.Should().Be(6);
    }

    [Test]
    public void GivenDirectionIsWaiting_WhenMove_ThenFloorDoesNotChangeAndPassTimeByOne()
    {
        elevator.Direction = Direction.Waiting;
        elevator.CurrentFloor = 5;
        elevator.Time = 5;

        elevator.Move();
        elevator.CurrentFloor.Should().Be(5);
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
        elevator.Passengers.Count(p => p.Floor == 2).Should().Be(0);
    }

    [Test]
    public void GivenNoMorePassengersOrPendingRequests_WhenDisembarkPassengers_ThenSetDirectionToWaiting()
    {
        var currentFloor = 5;

        elevator.CurrentFloor = currentFloor;
        var passenger = new Passenger(5);
        elevator.Passengers.Add(passenger);
        elevator.Direction = Direction.Up;

        elevator.DisembarkPassengers();

        elevator.Direction.Should().Be(Direction.Waiting);
    }

    [Test]
    public void GivenPassengersStillOn_WhenDisembarkPassengers_DirectionDoesNotChange()
    {
        var currentFloor = 5;

        elevator.CurrentFloor = currentFloor;
        var passenger = new Passenger(8);
        elevator.Passengers.Add(passenger);
        elevator.Direction = Direction.Up;

        elevator.DisembarkPassengers();

        elevator.Direction.Should().Be(Direction.Up);
    }

    [Test]
    public void GivenNoMorePassengersButPendingRequests_WhenDisembarkPassengers_ThenSetDirectionToFirstPendingRequest()
    {
        var currentFloor = 5;
        elevator.CurrentFloor = 5;
        var pendingRequest1 = new PendingRequest { Direction = Direction.Up };
        var pendingRequest2 = new PendingRequest { Direction = Direction.Down };

        elevator.PendingRequests.Add(pendingRequest1);
        elevator.PendingRequests.Add(pendingRequest2);

        elevator.Direction = Direction.Down;

        elevator.DisembarkPassengers();

        elevator.Direction.Should().Be(Direction.Up);
    }

    [Test]
    public void GivenPassengersToDisembark_WhenDisembarkPassengers_ThenRemoveThosePassengersAndReturnCount()
    {
        var currentFloor = 5;

        var passenger1 = new Passenger(5);
        var passenger2 = new Passenger(6);

        elevator.CurrentFloor = 5;
        elevator.Passengers.Add(passenger1);
        elevator.Passengers.Add(passenger2);

        var result = elevator.DisembarkPassengers();

        result.Should().Be(1);
        elevator.Passengers.Count().Should().Be(1);
        elevator.Passengers.Single().Should().Be(passenger2);
    }

    [Test]
    public void GivenElevatorIsWaiting_WhenRequestElevator_ThenSetDirectionToRequestedDirection()
    {
        var floor = 5;
        var direction = Direction.Up;

        elevator.RequestElevator(floor, direction);

        elevator.Direction.Should().Be(Direction.Up);
    }

    [Test]
    public void GivenElevatorIsNotWaiting_WhenRequestElevator_ThenDoNotChangeDirection()
    {
        var floor = 5;
        var direction = Direction.Up;

        elevator.Direction = Direction.Down;
        elevator.RequestElevator(floor, direction);

        elevator.Direction.Should().Be(Direction.Down);
    }

    [Test]
    public void GivenFloorAndDirection_WhenRequestElevator_ThenAddPendingRequest()
    {
        var floor = 5;
        var direction = Direction.Up;

        elevator.RequestElevator(floor, direction);

        elevator.Direction.Should().Be(Direction.Up);
    }

    [Test]
    public void GivenDirectionIsUp_WhenGetMinFloor_ThenReturnCurrentFloorPlusOne()
    {
        var currentFloor = 5;

        elevator.CurrentFloor = currentFloor;
        elevator.Direction = Direction.Up;

        var result = elevator.MinFloor;

        result.Should().Be(6);
    }

    [Test]
    public void GivenDirectionIsDown_WhenGetMinFloor_ThenReturnOne()
    {
        var currentFloor = 5;

        elevator.CurrentFloor = currentFloor;
        elevator.Direction = Direction.Down;

        var result = elevator.MinFloor;

        result.Should().Be(1);
    }

    [Test]
    public void GivenDirectionIsUp_WhenGetMaxFloor_ThenReturnTopFloor()
    {
        var currentFloor = 5;

        elevator.CurrentFloor = currentFloor;
        elevator.Direction = Direction.Up;

        var result = elevator.MaxFloor;

        result.Should().Be(elevator.TopFloor);
    }

    [Test]
    public void GivenDirectionIsDown_WhenGetMaxFloor_ThenReturnCurrentFloorMinusOne()
    {
        var currentFloor = 5;

        elevator.CurrentFloor = currentFloor;
        elevator.Direction = Direction.Down;

        var result = elevator.MaxFloor;

        result.Should().Be(4);
    }

    [Test]
    public void GivenArrayOfPassengerFloors_WhenEmbarkPassengers_ThenAddPassengers()
    {
        var floors = Enumerable.Range(2, 6).Select(i => i).ToArray();

        elevator.CurrentFloor = 5;
        elevator.PendingRequests.AddRange(floors.Select(i => new PendingRequest { Floor = 5, Direction = Direction.Up }));
        elevator.Direction = Direction.Up;

        elevator.EmbarkPassengers(floors);
        elevator.Passengers.Count.Should().Be(6);
    }

    [Test]
    public void GivenArrayOfPassengerFloors_WhenEmbarkPassengers_ThenRemoveAllPendingRequestsForFloorAndDirection()
    {
        var floors = Enumerable.Range(2, 6).Select(i => i).ToArray();

        elevator.CurrentFloor = 5;
        elevator.PendingRequests.AddRange(floors.Select(i => new PendingRequest { Floor = 5, Direction = Direction.Up }));
        elevator.PendingRequests.Add(new PendingRequest { Floor = 5, Direction = Direction.Down });
        elevator.Direction = Direction.Up;

        elevator.EmbarkPassengers(floors);
        elevator.PendingRequests.Count.Should().Be(1);

    }

    [Test]
    public void GivenNoPendingRequestsAtFloorInDirection_WhenEmbarkPassengers_ThenDoNothing()
    {
        var floors = Enumerable.Range(2, 6).Select(i => i).ToArray();

        elevator.CurrentFloor = 5;
        elevator.PendingRequests.AddRange(floors.Select(i => new PendingRequest { Floor = 6, Direction = Direction.Up }));
        elevator.Direction = Direction.Up;

        elevator.EmbarkPassengers(floors);
        elevator.Passengers.Count.Should().Be(0);
        elevator.PendingRequests.Count.Should().Be(6);
    }
}