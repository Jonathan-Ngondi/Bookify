﻿namespace Infrastructure.Authentication
{
    public sealed class KeyCloakOptions
    {
        public string AdminUrl { get; init; } = string.Empty;
        public string TokenUrl { get; init; } = string.Empty;
        public string AdminClientId { get; init; } = string.Empty;
        public string AdminClientSecret { get; init; } = string.Empty;
        public string AuthClientId { get; init; } = string.Empty;
        public string AuthClientSecret { get; init; } = string.Empty;

    }
}