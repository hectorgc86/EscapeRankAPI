using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ApiEscapeRank.Helpers
{
    public static class StorageHelper
    {
        public static async Task<bool> PostFotoAStorage(IConfiguration configuration, byte[] foto, string nombre)
        {
            string cuenta = configuration.GetSection("StorageSettings").GetSection("Cuenta").Value;
            string clave = configuration.GetSection("StorageSettings").GetSection("Clave").Value;
            string subruta = configuration.GetSection("AppSettings").GetSection("RutaImagenesPartidasRemota").Value;
            string conexion = "https://" + cuenta + ".blob.core.windows.net";

            Stream stream = new MemoryStream(foto);

            Uri blobUri = new Uri(conexion + subruta + nombre);

            StorageSharedKeyCredential storageCredentials =
                new StorageSharedKeyCredential(cuenta, clave);

            BlobClient blobClient = new BlobClient(blobUri, storageCredentials);

            await blobClient.UploadAsync(stream);

            return await Task.FromResult(true);
        }
    }
}
