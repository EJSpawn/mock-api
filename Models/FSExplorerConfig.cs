namespace mock_api.Models {
    public class FsExplorerConfig {
        public bool AllowDelete { get; set; }
        public bool AllowCopy { get; set; }
        public bool AllowBackward { get; set; }
        public bool AllowFoward { get; set; }
        public bool AllowUpload { get; set; }
        public bool AllowDownload { get; set; }
        public bool AllowThumbnail { get; set; }
        public string RootDirectory { get; set; }
        public string Selected { get; set; }
        public string CopyTo { get; set; }
        public string MoveTo { get; set; }
        public string RenameTo { get; set; }
    }

    public class FsExplorerFolder {

    }

    public class FsExplorerElement {
        public string path { get; set; }
        public string name { get; set; }
        public string nameWithoutExtension { get; set; }
        public string extension { get; set; }
        public FsExplorerElementType type { get; set; }
    }

    public enum FsExplorerElementType {
        Folder,
        File,
        image
    }
}