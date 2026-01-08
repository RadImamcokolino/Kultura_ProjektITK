using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;

namespace Projekt_razvoj.Pages.Admin
{
    public class OrganizerPdfsModel : PageModel
    {
        private readonly IWebHostEnvironment _env;

        public OrganizerPdfsModel(IWebHostEnvironment env)
        {
            _env = env;
        }

        [BindProperty(SupportsGet = true)]
        public int OrganizerId { get; set; }

        public List<PdfInfo> Pdfs { get; } = new();

        public void OnGet(int id)
        {
            OrganizerId = id;
            var folder = Path.Combine(_env.WebRootPath, "uploads", "organizers", id.ToString());

            if (!Directory.Exists(folder))
                return;

            var files = Directory.GetFiles(folder, "*.pdf", SearchOption.TopDirectoryOnly);
            foreach (var path in files)
            {
                var fi = new FileInfo(path);
                Pdfs.Add(new PdfInfo
                {
                    FileName = fi.Name,
                    Url = $"/uploads/organizers/{OrganizerId}/{fi.Name}",
                    SizeKb = (fi.Length / 1024)
                });
            }
        }

        public class PdfInfo
        {
            public string FileName { get; set; } = "";
            public string Url { get; set; } = "";
            public long SizeKb { get; set; }
        }
    }
}