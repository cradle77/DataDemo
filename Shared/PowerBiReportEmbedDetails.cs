using Microsoft.PowerBI.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Shared
{
    public class PowerBiReportEmbedDetails
    {
        public string TokenType { get; set; }

        public EmbedToken EmbedToken { get; set; }

        public string EmbedUrl { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}
