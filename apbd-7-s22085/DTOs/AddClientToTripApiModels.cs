namespace apbd_7_s22085.DTOs.AddClientToTripApiModels;

public record AddClientToTripRequest(
    string FirstName,
    string LastName,
    string Email,
    string Telephone,
    string Pesel,
    int IdTrip,
    string TripName,
    DateOnly PaymentDate);