namespace apbd_7_s22085.DTOs.GetTripApiModels;

public record ClientInfoDto(string FirstName, string LastName);

public record CountryInfoDto(string Name);

public record TripInfoDto(
    string Name,
    String Description,
    DateOnly DateFrom,
    DateOnly DateTo,
    int MaxPeople,
    IEnumerable<CountryInfoDto> Countries,
    IEnumerable<ClientInfoDto> Clients);