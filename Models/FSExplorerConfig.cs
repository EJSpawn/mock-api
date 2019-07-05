namespace mock_api.Models {
    using System.Collections.Generic;
    public class FSExplorerConfig {
        public bool AllowDelete { get; set; }
        public bool AllowCopy { get; set; }
        public bool AllowBack { get; set; }
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

    public class FSFolder {

    }

    public class FSFile {
        public string path { get; set; }
        public string name { get; set; }
        public string nameWithoutExtension { get; set; }
        public string extension { get; set; }
        public FSFileType type { get; set; }
    }

    public enum FSFileType {
        Folder,
        File,
        image
    }
}