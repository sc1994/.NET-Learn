using System;
using System.Collections.Generic;

namespace EFDemo.Models
{
    public partial class TouTiaoInformationContent
    {
        public long Ticid { get; set; }
        public long Tuctiid { get; set; }
        public string Tictitle { get; set; }
        public string TicimageUrls { get; set; }
        public string Tichtml { get; set; }
        public string Ticcontent { get; set; }
    }
}
