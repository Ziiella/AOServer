using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AOServer
{
    class YAML
    {
        List<Dictionary<string, string>> DataList;
        Dictionary<string, string> DataDict;
        public YAML(string filePath)
        {


            DataList = new List<Dictionary<string, string>>();
            DataDict = new Dictionary<string, string>();
        }

    }
}
