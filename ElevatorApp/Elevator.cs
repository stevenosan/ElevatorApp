namespace ElevatorApp;
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

        if (nextFloorVal.HasValue)
        {
            return nextFloorVal.Value > CurrentFloor ? Direction.Up : Direction.Down;
        }
        else
        {
            return nextRequestedPickupFloor > CurrentFloor ? Direction.Up : Direction.Down;
        }
    }

    private int calculateNextFloor()
    {
        if (Direction == Direction.Up)
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
        var request = PendingRequests.First(r => r.Floor == CurrentFloor);
        PendingRequests.Remove(request);
    }

    public void RequestElevator(int passengerFloor, Direction direction)
    {
        PendingRequests.Add(new PendingRequest { RequestedDirection = direction, Floor = passengerFloor });
    }

    public void Move()
    {
        if(Passengers.Any() || PendingRequests.Any())
        {
            if (Direction == Direction.Up)
            {
                CurrentFloor++;
            }
            else
            {
                CurrentFloor--;
            }
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