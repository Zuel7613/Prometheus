namespace PlayWrightTestProject.PlaywrightWrapper
{
    public sealed record TestDriverConfig
    {
        public BrowserKind BrowserKind { get; init; } = BrowserKind.Chromium;
        public bool Headless { get; init; } = true;
        public int SlowMoMs { get; init; } = 0;
        public int ViewportWidth { get; init; } = 1280;
        public int ViewportHeight { get; init; } = 800;
        public string? UserAgent { get; init; }
        public string? StorageStatePath { get; init; }
        public bool AcceptDownloads { get; init; }
        public string? Locale { get; init; } = "en-US";
    }

}