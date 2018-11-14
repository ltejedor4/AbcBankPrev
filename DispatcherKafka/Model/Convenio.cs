using System;
using System.Collections.Generic;
using System.Text;

namespace DispatcherKafka.Model
{
    public class Convenio
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string CasoUso { get; set; }
        public string BaseUrl { get; set; }
        public bool Estado { get; set; }
        public string Metodo { get; set; }
        public string Accion { get; set; }
    }
}
