﻿namespace ElevatorApp;
public class Elevator
{
    public int CurrentFloor = 1;
    public int TopFloor => 10;
    public int Time = 0;
    public Direction Direction => calculateDirection();
    private Direction _direction;
    public List<Passenger> Passengers = [];

    public List<PendingRequest> PendingRequests = new List<PendingRequest>();

    private Direction calculateDirection()
    {
        var nextFloorVal = Passengers.FirstOrDefault(p => p.DesiredFloor > CurrentFloor)?.DesiredFloor;

        var nextPickupVal = PendingRequests.FirstOrDefault()?.Floor;
        var nextRequestedPickupFloor = nextPickupVal ??= 0;

        var direction = nextFloorVal > nextRequestedPickupFloor || nextRequestedPickupFloor > CurrentFloor ? Direction.Up : Direction.Down;
        return direction;
    }

    private int calculateNextFloor()
    {
        if(Direction == Direction.Up)
        {
            var destinationFloor = Passengers.Where(p => p.DesiredFloor > CurrentFloor).Select(p => p.DesiredFloor)
                .Concat(PendingRequests.Where(r => r.Floor > CurrentFloor).Select(r => r.Floor)).OrderBy(f => f).First();

            return destinationFloor;
        }
        else
        {
            var destinationFloor = Passengers.Where(p => p.DesiredFloor > CurrentFloor).Select(p => p.DesiredFloor)
                .Concat(PendingRequests.Where(r => r.Floor > CurrentFloor).Select(r => r.Floor)).OrderByDescending(f => f).First();

            return destinationFloor;
        }
    }

    public int DisembarkPassengers()
    {
        return Passengers.RemoveAll(p => p.DesiredFloor == CurrentFloor);
    }

    public void EmbarkPassenger(int destinationFloor)
    {
        var passenger = new Passenger(destinationFloor);
        Passengers.Add(passenger);
        var request = PendingRequests.Single(r => r.Floor == CurrentFloor);
        PendingRequests.Remove(request);


        //if (PendingRequests.Any(r => r.Floor == CurrentFloor))
        //{
        //    //Console.WriteLine($"The elevator is currently {CurrentFloor} at floor with a waiting passenger going {PendingRequests.First().RequestedDirection}. Please enter your desired floor (1-10) for this passenger:");
        //    //var desiredFloor = int.Parse(Console.ReadLine());

        //    var passenger = new Passenger(destinationFloor);
        //    Passengers.Add(passenger);

        //    PendingRequests.RemoveAll(r => r.Floor == CurrentFloor);
        //}
    }

    public void RequestElevator(int passengerFloor, Direction direction)
    {
        PendingRequests.Add(new PendingRequest { RequestedDirection = direction, Floor = passengerFloor });
    }

    public void Move()
    {
        if(Direction == Direction.Up)
        {
            CurrentFloor++;
        }
        else
        {
            CurrentFloor--;
        }
        Time++;
    }
}

public class PendingRequest
{
    public int Floor;
    public Direction RequestedDirection;
}

public class Passenger(int desiredFloor)
{
    public int DesiredFloor = desiredFloor;
}

public enum Direction
{
    Up = 'U',
    Down = 'D',
}