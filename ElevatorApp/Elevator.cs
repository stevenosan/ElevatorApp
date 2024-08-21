namespace ElevatorApp;
public class Elevator
{
    public int MinFloor
    {
        get
        {
            return Direction == Direction.Up ? CurrentFloor + 1 : 1;
        }
    }
    public int MaxFloor
    {
        get
        {
            return Direction == Direction.Up ? TopFloor : CurrentFloor - 1;
        }
    }

    public int CurrentFloor = 1;
    public int TopFloor => 10;
    public int Time = 0;
    public Direction Direction
    {
        get
        {
            if(_finalDestinationFloor == 0)
            {
                return Direction.Waiting;
            }

            return _finalDestinationFloor > CurrentFloor ? Direction.Up : Direction.Down;
        }
    }
    //public Direction Direction = Direction.Waiting;
    public List<Passenger> Passengers = [];
    private int _finalDestinationFloor;

    public List<PendingRequest> PendingRequests = new List<PendingRequest>();

    public int DisembarkPassengers()
    {
        var disembarkCount = Passengers.RemoveAll(p => p.Floor == CurrentFloor);

        if (!Passengers.Any() && !PendingRequests.Any())
        {
            _finalDestinationFloor = 0;
        }

        if (PendingRequests.Any() && Direction == Direction.Waiting)
        {
            _finalDestinationFloor = PendingRequests.First().Floor;
        }

        return disembarkCount;
    }

    public void EmbarkPassengers(int[] desiredPassengerFloors)
    {
        if(!PendingRequests.Any(p => p.Floor == CurrentFloor && p.Direction == Direction))
        {
            return;
        }

        var passengers = desiredPassengerFloors.Select(f => new Passenger(f));

        if (Direction == Direction.Up && desiredPassengerFloors.Max() > _finalDestinationFloor)
        {
            _finalDestinationFloor = desiredPassengerFloors.Max();
        }
        else
        {
            _finalDestinationFloor = desiredPassengerFloors.Min();
        }

        Passengers.AddRange(passengers);

        PendingRequests.RemoveAll(p => p.Floor == CurrentFloor && p.Direction == Direction);
    }

    public void RequestElevator(int passengerFloor, Direction direction)
    {
        if(_finalDestinationFloor == 0)
        {
            _finalDestinationFloor = passengerFloor;
        }

            PendingRequests.Add(new PendingRequest { Direction = direction, Floor = passengerFloor });
    }

    public void Move()
    {
        if (Direction == Direction.Up)
        {
            CurrentFloor++;
        }
        else if (Direction == Direction.Down)
        {
            CurrentFloor--;
        }

        Time++;
    }
}

public class PendingRequest
{
    public int Floor;
    public Direction Direction;
}

public class Passenger(int desiredFloor)
{
    public int Floor = desiredFloor;
}

public enum Direction
{
    Up = 'U',
    Down = 'D',
    Waiting = 'W'
}