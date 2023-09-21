﻿namespace AppointmentMaker.Application.Models.Identity.Authentication;

public class JwtSettings
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public double DurationInMinutes { get; set; }
}
