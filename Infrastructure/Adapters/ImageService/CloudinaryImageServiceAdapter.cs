using Application.Services.ImageServices;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Adapters.ImageService;

/// <summary>
/// Cloudinary servisine erişim sağlayan Image Servisi adaptörü.
/// </summary>
public class CloudinaryImageServiceAdapter : ImageServiceBase
{
    private readonly Cloudinary _cloudinary;

    /// <summary>
    /// CloudinaryImageServiceAdapter sınıfının yapıcı metodu.
    /// </summary>
    /// <param name="configuration">Uygulama yapılandırmasını temsil eden IConfiguration nesnesi.</param>
    public CloudinaryImageServiceAdapter(IConfiguration configuration)
    {
        // Cloudinary hesap bilgilerini almak için yapılandırmadan ilgili bölümü alıyoruz.
        Account? account = configuration.GetSection("CloudinaryAccount").Get<Account>();
        _cloudinary = new Cloudinary(account);
    }

    /// <summary>
    /// Bir form dosyasını Cloudinary'ye yükler.
    /// </summary>
    /// <param name="formFile">Yüklenmek istenen dosyayı temsil eden IFormFile nesnesi.</param>
    /// <returns>Yüklenen dosyanın URL'sini içeren bir Task.</returns>
    public override async Task<string> UploadAsync(IFormFile formFile)
    {
        await FileMustBeInImageFormat(formFile);

        ImageUploadParams imageUploadParams =
            new ImageUploadParams
            {
                File = new FileDescription(formFile.FileName, stream: formFile.OpenReadStream()),
                UseFilename = false,
                UniqueFilename = true,
                Overwrite = false
            };
        ImageUploadResult imageUploadResult = await _cloudinary.UploadAsync(imageUploadParams);

        return imageUploadResult.Url.ToString();
    }

    /// <summary>
    /// Verilen bir resim URL'sini Cloudinary'den siler.
    /// </summary>
    /// <param name="imageUrl">Silinmek istenen resmin URL'si.</param>
    /// <returns>İşlemin tamamlandığını belirten bir Task.</returns>
    public override async Task DeleteAsync(string imageUrl)
    {
        DeletionParams deletionParams = new DeletionParams(GetPublicId(imageUrl));
        await _cloudinary.DestroyAsync(deletionParams);
    }

    /// <summary>
    /// Verilen bir resim URL'sinden public ID'yi çıkarır.
    /// </summary>
    /// <param name="imageUrl">Resim URL'si.</param>
    /// <returns>Public ID'yi içeren bir dize.</returns>
    private string GetPublicId(string imageUrl)
    {
        int startIndex = imageUrl.LastIndexOf('/') + 1;
        int endIndex = imageUrl.LastIndexOf('.');
        int length = endIndex - startIndex;
        return imageUrl.Substring(startIndex, length);
    }
}
