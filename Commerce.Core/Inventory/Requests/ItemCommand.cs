﻿using Commerce.Infrastructure.CQRS;

namespace Commerce.Core.Identity.Requests;

public class ItemCommand : Command
{
    public object? Argument { get; set; }
}
