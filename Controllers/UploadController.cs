using System.Runtime.InteropServices.ComTypes;
using System.Diagnostics;
using System;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace mock_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        
        [HttpPost]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            try{
                long size = files.Sum(f => f.Length);

                // full path to file in temp location
                FileInfo info = new FileInfo(@"teste");
                Trace.WriteLine("File Info");
                var arqPath = Directory.GetCurrentDirectory() + @"\wwwroot\sys\arq\";
                //var imgPath = @"\sys\img\";

                //AppVarUtils.Set("sys.arq.author", AppVarUtils.Get("sys.arq") + "author/");
                //AppVarUtils.Set("sys.arq.post", AppVarUtils.Get("sys.arq") + "post/");
                //AppVarUtils.Set("sys.arq.partner", AppVarUtils.Get("sys.arq") + "partner/");
                //AppVarUtils.Set("sys.arq.adsense", AppVarUtils.Get("sys.arq") + "adsense/");

                //Profile
                //AppVarUtils.Set("sys.author.profilephoto", $"{AppVarUtils.Get("sys.path.arq.author")}{{0}}/profilePhoto.jpg");
                //AppVarUtils.Set("sys.author.defaultphoto", $"{AppVarUtils.Get("code.templates.monster")}assets/images/users/defaultPhoto.jpg");

                if(!Directory.Exists(arqPath)){
                    //info.Directory.Create();       
                    Directory.CreateDirectory(arqPath);
                }                

                var aux = 1;
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        var filePath = arqPath + @"teste-"+aux+".png";
                        using (var stream = new FileStream(path: filePath, mode: FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                        aux++;
                    }
                }
                // process uploaded files
                // Don't rely on or trust the FileName property without validation.            
                return Ok(new { count = files.Count, size});
            } catch ( Exception e){
                Trace.WriteLine(e);
                return BadRequest();
            }
        }
    }

    public static class SysPath
    {
        public static readonly string ROOT = @"\wwwroot";
        public static readonly string SYS = ROOT + @"\sys";
        public static readonly string ARQ = SYS + @"\arq";
        public static readonly string AUTHOR = ARQ + @"\author";
        public static readonly string POST = ARQ + @"\post";
        public static readonly string PARTNER = ARQ + @"\partner";
    }
}
