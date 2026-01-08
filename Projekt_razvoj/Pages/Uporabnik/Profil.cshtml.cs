using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt_razvoj.Storitve;
using System.ComponentModel.DataAnnotations;

namespace Projekt_razvoj.Pages.Uporabnik;

[Authorize]
[IgnoreAntiforgeryToken] // TEMPORARY - for debugging
public class ProfilModel : PageModel
{
    private readonly UporabnikiStoritev _users;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ProfilModel> _logger;

    public ProfilModel(UporabnikiStoritev users, IWebHostEnvironment env, ILogger<ProfilModel> logger)
    {
        _users = users;
        _env = env;
        _logger = logger;
    }

    public Projekt_razvoj.Storitve.Uporabnik? Uporabnik { get; private set; }

    [BindProperty]
    [StringLength(100, ErrorMessage = "Lokacija je predolga")]
    public string? Lokacija { get; set; }

    [BindProperty]
    public string? InteresiText { get; set; }

    [BindProperty]
    public IFormFile? NovaFotografija { get; set; }

    public string? Sporocilo { get; set; }

    public string? UploadMessage { get; set; }

    public IActionResult OnGet()
    {
        _logger.LogInformation("OnGet called");
        
        var email = User?.Identity?.Name;
        if (string.IsNullOrEmpty(email))
            return RedirectToPage("/Uporabnik/Prijava");

        Uporabnik = _users.Najdi(email);
        if (Uporabnik is null)
            return NotFound();

        Lokacija = Uporabnik.Lokacija;
        InteresiText = string.Join(", ", Uporabnik.Interesi);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        _logger.LogInformation("=== OnPostAsync START ===");
        
        try
        {
            _logger.LogInformation("Step 1: Getting user email");
            var email = User?.Identity?.Name;
            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning("Email is null or empty, redirecting to login");
                return RedirectToPage("/Uporabnik/Prijava");
            }
            _logger.LogInformation("User email: {Email}", email);

            _logger.LogInformation("Step 2: Finding user");
            Uporabnik = _users.Najdi(email);
            if (Uporabnik is null)
            {
                _logger.LogWarning("User not found: {Email}", email);
                return NotFound();
            }
            _logger.LogInformation("User found: {Email}", email);

            _logger.LogInformation("Step 3: Checking ModelState");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid");
                return Page();
            }

