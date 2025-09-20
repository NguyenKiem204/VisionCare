using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class TokenUser
{
    public int TokenId { get; set; }

    public string? TokenUser1 { get; set; }

    public int? AccountId { get; set; }

    public DateTime? CreatedDateToken { get; set; }

    public virtual Account? Account { get; set; }
}
