﻿using System;

namespace Riddleyinnai.Database.Model
{
    public class Warn
    {
        public int Warnid { get; set; } = 0;
        public string UserId { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string Admin { get; set; } = string.Empty;
        public string Time { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd");
    }
}
