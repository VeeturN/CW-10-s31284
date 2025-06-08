namespace APBD10.DTOs;

public class TripDto
{
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public List<TripDetailsDto> Trips { get; set; }
}

public class TripDetailsDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string DateFrom { get; set; }
    public string DateTo { get; set; }
    public int MaxPeople { get; set; }
    public List<CountryDto> Countries { get; set; }
    public List<ClientTripDto> Clients { get; set; }
}

public class CountryDto
{
    public string Name { get; set; }
}

public class ClientTripDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}