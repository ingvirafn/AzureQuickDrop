using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureQuickDrop.Contracts
{
    /// <summary>
    /// Used for send out notifications to other clients 
    /// (because f.eg. Azure Storage does not provide any notification system)
    /// </summary>
    public interface INotifications
    {
        void Publish(string message);
        event EventHandler<string> OnReceived;
    }
}
