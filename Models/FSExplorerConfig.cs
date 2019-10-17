namespace mock_api.Models {
    using System.Collections.Generic;    
    using System.IO;
    using System.Linq;

    public class FsExplorerConfig {
        public bool AllowSelect { get; set; }  = true;
        public bool AllowDelete { get; set; }
        public bool AllowCopy { get; set; }
        public bool AllowBackward { get; set; }
        public bool AllowFoward { get; set; }
        public bool AllowUpload { get; set; }
        public bool AllowDownload { get; set; }
        public bool AllowThumbnail { get; set; }
        public bool AllowRename = true;
        public bool AllowPathDefinition = true;
        public bool ShowFileNames = true;
        public string RootDirectory { get; set; }
        public string Selected { get; set; }
        public string CopyTo { get; set; }
        public string MoveTo { get; set; }
        public string RenameTo { get; set; }
    }

    public class FsExplorerFolder {
        public IList<FsExplorerElement> Elements = new List<FsExplorerElement>();
        public FsExplorerFolder(string path) {            
            var folders = Directory.GetDirectories(path);
            var files = Directory.GetFiles(path);
            foreach(var folder in folders){
                var element = new FsExplorerElement(folder);
                Elements.Add(element);
            }

            foreach(var file in files){
                var element = new FsExplorerElement(file);
                Elements.Add(element);
            }
        }
    }

    public class FsExplorerElement {
        public string FullPath { get; set; }
        public string Name { get; set; }
        public string NameWithoutExtension { get; set; }
        public FsExplorerElementType Type { get; set; }
        private string[] AllowedImageExtension = new string[] {".jpg",".jpeg",".png",".gif"};

        public FsExplorerElement(string path) {
            FullPath = path;
            Name = Path.GetFileName(path);
            NameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            if(Path.HasExtension(path)) {
                Type = AllowedImageExtension.Any(e => e.Equals(Path.GetExtension(path).ToLower())) ? FsExplorerElementType.Image : FsExplorerElementType.File;
            }
            else
            {
                Type = FsExplorerElementType.Folder;
            }
        }
    }

    public enum FsExplorerElementType {
        Folder,
        File,
        Image
    }
}