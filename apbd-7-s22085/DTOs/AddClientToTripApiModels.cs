using System.ComponentModel.DataAnnotations;

namespace apbd_7_s22085.DTOs.AddClientToTripApiModels;

public record AddClientToTripRequest(
    [Required] string FirstName,
    [Required] string LastName,
    [Required] [EmailAddress] string Email,
    [Required] string Telephone,
    [Required] string Pesel,
    [Required] int IdTrip,
    [Required] string TripName,
    DateTime? PaymentDate);