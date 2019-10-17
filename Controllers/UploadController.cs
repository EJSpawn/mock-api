using mock_api.Models;
using System.Diagnostics;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace mock_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FsExplorerController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        // Refresh
        // Delete
        // Upload        
        // Thumbnail
        // Backward
        // Foward
        // Copy
        // Move

        public FsExplorerController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        private string GetRootDirectory(String rootDirectory) {
            switch(rootDirectory.ToLower()){
                case "author":
                    return SysPath.AUTHOR;
                case "adsense":
                    return SysPath.ADSENSE;
                case "partner":
                    return SysPath.PARTNER;
                case "post":                
                    return SysPath.POST;
                default:
                    throw new FileNotFoundException();
            }
        }

        private string GetExtension(IFormFile file) {
            if(!string.IsNullOrEmpty(Path.GetExtension(file.FileName))){
                return Path.GetExtension(file.FileName).ToLower();
            }
            
            switch(file.ContentType){
                case "image/jpeg":
                    return ".jpg";
                case "image/gif":
                    return ".gif";
                case "image/png":
                    return ".png";
                default:
                    return "";
            }
        }

        [HttpPost("refresh/")]
        public IActionResult Refresh([FromForm]FsExplorerDTO dto)
        {
            var directory = Path.Combine(GetRootDirectory(dto.RootDirectory), dto.Folder);

            if(Directory.Exists(directory))
            {
                return Ok(new FsExplorerFolder(directory).Elements);
            }
            return BadRequest(dto);
        }
        
        [HttpPost("upload/")]
        public async Task<IActionResult> Upload([FromForm]FsExplorerDTO  dto)
        {   
            try{
                var size = dto.Files.Sum(f => f.Length);
                if(size == 0){
                    return BadRequest();
                }

                var directory = Path.Combine(GetRootDirectory(dto.RootDirectory), dto.Folder);
                if(!Directory.Exists(directory)){
                    Directory.CreateDirectory(directory);
                }
                
                var fileNameOldList = new List<string>();
                var fileNameNewList = new List<string>();
                foreach (var file in dto.Files)
                {
                    if (!(file?.Length > 0)) continue;
                    var index = 0;
                    var filePath = $"{Path.Combine(directory, dto.FileName)}-{index}{GetExtension(file).ToLower()}";
                    do
                    {
                        index++;
                        filePath = $"{Path.Combine(directory, dto.FileName)}-{index}{GetExtension(file).ToLower()}";
                        Trace.WriteLine(filePath);
                    } while (System.IO.File.Exists(filePath));
                    
                    using (var stream = new FileStream(path: filePath, mode: FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }                        

                    fileNameOldList.Add(GetExtension(file));
                    fileNameNewList.Add(filePath);
                }
                
                return Ok(fileNameOldList);
            } catch ( Exception e){
                Trace.WriteLine(e);
                return BadRequest();
            }
        }

        [HttpPost("copy/")]
        public IActionResult Copy([FromBody]FsExplorerDTO dto)
        {
            var selectFilePath = Path.Combine(GetRootDirectory(dto.RootDirectory), dto.Folder, dto.Selected);
            var copyToToFilePath = Path.Combine(GetRootDirectory(dto.RootDirectory), dto.CopyTo, dto.Selected);

            if(Directory.Exists(selectFilePath))
            {
                if (System.IO.File.Exists(copyToToFilePath))
                {
                    //Message File Exists, Overwrite?
                    return BadRequest();
                }
                System.IO.File.Copy(selectFilePath, copyToToFilePath);
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("move/")]
        public IActionResult Move([FromBody]FsExplorerDTO dto)
        {
            var selectFilePath = Path.Combine(GetRootDirectory(dto.RootDirectory), dto.Folder, dto.Selected);
            var moveToToFilePath = Path.Combine(GetRootDirectory(dto.RootDirectory), dto.MoveTo, dto.Selected);

            if(Directory.Exists(selectFilePath))
            {
                if (System.IO.File.Exists(moveToToFilePath))
                {
                    //Message File Exists, Overwrite?
                    return BadRequest();
                }
                System.IO.File.Move(selectFilePath, moveToToFilePath);
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("rename/")]
        public IActionResult Rename([FromBody]FsExplorerDTO dto)
        {
            var selectFilePath = Path.Combine(GetRootDirectory(dto.RootDirectory), dto.Folder, dto.Selected);
            var renameToFilePath = Path.Combine(GetRootDirectory(dto.RootDirectory), dto.Folder, dto.RenameTo);

            if(Directory.Exists(selectFilePath))
            {
                if (System.IO.File.Exists(renameToFilePath))
                {
                    //Message File Exists
                    return BadRequest();
                }
                System.IO.File.Move(selectFilePath, renameToFilePath);
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("delete/")]
        public IActionResult Delete([FromForm]FsExplorerDTO dto)
        {
            var selectFilePath = Path.Combine(GetRootDirectory(dto.RootDirectory), dto.Folder, dto.Selected);

            if(System.IO.File.Exists(selectFilePath))
            {
                System.IO.File.Delete(selectFilePath);
                return Ok(new {message = "Arquivo deletado com sucesso"});
            }

            return BadRequest("Arquivo não existe:" + dto.Selected);
        }
    }

    public class FsExplorerDTO
    {
        public IList<IFormFile> Files {get; set;} = new List<IFormFile>();
        public string RootDirectory {get; set;}
        public string Folder {get; set;}
        public string FileName{get; set;}
        public string Selected { get; set; }
        public string CopyTo { get; set; }
        public string MoveTo { get; set; }
        public string RenameTo { get; set; }
    }

    public static class SysPath
    {
        public static readonly string ROOT = Directory.GetCurrentDirectory() + @"\wwwroot";
        public static readonly string SYS = ROOT + @"\sys";
        public static readonly string ARQ = SYS + @"\arq";
        public static readonly string AUTHOR = ARQ + @"\author";
        public static readonly string POST = ARQ + @"\post";
        public static readonly string PARTNER = ARQ + @"\partner";
        public static readonly string ADSENSE = ARQ + @"\adsense";
        public static readonly string PROFILE_PHOTO = AUTHOR + @"\{{0}}\profilePhoto.jpg";
        public static readonly string PROFILE_PHOTO_DEFAULT = AUTHOR + @"\default\defaultPhoto.jpg";
    }
}
