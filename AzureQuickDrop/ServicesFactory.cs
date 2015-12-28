using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;

namespace AzureQuickDrop
{
    public class ServicesFactory
    {
        private static readonly ServicesFactory _instance = new ServicesFactory();

        public static ServicesFactory Instance
        {
            get { return _instance; }
        }

        private System.ComponentModel.Composition.Hosting.CompositionContainer _cc;

        public ServicesFactory()
        {
            AssemblyCatalog ac = new AssemblyCatalog(typeof(ServicesFactory).Assembly);
            _cc = new CompositionContainer(ac);

        }
        public T Get<T>()
        {
            return _cc.GetExportedValueOrDefault<T>();
        }
        public void SatisfyImportsOnce(object attributedObject)
        {
            _cc.SatisfyImportsOnce(attributedObject);
        }
    }
}
