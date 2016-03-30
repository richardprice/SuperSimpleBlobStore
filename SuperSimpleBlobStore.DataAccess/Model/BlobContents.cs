using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleBlobStore.DataAccess.Model
{
    public class BlobContents
    {
        public int Id { get; set; }
        public byte[] Content { get; set; }
    }
}
