# Laboratorio No. 2 - Azure Blob Storage - 1304424

1. Objetivo

Desarrollar una aplicación de consola en C# capaz de conectarse
a Azure Blob Storage y realizar operaciones básicas de archivos.

2. Tecnologías utilizadas

- C#
- .NET
- Azure Blob Storage
- Azure.Storage.Blobs
- Visual Studio Code

3. Configuración de Azure

Storage Account:
miastoragesemana12

Container:
miaarchivos

Nivel de acceso:
Private

4. Arquitectura de la solución

La aplicación utiliza las clases:

- BlobServiceClient
- BlobContainerClient
- BlobClient

5. Operaciones

5.1 Subir archivo

Permite seleccionar un archivo local y almacenarlo en Azure.

5.2 Listar archivos

Muestra los blobs existentes y su tamaño.

5.3 Descargar archivo

Permite descargar un blob a una carpeta local.

5.4 Eliminar archivo

Permite eliminar un blob después de solicitar confirmación.

5.5 Manejo de errores

La aplicación valida:

- rutas vacías
- archivos inexistentes
- blobs inexistentes
- errores de conexión
- errores durante operaciones de Azure

6. Seguridad

La Connection String se almacena en una variable de entorno.

No se almacena la Account Key dentro del código fuente.

7. Ejecución

Instalar el paquete:

dotnet add package Azure.Storage.Blobs

Configurar la variable:

$env:AZURE_STORAGE_CONNECTION_STRING="DefaultEndpointsProtocol=https;AccountName=miastoragesemana12;AccountKey=Wgq+8CQXO0+0KO04g2lK9DLYeHZk/84Dv9hedTBkjsmyywbbHYQXTmfDtXM8EEYseBkD1f7Oa10c+AStDe/bWg==;EndpointSuffix=core.windows.net"

Ejecutar:

dotnet run
