namespace AutoPatcherAdmin
{
    public sealed class PListFile
    {
        public string Path { get; set; } = string.Empty;
        public int Length { get; set; }
    }

    public sealed class PatchCompareResult
    {
        public List<PListFile> Added { get; } = new();
        public List<PListFile> Changed { get; } = new();
        public List<PListFile> Unchanged { get; } = new();
        public List<PListFile> Deleted { get; } = new();

        public int UploadCount => Added.Count + Changed.Count;
        public long UploadBytes => Added.Sum(x => (long)x.Length) + Changed.Sum(x => (long)x.Length);
    }
}
