﻿namespace Common.Application.Abstractions
{
    public interface ICurrentUserService
    {
        public int CurrentUserId { get; }
        public string[] UserRole { get; }
    }
}
