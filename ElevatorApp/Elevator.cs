namespace ElevatorApp;
public class Elevator
{
    public int CurrentFloor = 1;
    public int TopFloor => 10;
    public int Time = 0;
    public Direction Direction = Direction.Up;
    public List<Passenger> Passengers = [];

    public List<Floor> Floors = Enumerable.Range(1, 10).Select(x => new Floor(x)).ToList();

    public List<PendingRequest> PendingRequests = new List<PendingRequest>();
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