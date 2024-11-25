﻿namespace ByteBox.FileStore.Domain.Entities;

public class User
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string ProfilePictureUrl { get; set; }
}
