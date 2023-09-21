﻿using System.ComponentModel.DataAnnotations;

namespace AppointmentMaker.Application.Models.Identity.Authentication;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}
