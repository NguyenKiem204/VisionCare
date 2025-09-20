using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class TokenGoogle
{
    public int TokenId { get; set; }

    public string? Token { get; set; }

    public DateTime? CreatedDate { get; set; }
}
