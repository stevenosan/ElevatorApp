namespace ElevatorApp;
public class Elevator
{
    public int CurrentFloor = 1;
    public int TopFloor => 10;
    public int Time = 0;
    public Direction Direction => calculateDirection();
    public List<Passenger> Passengers = [];

    public List<Floor> Floors = Enumerable.Range(1, 10).Select(x => new Floor(x)).ToList();

    public List<PendingRequest> PendingRequests = new List<PendingRequest>();

    private Direction calculateDirection()
    {
        var nextFloorVal = Passengers.FirstOrDefault(p => p.DesiredFloor > CurrentFloor)?.DesiredFloor;

        var nextPickupVal = PendingRequests.FirstOrDefault()?.Floor;
        var nextRequestedPickupFloor = nextPickupVal ??= 0;

        return nextFloorVal > nextRequestedPickupFloor && nextRequestedPickupFloor < CurrentFloor ? Direction.Up : Direction.Down;
    }

    private int calculateNextDestinationFloor()
    {

    }

    public void DisembarkPassengers()
    {

    }

    public void EmbarkPassenger()
    {

    }

    public void RequestElevator()
    {

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
    Up =  'U',
    Down = 'D',
}

public class Floor
{
    public int FloorNumber;
    public Passenger WaitingPassenger;

    public Floor(int floorNumber)
    {
        FloorNumber = floorNumber;
    }
}