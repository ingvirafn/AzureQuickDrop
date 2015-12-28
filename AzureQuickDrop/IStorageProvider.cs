using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureQuickDrop
{
    public interface IStorageProvider
    {

        void EnqueUpload(string filename);
        void EnqueUploads(string[] filenames);
        event EventHandler<string> FileUploadedSuccess;
        event EventHandler<string> FileUploadedFailed;
        event EventHandler<int> QueueUpdated;

    }
}
