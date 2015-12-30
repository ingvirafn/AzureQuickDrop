using AzureQuickDrop.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureQuickDrop.Services
{
    [Export(typeof(IStorageProvider))]
    public class StorageProvider
        : IStorageProvider
    {
        public event EventHandler<string> FileUploadedFailed;
        public event EventHandler<string> FileUploadedSuccess;
        public event EventHandler<int> QueueUpdated;

        ConcurrentQueue<string> _fileQueue;
        Thread _uploadThread;
        AzureCloudStorageUploader _uploader;

        [ImportingConstructor]
        public StorageProvider([Import(AllowDefault = true)] INotifications notifications)
        {
            Notifications = notifications;
            if (notifications != null)
                notifications.OnReceived += Notifications_OnReceived;
            _fileQueue = new ConcurrentQueue<string>();
            _uploader = new AzureCloudStorageUploader();
            _uploadThread = new Thread(UploadThread);
            _uploadThread.Start();
        }

        private void Notifications_OnReceived(object sender, string e)
        {

        }

        public INotifications Notifications { get; private set; }

        public void EnqueUpload(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                _fileQueue.Enqueue(filename);
                NotifyQueueLength();
            }
            else
            {
                var di = new System.IO.DirectoryInfo(filename);
                if (di.Exists)
                {
                    var files = di.GetFiles("*", System.IO.SearchOption.AllDirectories);
                    this.EnqueUploads(files.Select(x => x.FullName).ToArray());
                }
            }
        }

        public void EnqueUploads(string[] filenames)
        {
            foreach (var f in filenames)
            {
                this.EnqueUpload(f);
            }
        }

        #region Private

        private async void UploadThread()
        {
            string fileName;

            while (true)
            {
                if (!_fileQueue.IsEmpty && _fileQueue.TryDequeue(out fileName))
                {
                    try
                    {
                        await _uploader.UploadAsync(fileName);

                        EventHandler<string> fus = FileUploadedSuccess;
                        if (fus != null)
                            fus(this, fileName);

                        if (Notifications != null)
                            Notifications.Publish($"upload;{fileName}");

                    }
                    catch (Exception)
                    {
                        EventHandler<string> fuf = FileUploadedFailed;
                        if (fuf != null)
                        {
                            fuf(this, fileName);
                        }

                    }
                    NotifyQueueLength();
                }
            }
        }

        private void NotifyQueueLength()
        {
            EventHandler<int> qu = QueueUpdated;
            if (qu != null)
                qu(this, _fileQueue.Count);
        }

        #endregion
    }
}
