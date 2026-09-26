using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace MIA_AzureBlob.Services;

public class AzureBlobService
{
    private readonly BlobContainerClient containerClient;

    public AzureBlobService(string connectionString, string containerName)
    {
        BlobServiceClient blobServiceClient =
            new BlobServiceClient(connectionString);

        containerClient =
            blobServiceClient.GetBlobContainerClient(containerName);
    }

    public async Task InicializarAsync()
    {
        await containerClient.CreateIfNotExistsAsync();
    }


    //Metodo para subir un archivo a Azure Blob Storage
    public async Task SubirArchivoAsync(string ruta)
{
    if (!File.Exists(ruta))
    {
        Console.WriteLine("El archivo indicado no existe.");
        return;
    }

    string nombreArchivo = Path.GetFileName(ruta);

    BlobClient blobClient =
        containerClient.GetBlobClient(nombreArchivo);

    await blobClient.UploadAsync(ruta, overwrite: true);

    Console.WriteLine();
    Console.WriteLine("Archivo subido correctamente.");
    Console.WriteLine($"Nombre: {nombreArchivo}");
    }

    //Metodo para descargar un archivo desde Azure Blob Storage
    public async Task DescargarArchivoAsync(
    string nombreBlob,
    string carpetaDestino)
{
    BlobClient blobClient =
        containerClient.GetBlobClient(nombreBlob);

    if (!await blobClient.ExistsAsync())
    {
        Console.WriteLine("El archivo no existe en Azure.");
        return;
    }

    if (!Directory.Exists(carpetaDestino))
    {
        Directory.CreateDirectory(carpetaDestino);
    }

    string rutaDestino =
        Path.Combine(carpetaDestino, nombreBlob);

    await blobClient.DownloadToAsync(rutaDestino);

    Console.WriteLine();
    Console.WriteLine("Archivo descargado correctamente.");
    Console.WriteLine($"Ubicación: {rutaDestino}");
    }

    //Metodo para eliminar un archivo de Azure Blob Storage

    public async Task EliminarArchivoAsync(string nombreBlob)
{
    BlobClient blobClient =
        containerClient.GetBlobClient(nombreBlob);

    if (!await blobClient.ExistsAsync())
    {
        Console.WriteLine("El archivo no existe en Azure.");
        return;
    }

    Console.Write(
        $"¿Está seguro que desea eliminar '{nombreBlob}'? (S/N): "
    );

    string? respuesta = Console.ReadLine();

    if (respuesta?.Trim().ToUpper() != "S")
    {
        Console.WriteLine("Operación cancelada.");
        return;
    }

    await blobClient.DeleteAsync();

    Console.WriteLine("Archivo eliminado correctamente.");
    }


}