using System;
namespace Domain.Apartments;

public record Address(
    string Coutry,
    string State,
    string ZipCode,
    string City,
    string Street);
