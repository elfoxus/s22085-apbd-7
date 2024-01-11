namespace apbd_7_s22085.DTOs.GetTripApiModels;
// validations not needed, used only for GET requests
public record ClientInfoDto(string FirstName, string LastName);

public record CountryInfoDto(string Name);

public record TripInfoDto(
    string Name,
    String Description,
    String DateFrom,
    String DateTo,
    int MaxPeople,
    IEnumerable<CountryInfoDto> Countries,
    IEnumerable<ClientInfoDto> Clients);