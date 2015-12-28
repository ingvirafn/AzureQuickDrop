using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureQuickDrop
{
    public class AzureCloudStorageUploader
    {
        private string _uri, _accesskey, _account, _container;

        private CloudStorageAccount csa;
        private CloudBlobClient cbc;

        public AzureCloudStorageUploader()
        {
            _uri = System.Configuration.ConfigurationManager.AppSettings["uri"].ToString();
            _account = System.Configuration.ConfigurationManager.AppSettings["account"].ToString();
            _accesskey = System.Configuration.ConfigurationManager.AppSettings["accesskey"].ToString();
            _container = System.Configuration.ConfigurationManager.AppSettings["container"].ToString();
            csa = new CloudStorageAccount(new StorageCredentials(_account, _accesskey), true);
            cbc = csa.CreateCloudBlobClient();
        }

        public async Task Upload(string fullPath, string blockName)
        {
            var bn = blockName.Replace(@"\", @"/");
            if (System.IO.File.Exists(fullPath))
            {
                CloudBlobContainer container = cbc.GetContainerReference(this._container);
                using (var fileStream = System.IO.File.OpenRead(fullPath))
                {
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(bn);
                    await blockBlob.UploadFromStreamAsync(fileStream);
                }
            }

        }
    }
}