using Core.CrossCuttingConcerns.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Application.Services.ImageServices;

/// <summary>
/// Resim hizmeti temel sınıfı.
/// </summary>
public abstract class ImageServiceBase
{
    /// <summary>
    /// Bir form dosyasını yükler.
    /// </summary>
    /// <param name="formFile">Yüklenmek istenen dosyayı temsil eden IFormFile nesnesi.</param>
    /// <returns>Yüklenen dosyanın URL'sini içeren bir Task.</returns>
    public abstract Task<string> UploadAsync(IFormFile formFile);

    /// <summary>
    /// Bir form dosyasını günceller.
    /// </summary>
    /// <param name="formFile">Güncellenmek istenen dosyayı temsil eden IFormFile nesnesi.</param>
    /// <param name="imageUrl">Eski resim URL'si.</param>
    /// <returns>Güncellenen dosyanın URL'sini içeren bir Task.</returns>
    public async Task<string> UpdateAsync(IFormFile formFile, string imageUrl)
    {
        await FileMustBeInImageFormat(formFile);

        await DeleteAsync(imageUrl);
        return await UploadAsync(formFile);
    }

    /// <summary>
    /// Bir resmi siler.
    /// </summary>
    /// <param name="imageUrl">Silinmek istenen resmin URL'si.</param>
    /// <returns>İşlemin tamamlandığını belirten bir Task.</returns>
    public abstract Task DeleteAsync(string imageUrl);

    /// <summary>
    /// Bir dosyanın resim formatında olması gerektiğini kontrol eder.
    /// </summary>
    /// <param name="formFile">Kontrol edilecek dosyayı temsil eden IFormFile nesnesi.</param>
    /// <returns>İşlemin tamamlandığını belirten bir Task.</returns>
    /// <exception cref="BusinessException">Desteklenmeyen formatta dosya hatası durumunda fırlatılır.</exception>
    protected async Task FileMustBeInImageFormat(IFormFile formFile)
    {
        List<string> extensions = new List<string> { ".jpg", ".png", ".jpeg", ".webp" };

        string extension = Path.GetExtension(formFile.FileName).ToLower();
        if (!extensions.Contains(extension))
            throw new BusinessException("Unsupported format");
        await Task.CompletedTask;
    }
}