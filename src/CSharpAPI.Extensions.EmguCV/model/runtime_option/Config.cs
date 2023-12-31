using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.Extensions.model
{
    public interface Config
    {
        public void set_model(string model_path);
    }
}