            _logger.LogInformation("Step 4: Parsing interests");
            var interesi = InteresiText?
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Trim())
                .Where(i => !string.IsNullOrWhiteSpace(i))
                .ToList() ?? new List<string>();
            _logger.LogInformation("Parsed {Count} interests", interesi.Count);

            _logger.LogInformation("Step 5: Checking for photo upload");
            string? fotografijaPot = Uporabnik.Fotografija;
            
            if (NovaFotografija is not null && NovaFotografija.Length > 0)
            {
                _logger.LogInformation("Photo upload detected: {FileName}, Size: {Size} bytes", 
                    NovaFotografija.FileName, NovaFotografija.Length);
                
                _logger.LogInformation("Step 5a: Validating file extension");
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(NovaFotografija.FileName).ToLowerInvariant();
                _logger.LogInformation("File extension: {Extension}", extension);
                
                if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
                {
                    _logger.LogWarning("Invalid file extension: {Extension}", extension);
                    ModelState.AddModelError("NovaFotografija", "Dovoljeni so samo JPG, PNG in GIF formati.");
                    return Page();
                }

                _logger.LogInformation("Step 5b: Validating file size");
                if (NovaFotografija.Length > 5 * 1024 * 1024)
                {
                    _logger.LogWarning("File too large: {Size} bytes", NovaFotografija.Length);
                    ModelState.AddModelError("NovaFotografija", "Fotografija je prevelika (max 5MB).");
                    return Page();
                }

                _logger.LogInformation("Step 5c: Checking WebRootPath");
                if (string.IsNullOrEmpty(_env.WebRootPath))
                {
                    _logger.LogError("WebRootPath is null or empty!");
                    ModelState.AddModelError("NovaFotografija", "Napaka pri shranjevanju fotografije.");
                    return Page();
                }
                _logger.LogInformation("WebRootPath: {WebRootPath}", _env.WebRootPath);

                _logger.LogInformation("Step 5d: Creating uploads directory");
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "profili");
                _logger.LogInformation("Uploads folder: {UploadsFolder}", uploadsFolder);
                
                Directory.CreateDirectory(uploadsFolder);
                _logger.LogInformation("Directory created/verified");

                _logger.LogInformation("Step 5e: Generating filename and saving");
                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                _logger.LogInformation("Saving to: {FilePath}", filePath);

                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await NovaFotografija.CopyToAsync(stream);
                }
                _logger.LogInformation("Photo saved successfully");

                _logger.LogInformation("Step 5f: Deleting old photo if exists");
                if (!string.IsNullOrEmpty(Uporabnik.Fotografija))
                {
                    try
                    {
                        var oldPhotoPath = Path.Combine(_env.WebRootPath, Uporabnik.Fotografija.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                        _logger.LogInformation("Old photo path: {OldPhotoPath}", oldPhotoPath);
                        
                        if (System.IO.File.Exists(oldPhotoPath))
                        {
                            System.IO.File.Delete(oldPhotoPath);
                            _logger.LogInformation("Old photo deleted");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to delete old photo");
                    }
                }

                fotografijaPot = $"/uploads/profili/{uniqueFileName}";
                _logger.LogInformation("New photo path: {FotografijaPot}", fotografijaPot);
            }
            else
            {
                _logger.LogInformation("No photo upload");
            }

            _logger.LogInformation("Step 6: Updating profile");
            _users.PosodobiProfil(email, Lokacija, interesi, fotografijaPot);
            _logger.LogInformation("Profile updated successfully");

            Sporocilo = "Profil uspešno posodobljen!";
            _logger.LogInformation("=== OnPostAsync END (SUCCESS) ===");
            
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "EXCEPTION in OnPostAsync: {Message}", ex.Message);
            _logger.LogError("Stack Trace: {StackTrace}", ex.StackTrace);
            
            ModelState.AddModelError(string.Empty, $"Napaka: {ex.Message}");
            
            var email = User?.Identity?.Name;
            if (!string.IsNullOrEmpty(email))
            {
                Uporabnik = _users.Najdi(email);
            }
            
            _logger.LogInformation("=== OnPostAsync END (ERROR) ===");
            return Page();
        }
    }

    public async Task<IActionResult> OnPostUploadPdfAsync(int id, IFormFile? pdfFile)
    {
        if (pdfFile is null || pdfFile.Length == 0)
        {
            UploadMessage = "Izberite PDF datoteko.";
            return Page();
        }

        const long MaxBytes = 10 * 1024 * 1024;
        if (pdfFile.Length > MaxBytes)
        {
            UploadMessage = "Datoteka je prevelika. Najveè 10MB.";
            return Page();
        }

        var ext = Path.GetExtension(pdfFile.FileName);
        var isPdfExtension = ext.Equals(".pdf", StringComparison.OrdinalIgnoreCase);
        var isPdfContentType = string.Equals(pdfFile.ContentType, "application/pdf", StringComparison.OrdinalIgnoreCase);
        if (!isPdfExtension || !isPdfContentType)
        {
            UploadMessage = "Dovoljeni so le PDF dokumenti.";
            return Page();
        }

        var safeName = Path.GetFileNameWithoutExtension(pdfFile.FileName);
        foreach (var c in Path.GetInvalidFileNameChars()) safeName = safeName.Replace(c, '_');
        var fileName = $"{safeName}_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";

        var organizerFolder = Path.Combine(_env.WebRootPath, "uploads", "organizers", id.ToString());
        Directory.CreateDirectory(organizerFolder);

        var filePath = Path.Combine(organizerFolder, fileName);
        using (var stream = System.IO.File.Create(filePath))
        {
            await pdfFile.CopyToAsync(stream);
        }

        UploadMessage = "PDF uspešno naložen.";
        return RedirectToPage(); // refresh the profile page
    }
}

    // Example DTO to align with your existing view usage; replace with your actual model
    public class UporabnikDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
        public string? Lokacija { get; set; }
        public List<string> Interesi { get; set; } = new();
        public string? Fotografija { get; set; }
    }
