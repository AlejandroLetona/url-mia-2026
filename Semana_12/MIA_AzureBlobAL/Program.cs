using MIA_AzureBlob.Services;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

class Program
{
    static async Task Main(string[] args)
    {
        string connectionString =
            "DefaultEndpointsProtocol=https;AccountName=miastoragesemana12;AccountKey=Wgq+8CQXO0+0KO04g2lK9DLYeHZk/84Dv9hedTBkjsmyywbbHYQXTmfDtXM8EEYseBkD1f7Oa10c+AStDe/bWg==;EndpointSuffix=core.windows.net";

        string containerName = "miaarchivos";

        BlobServiceClient blobServiceClient =
            new BlobServiceClient(connectionString);

        BlobContainerClient containerClient =
            blobServiceClient.GetBlobContainerClient(containerName);

        await containerClient.CreateIfNotExistsAsync();

        bool salir = false;

        while (!salir)
        {
            Console.Clear();

            Console.WriteLine("=================================");
            Console.WriteLine("     MIA - AZURE BLOB STORAGE");
            Console.WriteLine("=================================");
            Console.WriteLine("1. Subir archivo");
            Console.WriteLine("2. Listar archivos");
            Console.WriteLine("3. Descargar archivo");
            Console.WriteLine("4. Eliminar archivo");
            Console.WriteLine("5. Salir");
            Console.WriteLine("=================================");
            Console.Write("Seleccione una opcion: ");

            string? opcion = Console.ReadLine();

            Console.WriteLine();

            try
            {
                switch (opcion)
                {
                    case "1":
                        await SubirArchivo(containerClient);
                        break;

                    case "2":
                        await ListarArchivos(containerClient);
                        break;

                    case "3":
                        await DescargarArchivo(containerClient);
                        break;

                    case "4":
                        await EliminarArchivo(containerClient);
                        break;

                    case "5":
                        salir = true;
                        Console.WriteLine("Programa finalizado.");
                        break;

                    default:
                        Console.WriteLine("Opcion invalida.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Ocurrio un error durante la operacion.");
                Console.WriteLine(ex.Message);
            }

            if (!salir)
            {
                Console.WriteLine();
                Console.WriteLine("Presione ENTER para continuar...");
                Console.ReadLine();
            }
        }
    }

    static async Task SubirArchivo(
        BlobContainerClient containerClient)
    {
        Console.WriteLine("===== SUBIR ARCHIVO =====");
        Console.WriteLine();

        Console.Write("Ingrese la ruta completa del archivo: ");

        string? ruta = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(ruta))
        {
            Console.WriteLine("Debe ingresar una ruta.");
            return;
        }

        ruta = ruta.Trim('"');

        if (!File.Exists(ruta))
        {
            Console.WriteLine("El archivo indicado no existe.");
            return;
        }

        string nombreArchivo =
            Path.GetFileName(ruta);

        BlobClient blobClient =
            containerClient.GetBlobClient(nombreArchivo);

        await blobClient.UploadAsync(
            ruta,
            overwrite: true
        );

        Console.WriteLine();
        Console.WriteLine("Archivo subido correctamente.");
        Console.WriteLine($"Nombre: {nombreArchivo}");
        Console.WriteLine($"Ruta local: {ruta}");
    }

    static async Task ListarArchivos(
        BlobContainerClient containerClient)
    {
        Console.WriteLine("===== LISTADO DE ARCHIVOS =====");
        Console.WriteLine();

        Console.WriteLine(
            "{0,-40} {1,15}",
            "Nombre",
            "Tamaño"
        );

        Console.WriteLine(
            new string('-', 58)
        );

        bool existenArchivos = false;

        await foreach (
            BlobItem blobItem
            in containerClient.GetBlobsAsync())
        {
            existenArchivos = true;

            long tamano =
                blobItem.Properties.ContentLength ?? 0;

            Console.WriteLine(
                "{0,-40} {1,12} bytes",
                blobItem.Name,
                tamano
            );
        }

        if (!existenArchivos)
        {
            Console.WriteLine(
                "No existen archivos en el container."
            );
        }
    }

    static async Task DescargarArchivo(
        BlobContainerClient containerClient)
    {
        Console.WriteLine("===== DESCARGAR ARCHIVO =====");
        Console.WriteLine();

        Console.Write("Ingrese el nombre del archivo: ");

        string? nombreBlob =
            Console.ReadLine();

        if (string.IsNullOrWhiteSpace(nombreBlob))
        {
            Console.WriteLine(
                "Debe ingresar el nombre del archivo."
            );

            return;
        }

        BlobClient blobClient =
            containerClient.GetBlobClient(nombreBlob);

        bool existe =
            await blobClient.ExistsAsync();

        if (!existe)
        {
            Console.WriteLine(
                "El archivo no existe en Azure."
            );

            return;
        }

        Console.Write(
            "Ingrese la carpeta donde desea guardar el archivo: "
        );

        string? carpetaDestino =
            Console.ReadLine();

        if (string.IsNullOrWhiteSpace(carpetaDestino))
        {
            Console.WriteLine(
                "Debe ingresar una carpeta de destino."
            );

            return;
        }

        carpetaDestino =
            carpetaDestino.Trim('"');

        if (!Directory.Exists(carpetaDestino))
        {
            Directory.CreateDirectory(
                carpetaDestino
            );
        }

        string rutaDestino =
            Path.Combine(
                carpetaDestino,
                nombreBlob
            );

        await blobClient.DownloadToAsync(
            rutaDestino
        );

        Console.WriteLine();
        Console.WriteLine(
            "Archivo descargado correctamente."
        );

        Console.WriteLine(
            $"Ubicación: {rutaDestino}"
        );
    }

    static async Task EliminarArchivo(
        BlobContainerClient containerClient)
    {
        Console.WriteLine("===== ELIMINAR ARCHIVO =====");
        Console.WriteLine();

        Console.Write("Ingrese el nombre del archivo: ");

        string? nombreBlob =
            Console.ReadLine();

        if (string.IsNullOrWhiteSpace(nombreBlob))
        {
            Console.WriteLine(
                "Debe ingresar el nombre del archivo."
            );

            return;
        }

        BlobClient blobClient =
            containerClient.GetBlobClient(nombreBlob);

        bool existe =
            await blobClient.ExistsAsync();

        if (!existe)
        {
            Console.WriteLine(
                "El archivo no existe en Azure."
            );

            return;
        }

        Console.WriteLine();

        Console.Write(
            $"¿Esta seguro de eliminar '{nombreBlob}'? (S/N): "
        );

        string? confirmacion =
            Console.ReadLine();

        if (confirmacion?.Trim().ToUpper() != "S")
        {
            Console.WriteLine(
                "La eliminacion fue cancelada."
            );

            return;
        }

        await blobClient.DeleteAsync();

        Console.WriteLine();
        Console.WriteLine(
            "Archivo eliminado correctamente."
        );
    }
}