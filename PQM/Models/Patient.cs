using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PQM.Models
{
    public class Patient
    {
        public string PatientId { get; }
        public Graph currentDensity { get; set; }
        public Graph eField { get; set; }
        public Graph absorbtionRate { get; set; }

    }
}
