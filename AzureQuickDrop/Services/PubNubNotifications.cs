using AzureQuickDrop.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureQuickDrop.Services
{
    //[Export(typeof(INotifications))]
    public class PubNubNotifications
        : INotifications
    {

        public PubNubNotifications()
        {

        }

        public event EventHandler<string> OnReceived;

        public void Publish(string message)
        {

        }
    }
}
